import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { computed, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import { ApiError } from '../models/api-error';
import { AuthResponse } from '../models/auth-response';
import { ChangePasswordRequest } from '../models/change-password-request';
import { CurrentUser, UserRole } from '../models/current-user';
import { LoginRequest } from '../models/login-request';
import { RegisterPatientRequest } from '../models/register-patient-request';

const ACCESS_TOKEN_KEY = 'healthapp_access_token';
const REFRESH_TOKEN_KEY = 'healthapp_refresh_token';
const CURRENT_USER_KEY = 'healthapp_current_user';
const PASSWORD_CHANGE_EMAIL_KEY = 'healthapp_password_change_email';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/auth`;

  private readonly currentUserSignal = signal<CurrentUser | null>(
    this.readCurrentUserFromStorage()
  );

  readonly currentUser = this.currentUserSignal.asReadonly();

  readonly isAuthenticated = computed(() => {
    const user = this.currentUserSignal();

    return !!user?.accessToken && !this.isAccessTokenExpired();
  });

  readonly currentRole = computed(() => this.currentUserSignal()?.role ?? null);

  constructor(private readonly http: HttpClient) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(`${this.apiUrl}/login`, request)
      .pipe(tap(response => this.handleLoginResponse(response)));
  }

  registerPatient(request: RegisterPatientRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, request);
  }

  changePassword(request: ChangePasswordRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      `${this.apiUrl}/change-password`,
      request
    );
  }

  logout(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    localStorage.removeItem(CURRENT_USER_KEY);
    localStorage.removeItem(PASSWORD_CHANGE_EMAIL_KEY);

    this.currentUserSignal.set(null);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  getPasswordChangeEmail(): string {
    return localStorage.getItem(PASSWORD_CHANGE_EMAIL_KEY) ?? '';
  }

  setPasswordChangeEmail(email: string): void {
    localStorage.setItem(PASSWORD_CHANGE_EMAIL_KEY, email);
  }

  clearPasswordChangeEmail(): void {
    localStorage.removeItem(PASSWORD_CHANGE_EMAIL_KEY);
  }

  getErrorMessage(error: unknown): string {
    if (!(error instanceof HttpErrorResponse)) {
      return 'Something went wrong. Please try again.';
    }

    if (error.status === 0) {
      return 'Unable to connect to API. Please check whether backend is running.';
    }

    const apiError = error.error as unknown;

    if (typeof apiError === 'string') {
      return apiError.trim() || this.getDefaultErrorMessage(error.status);
    }

    if (this.isApiError(apiError)) {
      const message = this.getMessageFromApiError(apiError);

      if (message) {
        return message;
      }
    }

    return this.getDefaultErrorMessage(error.status);
  }

  private handleLoginResponse(response: AuthResponse): void {
    if (response.mustChangePassword) {
      this.logout();
      this.setPasswordChangeEmail(response.email);
      return;
    }

    const currentUser: CurrentUser = {
    userId: response.userId,
    email: response.email,
    role: this.toUserRole(response.role),
    accessToken: response.accessToken,
    refreshToken: response.refreshToken,
    accessTokenExpiresAt: response.accessTokenExpiresAt,
    patientId: response.patientId,
    doctorId: response.doctorId
    };

    localStorage.setItem(ACCESS_TOKEN_KEY, response.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken);
    localStorage.setItem(CURRENT_USER_KEY, JSON.stringify(currentUser));

    this.currentUserSignal.set(currentUser);
  }

  private readCurrentUserFromStorage(): CurrentUser | null {
    const storedUser = localStorage.getItem(CURRENT_USER_KEY);

    if (!storedUser) {
      return null;
    }

    try {
      const user = JSON.parse(storedUser) as CurrentUser;

      if (!user.accessToken || this.isStoredUserExpired(user)) {
        localStorage.removeItem(ACCESS_TOKEN_KEY);
        localStorage.removeItem(REFRESH_TOKEN_KEY);
        localStorage.removeItem(CURRENT_USER_KEY);
        return null;
      }

      return user;
    } catch {
      localStorage.removeItem(ACCESS_TOKEN_KEY);
      localStorage.removeItem(REFRESH_TOKEN_KEY);
      localStorage.removeItem(CURRENT_USER_KEY);
      return null;
    }
  }

  private isAccessTokenExpired(): boolean {
    const user = this.currentUserSignal();

    if (!user) {
      return true;
    }

    return this.isStoredUserExpired(user);
  }

  private isStoredUserExpired(user: CurrentUser): boolean {
    const expiresAt = new Date(user.accessTokenExpiresAt).getTime();

    if (Number.isNaN(expiresAt)) {
      return true;
    }

    return expiresAt <= Date.now();
  }

  private toUserRole(role: string): UserRole {
    if (role === 'Admin' || role === 'Doctor' || role === 'Patient') {
      return role;
    }

    return 'Patient';
  }

  private isApiError(value: unknown): value is ApiError {
    return typeof value === 'object' && value !== null;
  }

  private getMessageFromApiError(apiError: ApiError): string {
    if (apiError.message) {
      return apiError.message;
    }

    if (apiError.detail) {
      return apiError.detail;
    }

    if (apiError.title) {
      return apiError.title;
    }

    if (apiError.errors) {
      const firstValidationMessage = Object.values(apiError.errors)
        .flat()
        .find(message => !!message);

      return firstValidationMessage ?? '';
    }

    return '';
  }

  private getDefaultErrorMessage(status: number): string {
    if (status === 400) {
      return 'Invalid request. Please check the details and try again.';
    }

    if (status === 401) {
      return 'Session expired. Please login again.';
    }

    if (status === 403) {
      return 'You are not allowed to perform this action.';
    }

    if (status === 404) {
      return 'Requested data was not found.';
    }

    if (status === 409) {
      return 'This action conflicts with existing data.';
    }

    return 'Something went wrong. Please try again.';
  }
}