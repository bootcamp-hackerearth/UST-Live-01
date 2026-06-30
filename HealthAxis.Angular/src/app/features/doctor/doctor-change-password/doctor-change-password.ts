import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { ChangePasswordRequest } from '../../../core/models/change-password-request';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-doctor-change-password',
  imports: [FormsModule],
  templateUrl: './doctor-change-password.html',
  styleUrls: ['./doctor-change-password.css']
})
export class DoctorChangePassword {
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  form: ChangePasswordRequest = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  constructor(private authService: AuthService) {}

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.form.currentPassword ||
      !this.form.newPassword ||
      !this.form.confirmPassword
    ) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    if (this.form.newPassword.length < 8) {
      this.errorMessage = 'New password must be at least 8 characters.';
      return;
    }

    if (this.form.newPassword !== this.form.confirmPassword) {
      this.errorMessage =
        'New password and confirm password do not match.';
      return;
    }

    this.isSaving = true;

    this.authService.changePassword(this.form).subscribe({
      next: () => {
        this.successMessage = 'Password changed successfully.';

        this.form = {
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        };
      },

      error: (error: HttpErrorResponse) => {
        console.log('Doctor change password error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to change password. Status: ${error.status}`;
      },

      complete: () => {
        this.isSaving = false;
      }
    });
  }
}