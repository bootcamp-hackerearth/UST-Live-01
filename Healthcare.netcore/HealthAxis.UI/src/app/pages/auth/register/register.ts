import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

interface RegisterResponse {
  success?: boolean;
  message?: string;
}

interface RegistrationError {
  message?: string;
  title?: string;
}

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  fullName = '';
  dateOfBirth = '';
  gender = '';
  phoneNumber = '';
  email = '';
  password = '';
  confirmPassword = '';

  submitted = false;
  isRegistering = false;

  readonly todayDate = new Date()
    .toISOString()
    .split('T')[0];

  private readonly registerUrl =
    'https://localhost:7130/api/auth/register';

  get fullNameError(): string {
    if (!this.submitted) {
      return '';
    }

    const value = this.fullName.trim();

    if (!value) {
      return 'Full name is required.';
    }

    if (value.length < 3) {
      return 'Full name must be at least 3 characters.';
    }

    return '';
  }

  get dateOfBirthError(): string {
    if (!this.submitted) {
      return '';
    }

    if (!this.dateOfBirth) {
      return 'Date of birth is required.';
    }

    const selectedDate = new Date(
      `${this.dateOfBirth}T00:00:00`
    );

    const minimumDate = new Date(
      '1900-01-01T00:00:00'
    );

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    if (selectedDate < minimumDate) {
      return 'Date of birth cannot be before 01 January 1900.';
    }

    if (selectedDate > today) {
      return 'Future date is not allowed.';
    }

    return '';
  }

  get genderError(): string {
    if (!this.submitted) {
      return '';
    }

    if (this.gender === '') {
      return 'Gender is required.';
    }

    return '';
  }

  get phoneNumberError(): string {
    if (!this.submitted) {
      return '';
    }

    const value = this.phoneNumber.trim();

    if (!value) {
      return 'Phone number is required.';
    }

    const phonePattern = /^\d{10}$/;

    if (!phonePattern.test(value)) {
      return 'Phone number must be 10 digits.';
    }

    return '';
  }

  get emailError(): string {
    if (!this.submitted) {
      return '';
    }

    const value = this.email.trim();

    if (!value) {
      return 'Email is required.';
    }

    const emailPattern =
      /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(value)) {
      return 'Enter a valid email address.';
    }

    return '';
  }

  get passwordError(): string {
    if (!this.submitted) {
      return '';
    }

    if (!this.password.trim()) {
      return 'Password is required.';
    }

    if (this.password.length < 6) {
      return 'Password must be at least 6 characters.';
    }

    return '';
  }

  get confirmPasswordError(): string {
    if (!this.submitted) {
      return '';
    }

    if (!this.confirmPassword.trim()) {
      return 'Confirm password is required.';
    }

    if (this.password !== this.confirmPassword) {
      return 'Password and confirm password do not match.';
    }

    return '';
  }

  hasErrors(): boolean {
    return Boolean(
      this.fullNameError ||
      this.dateOfBirthError ||
      this.genderError ||
      this.phoneNumberError ||
      this.emailError ||
      this.passwordError ||
      this.confirmPasswordError
    );
  }

  register(): void {
    this.submitted = true;

    if (this.hasErrors() || this.isRegistering) {
      return;
    }

    const payload = {
      fullName: this.fullName.trim(),
      dateOfBirth: this.dateOfBirth,
      gender: Number(this.gender),
      phoneNumber: this.phoneNumber.trim(),
      email: this.email.trim(),
      password: this.password,
      confirmPassword: this.confirmPassword,
      insuranceId: 'NA'
    };

    this.isRegistering = true;

    this.http
      .post<RegisterResponse>(
        this.registerUrl,
        payload
      )
      .subscribe({
        next: (response) => {
          this.isRegistering = false;

          alert(
            response.message ||
            'Registration successful. Please login.'
          );

          void this.router.navigate(['/login']);
        },

        error: (error: HttpErrorResponse) => {
          this.isRegistering = false;

          const response = error.error as
            | RegistrationError
            | string
            | null;

          let message = 'Registration failed.';

          if (
            typeof response === 'string' &&
            response.trim()
          ) {
            message = response;
          } else if (
            response &&
            typeof response !== 'string'
          ) {
            message =
              response.message ||
              response.title ||
              message;
          }

          alert(message);
        }
      });
  }
}