import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { LoginResponse } from '../login/login-response.model';
import { LoginRequest } from '../login/login-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7066/api/auth';

  constructor(private http: HttpClient) {}

  // LOGIN
  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/login`, data)
      .pipe(
        tap(res => {
          if (res.success) {
            localStorage.setItem('token', res.accessToken);
            localStorage.setItem('role', res.role);
          }
        })
      );
  }

  // REGISTER 
  register(data: {
    email: string;
    password: string;
    confirmPassword: string;
    fullName: string;
    dateOfBirth: string;
    gender: string;
    phoneNumber: string;
    insuranceId?: string;
  }): Observable<any> {

    return this.http.post(`${this.baseUrl}/patientregister`, data);
  }

  // LOGOUT
  logout() {
    localStorage.clear();
  }

  // HELPERS
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }


  changePassword(data: any) {
  return this.http.post('https://localhost:7066/api/Auth/change-password', data);
}
}