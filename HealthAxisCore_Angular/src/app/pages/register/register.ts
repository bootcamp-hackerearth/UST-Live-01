import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerForm: FormGroup;

  showPassword = false;

  showConfirmPassword = false;

  submitted = false;

  constructor(private formBuilder: FormBuilder) {
    this.registerForm = this.formBuilder.group(
      {
        patientName: [
          '',
          [
            Validators.required,
            Validators.minLength(2)
          ]
        ],
        dateOfBirth: [
          '',
          [
            Validators.required
          ]
        ],
        gender: [
          '',
          [
            Validators.required
          ]
        ],
        email: [
          '',
          [
            Validators.required,
            Validators.email
          ]
        ],
        phoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern(/^[0-9]{10}$/)
          ]
        ],
        insuranceID: [
          '',
          [
            Validators.required
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
          [
            Validators.required
          ]
        ],
        termsAccepted: [
          false,
          [
            Validators.requiredTrue
          ]
        ]
      },
      {
        validators: this.passwordsShouldMatch
      }
    );
  }

  get patientName() {
    return this.registerForm.get('patientName');
  }

  get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }

  get gender() {
    return this.registerForm.get('gender');
  }

  get email() {
    return this.registerForm.get('email');
  }

  get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }

  get insuranceID() {
    return this.registerForm.get('insuranceID');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  get termsAccepted() {
    return this.registerForm.get('termsAccepted');
  }

  get passwordsDoNotMatch() {
    return this.registerForm.errors?.['passwordMismatch'] &&
      (this.confirmPassword?.touched || this.submitted);
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  submitRegister(): void {
    this.submitted = true;

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    console.log('Register form submitted:', this.registerForm.value);

    // API connection will be added later.
  }

  private passwordsShouldMatch(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;

    const confirmPassword = control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }
}
