import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

// Services & DTOs
import { DoctorService } from '../../../core/services/doctor.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { DoctorDto } from '../../../dtos/doctor.dto';
import { ChangePasswordDto } from '../../../dtos/auth.dto';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class DoctorProfile implements OnInit {
  // Services
  private readonly doctorService = inject(DoctorService);
  private readonly authService = inject(AuthService);
  private readonly notificationService = inject(NotificationService);

  // Signals
  readonly isLoadingProfile = signal(false);
  readonly showChangePassword = signal(false);
  readonly isChangingPassword = signal(false);
  readonly showStatusConfirmModal = signal(false);
  readonly isUpdatingStatus = signal(false);

  // State
  doctorProfile: DoctorDto | null = null;
  pendingStatus: boolean | null = null;

  passwordForm: ChangePasswordDto = {
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: '',
  };

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoadingProfile.set(true);
    this.doctorService
      .getMyProfile()
      .pipe(finalize(() => this.isLoadingProfile.set(false)))
      .subscribe({
        next: (profile) => (this.doctorProfile = profile),
      });
  }

  // --- Status Methods ---
  onStatusToggleClick(event: Event): void {
    event.preventDefault();
    if (!this.doctorProfile || this.isUpdatingStatus()) return;
    this.openStatusConfirmModal(!this.doctorProfile.isActive);
  }

  openStatusConfirmModal(nextStatus: boolean): void {
    this.pendingStatus = nextStatus;
    this.showStatusConfirmModal.set(true);
  }

  closeStatusConfirmModal(): void {
    if (this.isUpdatingStatus()) return;
    this.pendingStatus = null;
    this.showStatusConfirmModal.set(false);
  }

  confirmStatusChange(): void {
    if (this.pendingStatus === null || !this.doctorProfile) return;

    const nextStatus = this.pendingStatus;
    this.isUpdatingStatus.set(true);

    this.doctorService
      .changeMyStatus(nextStatus)
      .pipe(finalize(() => this.isUpdatingStatus.set(false)))
      .subscribe({
        next: (response) => {
          this.doctorProfile = { ...this.doctorProfile!, isActive: nextStatus };
          this.notificationService.success(response.message || 'Status updated successfully.');
          this.pendingStatus = null;
          this.showStatusConfirmModal.set(false);
        },
      });
  }

  // --- Password Methods ---
  toggleChangePassword(): void {
    this.showChangePassword.update((value) => !value);
    if (!this.showChangePassword()) this.resetPasswordForm();
  }

  changePassword(): void {
    if (!this.validatePasswordForm()) return;

    this.isChangingPassword.set(true);
    this.authService
      .changePassword(this.passwordForm)
      .pipe(finalize(() => this.isChangingPassword.set(false)))
      .subscribe({
        next: (response) => {
          this.notificationService.success(response.message || 'Password changed successfully.');
          this.resetPasswordForm();
          this.showChangePassword.set(false);
        },
      });
  }

  resetPasswordForm(): void {
    this.passwordForm = { currentPassword: '', newPassword: '', confirmNewPassword: '' };
  }

  private validatePasswordForm(): boolean {
    if (!this.passwordForm.currentPassword.trim()) {
      this.notificationService.warning('Current password is required.');
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
}
