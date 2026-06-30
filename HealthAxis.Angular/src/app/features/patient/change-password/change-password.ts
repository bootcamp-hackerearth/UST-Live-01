import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../../core/services/auth.service';
import { ChangePasswordRequest } from '../../../core/models/change-password-request';

@Component({
  selector: 'app-change-password',
  imports: [FormsModule],
  templateUrl: './change-password.html',
  styleUrls: ['./change-password.css']
})
export class ChangePassword {
  isSaving = false;
  errorMessage = '';
  successMessage = '';

  form: ChangePasswordRequest = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  constructor(private authService: AuthService) {}

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.form.currentPassword || !this.form.newPassword || !this.form.confirmPassword) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    if (this.form.newPassword !== this.form.confirmPassword) {
      this.errorMessage = 'New password and confirm password do not match.';
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
