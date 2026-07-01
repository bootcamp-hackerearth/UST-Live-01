import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { TokenService } from '../../core/services/token.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private readonly blazorAdminLoginUrl = 'https://localhost:7206/login';

  loading = false;
  errorMessage = '';

  showPassword = false;

  loginRequest = {
    email: '',
    password: ''
  };

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  login(): void {
    this.errorMessage = '';

    if (!this.loginRequest.email.trim()) {
      this.errorMessage = 'Email is required.';
      return;
    }

    if (!this.loginRequest.password.trim()) {
      this.errorMessage = 'Password is required.';
      return;
    }

    this.loading = true;

    this.authService.login({
      email: this.loginRequest.email.trim(),
      password: this.loginRequest.password
    }).subscribe({
      next: (response) => {
        this.loading = false;

        const role = response.role.toLowerCase();

        if (role === 'admin') {
          this.redirectAdminToBlazor(response);
          return;
        }

        this.tokenService.saveAuthData(response);

        if (role === 'doctor' && response.mustChangePassword) {
          this.router.navigate(['/doctor/change-password-required']);
          return;
        }

        if (role === 'doctor') {
          this.router.navigate(['/doctor/dashboard']);
          return;
        }

        if (role === 'patient') {
          this.router.navigate(['/patient/dashboard']);
          return;
        }

        this.router.navigate(['/']);
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'Invalid email or password.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage =
            'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Invalid email or password.';
      }
    });
  }

  private redirectAdminToBlazor(response: any): void {
    this.tokenService.clearAuthData();

    const query = new URLSearchParams({
      accessToken: response.accessToken ?? '',
      refreshToken: response.refreshToken ?? '',
      email: response.email ?? '',
      role: response.role ?? '',
      referenceId: response.referenceId
        ? String(response.referenceId)
        : '',
      mustChangePassword: String(response.mustChangePassword ?? false)
    });

    window.location.href = `${this.blazorAdminLoginUrl}?${query.toString()}`;
  }
}

