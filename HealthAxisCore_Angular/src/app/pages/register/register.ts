import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';

import { AuthService } from '../../core/services/auth.service';
import { RegisterPatientRequest } from '../../core/models/register-patient-request';

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

  showPassword = signal(false);

  showConfirmPassword = signal(false);

  submitted = signal(false);

  isLoading = signal(false);

  errorMessage = signal('');

  todayDate = this.getTodayDate();

  minimumDateOfBirth = this.getMinimumDateOfBirth();

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService
  ) {
    this.registerForm = this.formBuilder.group(
      {
        patientName: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.pattern(/^[a-zA-Z\s]+$/)
          ]
        ],
        dateOfBirth: [
          '',
          [
            Validators.required,
            Register.dateOfBirthValidator
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
            Validators.email,
            Register.emailDomainValidator
          ]
        ],
        phoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern(/^\d{10}$/)
          ]
        ],
        insuranceID: [
          ''
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
      (this.confirmPassword?.touched || this.submitted());
  }

  togglePasswordVisibility(): void {
    this.showPassword.update(value => !value);
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword.update(value => !value);
  }

  submitRegister(): void {
    this.submitted.set(true);
    this.errorMessage.set('');

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const request: RegisterPatientRequest = {
      patientName: this.registerForm.value.patientName,
      dateOfBirth: this.registerForm.value.dateOfBirth,
      gender: this.registerForm.value.gender,
      email: this.registerForm.value.email,
      phoneNumber: this.registerForm.value.phoneNumber,
      insuranceID: this.registerForm.value.insuranceID,
      password: this.registerForm.value.password
    };

    this.isLoading.set(true);

    this.authService.registerPatient(request).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: error => {
        this.isLoading.set(false);
        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
      }
    });
  }

  private static emailDomainValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const email = control.value?.toString().trim().toLowerCase();

    if (!email?.includes('@')) {
      return null;
    }

    const domainPart = email.split('@')[1];

    if (!domainPart?.includes('.')) {
      return null;
    }

    const domainName = domainPart.split('.')[0];

    if (domainName === 'healthaxis') {
      return {
        blockedPatientDomain: true
      };
    }

    return null;
  }

  private static dateOfBirthValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const value = control.value;

    if (!value) {
      return null;
    }

    const selectedDate = new Date(value);

    const today = new Date();

    today.setHours(0, 0, 0, 0);

    selectedDate.setHours(0, 0, 0, 0);

    const minimumDate = new Date(
      today.getFullYear() - 120,
      today.getMonth(),
      today.getDate()
    );

    minimumDate.setHours(0, 0, 0, 0);

    if (selectedDate > today) {
      return {
        futureDateOfBirth: true
      };
    }

    if (selectedDate < minimumDate) {
      return {
        tooOldDateOfBirth: true
      };
    }

    return null;
  }

  private passwordsShouldMatch(
    control: AbstractControl
  ): ValidationErrors | null {
    const password = control.get('password')?.value;

    const confirmPassword = control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword
      ? null
      : { passwordMismatch: true };
  }

  private getTodayDate(): string {
    const today = new Date();

    return today.toISOString().split('T')[0];
  }

  private getMinimumDateOfBirth(): string {
    const today = new Date();

    const minimumDate = new Date(
      today.getFullYear() - 120,
      today.getMonth(),
      today.getDate()
    );

    return minimumDate.toISOString().split('T')[0];
  }
}
