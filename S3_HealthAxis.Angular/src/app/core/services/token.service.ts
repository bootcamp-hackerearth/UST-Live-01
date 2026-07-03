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
  private readonly referenceIdKey = 'healthaxis_reference_id';
  private readonly credentialResetRequiredKey = 'healthaxis_credential_reset_required';

  saveAuthData(response: AuthResponse): void {
    localStorage.setItem(this.accessTokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    localStorage.setItem(this.emailKey, response.email);
    localStorage.setItem(this.roleKey, response.role);
    localStorage.setItem(this.referenceIdKey, String(response.referenceId));
    localStorage.setItem(
      this.credentialResetRequiredKey,
      String(response.mustChangePassword)
    );
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

  getReferenceId(): number | null {
    const value = localStorage.getItem(this.referenceIdKey);

    if (!value) {
      return null;
    }

    const referenceId = Number(value);

    return Number.isNaN(referenceId) ? null : referenceId;
  }

  setMustChangePassword(value: boolean): void {
    localStorage.setItem(this.credentialResetRequiredKey, String(value));
  }

  getMustChangePassword(): boolean {
    return localStorage.getItem(this.credentialResetRequiredKey) === 'true';
  }

  isLoggedIn(): boolean {
    return !!this.getAccessToken();
  }

  clearAuthData(): void {
    localStorage.removeItem(this.accessTokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.emailKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.referenceIdKey);
    localStorage.removeItem(this.credentialResetRequiredKey);
  }
}

