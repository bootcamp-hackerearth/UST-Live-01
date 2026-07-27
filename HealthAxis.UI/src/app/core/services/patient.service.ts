import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { HealthRecord } from '../models/health-record.model';
import { Patient } from '../models/patient.model';

const PATIENT_API_PATH = '/patients';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly http = inject(HttpClient);

  private readonly patientUrl =
    `${environment.apiBaseUrl}${PATIENT_API_PATH}`;

  getMyProfile(): Observable<Patient> {
    return this.http.get<Patient>(
      `${this.patientUrl}/me`
    );
  }

  updateMyProfile(
    data: Patient
  ): Observable<Patient> {
    return this.http.put<Patient>(
      `${this.patientUrl}/me`,
      data
    );
  }

  getMyHealthRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.patientUrl}/me/health-records`
    );
  }

  getPatientById(
    id: number
  ): Observable<Patient> {
    return this.http.get<Patient>(
      `${this.patientUrl}/${id}`
    );
  }

  updatePatientById(
    id: number,
    data: Patient
  ): Observable<Patient> {
    return this.http.put<Patient>(
      `${this.patientUrl}/${id}`,
      data
    );
  }

  getPatientHealthRecordsById(
    id: number
  ): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.patientUrl}/${id}/health-records`
    );
  }
}