import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../shared/models/auth.models';

@Component({
  selector: 'app-login',
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginRequest: LoginRequest = {
    email: '',
    password: ''
  };

  loading = false;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login(): void {
    this.errorMessage = '';

    if (!this.loginRequest.email || !this.loginRequest.password) {
      this.errorMessage = 'Please enter both email and password.';
      return;
    }

    this.loading = true;

    this.authService.login(this.loginRequest).subscribe({
      next: (response) => {
        this.loading = false;

        const role = response.role?.toLowerCase();

        if (role === 'doctor') {
          this.router.navigate(['/doctor/dashboard']);
          return;
        }

        if (role === 'patient') {
          this.router.navigate(['/patient/doctors']);
          return;
        }

        this.router.navigate(['/']);
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 401) {
          this.errorMessage = 'Invalid email or password.';
          return;
        }

        this.errorMessage = 'Login failed. Please make sure the API is running and try again.';
      }
    });
  }
}
