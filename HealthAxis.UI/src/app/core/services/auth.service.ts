import {
  computed,
  inject,
  Injectable,
  signal
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  catchError,
  map,
  Observable,
  of,
  tap,
  throwError
} from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  ChangePasswordRequest,
  LoginRequest,
  LoginResponse,
  RefreshTokenRequest,
  RefreshTokenResponse,
  RegisterPatientRequest
} from '../models/auth.model';

export type UserRole =
  | 'Patient'
  | 'Doctor'
  | 'Admin'
  | '';

interface JwtPayload
  extends Record<string, unknown> {
  exp?: number | string;
  email?: unknown;
  role?: unknown;
  roles?: unknown;
}

const ROLE_CLAIM =
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

const EMAIL_CLAIM =
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress';

const MILLISECONDS_PER_MINUTE =
  60 * 1000;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly authUrl =
    `${environment.apiBaseUrl}/Auth`;

  private readonly accessTokenKey =
    'healthaxis_access_token';

  private readonly refreshTokenKey =
    'healthaxis_refresh_token';

  private readonly roleKey =
    'healthaxis_role';

  private readonly expiresAtKey =
    'healthaxis_expires_at';

  private readonly expiresInMinutesKey =
    'healthaxis_expires_in_minutes';

  private readonly tokenSubject =
    new BehaviorSubject<string | null>(
      localStorage.getItem(
        this.accessTokenKey
      )
    );

  private readonly roleSubject =
    new BehaviorSubject<UserRole>(
      this.normalizeRole(
        localStorage.getItem(
          this.roleKey
        ) ?? ''
      )
    );

  private readonly tokenSignal =
    signal<string | null>(
      localStorage.getItem(
        this.accessTokenKey
      )
    );

  readonly token$ =
    this.tokenSubject.asObservable();

  readonly role$ =
    this.roleSubject.asObservable();

  readonly role = signal<UserRole>(
    this.normalizeRole(
      localStorage.getItem(
        this.roleKey
      ) ?? ''
    )
  );

  readonly isLoggedIn$ =
    this.token$.pipe(
      map(
        (token) =>
          Boolean(token) &&
          this.hasRefreshToken()
      )
    );

  readonly isLoggedIn = computed(
    () =>
      Boolean(this.tokenSignal()) &&
      this.hasRefreshToken()
  );

  login(
    data: LoginRequest
  ): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(
        `${this.authUrl}/login`,
        data
      )
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

  registerPatient(
    data: RegisterPatientRequest
  ): Observable<object> {
    return this.http.post<object>(
      `${this.authUrl}/register`,
      data
    );
  }

  refreshToken():
    Observable<RefreshTokenResponse> {
    const accessToken =
      localStorage.getItem(
        this.accessTokenKey
      );

    const refreshToken =
      localStorage.getItem(
        this.refreshTokenKey
      );

    if (!accessToken || !refreshToken) {
      return throwError(
        () =>
          new Error(
            'Refresh token details are not available.'
          )
      );
    }

    const request: RefreshTokenRequest = {
      accessToken,
      refreshToken
    };

    return this.http
      .post<RefreshTokenResponse>(
        `${this.authUrl}/refresh-token`,
        request
      )
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

  ensureAuthenticated():
    Observable<boolean> {
    const accessToken =
      localStorage.getItem(
        this.accessTokenKey
      );

    if (!accessToken) {
      this.clearSession();
      return of(false);
    }

    if (
      !this.isTokenExpired(accessToken)
    ) {
      return of(true);
    }

    if (!this.hasRefreshToken()) {
      this.clearSession();
      return of(false);
    }

    return this.refreshToken().pipe(
      map(() => true),
      catchError(() => {
        this.clearSession();
        return of(false);
      })
    );
  }

  changePassword(
    data: ChangePasswordRequest
  ): Observable<object> {
    return this.http.post<object>(
      `${this.authUrl}/change-password`,
      data
    );
  }

  logout(): void {
    this.clearAuthData();
    void this.router.navigate(['/login']);
  }

  clearSession(): void {
    this.clearAuthData();
  }

  getToken(): string | null {
    return this.tokenSignal();
  }

  getRole(): UserRole {
    return this.role();
  }

  hasValidAccessToken(): boolean {
    const accessToken =
      localStorage.getItem(
        this.accessTokenKey
      );

    return Boolean(accessToken) &&
      !this.isTokenExpired(
        accessToken ?? ''
      );
  }

  redirectByRole(): void {
    const currentRole = this.role();

    if (currentRole === 'Patient') {
      void this.router.navigate([
        '/patient/dashboard'
      ]);
      return;
    }

    if (currentRole === 'Doctor') {
      void this.router.navigate([
        '/doctor/dashboard'
      ]);
      return;
    }

    if (currentRole === 'Admin') {
      this.redirectToBlazorAdmin();
      return;
    }

    void this.router.navigate(['/login']);
  }

  private redirectToBlazorAdmin(): void {
    const accessToken =
      localStorage.getItem(
        this.accessTokenKey
      );

    const refreshToken =
      localStorage.getItem(
        this.refreshTokenKey
      );

    const expiresIn =
      localStorage.getItem(
        this.expiresInMinutesKey
      );

    const expiresAt =
      localStorage.getItem(
        this.expiresAtKey
      );

    if (
      !accessToken ||
      !refreshToken ||
      !expiresIn ||
      !expiresAt
    ) {
      this.clearAuthData();

      void this.router.navigate([
        '/login'
      ]);

      return;
    }

    const email =
      this.getEmailFromToken(
        accessToken
      );

    const adminBaseUrl =
      environment.blazorAdminUrl
        .replace(/\/$/, '');

    const fragment =
      new URLSearchParams({
        accessToken,
        refreshToken,
        expiresIn,
        expiresAt,
        role: 'Admin',
        email
      });

    globalThis.location.href =
      `${adminBaseUrl}/external-login#${fragment.toString()}`;
  }

  private storeAuthData(
    accessToken: string,
    refreshToken: string,
    expiresInMinutes: number
  ): void {
    if (!accessToken) {
      throw new Error(
        'Access token was not received from the API.'
      );
    }

    if (!refreshToken) {
      throw new Error(
        'Refresh token was not received from the API.'
      );
    }

    const userRole =
      this.normalizeRole(
        this.getRoleFromToken(
          accessToken
        )
      );

    const expiresAt =
      this.getExpiryDateTime(
        expiresInMinutes
      );

    localStorage.setItem(
      this.accessTokenKey,
      accessToken
    );

    localStorage.setItem(
      this.refreshTokenKey,
      refreshToken
    );

    localStorage.setItem(
      this.roleKey,
      userRole
    );

    localStorage.setItem(
      this.expiresAtKey,
      expiresAt
    );

    localStorage.setItem(
      this.expiresInMinutesKey,
      expiresInMinutes.toString()
    );

    this.tokenSignal.set(accessToken);
    this.role.set(userRole);

    this.tokenSubject.next(accessToken);
    this.roleSubject.next(userRole);
  }

  private clearAuthData(): void {
    localStorage.removeItem(
      this.accessTokenKey
    );

    localStorage.removeItem(
      this.refreshTokenKey
    );

    localStorage.removeItem(
      this.roleKey
    );

    localStorage.removeItem(
      this.expiresAtKey
    );

    localStorage.removeItem(
      this.expiresInMinutesKey
    );

    localStorage.removeItem(
      'healthaxis_token'
    );

    localStorage.removeItem(
      'healthaxis_expires_in_seconds'
    );

    this.tokenSignal.set(null);
    this.role.set('');

    this.tokenSubject.next(null);
    this.roleSubject.next('');
  }

  private hasRefreshToken(): boolean {
    return Boolean(
      localStorage.getItem(
        this.refreshTokenKey
      )
    );
  }

  private getExpiryDateTime(
    expiresInMinutes: number
  ): string {
    const safeMinutes =
      Number.isFinite(expiresInMinutes) &&
      expiresInMinutes > 0
        ? expiresInMinutes
        : 1;

    return new Date(
      Date.now() +
      safeMinutes * MILLISECONDS_PER_MINUTE
    ).toISOString();
  }

  private getRoleFromToken(
    token: string
  ): string {
    const payload =
      this.decodeToken(token);

    const roleClaim =
      payload.role ??
      payload.roles ??
      payload[ROLE_CLAIM];

    if (Array.isArray(roleClaim)) {
      const firstRole =
        roleClaim.find(
          (item): item is string =>
            typeof item === 'string'
        );

      return firstRole ?? '';
    }

    return typeof roleClaim === 'string'
      ? roleClaim
      : '';
  }

  private getEmailFromToken(
    token: string
  ): string {
    const payload =
      this.decodeToken(token);

    const emailClaim =
      payload.email ??
      payload[EMAIL_CLAIM];

    return typeof emailClaim === 'string'
      ? emailClaim
      : '';
  }

  private decodeToken(
    token: string
  ): JwtPayload {
    try {
      const payloadPart =
        token.split('.')[1];

      if (!payloadPart) {
        return {};
      }

      const base64 =
        payloadPart
          .replaceAll('-', '+')
          .replaceAll('_', '/');

      const paddedBase64 =
        base64.padEnd(
          Math.ceil(base64.length / 4) * 4,
          '='
        );

      const parsedPayload: unknown =
        JSON.parse(
          atob(paddedBase64)
        );

      return this.isRecord(
        parsedPayload
      )
        ? parsedPayload
        : {};
    } catch {
      return {};
    }
  }

  private isRecord(
    value: unknown
  ): value is JwtPayload {
    return (
      typeof value === 'object' &&
      value !== null
    );
  }

  private normalizeRole(
    role: string
  ): UserRole {
    switch (
      role.trim().toLowerCase()
    ) {
      case 'patient':
        return 'Patient';

      case 'doctor':
        return 'Doctor';

      case 'admin':
        return 'Admin';

      default:
        return '';
    }
  }

  private isTokenExpired(
    token: string
  ): boolean {
    if (!token) {
      return true;
    }

    const expiryTimes: number[] = [];

    const expiresAt =
      localStorage.getItem(
        this.expiresAtKey
      );

    if (expiresAt) {
      const storedExpiryTime =
        new Date(expiresAt).getTime();

      if (Number.isFinite(storedExpiryTime)) {
        expiryTimes.push(storedExpiryTime);
      }
    }

    const expiryValue =
      this.decodeToken(token).exp;

    if (expiryValue !== undefined) {
      const expirySeconds =
        typeof expiryValue === 'string'
          ? Number(expiryValue)
          : expiryValue;

      if (!Number.isFinite(expirySeconds)) {
        return true;
      }

      expiryTimes.push(
        expirySeconds * 1000
      );
    }

    if (expiryTimes.length === 0) {
      return false;
    }

    const currentTime = Date.now();

    return expiryTimes.some(
      (expiryTime) =>
        currentTime >= expiryTime
    );
  }

}