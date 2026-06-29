import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import {AuthResponse, LoginRequest, RegisterPatientRequest} from '../../shared/models/auth.models';
import {ApiMessageResponse, ChangePasswordRequest} from '../../shared/models/auth.models';

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
    return this.http
      .post<AuthResponse>(`${this.apiBaseUrl}/auth/login`, request)
      .pipe(
        tap((response) => {
          this.tokenService.saveAuthData(response);
        })
      );
  }

  registerPatient(request: RegisterPatientRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiBaseUrl}/auth/register-patient`, request)
      .pipe(
        tap((response) => {
          this.tokenService.saveAuthData(response);
        })
      );
  }

  changePassword(request: ChangePasswordRequest) {
  return this.http.put<ApiMessageResponse>(
    `${this.apiBaseUrl}/auth/change-password`,
    request
  );
}

  logout(): void {
    this.tokenService.clearAuthData();
  }
}