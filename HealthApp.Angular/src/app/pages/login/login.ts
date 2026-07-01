import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { API_CONFIG } from '../../core/config/api.config';
import { AuthResponse } from '../../core/models/auth-response';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  isLoading = false;
  errorMessage = '';

  login(): void {
    this.errorMessage = '';

    if (!this.email.trim() || !this.password.trim()) {
      this.errorMessage = 'Please enter email and password.';
      return;
    }

    this.isLoading = true;

    this.authService.login({
      email: this.email.trim(),
      password: this.password
    }).subscribe({
      next: response => this.handleLoginSuccess(response),
      error: error => this.handleLoginError(error)
    });
  }

  private handleLoginSuccess(response: AuthResponse): void {
    this.isLoading = false;

    if (response.mustChangePassword) {
      this.router.navigate(['/change-password']);
      return;
    }

    if (response.role === 'Admin') {
      this.redirectToAdminPortal(response);
      return;
    }

    if (response.role === 'Doctor') {
      this.router.navigate(['/doctor/dashboard']);
      return;
    }

    if (response.role === 'Patient') {
      this.router.navigate(['/patient/dashboard']);
      return;
    }

    this.errorMessage = 'Invalid user role.';
  }

  private handleLoginError(error: unknown): void {
    this.isLoading = false;
    this.errorMessage = this.authService.getErrorMessage(error);
  }

  private redirectToAdminPortal(response: AuthResponse): void {
    const queryParams = new URLSearchParams({
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      userId: response.userId,
      email: response.email,
      role: response.role,
      accessTokenExpiresAt: response.accessTokenExpiresAt
    });

    window.location.href = `${API_CONFIG.adminAuthBridgeUrl}?${queryParams.toString()}`;
  }
}