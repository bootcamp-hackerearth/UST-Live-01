import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Doctor } from '../models/doctor.model';
import { Patient } from '../models/patient.model';
import { environment } from '../../../environments/environment';

export interface DoctorStatusResponse {
  message: string;
  isActive: boolean;
}

export interface DoctorAvailabilityResponse {
  doctorId: number;
  fullName: string;
  isActive: boolean;
  date: string;
  message: string;
  availableSlots: string[];
}

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly http = inject(HttpClient);
  private readonly doctorUrl =
    `${environment.apiBaseUrl}/doctors`;

  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get<Doctor[]>(this.doctorUrl);
  }

  getMyDoctorProfile(): Observable<Doctor> {
    return this.http.get<Doctor>(
      `${this.doctorUrl}/me`
    );
  }

  getDoctorById(id: number): Observable<Doctor> {
    return this.http.get<Doctor>(
      `${this.doctorUrl}/${id}`
    );
  }

  getDoctorAvailability(
    id: number,
    date: string
  ): Observable<DoctorAvailabilityResponse> {
    const parameters = new HttpParams()
      .set('date', date);

    return this.http.get<DoctorAvailabilityResponse>(
      `${this.doctorUrl}/${id}/availability`,
      {
        params: parameters
      }
    );
  }

  getMyPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(
      `${this.doctorUrl}/me/patients`
    );
  }

  getMyPatientById(
    patientId: number
  ): Observable<Patient> {
    return this.http.get<Patient>(
      `${this.doctorUrl}/me/patients/${patientId}`
    );
  }

  updateMyStatus(
    isActive: boolean
  ): Observable<DoctorStatusResponse> {
    return this.http.put<DoctorStatusResponse>(
      `${this.doctorUrl}/me/status`,
      {
        isActive
      }
    );
  }
}