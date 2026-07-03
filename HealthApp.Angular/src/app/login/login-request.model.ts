import { Component } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../service/auth.service';
import { LoginResponse } from '../login/login-response.model';




export interface LoginRequest {
  email: string;
  password: string;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  email = '';
  password = '';

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
        alert(res.message);
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
      alert('Login failed');
    }
  });
}

}