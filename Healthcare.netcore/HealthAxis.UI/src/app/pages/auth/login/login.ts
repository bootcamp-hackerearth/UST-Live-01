import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

interface LoginResponse {
  success: boolean;
  message?: string;
  accessToken?: string;
  refreshToken?: string;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  email = '';
  password = '';
  formSubmitted = false;
  isLoggingIn = false;

  private readonly apiBaseUrl =
    '';

  private readonly blazorAdminUrl =
    '/blazor';

  constructor(
    private readonly http: HttpClient,
    private readonly router: Router
  ) {}

  get emailError(): string {
    const value = this.email.trim();

    if (!value && this.formSubmitted) {
      return 'Email is required';
    }

    if (
      value &&
      !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)
    ) {
      return 'Enter a valid email address';
    }

    return '';
  }

  get passwordError(): string {
    const value = this.password.trim();

    if (!value && this.formSubmitted) {
      return 'Password is required';
    }

    return '';
  }

  login(): void {
    this.formSubmitted = true;

    if (
      this.emailError ||
      this.passwordError ||
      this.isLoggingIn
    ) {
      return;
    }

    this.isLoggingIn = true;

    const request = {
      email: this.email.trim(),
      password: this.password
    };

    this.http
      .post<LoginResponse>(
        `${this.apiBaseUrl}/api/auth/login`,
        request
      )
      .subscribe({
        next: (response) => {
          this.isLoggingIn = false;

          if (
            !response.success ||
            !response.accessToken
          ) {
            alert(
              response.message ||
              'Login failed'
            );

            return;
          }

          const accessToken =
            response.accessToken;

          const refreshToken =
            response.refreshToken || '';

          const role =
            this.getRoleFromToken(
              accessToken
            ).toLowerCase();

          if (role === 'admin') {
            this.redirectAdmin(
              accessToken,
              refreshToken
            );

            return;
          }

          if (role === 'patient') {
            this.storeTokens(
              accessToken,
              refreshToken
            );

            void this.router.navigate([
              '/patient/dashboard'
            ]);

            return;
          }

          if (role === 'doctor') {
            this.storeTokens(
              accessToken,
              refreshToken
            );

            void this.router.navigate([
              '/doctor/dashboard'
            ]);

            return;
          }

          alert(
            'Login succeeded, but the user role is missing or invalid.'
          );
        },
        error: (error) => {
          this.isLoggingIn = false;

          const message =
            error?.error?.message ||
            'Invalid email or password';

          alert(message);
        }
      });
  }

  private storeTokens(
    accessToken: string,
    refreshToken: string
  ): void {
    if (globalThis.window === undefined) {
      return;
    }

    globalThis.window.localStorage.setItem(
      'token',
      accessToken
    );

    globalThis.window.localStorage.setItem(
      'refreshToken',
      refreshToken
    );
  }

  private redirectAdmin(
    accessToken: string,
    refreshToken: string
  ): void {
    if (globalThis.window === undefined) {
      return;
    }

    const redirectUrl =
      `${this.blazorAdminUrl}/admin-token-login` +
      `?accessToken=${encodeURIComponent(accessToken)}` +
      `&refreshToken=${encodeURIComponent(refreshToken)}`;

    globalThis.window.location.href =
      redirectUrl;
  }

  private getRoleFromToken(
    token: string
  ): string {
    try {
      const payload = token.split('.')[1];

      if (!payload) {
        return '';
      }

      const normalizedPayload =
        payload
          .replaceAll('-', '+')
          .replaceAll('_', '/')
          .padEnd(
            Math.ceil(payload.length / 4) * 4,
            '='
          );

      const decodedPayload =
        JSON.parse(
          globalThis.atob(normalizedPayload)
        );

      const roleClaim =
        decodedPayload.role ??
        decodedPayload.roles ??
        decodedPayload[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ];

      if (Array.isArray(roleClaim)) {
        return roleClaim[0] || '';
      }

      return roleClaim || '';
    } catch {
      return '';
    }
  }
}