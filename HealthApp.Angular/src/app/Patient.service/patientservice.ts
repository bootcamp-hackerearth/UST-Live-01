import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Patient } from '../models/patient/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private readonly baseUrl = '/api/PatientApi';

  constructor(private readonly http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token || ''}`,
      'Content-Type': 'application/json'
    });
  }

  getMyProfile(): Observable<Patient> {
    return this.http.get<Patient>(
      `${this.baseUrl}/me`,
      { headers: this.getHeaders() }
    );
  }

  updateMyProfile(data: any): Observable<Patient> {
    return this.http.put<Patient>(
      `${this.baseUrl}/me`,
      data,
      { headers: this.getHeaders() }
    );
  }
}