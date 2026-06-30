import { computed, Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ChangePasswordRequest,
  LoginRequest,
  LoginResponse,
  RefreshTokenRequest,
  RefreshTokenResponse,
  RegisterPatientRequest
} from '../models/auth.model';

type UserRole = 'Patient' | 'Doctor' | 'Admin' | '';

interface JwtPayload extends Record<string, unknown> {
  exp?: number | string;
  role?: unknown;
  roles?: unknown;
}

const ROLE_CLAIM =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly authUrl = `${environment.apiBaseUrl}/Auth`;

  private readonly accessTokenKey = 'healthaxis_access_token';
  private readonly refreshTokenKey = 'healthaxis_refresh_token';
  private readonly roleKey = 'healthaxis_role';
  private readonly expiresAtKey = 'healthaxis_expires_at';
  private readonly expiresInMinutesKey = 'healthaxis_expires_in_minutes';

  private readonly oldTokenKey = 'healthaxis_token';
  private readonly oldExpiresInSecondsKey = 'healthaxis_expires_in_seconds';

  private readonly tokenSignal = signal<string | null>(
    localStorage.getItem(this.accessTokenKey)
  );

  readonly role = signal<UserRole>(
    this.normalizeRole(localStorage.getItem(this.roleKey) ?? '')
  );

  readonly isLoggedIn = computed(() => {
    const token = this.tokenSignal();

    if (!token) {
      return false;
    }

    return !this.isTokenExpired(token);
  });

  login(data: LoginRequest) {
    return this.http.post<LoginResponse>(`${this.authUrl}/login`, data).pipe(
      tap((response) => {
        this.storeAuthData(
          response.accessToken,
          response.refreshToken,
          response.expiresIn
        );
      })
    );
  }

  registerPatient(data: RegisterPatientRequest) {
    return this.http.post(`${this.authUrl}/register`, data);
  }

  refreshToken() {
    const accessToken = localStorage.getItem(this.accessTokenKey);
    const refreshToken = localStorage.getItem(this.refreshTokenKey);

    if (!accessToken || !refreshToken) {
      throw new Error('Refresh token details not available.');
    }

    const request: RefreshTokenRequest = {
      accessToken,
      refreshToken
    };

    return this.http
      .post<RefreshTokenResponse>(`${this.authUrl}/refresh-token`, request)
      .pipe(
        tap((response) => {
          this.storeAuthData(
            response.accessToken,
            response.refreshToken,
            response.expiresIn
          );
        })
      );
  }

  changePassword(data: ChangePasswordRequest) {
    return this.http.post(`${this.authUrl}/change-password`, data);
  }

  logout(): void {
    this.clearAuthData();
    void this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  getRole(): UserRole {
    return this.role();
  }

  redirectByRole(): void {
    const currentRole = this.role();

    if (currentRole === 'Patient') {
      void this.router.navigate(['/patient/dashboard']);
      return;
    }

    if (currentRole === 'Doctor') {
      void this.router.navigate(['/doctor/dashboard']);
      return;
    }

    if (currentRole === 'Admin') {
      window.location.href = environment.blazorAdminUrl;
      return;
    }

    void this.router.navigate(['/login']);
  }

  private storeAuthData(
    accessToken: string,
    refreshToken: string,
    expiresInMinutes: number
  ): void {
    this.removeOldStorageKeys();

    if (!accessToken) {
      throw new Error('Access token not received from API.');
    }

    const role = this.normalizeRole(this.getRoleFromToken(accessToken));
    const expiresAt = this.getExpiryDateTime(expiresInMinutes);

    localStorage.setItem(this.accessTokenKey, accessToken);
    localStorage.setItem(this.refreshTokenKey, refreshToken);
    localStorage.setItem(this.roleKey, role);
    localStorage.setItem(this.expiresAtKey, expiresAt);
    localStorage.setItem(this.expiresInMinutesKey, expiresInMinutes.toString());

    this.tokenSignal.set(accessToken);
    this.role.set(role);
  }

  private clearAuthData(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.expiresAtKey);
    localStorage.removeItem(this.expiresInMinutesKey);

    this.removeOldStorageKeys();

    this.tokenSignal.set(null);
    this.role.set('');
  }

  private removeOldStorageKeys(): void {
    localStorage.removeItem(this.oldTokenKey);
    localStorage.removeItem(this.oldExpiresInSecondsKey);
  }

  private getExpiryDateTime(expiresInMinutes: number): string {
    return new Date(Date.now() + expiresInMinutes * 60 * 1000).toISOString();
  }

  private getRoleFromToken(token: string): string {
    const payload = this.decodeToken(token);
    const roleClaim = payload.role ?? payload.roles ?? payload[ROLE_CLAIM];

    if (Array.isArray(roleClaim)) {
      const firstRole = roleClaim.find(
        (item): item is string => typeof item === 'string'
      );

      return firstRole ?? '';
    }

    if (typeof roleClaim === 'string') {
      return roleClaim;
    }

    return '';
  }

  private decodeToken(token: string): JwtPayload {
    try {
      const payloadPart = token.split('.')[1];

      if (!payloadPart) {
        return {};
      }

      const base64 = payloadPart.replace(/-/g, '+').replace(/_/g, '/');
      const parsedPayload: unknown = JSON.parse(atob(base64));

      if (this.isRecord(parsedPayload)) {
        return parsedPayload;
      }

      return {};
    } catch {
      return {};
    }
  }

  private isRecord(value: unknown): value is JwtPayload {
    return typeof value === 'object' && value !== null;
  }

  private normalizeRole(role: string): UserRole {
    const cleanRole = role.trim().toLowerCase();

    if (cleanRole === 'patient') {
      return 'Patient';
    }

    if (cleanRole === 'doctor') {
      return 'Doctor';
    }

    if (cleanRole === 'admin') {
      return 'Admin';
    }

    return '';
  }

  private isTokenExpired(token: string): boolean {
    const expiresAt = localStorage.getItem(this.expiresAtKey);

    if (expiresAt) {
      return Date.now() > new Date(expiresAt).getTime();
    }

    const payload = this.decodeToken(token);
    const expiryValue = payload.exp;

    if (!expiryValue) {
      return false;
    }

    const expiryTime =
      typeof expiryValue === 'string'
        ? Number(expiryValue) * 1000
        : expiryValue * 1000;

    return Date.now() > expiryTime;
  }
}