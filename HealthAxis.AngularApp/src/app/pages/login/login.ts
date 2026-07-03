import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../core/auth-state';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {

  email = signal('');
  password = signal('');
  error = signal('');


  onEmailChange(value: string) {
    this.email.set(value);
    this.error.set('');
  }

  onPasswordChange(value: string) {
    this.password.set(value);
    this.error.set('');
  }


  constructor(private http: HttpClient, private router: Router) { }

  login() {
    this.error.set('');

    const payload = {
      email: this.email(),
      password: this.password()
    };

    this.http.post<any>('https://localhost:7038/api/auth/login', payload)
      .subscribe({
        next: (res: any) => {

          const token = res.accessToken;            
          const isFirstLogin = res.isFirstLogin;    

          if (!token) {
            this.error.set(res.message || "Invalid login ❌");
            return;
          }

          const role = this.getRoleFromToken(token);

          console.log("ROLE:", role);
          console.log("IsFirstLogin:", isFirstLogin);

          authState.set({
            token: token,
            role: role,
            name: '',
            isLoggedIn: true
          });

          this.fetchUserDetails(role);

          // ✅ ✅ DOCTOR FIRST LOGIN FLOW
          if (role?.toLowerCase() === 'doctor' && isFirstLogin) {
            this.router.navigateByUrl('/change-password');
            return;
          }

          // ✅ NORMAL FLOW
          if (role?.toLowerCase() === 'patient') {
            this.router.navigateByUrl('/patientdashboard');
          }
          else if (role?.toLowerCase() === 'doctor') {
            this.router.navigateByUrl('/doctor-dashboard');
          }
        },

        error: () => {
          this.error.set("Login failed ❌");
        }
      });
  }

  getRoleFromToken(token: string): string {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));

      console.log("JWT Payload:", payload);

      return payload["role"]
        || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        || '';
    } catch {
      return '';
    }
  }

  // ✅ FETCH USER NAME FROM BACKEND
  fetchUserDetails(role: string) {

    const token = authState().token;

    const headers = {
      Authorization: `Bearer ${token}`
    };

    if (role?.toLowerCase() === 'patient') {
      this.http.get<any>('https://localhost:7038/api/patients/me', { headers })
        .subscribe(res => {
          authState.update(state => ({
            ...state,
            name: res.patientName
          }));

          console.log("Updated state:", authState());
        });
    }

    else if (role?.toLowerCase() === 'doctor') {
      this.http.get<any>('https://localhost:7038/api/doctors/me', { headers })
        .subscribe(res => {
          authState.update(state => ({
            ...state,
            name: res.doctorName
          }));

          console.log("Updated state:", authState());
        });
    }
  }

  goToAdmin() {
    window.location.href = "https://localhost:7235/login";
  }
}
