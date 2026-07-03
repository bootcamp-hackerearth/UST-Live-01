import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { LoginRequest } from '../../../core/models/login-request';
import { AuthService } from '../../../core/services/auth.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  isSubmitting = false;
  showPassword = false;
  errorMessage = '';

  loginForm: FormGroup;

  private readonly blazorAdminUrl =
    'https://localhost:7051/auth-callback';

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly tokenService: TokenService,
    private  readonly router: Router
  ) {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/)
        ]
      ],
      password: [
        '',
        Validators.required
      ]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  submitLogin(): void {
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    const formValue = this.loginForm.value;

    const request: LoginRequest = {
      email: formValue.email ?? '',
      password: formValue.password ?? ''
    };

    this.authService.login(request).subscribe({
      next: (response) => {
        this.tokenService.saveAuthData(response);
        this.redirectByRole(response);
      },

      error: (error: HttpErrorResponse) => {
        console.log('Login error:', error);

        this.errorMessage =
          error.error?.message ??
          'Login failed. Please check your email and password.';

        this.isSubmitting = false;
      },

      complete: () => {
        this.isSubmitting = false;
      }
    });
  }

  private redirectByRole(response: any): void {
    const normalizedRole =
      response.role?.toLowerCase();

    if (normalizedRole === 'admin') {
      this.redirectAdminToBlazor(response);
      return;
    }

    if (normalizedRole === 'patient') {
      this.router.navigate(['/patient/dashboard']);
      return;
    }

    if (normalizedRole === 'doctor') {
      this.router.navigate(['/doctor/dashboard']);
      return;
    }

    this.errorMessage = 'Unknown user role. Please contact support.';
    this.tokenService.clearAuthData();
  }

  private redirectAdminToBlazor(response: any): void {
    const payload = {
      accessToken: response.accessToken,
      refreshToken: response.refreshToken,
      role: response.role,
      email: response.email,
      userId: response.userId,
      referenceId: response.referenceId
    };

    const encodedPayload =
      encodeURIComponent(
        btoa(JSON.stringify(payload))
      );

    window.location.href =
      `${this.blazorAdminUrl}#auth=${encodedPayload}`;
  }
}