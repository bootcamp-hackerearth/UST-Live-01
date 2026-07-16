import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../service/auth.service';
import { LoginResponse } from '../login/login-response.model';
import { AppPopupComponent } from '../shared/app-popup/app-popup';




export interface LoginRequest {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule, AppPopupComponent],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  email = '';
  password = '';
  popupVisible = false;
  popupTitle = '';
  popupMessage = '';
  popupType: 'success' | 'error' | 'warning' = 'success';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

 

  login() {
  const data = {
    email: this.email,
    password: this.password
  };

  this.authService.login(data).subscribe({
    next: (res: LoginResponse) => {

      if (!res.success) {
        this.showPopup('Login Failed', res.message || 'Unable to sign in. Please try again.', 'error');
        return;
      }

      localStorage.setItem('token', res.accessToken);
      localStorage.setItem('role', res.role);

      switch (res.role) {

        case 'Admin':
          window.location.href = 'https://localhost:7002/';
          break;

        case 'Doctor':
          this.router.navigate(['/doctor-dashboard']);
          break;

        case 'User':
          this.router.navigate(['/patient-dashboard']);
          break;

        default:
          this.router.navigate(['/login']);
      }
    },

    error: (err) => {
      console.error(err);
      this.showPopup('Login Failed', 'Unable to sign in. Please try again.', 'error');
    }
  });
}

  private showPopup(title: string, message: string, type: 'success' | 'error' | 'warning' = 'success') {
    this.popupTitle = title;
    this.popupMessage = message;
    this.popupType = type;
    this.popupVisible = true;
  }

  closePopup() {
    this.popupVisible = false;
    this.popupTitle = '';
    this.popupMessage = '';
  }

}