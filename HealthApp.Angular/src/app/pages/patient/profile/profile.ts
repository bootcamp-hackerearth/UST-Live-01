import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { NotificationService } from '../../../core/services/notification.service';
import { PatientService } from '../../../core/services/patient.service';
import { AuthService } from '../../../core/services/auth.service';

import { PatientCreateDto, PatientDto } from '../../../dtos/patient.dto';

import { ChangePasswordDto } from '../../../dtos/auth.dto';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class PatientProfile implements OnInit {
  private readonly notificationService = inject(NotificationService);
  private readonly patientService = inject(PatientService);
  private readonly authService = inject(AuthService);

  readonly isLoadingProfile = signal(false);
  readonly isSavingProfile = signal(false);
  readonly isEditing = signal(false);
  readonly showChangePassword = signal(false);
  readonly isChangingPassword = signal(false);

  profile: PatientDto | null = null;

  editableProfile: PatientCreateDto = this.getEmptyEditableProfile();

  passwordForm: ChangePasswordDto = {
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  };

  genderOptions = ['Male', 'Female', 'Other'];

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoadingProfile.set(true);

    this.patientService
      .getMyProfile()
      .pipe(
        finalize(() => {
          this.isLoadingProfile.set(false);
        }),
      )
      .subscribe({
        next: (profile) => {
          this.profile = profile;
          this.editableProfile = this.mapProfileToEditable(profile);
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  enableEdit(): void {
    if (!this.profile) {
      this.notificationService.warning('Profile is not loaded yet.');
      return;
    }

    this.editableProfile = this.mapProfileToEditable(this.profile);
    this.isEditing.set(true);
  }

  cancelEdit(): void {
    if (this.profile) {
      this.editableProfile = this.mapProfileToEditable(this.profile);
    }

    this.isEditing.set(false);
  }

  saveProfile(): void {
    if (!this.validateProfile()) {
      return;
    }

    const payload: PatientCreateDto = {
      fullName: this.editableProfile.fullName.trim(),
      dateOfBirth: this.editableProfile.dateOfBirth,
      gender: this.editableProfile.gender,
      email: this.editableProfile.email.trim(),
      phoneNumber: this.editableProfile.phoneNumber.trim(),
      insuranceId: this.editableProfile.insuranceId?.trim() || null,
    };

    this.isSavingProfile.set(true);

    this.patientService
      .updateMyProfile(payload)
      .pipe(
        finalize(() => {
          this.isSavingProfile.set(false);
        }),
      )
      .subscribe({
        next: (response) => {
          this.notificationService.success(response.message || 'Profile updated successfully.');

          this.isEditing.set(false);
          this.loadProfile();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  toggleChangePassword(): void {
    this.showChangePassword.update((value) => !value);

    if (!this.showChangePassword()) {
      this.resetPasswordForm();
    }
  }

  changePassword(): void {
    if (!this.validatePasswordForm()) {
      return;
    }

    this.isChangingPassword.set(true);

    this.authService
      .changePassword(this.passwordForm)
      .pipe(
        finalize(() => {
          this.isChangingPassword.set(false);
        }),
      )
      .subscribe({
        next: (response) => {
          this.notificationService.success(response.message || 'Password changed successfully.');

          this.resetPasswordForm();
          this.showChangePassword.set(false);
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmNewPassword: '',
    };
  }

  getInitials(): string {
    if (!this.profile?.fullName) {
      return 'PT';
    }

    const parts = this.profile.fullName.trim().split(' ');

    if (parts.length === 1) {
      return parts[0].charAt(0).toUpperCase();
    }

    return `${parts[0].charAt(0)}${parts.at(-1)?.charAt(0) ?? ''}`.toUpperCase();
  }

  private validateProfile(): boolean {
    if (!this.editableProfile.fullName.trim()) {
      this.notificationService.warning('Full name is required.');
      return false;
    }

    if (this.editableProfile.fullName.trim().length < 3) {
      this.notificationService.warning('Full name must be at least 3 characters.');
      return false;
    }

    if (!this.editableProfile.dateOfBirth) {
      this.notificationService.warning('Date of birth is required.');
      return false;
    }

    if (!this.editableProfile.gender) {
      this.notificationService.warning('Gender is required.');
      return false;
    }

    if (!this.editableProfile.email.trim()) {
      this.notificationService.warning('Email is required.');
      return false;
    }

    if (!this.isValidEmail(this.editableProfile.email)) {
      this.notificationService.warning('Please enter a valid email address.');
      return false;
    }

    if (!this.editableProfile.phoneNumber.trim()) {
      this.notificationService.warning('Phone number is required.');
      return false;
    }

    if (!/^\d{10}$/.test(this.editableProfile.phoneNumber)) {
      this.notificationService.warning('Phone number must be exactly 10 digits.');
      return false;
    }

    return true;
  }

  private validatePasswordForm(): boolean {
    if (!this.passwordForm.currentPassword.trim()) {
      this.notificationService.warning('Current password is required.');
      return false;
    }

    if (!this.passwordForm.newPassword.trim()) {
      this.notificationService.warning('New password is required.');
      return false;
    }

    if (this.passwordForm.newPassword.length < 6) {
      this.notificationService.warning('New password must be at least 6 characters.');
      return false;
    }

    if (this.passwordForm.currentPassword === this.passwordForm.newPassword) {
      this.notificationService.warning('New password must be different from current password.');
      return false;
    }

    if (this.passwordForm.newPassword !== this.passwordForm.confirmNewPassword) {
      this.notificationService.warning('New password and confirm password do not match.');
      return false;
    }

    return true;
  }

  private mapProfileToEditable(profile: PatientDto): PatientCreateDto {
    return {
      fullName: profile.fullName,
      dateOfBirth: profile.dateOfBirth?.substring(0, 10),
      gender: profile.gender,
      email: profile.email,
      phoneNumber: profile.phoneNumber,
      insuranceId: profile.insuranceId ?? null,
    };
  }

  private getEmptyEditableProfile(): PatientCreateDto {
    return {
      fullName: '',
      dateOfBirth: '',
      gender: '',
      email: '',
      phoneNumber: '',
      insuranceId: '',
    };
  }

  private isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }
}
