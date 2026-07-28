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
  Params,
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

const BOOK_APPOINTMENT_ROUTE =
  '/patient/book-appointment';

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
  private readonly formBuilder =
    inject(FormBuilder);

  private readonly authService =
    inject(AuthService);

  private readonly route =
    inject(ActivatedRoute);

  private readonly router =
    inject(Router);

  private bookingReturnUrl = '';

  readonly loading = signal(false);
  readonly errorMessage = signal('');
  readonly showPassword = signal(false);
  readonly flowQueryParams = signal<Params>({});

  readonly loginForm =
    this.formBuilder.nonNullable.group({
      email: [
        '',
        [
          Validators.required,
          Validators.email
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
    this.captureBookingFlow();
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

      this.errorMessage.set(
        this.getFormErrorMessage()
      );

      return;
    }

    this.loading.set(true);

    this.authService
      .login(this.loginForm.getRawValue())
      .subscribe({
        next: () => {
          this.loading.set(false);
          this.redirectAfterLogin();
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            this.getLoginErrorMessage(error)
          );
        }
      });
  }

  togglePasswordVisibility(): void {
    this.showPassword.update(
      (isVisible) => !isVisible
    );
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

    if (this.email.hasError('email')) {
      return 'Enter a valid email address.';
    }

    return '';
  }

  private captureBookingFlow(): void {
    const queryParams =
      this.route.snapshot.queryParamMap;

    const returnUrl =
      queryParams.get('returnUrl');

    if (
      returnUrl !==
      BOOK_APPOINTMENT_ROUTE
    ) {
      return;
    }

    this.bookingReturnUrl =
      BOOK_APPOINTMENT_ROUTE;

    const flowParams: Params = {
      returnUrl:
        BOOK_APPOINTMENT_ROUTE
    };

    const doctorId =
      Number(queryParams.get('doctorId'));

    if (
      Number.isInteger(doctorId) &&
      doctorId > 0
    ) {
      flowParams['doctorId'] =
        doctorId;
    }

    const specialisation =
      queryParams
        .get('specialisation')
        ?.trim();

    if (specialisation) {
      flowParams['specialisation'] =
        specialisation;
    }

    this.flowQueryParams.set(flowParams);
  }

  private redirectAfterLogin(): void {
    if (
      this.authService.getRole() !== 'Patient' ||
      !this.bookingReturnUrl
    ) {
      this.authService.redirectByRole();
      return;
    }

    const flowParams = {
      ...this.flowQueryParams()
    };

    delete flowParams['returnUrl'];

    void this.router.navigate(
      [this.bookingReturnUrl],
      {
        queryParams: flowParams
      }
    );
  }

  private handleLoginQueryParams(): void {
    const queryParams =
      this.route.snapshot.queryParamMap;

    const isAdminLogout =
      queryParams.get('adminLogout') ===
      'true';

    const isSessionExpired =
      queryParams.get('sessionExpired') ===
      'true';

    if (
      isAdminLogout ||
      isSessionExpired
    ) {
      this.authService.clearSession();
    }

    if (isSessionExpired) {
      this.errorMessage.set(
        'Your session expired. Please login again.'
      );
    }

    if (
      isAdminLogout ||
      isSessionExpired
    ) {
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
      return (
        'Please enter a valid email address.'
      );
    }

    if (this.password.invalid) {
      return 'Please enter your password.';
    }

    return (
      'Please enter valid login details.'
    );
  }

  private getLoginErrorMessage(
    error: unknown
  ): string {
    if (error instanceof HttpErrorResponse) {
      if (error.status === 0) {
        return (
          'Unable to connect to the server. Please try again.'
        );
      }

      if (
        error.status === 400 ||
        error.status === 401
      ) {
        return (
          'Incorrect email or password.'
        );
      }

      if (error.status === 403) {
        return (
          'You are not allowed to access this portal.'
        );
      }

      if (error.status >= 500) {
        return (
          'The server is temporarily unavailable. Please try again.'
        );
      }
    }

    return (
      'Unable to complete login. Please try again.'
    );
  }
}