import { Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize, timeout } from 'rxjs';

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

  readonly email = signal('');
  readonly password = signal('');

  readonly isLoading = signal(false);
  readonly submitAttempted = signal(false);
  readonly errorMessage = signal('');

  readonly emailError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    const value = this.email().trim();

    if (!value) {
      return 'Email is required.';
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

    if (!emailPattern.test(value)) {
      return 'Enter a valid email address, for example user@example.com.';
    }

    return '';
  });

  readonly passwordError = computed(() => {
    if (!this.submitAttempted()) {
      return '';
    }

    if (!this.password().trim()) {
      return 'Password is required.';
    }

    return '';
  });

  readonly hasValidationErrors = computed(() =>
    !!this.emailError() || !!this.passwordError()
  );

  login(): void {
    this.submitAttempted.set(true);
    this.errorMessage.set('');

    if (this.hasValidationErrors()) {
      return;
    }

    this.isLoading.set(true);

    this.authService
      .login({
        email: this.email().trim(),
        password: this.password()
      })
      .pipe(
        timeout({ first: 15000 }),
        finalize(() => this.isLoading.set(false))
      )
      .subscribe({
        next: response => this.handleLoginSuccess(response),
        error: error => this.handleLoginError(error)
      });
  }

  updateEmail(value: string): void {
    this.email.set(value);
    this.errorMessage.set('');
  }

  updatePassword(value: string): void {
    this.password.set(value);
    this.errorMessage.set('');
  }

  private handleLoginSuccess(response: AuthResponse): void {
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

    this.errorMessage.set('Invalid user role.');
  }

  private handleLoginError(error: unknown): void {
    this.errorMessage.set(this.authService.getErrorMessage(error));
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