import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { TokenService } from '../../core/services/token.service';

@Component({
  selector: 'app-doctor-change-password-required',
  imports: [
    FormsModule
  ],
  templateUrl: './doctor-change-password-required.html',
  styleUrl: './doctor-change-password-required.css'
})
export class DoctorChangePasswordRequired {
  changingPassword = false;

  errorMessage = '';
  successMessage = '';

  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  form = {
    currentPassword: '',
    newPassword: '',
    confirmNewPassword: ''
  };

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  get hasMinLength(): boolean {
    return this.form.newPassword.length >= 8;
  }

  get hasUppercase(): boolean {
    return /[A-Z]/.test(this.form.newPassword);
  }

  get hasLowercase(): boolean {
    return /[a-z]/.test(this.form.newPassword);
  }

  get hasNumber(): boolean {
    return /[0-9]/.test(this.form.newPassword);
  }

  get hasSpecialCharacter(): boolean {
    return /[^A-Za-z0-9]/.test(this.form.newPassword);
  }

  get isNewPasswordStrong(): boolean {
    return (
      this.hasMinLength &&
      this.hasUppercase &&
      this.hasLowercase &&
      this.hasNumber &&
      this.hasSpecialCharacter
    );
  }

  get isNewPasswordSameAsCurrent(): boolean {
    return (
      !!this.form.currentPassword &&
      !!this.form.newPassword &&
      this.form.currentPassword === this.form.newPassword
    );
  }

  get passwordStrengthScore(): number {
    let score = 0;

    if (this.hasMinLength) {
      score++;
    }

    if (this.hasUppercase) {
      score++;
    }

    if (this.hasLowercase) {
      score++;
    }

    if (this.hasNumber) {
      score++;
    }

    if (this.hasSpecialCharacter) {
      score++;
    }

    return score;
  }

  get passwordStrengthLabel(): string {
    if (!this.form.newPassword) {
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
    if (!this.form.newPassword) {
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
    return this.form.newPassword === this.form.confirmNewPassword;
  }

  get canSubmit(): boolean {
    return (
      !!this.form.currentPassword &&
      !!this.form.newPassword &&
      !!this.form.confirmNewPassword &&
      this.isNewPasswordStrong &&
      !this.isNewPasswordSameAsCurrent &&
      this.passwordsMatch
    );
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.form.currentPassword) {
      this.errorMessage = 'Current password is required.';
      return;
    }

    if (!this.form.newPassword) {
      this.errorMessage = 'New password is required.';
      return;
    }

    if (!this.form.confirmNewPassword) {
      this.errorMessage = 'Confirm password is required.';
      return;
    }

    if (this.isNewPasswordSameAsCurrent) {
      this.errorMessage = 'New password cannot be the same as current password.';
      return;
    }

    if (!this.passwordsMatch) {
      this.errorMessage = 'New password and confirm password do not match.';
      return;
    }

    if (!this.isNewPasswordStrong) {
      this.errorMessage =
        'Password must be at least 8 characters and include uppercase, lowercase, number, and special character.';
      return;
    }

    this.changingPassword = true;

    this.authService.changePassword({
      currentPassword: this.form.currentPassword,
      newPassword: this.form.newPassword,
      confirmNewPassword: this.form.confirmNewPassword
    }).subscribe({
      next: () => {
        this.changingPassword = false;
        this.successMessage = 'Password changed successfully. Redirecting...';

        this.tokenService.setMustChangePassword(false);

        setTimeout(() => {
          this.router.navigate(['/doctor/dashboard']);
        }, 900);
      },
      error: (error) => {
        this.changingPassword = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'Session expired. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage =
            'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not change password. Please try again.';
      }
    });
  }

  logout(): void {
    this.tokenService.clearAuthData();
    this.router.navigate(['/login']);
  }
}