import { Component, inject } from '@angular/core';
import {
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router,RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { AuthService } from '../../../core/services/auth-service';
import { Login as LoginRequest } from '../../../core/models/login';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private fb = inject(NonNullableFormBuilder);
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  isLoading = false;

  errorMessage = '';

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  login(): void {

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    const loginData: LoginRequest = this.loginForm.getRawValue();

    this.authService.login(loginData).subscribe({

      next: (response) => {

        this.isLoading = false;

        localStorage.setItem('token', response.accessToken);
        localStorage.setItem('role', response.role);
        localStorage.setItem('mustChangePassword', response.mustChangePassword.toString());

        this.toastr.success(response.message);

        if (response.role === 'Doctor') {

          if (response.mustChangePassword) {

            this.router.navigate(['/doctor/change-password']);

          }
          else {

            this.router.navigate(['/doctor']);

          }

        }
        else if (response.role === 'Patient') {

          this.router.navigate(['/patient']);

        }
        else if (response.role === 'Admin') {

          const token = response.accessToken;

          window.location.href =
            `https://localhost:7075/login?token=${encodeURIComponent(token)}`;
        }

      },

      error: (err) => {

        this.isLoading = false;

        this.errorMessage =
          err?.error?.message ?? 'Login failed';

        this.toastr.error(this.errorMessage);

      }

    });

  }

  showPassword = false;

togglePassword(): void {

  this.showPassword = !this.showPassword;

}

}