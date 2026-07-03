import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthResponse } from '../models/auth-response';
import { LoginRequest } from '../models/login-request';
import { ChangePasswordRequest } from '../models/change-password-request';
import { RegisterPatientRequest } from '../models/register-patient-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiBaseUrl = 'https://localhost:7224/api';

  constructor(private readonly http: HttpClient) {}

  registerPatient(request: RegisterPatientRequest): Observable<unknown> {
    return this.http.post(`${this.apiBaseUrl}/auth/register`, request);
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiBaseUrl}/auth/login`,
      request
    );
  }

  changePassword(request: ChangePasswordRequest): Observable<unknown> {
    return this.http.put(
      `${this.apiBaseUrl}/auth/change-password`,
      request
    );
  }
}