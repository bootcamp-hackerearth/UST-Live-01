import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterPatientRequest } from '../../../core/models/register-patient-request';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrls : ['./register.css']
})
export class Register {
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  minDate = '1900-01-01';
  maxDate = new Date().toISOString().split('T')[0];

  registerForm: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    this.registerForm = this.fb.group(
      {
        fullName: [
          '',
          [
            Validators.required,
            Validators.maxLength(100),
            Validators.pattern(/^[A-Za-z]+(?:[ .'’-][A-Za-z]+)*$/)
          ]
        ],

        dateOfBirth: [
          '',
          [
            Validators.required,
            this.dateOfBirthValidator
          ]
        ],

        gender: [
          null,
          Validators.required
        ],

        phoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern(/^[0-9]{10}$/)
          ]
        ],

        email: [
          '',
          [
            Validators.required,
            Validators.maxLength(100),
            Validators.pattern(/^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/)
          ]
        ],

        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8)
          ]
        ],

        confirmPassword: [
          '',
          Validators.required
        ]
      },
      {
        validators: this.passwordMatchValidator
      }
    );
  }

  get fullName() {
    return this.registerForm.get('fullName');
  }

  get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }

  get gender() {
    return this.registerForm.get('gender');
  }

  get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  submitRegister(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const formValue = this.registerForm.value;

    const request: RegisterPatientRequest = {
      fullName: formValue.fullName ?? '',
      dateOfBirth: formValue.dateOfBirth ?? '',
      gender: Number(formValue.gender),
      phoneNumber: formValue.phoneNumber ?? '',
      email: formValue.email ?? '',
      password: formValue.password ?? '',
      confirmPassword: formValue.confirmPassword ?? ''
    };

    this.authService.registerPatient(request).subscribe({
      next: () => {
        this.successMessage =
          'Registration successful. Please login to continue.';

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1200);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          'Registration failed. Please check the details and try again.';

        this.isSubmitting = false;
      },

      complete: () => {
        this.isSubmitting = false;
      }
    });
  }

  private passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const password =
      control.get('password')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  private dateOfBirthValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate =
      new Date(control.value);

    const minDate =
      new Date('1900-01-01');

    const today =
      new Date();

    selectedDate.setHours(0, 0, 0, 0);
    minDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    if (selectedDate < minDate) {
      return { dateBefore1900: true };
    }

    if (selectedDate > today) {
      return { futureDate: true };
    }

    return null;
  }
}
