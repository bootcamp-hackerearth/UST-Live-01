import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly API = 'https://localhost:7054/api/Auth';

  private readonly ADMIN_PORTAL_URL = 'https://localhost:7107';

  constructor(private readonly http: HttpClient) {}

  register(data: any) {
    return this.http.post<any>(`${this.API}/register`, data);
  }

  login(data: any) {
    return this.http.post<any>(`${this.API}/login`, data);
  }

  changePassword(data: any) {
    return this.http.post(
      `${this.API}/change-password`,
      data,
      { responseType: 'text' }
    );
  }

  saveTokens(res: any) {
    localStorage.setItem('accessToken', res.token || '');
    localStorage.setItem('refreshToken', res.refreshToken || '');

    localStorage.setItem('email', res.email || '');
    localStorage.setItem('role', res.role || '');
    localStorage.setItem('referenceId', String(res.referenceId || ''));
    localStorage.setItem('isFirstLogin', String(res.isFirstLogin || false));
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  getEmail(): string {
    return localStorage.getItem('email') || '';
  }

  getRole(): string {
    const storedRole = localStorage.getItem('role');

    if (storedRole) {
      return storedRole;
    }

    const token = this.getToken();

    if (!token) {
      return '';
    }

    const payload = this.decodeToken(token);

    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      || payload['role']
      || '';
  }

  getReferenceId(): string {
    return localStorage.getItem('referenceId') || '';
  }

  getUserId(): string {
    const token = this.getToken();

    if (!token) {
      return '';
    }

    const payload = this.decodeToken(token);

    return payload['nameid']
      || payload['sub']
      || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
      || '';
  }

  isFirstLogin(): boolean {
    return localStorage.getItem('isFirstLogin') === 'true';
  }

  isLoggedIn(): boolean {
    const token = this.getToken();

    if (!token) {
      return false;
    }

    if (this.isTokenExpired(token)) {
      this.logout();
      return false;
    }

    return true;
  }

  isTokenExpired(token: string): boolean {
    const payload = this.decodeToken(token);

    if (!payload?.exp) {
      return false;
    }

    const expiryTime = payload.exp * 1000;
    const currentTime = Date.now();

    return currentTime >= expiryTime;
  }

  getAdminPortalBridgeUrl(): string {
    const params = new URLSearchParams();

    params.set('token', this.getToken() || '');
    params.set('email', this.getEmail() || '');
    params.set('role', this.getRole() || '');
    params.set('referenceId', this.getReferenceId() || '');
    params.set('isFirstLogin', String(this.isFirstLogin()));

    return `${this.ADMIN_PORTAL_URL}/auth-bridge?${params.toString()}`;
  }

  logout() {
    localStorage.clear();
  }

  private decodeToken(token: string): any {
    try {
      const tokenParts = token.split('.');

      if (tokenParts.length !== 3) {
        return {};
      }

      return JSON.parse(atob(tokenParts[1]));
    } catch {
      return {};
    }
  }
}