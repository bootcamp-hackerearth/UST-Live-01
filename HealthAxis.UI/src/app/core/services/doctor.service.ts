import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Doctor } from '../models/doctor.model';
import { Patient } from '../models/patient.model';
import { environment } from '../../../environments/environment';

export interface DoctorStatusResponse {
  message: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly http = inject(HttpClient);
  private readonly doctorUrl = `${environment.apiBaseUrl}/doctors`;

  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.doctorUrl);
  }

  getMyDoctorProfile(): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.doctorUrl}/me`);
  }

  getDoctorById(id: number): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.doctorUrl}/${id}`);
  }

  getDoctorAvailability(id: number): Observable<{
    doctorId: number;
    fullName: string;
    isActive: boolean;
    message: string;
  }> {
    return this.http.get<{
      doctorId: number;
      fullName: string;
      isActive: boolean;
      message: string;
    }>(`${this.doctorUrl}/${id}/availability`);
  }

  getMyPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(`${this.doctorUrl}/me/patients`);
  }

  getMyPatientById(patientId: number): Observable<Patient> {
    return this.http.get<Patient>(
      `${this.doctorUrl}/me/patients/${patientId}`
    );
  }

  updateMyStatus(isActive: boolean): Observable<DoctorStatusResponse> {
    return this.http.put<DoctorStatusResponse>(
      `${this.doctorUrl}/me/status`,
      { isActive }
    );
  }
}