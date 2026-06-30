import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';

import {
  AuthResponse,
  ChangePasswordRequest,
  ChangePasswordResponse,
  DecodedToken,
  LoginRequest,
  PatientRegisterRequest,
  PatientRegisterResponse,
  UserRole
} from '../../shared/models/auth.models';

interface ApiAuthResponse {
  accessToken?: string;
  AccessToken?: string;
  message?: string;
  Message?: string;
  expiresIn?: number;
  ExpiresIn?: number;
  mustChangePassword?: boolean;
  MustChangePassword?: boolean;
}

interface JwtPayload {
  sub?: string;
  email?: string;
  exp?: number;
  role?: string | string[];
  roles?: string | string[];
  nameid?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = 'https://localhost:7250/api/Auth';

  private readonly tokenKey = 'healthaxis_token';
  private readonly roleKey = 'healthaxis_role';
  private readonly userIdKey = 'healthaxis_user_id';
  private readonly emailKey = 'healthaxis_email';
  private readonly expiryKey = 'healthaxis_token_expiry';
  private readonly mustChangePasswordKey = 'healthaxis_must_change_password';

  constructor(private http: HttpClient) {
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<ApiAuthResponse>(`${this.apiUrl}/login`, request).pipe(
      map((response: ApiAuthResponse) => this.normalizeAuthResponse(response)),
      tap((response: AuthResponse) => {
        this.saveLogin(response);
      })
    );
  }

  registerPatient(request: PatientRegisterRequest): Observable<PatientRegisterResponse> {
    return this.http.post<PatientRegisterResponse>(
      `${this.apiUrl}/register-patient`,
      request
    );
  }

  changePassword(request: ChangePasswordRequest): Observable<ChangePasswordResponse> {
    return this.http.post<ChangePasswordResponse>(
      `${this.apiUrl}/change-password`,
      request
    );
  }

  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    sessionStorage.removeItem(this.roleKey);
    sessionStorage.removeItem(this.userIdKey);
    sessionStorage.removeItem(this.emailKey);
    sessionStorage.removeItem(this.expiryKey);
    sessionStorage.removeItem(this.mustChangePasswordKey);
  }

  saveLogin(response: AuthResponse): void {
    const decodedToken = this.decodeToken(response.accessToken);

    sessionStorage.setItem(this.tokenKey, response.accessToken);
    sessionStorage.setItem(this.roleKey, decodedToken.role);
    sessionStorage.setItem(this.userIdKey, decodedToken.userId);
    sessionStorage.setItem(this.emailKey, decodedToken.email);
    sessionStorage.setItem(this.expiryKey, decodedToken.expiresAt.toString());
    sessionStorage.setItem(
      this.mustChangePasswordKey,
      String(response.mustChangePassword)
    );
  }

  getToken(): string {
    return sessionStorage.getItem(this.tokenKey) ?? '';
  }

  getRole(): UserRole | '' {
    const role = sessionStorage.getItem(this.roleKey);

    if (role === 'Admin' || role === 'Patient' || role === 'Doctor') {
      return role;
    }

    return '';
  }

  getUserId(): string {
    return sessionStorage.getItem(this.userIdKey) ?? '';
  }

  getEmail(): string {
    return sessionStorage.getItem(this.emailKey) ?? '';
  }

  getMustChangePassword(): boolean {
    return sessionStorage.getItem(this.mustChangePasswordKey) === 'true';
  }

  markPasswordChangeCompleted(): void {
    sessionStorage.setItem(this.mustChangePasswordKey, 'false');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    const expiry = Number(sessionStorage.getItem(this.expiryKey) ?? 0);

    if (!token || !expiry) {
      return false;
    }

    return Date.now() < expiry;
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }

  isPatient(): boolean {
    return this.getRole() === 'Patient';
  }

  isDoctor(): boolean {
    return this.getRole() === 'Doctor';
  }

  private normalizeAuthResponse(response: ApiAuthResponse): AuthResponse {
    return {
      accessToken: response.accessToken ?? response.AccessToken ?? '',
      message: response.message ?? response.Message ?? '',
      expiresIn: response.expiresIn ?? response.ExpiresIn ?? 0,
      mustChangePassword:
        response.mustChangePassword ??
        response.MustChangePassword ??
        false
    };
  }

  private decodeToken(token: string): DecodedToken {
    const payload = this.getJwtPayload(token);

    return {
      userId: this.getUserIdFromPayload(payload),
      email: this.getEmailFromPayload(payload),
      role: this.getRoleFromPayload(payload),
      expiresAt: this.getExpiryFromPayload(payload)
    };
  }

  private getJwtPayload(token: string): JwtPayload {
    const tokenParts = token.split('.');

    if (tokenParts.length !== 3) {
      return {};
    }

    const base64Payload = tokenParts[1]
      .replace(/-/g, '+')
      .replace(/_/g, '/');

    const decodedPayload = atob(base64Payload);

    return JSON.parse(decodedPayload) as JwtPayload;
  }

  private getUserIdFromPayload(payload: JwtPayload): string {
    return (
      payload.nameid ??
      payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] ??
      payload.sub ??
      ''
    );
  }

  private getEmailFromPayload(payload: JwtPayload): string {
    return (
      payload.email ??
      payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ??
      ''
    );
  }

  private getRoleFromPayload(payload: JwtPayload): UserRole | '' {
    const roleClaim =
      payload.role ??
      payload.roles ??
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    const role = Array.isArray(roleClaim) ? roleClaim[0] : roleClaim;

    if (role === 'Admin' || role === 'Patient' || role === 'Doctor') {
      return role;
    }

    return '';
  }

  private getExpiryFromPayload(payload: JwtPayload): number {
    if (!payload.exp) {
      return 0;
    }

    return payload.exp * 1000;
  }
}