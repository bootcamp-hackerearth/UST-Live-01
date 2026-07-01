import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private API = 'https://localhost:7054/api/Auth';

  // Blazor Admin Portal URL
  private ADMIN_PORTAL_URL = 'https://localhost:7107';

  constructor(private http: HttpClient) {}

  // REGISTER
  register(data: any) {
    return this.http.post<any>(`${this.API}/register`, data);
  }

  // LOGIN
  login(data: any) {
    return this.http.post<any>(`${this.API}/login`, data);
  }

  // CHANGE PASSWORD
  // Backend returns plain text, so responseType should be text
  changePassword(data: any) {
    return this.http.post(
      `${this.API}/change-password`,
      data,
      { responseType: 'text' }
    );
  }

  // SAVE LOGIN RESPONSE
  saveTokens(res: any) {
    localStorage.setItem('accessToken', res.token || '');
    localStorage.setItem('refreshToken', res.refreshToken || '');

    localStorage.setItem('email', res.email || '');
    localStorage.setItem('role', res.role || '');
    localStorage.setItem('referenceId', String(res.referenceId || ''));
    localStorage.setItem('isFirstLogin', String(res.isFirstLogin || false));
  }

  // ACCESS TOKEN
  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  // REFRESH TOKEN
  getRefreshToken(): string | null {
    return localStorage.getItem('refreshToken');
  }

  // EMAIL
  getEmail(): string {
    return localStorage.getItem('email') || '';
  }

  // ROLE
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

  // REFERENCE ID
  // Patient login => PatientId
  // Doctor login  => DoctorId
  // Admin login   => Admin/User reference if backend sends it
  getReferenceId(): string {
    return localStorage.getItem('referenceId') || '';
  }

  // IDENTITY USER ID FROM JWT
  // This is NOT PatientId/DoctorId.
  // This is ASP.NET Identity user id.
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

  // FIRST LOGIN FLAG
  isFirstLogin(): boolean {
    return localStorage.getItem('isFirstLogin') === 'true';
  }

  // LOGGED IN CHECK
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

  // TOKEN EXPIRY CHECK
  isTokenExpired(token: string): boolean {
    const payload = this.decodeToken(token);

    if (!payload || !payload.exp) {
      return false;
    }

    const expiryTime = payload.exp * 1000;
    const currentTime = Date.now();

    return currentTime >= expiryTime;
  }

  // ADMIN PORTAL BRIDGE URL
  // This sends the Angular login token to Blazor Admin Portal.
  // Blazor will store this token in its own localStorage and redirect to /admin/dashboard.
  getAdminPortalBridgeUrl(): string {
    const params = new URLSearchParams();

    params.set('token', this.getToken() || '');
    params.set('email', this.getEmail() || '');
    params.set('role', this.getRole() || '');
    params.set('referenceId', this.getReferenceId() || '');
    params.set('isFirstLogin', String(this.isFirstLogin()));

    return `${this.ADMIN_PORTAL_URL}/auth-bridge?${params.toString()}`;
  }

  // LOGOUT
  logout() {
    localStorage.clear();
  }

  // DECODE JWT
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