import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-change-password',
  imports: [RouterLink, FormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css',
})
export class ChangePassword implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  isSubmitting = false;
  message = '';
  isError = false;

  ngOnInit(): void {
    this.email = this.authService.getPasswordChangeEmail();

    if (!this.email) {
      this.router.navigate(['/login']);
    }
  }

  updatePassword(): void {
    this.message = '';
    this.isError = false;

    if (!this.currentPassword || !this.newPassword || !this.confirmPassword) {
      this.showError('Please fill all password fields.');
      return;
    }

    if (this.newPassword.length < 6) {
      this.showError('New password must be at least 6 characters long.');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.showError('New password and confirm password do not match.');
      return;
    }

    this.isSubmitting = true;

    this.authService.changePassword({
      email: this.email,
      currentPassword: this.currentPassword,
      newPassword: this.newPassword,
      confirmNewPassword: this.confirmPassword
    }).subscribe({
      next: response => this.handlePasswordChangeSuccess(response.message),
      error: error => this.handlePasswordChangeError(error)
    });
  }

  private handlePasswordChangeSuccess(message: string): void {
    this.isSubmitting = false;
    this.message = message;
    this.isError = false;

    this.authService.clearPasswordChangeEmail();

    setTimeout(() => {
      this.router.navigate(['/login']);
    }, 1000);
  }

  private handlePasswordChangeError(error: unknown): void {
    this.isSubmitting = false;
    this.showError(this.authService.getErrorMessage(error));
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }
}