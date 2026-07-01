import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  email = '';
  password = '';

  formSubmitted = false;

  private apiBaseUrl = 'https://localhost:7130';
  private blazorAdminUrl = 'https://localhost:7273';

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  get emailError(): string {
    const value = this.email.trim();

    if (!value && this.formSubmitted) {
      return 'Email is required';
    }

    if (value && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      return 'Enter valid email address';
    }

    return '';
  }

  get passwordError(): string {
    const value = this.password.trim();

    if (!value && this.formSubmitted) {
      return 'Password is required';
    }

    return '';
  }

  login() {
    this.formSubmitted = true;

    if (this.emailError || this.passwordError) {
      return;
    }

    const request = {
      email: this.email.trim(),
      password: this.password
    };

    this.http.post<any>(`${this.apiBaseUrl}/api/auth/login`, request)
      .subscribe({
        next: (response) => {
          if (!response.success || !response.accessToken) {
            alert(response.message || 'Login failed');
            return;
          }

          const role = this.getRoleFromToken(response.accessToken);

          if (role.toLowerCase() === 'admin') {
            const redirectUrl =
              `${this.blazorAdminUrl}/admin-token-login` +
              `?accessToken=${encodeURIComponent(response.accessToken)}` +
              `&refreshToken=${encodeURIComponent(response.refreshToken || '')}`;

            window.location.href = redirectUrl;
            return;
          }

          if (role.toLowerCase() === 'patient') {
            localStorage.setItem('token', response.accessToken);
            localStorage.setItem('refreshToken', response.refreshToken || '');
            this.router.navigate(['/patient/dashboard']);
            return;
          }if (role.toLowerCase() === 'patient') {
  localStorage.setItem('token', response.accessToken);
  localStorage.setItem('refreshToken', response.refreshToken || '');

  console.log("TOKEN STORED ✅:", response.accessToken); // ✅ ADD THIS

  setTimeout(() => {
    this.router.navigate(['/patient/dashboard']);  // ✅ DELAY NAVIGATION
  }, 100);

  return;
}


          if (role.toLowerCase() === 'doctor') {
            localStorage.setItem('token', response.accessToken);
            localStorage.setItem('refreshToken', response.refreshToken || '');
            this.router.navigate(['/doctor/dashboard']);
            return;
          }

          alert('Login successful, but role is missing or invalid.');
        },
        error: () => {
          alert('Invalid email or password');
        }
      });
  }

  private getRoleFromToken(token: string): string {
    try {
      const payload = token.split('.')[1];
      const decodedPayload = JSON.parse(atob(payload));

      return decodedPayload.role ||
             decodedPayload.roles ||
             decodedPayload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
             '';
    } catch {
      return '';
    }
  }
}