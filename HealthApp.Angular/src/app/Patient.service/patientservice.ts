import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Patient } from '../models/patient/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private baseUrl = 'https://localhost:7066/api/PatientApi';

  constructor(private http: HttpClient) {}

  getMyProfile(): Observable<Patient> {
    return this.http.get<Patient>(`${this.baseUrl}/me`);
  }

  updateMyProfile(patient: Patient): Observable<Patient> {
    return this.http.put<Patient>(`${this.baseUrl}/me`, patient);
  }

}
