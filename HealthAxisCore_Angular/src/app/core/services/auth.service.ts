import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import { AuthResponse } from '../models/auth-response';
import { CurrentUser } from '../models/current-user';
import { LoginRequest } from '../models/login-request';
import { RegisterPatientRequest } from '../models/register-patient-request';
import { AdminHandoffService } from './admin-handoff.service';
import { ChangeFirstLoginPasswordRequest } from '../models/change-first-login-password-request';
import { ChangePasswordRequest } from '../models/change-password-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly accessTokenKey = 'healthaxis_access_token';
  private readonly refreshTokenKey = 'healthaxis_refresh_token';
  private readonly currentUserKey = 'healthaxis_user';

  private readonly apiUrl = `${environment.apiBaseUrl}/auth`;

  private readonly currentUserSignal = signal<CurrentUser | null>(
    this.loadCurrentUserFromStorage()
  );

  private readonly accessTokenSignal = signal<string | null>(
    localStorage.getItem(this.accessTokenKey)
  );

  readonly currentUser = this.currentUserSignal.asReadonly();

  readonly accessToken = this.accessTokenSignal.asReadonly();

  readonly isAuthenticated = computed(() =>
    !!this.accessTokenSignal() && !!this.currentUserSignal()
  );

  readonly currentRole = computed(() =>
    this.currentUserSignal()?.role ?? null
  );

  readonly patientId = computed(() =>
    this.currentUserSignal()?.patientId ?? null
  );

  readonly doctorId = computed(() =>
    this.currentUserSignal()?.doctorId ?? null
  );

  constructor(
    private httpClient: HttpClient,
    private router: Router,
    private adminHandoffService : AdminHandoffService
  ) {
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.httpClient
      .post<AuthResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          this.saveSession(response);
          this.redirectByRole(response.role);
        })
      );
  }

  changePassword(request: ChangePasswordRequest): Observable<string> {
    return this.httpClient.post(
      `${this.apiUrl}/change-password`,
      request,
      {
        responseType: 'text'
      }
    );
  }


  registerPatient(request: RegisterPatientRequest): Observable<AuthResponse> {
    return this.httpClient
      .post<AuthResponse>(`${this.apiUrl}/register`, request)
      .pipe(
        tap(response => {
          this.saveSession(response);
          this.redirectByRole(response.role);
        })
      );
  }

  logout(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.currentUserKey);

    this.accessTokenSignal.set(null);
    this.currentUserSignal.set(null);

    this.router.navigate(['/login']);
  }

  getAccessTokenSnapshot(): string | null {
    return this.accessTokenSignal();
  }

  getCurrentUserSnapshot(): CurrentUser | null {
    return this.currentUserSignal();
  }

  isInRole(expectedRoles: string[]): boolean {
    const role = this.currentRole();

    if (!role) {
      return false;
    }

    return expectedRoles.includes(role);
  }

  redirectByRole(role: string): void {
    const currentUser = this.currentUserSignal();

    if (role === 'Patient') {
      this.router.navigate(['/patient/dashboard']);
      return;
    }

    if (role === 'Doctor') {
      if (currentUser?.firstLogin) {
        this.router.navigate(['/doctor/change-password']);
        return;
      }

      this.router.navigate(['/doctor/dashboard']);
      return;
    }

    if (role === 'Admin') {
      this.redirectAdminToBlazor();
      return;
    }

    this.router.navigate(['/forbidden']);
  }

  getErrorMessage(error: unknown): string {
    const fallbackMessage = 'Something went wrong. Please try again.';

    if (!error || typeof error !== 'object') {
      return fallbackMessage;
    }

    const httpError = error as {
      error?: {
        message?: string;
        detail?: string;
        title?: string;
      };
      message?: string;
    };

    return httpError.error?.message ||
      httpError.error?.detail ||
      httpError.error?.title ||
      httpError.message ||
      fallbackMessage;
  }

  private saveSession(response: AuthResponse): void {
    const currentUser: CurrentUser = {
      userId: response.userId,
      patientId: response.patientId,
      doctorId: response.doctorId,
      fullName: response.fullName,
      email: response.email,
      role: response.role,
      firstLogin: response.firstLogin
    };

    localStorage.setItem(this.accessTokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    localStorage.setItem(this.currentUserKey, JSON.stringify(currentUser));

    this.accessTokenSignal.set(response.accessToken);
    this.currentUserSignal.set(currentUser);
  }

  private loadCurrentUserFromStorage(): CurrentUser | null {
    const userJson = localStorage.getItem(this.currentUserKey);

    if (!userJson) {
      return null;
    }

    return JSON.parse(userJson) as CurrentUser;
  }
  changeFirstLoginPassword(
    request: ChangeFirstLoginPasswordRequest
  ): Observable<string> {
    return this.httpClient.post(
      `${this.apiUrl}/change-first-login-password`,
      request,
      {
        responseType: 'text'
      }
    ).pipe(
      tap(() => {
        const currentUser = this.currentUserSignal();

        if (!currentUser) {
          return;
        }

        const updatedUser: CurrentUser = {
          ...currentUser,
          firstLogin: false
        };

        localStorage.setItem(
          this.currentUserKey,
          JSON.stringify(updatedUser)
        );

        this.currentUserSignal.set(updatedUser);
      })
    );
  }
  private redirectAdminToBlazor(): void {
    this.adminHandoffService.create().subscribe({
      next: response => {
        const queryParams = new URLSearchParams({
          code: response.code
        });

        window.location.href =
          `${environment.adminAppUrl}?${queryParams.toString()}`;
      },
      error: error => {
        console.error('Admin handoff create failed:', error);
        this.router.navigate(['/forbidden']);
      }
    });
  }
}
