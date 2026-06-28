import { Injectable } from '@angular/core';
import { AuthResponse } from '../../shared/models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly accessTokenKey = 'healthaxis_access_token';
  private readonly refreshTokenKey = 'healthaxis_refresh_token';
  private readonly emailKey = 'healthaxis_email';
  private readonly roleKey = 'healthaxis_role';

  saveAuthData(auth: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, auth.accessToken);
    localStorage.setItem(this.refreshTokenKey, auth.refreshToken);
    localStorage.setItem(this.emailKey, auth.email);
    localStorage.setItem(this.roleKey, auth.role);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  getEmail(): string | null {
    return localStorage.getItem(this.emailKey);
  }

  getRole(): string | null {
    return localStorage.getItem(this.roleKey);
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  clearAuthData(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.emailKey);
    localStorage.removeItem(this.roleKey);
  }
}