import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { RegisterPatientRequest } from '../../shared/models/auth.models';

@Component({
  selector: 'app-register',
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  currentStep = 1;
  loading = false;
  errorMessage = '';
  successMessage = '';

  showPassword = false;
  showConfirmPassword = false;

  form: RegisterPatientRequest = {
    fullName: '',
    email: '',
    phoneNumber: '',
    password: '',
    confirmPassword: '',
    dateOfBirth: '',
    gender: 0,
    insuranceNumber: ''
  };

  touched = {
    fullName: false,
    email: false,
    phoneNumber: false,
    password: false,
    confirmPassword: false,
    dateOfBirth: false,
    gender: false
  };

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  markTouched(field: keyof typeof this.touched): void {
    this.touched[field] = true;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  isValidFullName(fullName: string): boolean {
    const trimmedName = fullName.trim();

    if (!trimmedName) {
      return false;
    }

    return [...trimmedName].every((character) => {
      return (
        character === ' ' ||
        (character >= 'A' && character <= 'Z') ||
        (character >= 'a' && character <= 'z')
      );
    });
  }

  get isNameValid(): boolean {
    return (
      this.form.fullName.trim().length >= 3 &&
      this.isValidFullName(this.form.fullName)
    );
  }

  get isEmailValid(): boolean {
    return this.isSafeEmail(this.form.email);
  }

  get isPhoneValid(): boolean {
    return this.isTenDigitPhoneNumber(this.form.phoneNumber);
  }

  get passwordStrengthScore(): number {
    let score = 0;

    if (this.form.password.length >= 8) {
      score++;
    }

    if (/[A-Z]/.test(this.form.password)) {
      score++;
    }

    if (/[a-z]/.test(this.form.password)) {
      score++;
    }

    if (/[0-9]/.test(this.form.password)) {
      score++;
    }

    if (/[^A-Za-z0-9]/.test(this.form.password)) {
      score++;
    }

    return score;
  }

  get isPasswordValid(): boolean {
    return this.passwordStrengthScore >= 3;
  }

  get passwordStrengthLabel(): string {
    if (!this.form.password) {
      return '';
    }

    if (this.passwordStrengthScore <= 2) {
      return 'Weak';
    }

    if (this.passwordStrengthScore <= 4) {
      return 'Medium';
    }

    return 'Strong';
  }

  get passwordStrengthClass(): string {
    if (!this.form.password) {
      return '';
    }

    if (this.passwordStrengthScore <= 2) {
      return 'weak';
    }

    if (this.passwordStrengthScore <= 4) {
      return 'medium';
    }

    return 'strong';
  }

  get passwordsMatch(): boolean {
    return this.form.password === this.form.confirmPassword;
  }

  get isDateOfBirthValid(): boolean {
    if (!this.form.dateOfBirth) {
      return false;
    }

    const selectedDate = new Date(this.form.dateOfBirth);
    const today = new Date();

    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    return selectedDate < today;
  }

  get isGenderValid(): boolean {
    return Number(this.form.gender) >= 1 &&
           Number(this.form.gender) <= 4;
  }

  get canContinueToStepTwo(): boolean {
    return (
      this.isNameValid &&
      this.isEmailValid &&
      this.isPhoneValid &&
      this.isPasswordValid &&
      this.passwordsMatch
    );
  }

  get canSubmit(): boolean {
    return (
      this.canContinueToStepTwo &&
      this.isDateOfBirthValid &&
      this.isGenderValid
    );
  }

  goToStepTwo(): void {
    this.errorMessage = '';

    this.touched.fullName = true;
    this.touched.email = true;
    this.touched.phoneNumber = true;
    this.touched.password = true;
    this.touched.confirmPassword = true;

    if (!this.canContinueToStepTwo) {
      this.errorMessage =
        'Please fix the highlighted fields before continuing.';
      return;
    }

    this.currentStep = 2;
  }

  goBackToStepOne(): void {
    this.errorMessage = '';
    this.currentStep = 1;
  }

  register(): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.touched.dateOfBirth = true;
    this.touched.gender = true;

    if (!this.canSubmit) {
      this.errorMessage =
        'Please complete all required patient details correctly.';
      return;
    }

    this.loading = true;

    const request: RegisterPatientRequest = {
      fullName: this.form.fullName.trim(),
      email: this.form.email.trim().toLowerCase(),
      phoneNumber: this.form.phoneNumber.trim(),
      password: this.form.password,
      confirmPassword: this.form.confirmPassword,
      dateOfBirth: this.form.dateOfBirth,
      gender: Number(this.form.gender),
      insuranceNumber: this.form.insuranceNumber?.trim() || ''
    };

    this.authService.registerPatient(request).subscribe({
      next: () => {
        this.loading = false;
        this.successMessage =
          'Registration successful. Redirecting to patient portal...';

        setTimeout(() => {
          this.router.navigate(['/patient/dashboard']);
        }, 900);
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 0) {
          this.errorMessage =
            'Unable to connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage =
          'Registration failed. Please try again.';
      }
    });
  }

  private isSafeEmail(email: string): boolean {
    const normalizedEmail = email.trim();

    if (
      normalizedEmail.length < 5 ||
      normalizedEmail.length > 254 ||
      normalizedEmail.includes(' ')
    ) {
      return false;
    }

    const atIndex = normalizedEmail.indexOf('@');
    const lastAtIndex = normalizedEmail.lastIndexOf('@');

    if (
      atIndex <= 0 ||
      atIndex !== lastAtIndex ||
      atIndex === normalizedEmail.length - 1
    ) {
      return false;
    }

    const localPart = normalizedEmail.slice(0, atIndex);
    const domainPart = normalizedEmail.slice(atIndex + 1);

    if (
      !localPart ||
      !domainPart ||
      localPart.length > 64 ||
      domainPart.length > 253
    ) {
      return false;
    }

    const dotIndex = domainPart.lastIndexOf('.');

    if (
      dotIndex <= 0 ||
      dotIndex === domainPart.length - 1
    ) {
      return false;
    }

    return this.hasOnlyAllowedEmailCharacters(localPart, domainPart);
  }

  private hasOnlyAllowedEmailCharacters(
    localPart: string,
    domainPart: string
  ): boolean {
    return (
      [...localPart].every((character) =>
        this.isAllowedEmailLocalCharacter(character)
      ) &&
      [...domainPart].every((character) =>
        this.isAllowedEmailDomainCharacter(character)
      )
    );
  }

  private isAllowedEmailLocalCharacter(character: string): boolean {
    return (
      this.isAlphaNumeric(character) ||
      ['.', '_', '%', '+', '-'].includes(character)
    );
  }

  private isAllowedEmailDomainCharacter(character: string): boolean {
    return (
      this.isAlphaNumeric(character) ||
      character === '.' ||
      character === '-'
    );
  }

  private isAlphaNumeric(character: string): boolean {
    return (
      (character >= 'A' && character <= 'Z') ||
      (character >= 'a' && character <= 'z') ||
      (character >= '0' && character <= '9')
    );
  }

  private isTenDigitPhoneNumber(phoneNumber: string): boolean {
    const normalizedPhoneNumber = phoneNumber.trim();

    return (
      normalizedPhoneNumber.length === 10 &&
      [...normalizedPhoneNumber].every((character) =>
        character >= '0' && character <= '9'
      )
    );
  }
}

