import { Component, computed, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { finalize, timeout } from 'rxjs';

import {
  GenderType,
  RegisterPatientRequest
} from '../../core/models/register-patient-request';
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

  readonly fullName = signal('');
  readonly email = signal('');
  readonly password = signal('');
  readonly confirmPassword = signal('');
  readonly phoneNumber = signal('');
  readonly gender = signal<GenderType | ''>('');
  readonly dateOfBirth = signal('');
  readonly insuranceId = signal('');

  readonly maxDateOfBirth = this.toDateInputValue(new Date());

  readonly genders: GenderType[] = ['Male', 'Female', 'Transgender', 'Other'];

  readonly submitAttempted = signal(false);
  readonly isSubmitting = signal(false);
  readonly message = signal('');
  readonly isError = signal(false);

  readonly fullNameError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.fullName().trim();

    if (!value) {
      return 'Full name is required.';
    }

    const fullNamePattern = /^[A-Z][a-zA-Z\s]{2,}$/;

    if (!fullNamePattern.test(value)) {
      return 'Full name must start with an uppercase letter and be at least 3 characters.';
    }

    return '';
  });

  readonly emailError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.email().trim();

    if (!value) {
      return 'Email is required.';
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

    if (!emailPattern.test(value)) {
      return 'Enter a valid email address, for example user@example.com.';
    }

    return '';
  });

  readonly phoneNumberError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.phoneNumber().trim();

    if (!value) {
      return 'Phone number is required.';
    }

    const phonePattern = /^[6-9]\d{9}$/;

    if (!phonePattern.test(value)) {
      return 'Phone number must start with 6-9 and contain 10 digits.';
    }

    return '';
  });

  readonly genderError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    if (!this.gender()) {
      return 'Gender is required.';
    }

    return '';
  });

  readonly dateOfBirthError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.dateOfBirth();

    if (!value) {
      return 'Date of birth is required.';
    }

    const selectedDate = new Date(value);
    const minimumDate = new Date(1900, 0, 1);
    const today = this.getTodayDateOnly();

    if (Number.isNaN(selectedDate.getTime()) || selectedDate < minimumDate) {
      return 'Please enter a valid date of birth.';
    }

    if (selectedDate > today) {
      return 'Date of birth cannot be in the future.';
    }

    return '';
  });

  readonly passwordError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.password();

    if (!value.trim()) {
      return 'Password is required.';
    }

    if (value.length < 6) {
      return 'Password must be at least 6 characters long.';
    }

    const passwordPattern = /^[A-Z][a-z]+(?=.*\d)(?=.*[^a-zA-Z0-9]).*$/;

    if (!passwordPattern.test(value)) {
      return 'Password must start with a capital letter and include lowercase letters, a number, and a special character.';
    }

    return '';
  });

  readonly confirmPasswordError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.confirmPassword();

    if (!value.trim()) {
      return 'Confirm password is required.';
    }

    if (value !== this.password()) {
      return 'Password and confirm password do not match.';
    }

    return '';
  });

  readonly hasValidationErrors = computed(() =>
    !!this.fullNameError() ||
    !!this.emailError() ||
    !!this.phoneNumberError() ||
    !!this.genderError() ||
    !!this.dateOfBirthError() ||
    !!this.passwordError() ||
    !!this.confirmPasswordError()
  );

  registerPatient(): void {
    this.submitAttempted.set(true);
    this.message.set('');
    this.isError.set(false);

    if (this.hasValidationErrors()) {
      return;
    }

    const request: RegisterPatientRequest = {
      fullName: this.fullName().trim(),
      email: this.email().trim(),
      password: this.password(),
      dateOfBirth: this.dateOfBirth(),
      gender: this.gender() as GenderType,
      phoneNumber: this.phoneNumber().trim(),
      insuranceId: this.insuranceId().trim()
    };

    this.isSubmitting.set(true);

    this.authService
      .registerPatient(request)
      .pipe(
        timeout({ first: 15000 }),
        finalize(() => this.isSubmitting.set(false))
      )
      .subscribe({
        next: () => {
          this.message.set('Patient registered successfully. You can login now.');
          this.isError.set(false);

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 900);
        },
        error: error => {
          this.message.set(this.authService.getErrorMessage(error));
          this.isError.set(true);
        }
      });
  }

  updateFullName(value: string): void {
    this.fullName.set(value);
    this.clearTopMessage();
  }

  updateEmail(value: string): void {
    this.email.set(value);
    this.clearTopMessage();
  }

  updatePhoneNumber(value: string): void {
    this.phoneNumber.set(value);
    this.clearTopMessage();
  }

  updateGender(value: GenderType | ''): void {
    this.gender.set(value);
    this.clearTopMessage();
  }

  updateDateOfBirth(value: string): void {
    this.dateOfBirth.set(value);
    this.clearTopMessage();
  }

  updateInsuranceId(value: string): void {
    this.insuranceId.set(value);
    this.clearTopMessage();
  }

  updatePassword(value: string): void {
    this.password.set(value);
    this.clearTopMessage();
  }

  updateConfirmPassword(value: string): void {
    this.confirmPassword.set(value);
    this.clearTopMessage();
  }

  private clearTopMessage(): void {
    this.message.set('');
    this.isError.set(false);
  }

  private getTodayDateOnly(): Date {
    const now = new Date();

    return new Date(
      now.getFullYear(),
      now.getMonth(),
      now.getDate()
    );
  }

  private toDateInputValue(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}