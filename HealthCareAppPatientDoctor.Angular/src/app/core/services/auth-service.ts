import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Login } from '../models/login';
import { AuthResponse } from '../models/auth-response';
import { PatientRegister } from '../models/patient-register';
import { ChangePassword } from '../models/change-password';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(request: Login): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiUrl}/Auth/login`,
      request
    );
  }

  getToken(): string | null {
    return localStorage.getItem('token');   // ✅ Fixed
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
    localStorage.removeItem('mustChangePassword');
   
  }

  register(request: PatientRegister) {

  return this.http.post<any>(
    `${this.apiUrl}/Auth/register-patient`,
    request
  );

}

changePassword(request: ChangePassword) {

  return this.http.post(
    `${this.apiUrl}/Auth/change-password`,
    request
  );

}


isDoctor(): boolean {

  return this.getRole() === 'Doctor';

}


}