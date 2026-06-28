import { Injectable } from '@angular/core';
import { AuthResponse } from '../models/auth-response';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly accessTokenKey = 'healthaxis_access_token';
  private readonly refreshTokenKey = 'healthaxis_refresh_token';
  private readonly userRoleKey = 'healthaxis_user_role';
  private readonly userEmailKey = 'healthaxis_user_email';
  private readonly userIdKey = 'healthaxis_user_id';
  private readonly referenceIdKey = 'healthaxis_reference_id';

  saveAuthData(response: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    localStorage.setItem(this.userRoleKey, response.role);
    localStorage.setItem(this.userEmailKey, response.email);
    localStorage.setItem(this.userIdKey, response.userId);
    localStorage.setItem(this.referenceIdKey, String(response.referenceId));
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  getUserRole(): string | null {
    return localStorage.getItem(this.userRoleKey);
  }

  getUserEmail(): string | null {
    return localStorage.getItem(this.userEmailKey);
  }

  getUserId(): string | null {
    return localStorage.getItem(this.userIdKey);
  }

  getReferenceId(): string | null {
    return localStorage.getItem(this.referenceIdKey);
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  clearAuthData(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.userRoleKey);
    localStorage.removeItem(this.userEmailKey);
    localStorage.removeItem(this.userIdKey);
    localStorage.removeItem(this.referenceIdKey);
  }
}