import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

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

  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]]
  });

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
          this.getFriendlyMessage(error, 'Invalid email or password.')
        );
      }
    });
  }

  private getFriendlyMessage(error: unknown, fallback: string): string {
    const possibleError = error as { friendlyMessage?: unknown };

    if (typeof possibleError.friendlyMessage === 'string') {
      return possibleError.friendlyMessage;
    }

    return fallback;
  }
}