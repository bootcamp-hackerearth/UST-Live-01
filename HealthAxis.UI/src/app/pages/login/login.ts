import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  private readonly emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: [
      '',
      [
        Validators.required,
        Validators.pattern(this.emailPattern)
      ]
    ],
    password: [
      '',
      [
        Validators.required
      ]
    ]
  });

  constructor() {
    this.handleLoginQueryParams();
  }

  get email() {
    return this.loginForm.controls.email;
  }

  get password() {
    return this.loginForm.controls.password;
  }

  login(): void {
    this.errorMessage.set('');

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      this.errorMessage.set(this.getFormErrorMessage());
      return;
    }

    this.loading.set(true);

    this.authService.login(this.loginForm.getRawValue()).subscribe({
      next: () => {
        this.loading.set(false);
        this.authService.redirectByRole();
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(this.getLoginErrorMessage(error));
      }
    });
  }

  clearError(): void {
    if (!this.loading()) {
      this.errorMessage.set('');
    }
  }

  getEmailErrorMessage(): string {
    if (this.email.hasError('required')) {
      return 'Email address is required.';
    }

    if (this.email.hasError('pattern')) {
      return 'Enter a valid email address.';
    }

    return '';
  }

  private handleLoginQueryParams(): void {
    const queryParams = this.route.snapshot.queryParamMap;

    const isAdminLogout =
      queryParams.get('adminLogout') === 'true';

    const isSessionExpired =
      queryParams.get('sessionExpired') === 'true';

    if (isAdminLogout || isSessionExpired) {
      this.authService.clearSession();
    }

    if (isSessionExpired) {
      this.errorMessage.set(
        'Your session expired. Please login again.'
      );
    }

    if (isAdminLogout || isSessionExpired) {
      void this.router.navigate([], {
        queryParams: {
          adminLogout: null,
          sessionExpired: null
        },
        queryParamsHandling: 'merge',
        replaceUrl: true
      });
    }
  }

  private getFormErrorMessage(): string {
    if (this.email.invalid) {
      return 'Please enter a valid email address.';
    }

    if (this.password.invalid) {
      return 'Please enter your password.';
    }

    return 'Please enter valid login details.';
  }

  private getLoginErrorMessage(error: unknown): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        return 'Unable to connect to server. Please try again.';
      }

      if (error.status === 400 || error.status === 401) {
        return 'Invalid email or password.';
      }

      if (error.status === 403) {
        return 'You are not allowed to access this portal.';
      }
    }

    return 'Login failed. Please try again.';
  }
}