import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { Login } from '../models/login.model';
import { API_ENDPOINTS } from '../config/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  // Login
  login(data: Login): Observable<any> {
    return this.http.post<any>(API_ENDPOINTS.LOGIN, data);
  }

  // Register
  register(data: any): Observable<any> {
    return this.http.post<any>(API_ENDPOINTS.REGISTER_PATIENT, data);
  }

  // Save JWT
  saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  // Get JWT
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  // Check login status
  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // Decode token
  private decodeToken(): any | null {

    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {
      return JSON.parse(atob(token.split('.')[1]));
    }
    catch {
      return null;
    }
  }

  // Get logged-in user's role
  getRole(): string | null {

    const payload = this.decodeToken();

    if (!payload) {
      return null;
    }

    return payload[
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ] || null;
  }

refreshToken(): Observable<{ token: string }> {
  const refreshToken = localStorage.getItem('refreshToken');

  return this.http.post<{ token: string }>(
    '/api/refresh',
    { refreshToken }
  );
}


logout() {
  localStorage.clear();
  // redirect to login
}
}