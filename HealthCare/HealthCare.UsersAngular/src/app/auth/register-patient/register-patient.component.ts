import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-patient-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register-patient.component.html',
  styleUrls: ['./register-patient.component.css']
})
export class PatientRegisterComponent {

  registerForm: FormGroup;

  submitted = false;
  successMessage = '';
  emailExists = false;

  showPopup = false;
  popupMessage = '';
  isSuccess = false;

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router,
    private readonly fb: FormBuilder
  ) {

    this.registerForm = this.fb.group({

      // Full Name: only alphabets with at least one space
      fullName: [
        '',
        [
          Validators.required,
          Validators.pattern('^[A-Za-z ]+$')
        ]
      ],

      // Email format validation
      email: [
        '',
        [
          Validators.required,
          Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.com$')
        ]
      ],

      // Phone number should start from 6-9 and contain 10 digits
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern('^[6-9][0-9]{9}$')
        ]
      ],

      // Gender required
      gender: ['', Validators.required],

      // DOB required
      dateOfBirth: ['', Validators.required],

      // Optional field
      insuranceId: [''],

      // Password validation
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/
          )
        ]
      ]
    });
  }

  get f() {
    return this.registerForm.controls;
  }

  isPastDate(): boolean {

    const dob = this.registerForm.get('dateOfBirth')?.value;

    if (!dob) {
      return false;
    }

    const selectedDate = new Date(dob);
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    return selectedDate < today;
  }

  register() {

    this.submitted = true;
    this.emailExists = false;
    this.successMessage = '';

    if (this.registerForm.invalid || !this.isPastDate()) {
      return;
    }

    const patient = this.registerForm.value;

    // Replace with your actual API URL
    this.http.post<any>(
      '/api/auth/register/patient',
      patient
    )
      .subscribe({

        next: () => {

          this.isSuccess = true;
          this.popupMessage = 'Registered Successfully';
          this.showPopup = true;

        },

        error: (err) => {

          this.isSuccess = false;

          if (err.status === 409) {
            this.popupMessage = 'Email already exists';
          }
          else {
            this.popupMessage = 'Registration failed';
          }

          this.showPopup = true;

          console.log(err);
        }
      });
  }

    goToLogin() {

      this.showPopup = false;
      this.router.navigate(['/']);
    }

  goBack() {
    this.router.navigate(['/']);
  }
}
