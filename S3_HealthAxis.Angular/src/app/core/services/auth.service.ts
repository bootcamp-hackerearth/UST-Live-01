import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  ApiMessageResponse,
  AuthResponse,
  ChangePasswordRequest,
  LoginRequest,
  RegisterPatientRequest
} from '../../shared/models/auth.models';

import { TokenService } from './token.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiBaseUrl = 'https://localhost:7258/api';

  constructor(
    private http: HttpClient,
    private tokenService: TokenService
  ) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiBaseUrl}/auth/login`,
      request
    );
  }

  registerPatient(request: RegisterPatientRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiBaseUrl}/auth/register-patient`,
      request
    );
  }

  changePassword(
    request: ChangePasswordRequest
  ): Observable<ApiMessageResponse> {
    return this.http.put<ApiMessageResponse>(
      `${this.apiBaseUrl}/auth/change-password`,
      request
    );
  }

  logout(): void {
    this.tokenService.clearAuthData();
  }
}
