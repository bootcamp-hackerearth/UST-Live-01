import {
  Component,
  EventEmitter,
  OnInit,
  Output
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { timeout } from 'rxjs';

import { DoctorDto } from '../../../../../shared/models/doctor.models';
import { DoctorApiService } from '../../../../../core/services/doctor-api.service';
import { AuthService } from '../../../../../core/services/auth.service';

type ToastType = 'success' | 'info' | 'warning';

interface DoctorToastEvent {
  message: string;
  type: ToastType;
}

interface DoctorPasswordForm {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile implements OnInit {
  doctor?: DoctorDto;

  isLoading = false;
  profileMessage = '';

  isPasswordMode = false;
  isPasswordConfirmModalOpen = false;
  isDiscardPasswordModalOpen = false;

  hasPasswordSubmitted = false;
  passwordMessage = '';

  passwordForm: DoctorPasswordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  @Output() doctorToast = new EventEmitter<DoctorToastEvent>();

  constructor(
    private readonly doctorApiService: DoctorApiService,
    private readonly authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  get isCurrentPasswordInvalid(): boolean {
    return !this.passwordForm.currentPassword.trim();
  }

  get isNewPasswordInvalid(): boolean {
    const password = this.passwordForm.newPassword;
    const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    return !passwordPattern.test(password);
  }

  get isConfirmPasswordInvalid(): boolean {
    return !this.passwordForm.confirmPassword.trim();
  }

  get isPasswordMismatch(): boolean {
    return (
      this.passwordForm.confirmPassword.trim().length > 0 &&
      this.passwordForm.newPassword !== this.passwordForm.confirmPassword
    );
  }

  get isSamePasswordInvalid(): boolean {
    return (
      this.passwordForm.currentPassword.trim().length > 0 &&
      this.passwordForm.currentPassword === this.passwordForm.newPassword
    );
  }

  get isPasswordFormInvalid(): boolean {
    return (
      this.isCurrentPasswordInvalid ||
      this.isNewPasswordInvalid ||
      this.isConfirmPasswordInvalid ||
      this.isPasswordMismatch ||
      this.isSamePasswordInvalid
    );
  }

  loadProfile(): void {
    this.isLoading = true;
    this.profileMessage = '';

    this.doctorApiService.getMyProfile().pipe(
      timeout(15000)
    ).subscribe({
      next: (doctor: DoctorDto) => {
        this.doctor = doctor;
        this.isLoading = false;
      },
      error: (error: unknown) => {
        console.log('Doctor profile API error:', error);
        this.isLoading = false;
        this.profileMessage = this.getErrorMessage(error);
      }
    });
  }

  enablePasswordChange(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = false;
    this.isPasswordMode = true;
    this.resetPasswordForm();
  }

  cancelPasswordChange(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = false;

    if (this.hasPasswordChanges()) {
      this.isDiscardPasswordModalOpen = true;
      return;
    }

    this.discardPasswordChanges();
  }

  closeDiscardPasswordModal(): void {
    this.isDiscardPasswordModalOpen = false;
  }

  confirmDiscardPasswordChanges(): void {
    this.isDiscardPasswordModalOpen = false;
    this.discardPasswordChanges();
  }

  changePassword(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = true;

    if (this.isPasswordFormInvalid) {
      this.passwordMessage = 'Please correct the highlighted password fields.';
      return;
    }

    this.isPasswordConfirmModalOpen = true;
  }

  closePasswordConfirmModal(): void {
    this.isPasswordConfirmModalOpen = false;
  }

  confirmPasswordChange(): void {
    this.authService.changePassword({
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword,
      confirmNewPassword: this.passwordForm.confirmPassword
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isPasswordConfirmModalOpen = false;
        this.isPasswordMode = false;
        this.hasPasswordSubmitted = false;

        this.resetPasswordForm();

        this.emitToast('Password changed successfully ✅', 'success');
      },
      error: (error: unknown) => {
        console.log('Doctor password change API error:', error);
        this.isPasswordConfirmModalOpen = false;
        this.passwordMessage = this.getErrorMessage(error);
      }
    });
  }

  getStatusText(isActive: boolean): string {
    return isActive ? 'Active' : 'Inactive';
  }

  private hasPasswordChanges(): boolean {
    return (
      this.passwordForm.currentPassword.trim().length > 0 ||
      this.passwordForm.newPassword.trim().length > 0 ||
      this.passwordForm.confirmPassword.trim().length > 0
    );
  }

  private discardPasswordChanges(): void {
    this.isPasswordMode = false;
    this.resetPasswordForm();
  }

  private resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  private emitToast(message: string, type: ToastType): void {
    this.doctorToast.emit({
      message,
      type
    });
  }

  private getErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          errors?: Record<string, string[]>;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (apiError.error?.message) {
        return apiError.error.message;
      }

      if (apiError.error?.Message) {
        return apiError.error.Message;
      }

      if (apiError.error?.errors) {
        const firstError = Object.values(apiError.error.errors)[0]?.[0];

        if (firstError) {
          return firstError;
        }
      }
    }

    return 'Something went wrong. Please try again.';
  }
}