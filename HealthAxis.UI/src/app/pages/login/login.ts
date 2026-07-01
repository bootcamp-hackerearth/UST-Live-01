import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Login {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

  constructor() {
    const isAdminLogout =
      this.route.snapshot.queryParamMap.get('adminLogout') === 'true';

    if (isAdminLogout) {
      this.authService.clearSession();
    }
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
      this.errorMessage.set('Please enter valid email and password.');
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
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Login failed. Please check your credentials.'
          )
        );
      }
    });
  }
}