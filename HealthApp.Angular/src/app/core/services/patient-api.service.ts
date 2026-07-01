import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import { HealthRecord } from '../models/health-record.model';
import { Patient, UpdatePatientRequest } from '../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientApiService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/patients`;

  constructor(private readonly http: HttpClient) {}

  getPatientById(patientId: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${patientId}`);
  }

  updatePatient(
    patientId: number,
    request: UpdatePatientRequest
  ): Observable<Patient> {
    return this.http.put<Patient>(`${this.apiUrl}/${patientId}`, request);
  }

  getPatientHealthRecords(patientId: number): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.apiUrl}/${patientId}/health-records`
    );
  }
}