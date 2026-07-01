import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { GenderType } from '../../core/models/register-patient-request';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register',
  imports: [RouterLink, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  phoneNumber = '';
  gender: GenderType | '' = '';
  dateOfBirth = '';
  insuranceId = '';

  readonly genders: GenderType[] = ['Male', 'Female', 'Transgender', 'Other'];

  readonly isSubmitting = signal(false);
  readonly message = signal('');
  readonly isError = signal(false);

  registerPatient(): void {
    this.message.set('');
    this.isError.set(false);

    const validationMessage = this.validateForm();

    if (validationMessage) {
      this.showError(validationMessage);
      return;
    }

    this.isSubmitting.set(true);

    this.authService.registerPatient({
      fullName: this.fullName.trim(),
      email: this.email.trim(),
      password: this.password,
      dateOfBirth: this.dateOfBirth,
      gender: this.gender as GenderType,
      phoneNumber: this.phoneNumber.trim(),
      insuranceId: this.insuranceId.trim()
    }).subscribe({
      next: () => {
        this.isSubmitting.set(false);
        this.message.set('Patient registered successfully. You can login now.');
        this.isError.set(false);

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 900);
      },
      error: error => {
        this.isSubmitting.set(false);
        this.showError(this.authService.getErrorMessage(error));
      }
    });
  }

  private validateForm(): string {
    const fullNamePattern = /^[A-Z][a-zA-Z\s]{2,}$/;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const phonePattern = /^[6-9]\d{9}$/;

    if (
      !this.fullName.trim() ||
      !this.email.trim() ||
      !this.password.trim() ||
      !this.confirmPassword.trim() ||
      !this.phoneNumber.trim() ||
      !this.gender ||
      !this.dateOfBirth
    ) {
      return 'Please fill all required fields.';
    }

    if (!fullNamePattern.test(this.fullName.trim())) {
      return 'Full name must start with an uppercase letter and be at least 3 characters long.';
    }

    if (!emailPattern.test(this.email.trim())) {
      return 'Valid email address is required.';
    }

    if (!phonePattern.test(this.phoneNumber.trim())) {
      return 'Invalid phone number. Phone number must start with 6-9 and contain 10 digits.';
    }

    if (this.password.length < 6) {
      return 'Password must be at least 6 characters long.';
    }

    if (this.password !== this.confirmPassword) {
      return 'Password and confirm password do not match.';
    }

    if (new Date(this.dateOfBirth) > new Date()) {
      return 'Date of birth cannot be in the future.';
    }

    return '';
  }

  private showError(message: string): void {
    this.message.set(message);
    this.isError.set(true);
  }
}