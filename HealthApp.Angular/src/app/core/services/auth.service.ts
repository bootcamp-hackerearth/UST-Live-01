import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  AuthResponse,
  ChangePasswordDto,
  ChangePasswordResponse,
  LoginDto,
  RegisterPatientDto,
  RegisterPatientResponse,
  UserRole
} from '../../dtos/auth.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/Auth`;

  private readonly tokenKey = 'healthapp_access_token';
  private readonly expiresAtKey = 'healthapp_token_expires_at';
  private readonly roleKey = 'healthapp_user_role';

  private readonly _accessToken = signal<string | null>(this.getStoredToken());
  private readonly _userRole = signal<UserRole | null>(this.getStoredRole());

  readonly accessToken = this._accessToken.asReadonly();
  readonly userRole = this._userRole.asReadonly();

  readonly isAuthenticated = computed(() => !!this._accessToken() && !this.isTokenExpired());

  constructor(private readonly http: HttpClient) {}

  // --- Public API Methods ---

  login(payload: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, payload).pipe(
      tap(response => this.storeAuthResponse(response))
    );
  }

  registerPatient(payload: RegisterPatientDto): Observable<RegisterPatientResponse> {
    return this.http.post<RegisterPatientResponse>(`${this.apiUrl}/register/patient`, payload);
  }

  changePassword(payload: ChangePasswordDto): Observable<ChangePasswordResponse> {
    return this.http.post<ChangePasswordResponse>(`${this.apiUrl}/change-password`, payload);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expiresAtKey);
    localStorage.removeItem(this.roleKey);

    this._accessToken.set(null);
    this._userRole.set(null);
  }

  getToken(): string | null {
    return this._accessToken();
  }

  getRole(): UserRole | null {
    return this._userRole();
  }

  getBlazorLaunchUrl(): string | null {
  const token = this.getToken();
  if (!token) return null;

  const encodedToken = encodeURIComponent(token);

  return `${API_CONFIG.adminPortalUrl}/login?token=${encodedToken}`;
}

  getRedirectUrlAfterLogin(): string | null {
    const role = this.getRole();

    if (role === 'Doctor') return '/doctor/dashboard';
    if (role === 'Patient') return '/patient/dashboard';

    if (role === 'Admin') {
      return 'ADMIN_EXTERNAL'; 
    }

    return '/login';
  }


  // --- Private Helpers ---

  private storeAuthResponse(response: AuthResponse): void {
    const token = response.accessToken;
    localStorage.setItem(this.tokenKey, token);

    const expiresAt = Date.now() + response.expiresIn * 60 * 1000;
    localStorage.setItem(this.expiresAtKey, expiresAt.toString());

    const role = this.extractRoleFromToken(token);
    if (role) localStorage.setItem(this.roleKey, role);

    this._accessToken.set(token);
    this._userRole.set(role);
  }

  private getStoredToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private getStoredRole(): UserRole | null {
    const role = localStorage.getItem(this.roleKey);
    return (role === 'Patient' || role === 'Doctor' || role === 'Admin') ? role : null;
  }

  private isTokenExpired(): boolean {
    const expiresAtValue = localStorage.getItem(this.expiresAtKey);
    return !expiresAtValue || Date.now() >= Number(expiresAtValue);
  }

  private extractRoleFromToken(token: string): UserRole | null {
    try {
      const payload = this.decodeJwtPayload(token);
      const roleClaim = payload['role'] || payload['Role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

      if (typeof roleClaim === 'string' && (roleClaim === 'Patient' || roleClaim === 'Doctor' || roleClaim === 'Admin')) {
        return roleClaim;
      }
      if (Array.isArray(roleClaim)) {
        return roleClaim.find(r => r === 'Patient' || r === 'Doctor' || r === 'Admin') ?? null;
      }
      return null;
    } catch {
      return null;
    }
  }

  private decodeJwtPayload(token: string): Record<string, unknown> {
    const parts = token.split('.');
    if (parts.length !== 3) return {};
    const base64 = parts[1].replaceAll('-', '+').replaceAll('_', '/');
    return JSON.parse(atob(base64));
  }
}