import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import {
  LoginRequest,
  AuthResponse,
  RegisterPatientRequest
} from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiBaseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) { }
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.apiBaseUrl}/api/auth/login`,
      request
    );
  }

  registerPatient(request: RegisterPatientRequest): Observable<string> {
    return this.http.post(
      `${this.apiBaseUrl}/api/auth/register/patient`,
      request,
      {
        responseType: 'text'
      }
    );
  }
}
