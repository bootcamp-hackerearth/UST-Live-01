import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Patient } from '../models/patient.model';
import { HealthRecord } from '../models/health-record.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly http = inject(HttpClient);
  private readonly patientUrl = `${environment.apiBaseUrl}/Patient`;

  getMyProfile() {
    return this.http.get<Patient>(`${this.patientUrl}/me`);
  }

  updateMyProfile(data: Patient) {
    return this.http.put<Patient>(`${this.patientUrl}/me`, data);
  }

  getMyHealthRecords() {
    return this.http.get<HealthRecord[]>(`${this.patientUrl}/me/health-records`);
  }

  getPatientById(id: number) {
    return this.http.get<Patient>(`${this.patientUrl}/${id}`);
  }

  updatePatientById(id: number, data: Patient) {
    return this.http.put<Patient>(`${this.patientUrl}/${id}`, data);
  }

  getPatientHealthRecordsById(id: number) {
    return this.http.get<HealthRecord[]>(`${this.patientUrl}/${id}/health-records`);
  }
}