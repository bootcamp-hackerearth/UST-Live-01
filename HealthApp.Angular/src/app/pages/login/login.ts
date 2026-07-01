import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth } from '../../services/mock-auth';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private auth = inject(MockAuth);
  private router = inject(Router);

  email = '';
  password = '';
  isLoading = false;
  errorMessage = '';

  login(): void {
    this.errorMessage = '';

    if (!this.email.trim() || !this.password.trim()) {
      this.errorMessage = 'Please enter email and password.';
      return;
    }

    this.isLoading = true;

    setTimeout(() => {
      const result = this.auth.login({
        email: this.email,
        password: this.password
      });

      this.isLoading = false;

      if (!result.success || !result.user) {
        this.errorMessage = result.message;
        return;
      }

      if (result.user.role === 'Patient') {
        this.router.navigate(['/patient/dashboard']);
        return;
      }

      if (result.user.role === 'Doctor') {
        if (result.user.mustChangePassword) {
          this.router.navigate(['/change-password']);
          return;
        }

        this.router.navigate(['/doctor/dashboard']);
        return;
      }

      if (result.user.role === 'Admin') {
        this.errorMessage = 'Admin redirect to Blazor will be connected later.';
        return;
      }
    }, 700);
  }
}