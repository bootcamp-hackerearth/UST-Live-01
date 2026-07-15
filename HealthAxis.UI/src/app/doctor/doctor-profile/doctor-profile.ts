import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { ChangePasswordRequest } from '../../core/models/auth.model';
import { Doctor } from '../../core/models/doctor.model';
import { AuthService } from '../../core/services/auth.service';
import { DoctorService } from '../../core/services/doctor.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const STRONG_PASSWORD_PATTERN =
  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/;

@Component({
  selector: 'app-doctor-profile',
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorProfile {
  readonly doctorStatusState =
    inject(DoctorStatusStateService);

  private readonly formBuilder =
    inject(FormBuilder);

  private readonly doctorService =
    inject(DoctorService);

  private readonly authService =
    inject(AuthService);

  readonly doctor = signal<Doctor | null>(null);

  readonly loading = signal(false);
  readonly changingPassword = signal(false);
  readonly updatingStatus = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly statusErrorMessage = signal('');
  readonly passwordErrorMessage = signal('');
  readonly passwordSuccessMessage = signal('');

  readonly showCurrentPassword = signal(false);
  readonly showNewPassword = signal(false);
  readonly showConfirmPassword = signal(false);

  readonly passwordForm =
    this.formBuilder.nonNullable.group(
      {
        currentPassword: [
          '',
          [
            Validators.required
          ]
        ],
        newPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(100),
            Validators.pattern(
              STRONG_PASSWORD_PATTERN
            )
          ]
        ],
        confirmPassword: [
          '',
          [
            Validators.required
          ]
        ]
      },
      {
        validators:
          DoctorProfile.passwordsMatchValidator
      }
    );

  get currentPassword() {
    return this.passwordForm.controls.currentPassword;
  }

  get newPassword() {
    return this.passwordForm.controls.newPassword;
  }

  get confirmPassword() {
    return this.passwordForm.controls.confirmPassword;
  }

  constructor() {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading.set(true);
    this.clearProfileMessages();

    this.doctorService
      .getMyDoctorProfile()
      .subscribe({
        next: (doctor) => {
          this.doctor.set(doctor);

          this.doctorStatusState.setStatus(
            Boolean(doctor.isActive)
          );

          this.loading.set(false);
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load doctor profile.'
            )
          );
        }
      });
  }

  toggleDoctorStatus(): void {
    const currentDoctor = this.doctor();

    const currentStatus =
      this.doctorStatusState.isActive();

    this.statusErrorMessage.set('');
    this.successMessage.set('');

    if (!currentDoctor) {
      this.statusErrorMessage.set(
        'Doctor profile is not loaded.'
      );

      return;
    }

    if (currentStatus === null) {
      this.statusErrorMessage.set(
        'Doctor status is still loading.'
      );

      return;
    }

    this.updatingStatus.set(true);

    this.doctorService
      .updateMyStatus(!currentStatus)
      .subscribe({
        next: (response) => {
          this.doctor.set({
            ...currentDoctor,
            isActive: response.isActive
          });

          this.doctorStatusState.setStatus(
            response.isActive
          );

          this.updatingStatus.set(false);

          this.successMessage.set(
            response.message ||
            'Availability status updated successfully.'
          );
        },
        error: (error: unknown) => {
          this.updatingStatus.set(false);

          this.statusErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not update availability status.'
            )
          );
        }
      });
  }

  changePassword(): void {
    this.passwordErrorMessage.set('');
    this.passwordSuccessMessage.set('');

    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();

      this.passwordErrorMessage.set(
        'Please correct the password details.'
      );

      return;
    }

    const formValue =
      this.passwordForm.getRawValue();

    if (
      formValue.currentPassword ===
      formValue.newPassword
    ) {
      this.passwordErrorMessage.set(
        'New password must be different from the current password.'
      );

      return;
    }

    const request: ChangePasswordRequest = {
      currentPassword:
        formValue.currentPassword,
      newPassword:
        formValue.newPassword,
      confirmNewPassword:
        formValue.confirmPassword
    };

    this.changingPassword.set(true);

    this.authService
      .changePassword(request)
      .subscribe({
        next: () => {
          this.changingPassword.set(false);
          this.passwordForm.reset();
          this.resetPasswordVisibility();

          this.passwordSuccessMessage.set(
            'Password changed successfully.'
          );
        },
        error: (error: unknown) => {
          this.changingPassword.set(false);

          this.passwordErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not change password.'
            )
          );
        }
      });
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword.update(
      (isVisible) => !isVisible
    );
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword.update(
      (isVisible) => !isVisible
    );
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword.update(
      (isVisible) => !isVisible
    );
  }

  getDoctorDisplayName(): string {
    const name =
      this.doctor()?.fullName?.trim();

    if (!name) {
      return 'Doctor';
    }

    const normalizedName =
      name.toLowerCase();

    if (
      normalizedName.startsWith('dr.') ||
      normalizedName.startsWith('dr ')
    ) {
      return name;
    }

    return `Dr. ${name}`;
  }

  getDoctorInitial(): string {
    const name =
      this.doctor()?.fullName
        ?.trim()
        .replace(/^dr\.?\s+/i, '');

    return name?.charAt(0)
      .toUpperCase() || 'D';
  }

  getDoctorStatusText(): string {
    const status =
      this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading status';
    }

    return status
      ? 'Available'
      : 'Unavailable';
  }

  getStatusButtonText(): string {
    const status =
      this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading...';
    }

    return status
      ? 'Set Unavailable'
      : 'Set Available';
  }

  getCurrentPasswordError(): string {
    if (
      !this.currentPassword.touched ||
      !this.currentPassword.errors
    ) {
      return '';
    }

    if (
      this.currentPassword.errors['required']
    ) {
      return 'Current password is required.';
    }

    return '';
  }

  getNewPasswordError(): string {
    if (
      !this.newPassword.touched ||
      !this.newPassword.errors
    ) {
      return '';
    }

    if (
      this.newPassword.errors['required']
    ) {
      return 'New password is required.';
    }

    if (
      this.newPassword.errors['minlength']
    ) {
      return (
        'New password must contain at least 8 characters.'
      );
    }

    if (
      this.newPassword.errors['maxlength']
    ) {
      return (
        'New password cannot exceed 100 characters.'
      );
    }

    if (
      this.newPassword.errors['pattern']
    ) {
      return (
        'Use uppercase, lowercase, number, and special character.'
      );
    }

    return '';
  }

  getConfirmPasswordError(): string {
    if (!this.confirmPassword.touched) {
      return '';
    }

    if (
      this.confirmPassword.errors?.['required']
    ) {
      return (
        'Please confirm the new password.'
      );
    }

    if (
      this.passwordForm.errors?.['passwordMismatch']
    ) {
      return (
        'New password and confirmation do not match.'
      );
    }

    return '';
  }

  hasMinimumLength(): boolean {
    return this.newPassword.value.length >= 8;
  }

  hasUppercaseLetter(): boolean {
    return /[A-Z]/.test(
      this.newPassword.value
    );
  }

  hasLowercaseLetter(): boolean {
    return /[a-z]/.test(
      this.newPassword.value
    );
  }

  hasNumber(): boolean {
    return /\d/.test(
      this.newPassword.value
    );
  }

  hasSpecialCharacter(): boolean {
    return /[^A-Za-z\d]/.test(
      this.newPassword.value
    );
  }

  private clearProfileMessages(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.statusErrorMessage.set('');
  }

  private resetPasswordVisibility(): void {
    this.showCurrentPassword.set(false);
    this.showNewPassword.set(false);
    this.showConfirmPassword.set(false);
  }

  private static passwordsMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const newPassword =
      control.get('newPassword')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (
      newPassword &&
      confirmPassword &&
      newPassword !== confirmPassword
    ) {
      return {
        passwordMismatch: true
      };
    }

    return null;
  }
}