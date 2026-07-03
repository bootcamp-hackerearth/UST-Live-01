import { Component, signal } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-change-password',
  standalone: false,
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {

  newPassword = signal('');
  confirmPassword = signal('');
  error = signal('');
  success = signal('');

  constructor(private http: HttpClient, private router: Router) { }

  changePassword() {

    this.error.set('');
    this.success.set('');

    if (this.newPassword() !== this.confirmPassword()) {
      this.error.set("Passwords do not match ❌");
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${authState().token}`
    });

    this.http.post<any>(
      'https://localhost:7038/api/auth/change-password',
      {
        newPassword: this.newPassword()
      },
      { headers }
    )
      .subscribe({
        next: () => {

          this.success.set("Password changed successfully ✅");

          authState.set({
            token: '',
            role: '',
            name: '',
            isLoggedIn: false
          });

          setTimeout(() => {
            this.router.navigateByUrl('/login');
          }, 1000);
        },

        error: () => {
          this.error.set("Password change failed ❌");
        }
      });
  }
}
