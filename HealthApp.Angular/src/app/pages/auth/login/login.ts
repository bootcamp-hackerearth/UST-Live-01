import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { NotificationService } from '../../../core/services/notification.service';
import { AuthService } from '../../../core/services/auth.service';
import { LoginDto } from '../../../dtos/auth.dto';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  private readonly notificationService = inject(NotificationService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';

  readonly isLoading = signal(false);

  login(): void {
    const email = this.email.trim();
    const password = this.password.trim();

    if (!email || !password) {
      this.notificationService.warning('Please enter email and password.');
      return;
    }

    const payload: LoginDto = {
      email,
      password,
    };

    this.isLoading.set(true);

    this.authService
      .login(payload)
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        })
      )
      .subscribe({
        next: response => {
          this.notificationService.success(response.message || 'Login successful.');

          const redirectUrl = this.authService.getRedirectUrlAfterLogin();

          if (redirectUrl === 'ADMIN_EXTERNAL') {
            const blazorUrl = this.authService.getBlazorLaunchUrl();

            if (!blazorUrl) {
              this.notificationService.error('Unable to launch admin portal.');
              return;
            }

            globalThis.location.href = blazorUrl;
            return;
          }

          if (redirectUrl) {
            this.router.navigateByUrl(redirectUrl);
          }
        },
      });
  }

  goBackHome(): void {
    this.router.navigate(['/']);
  }
}