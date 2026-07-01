import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import {
  Doctor,
  DoctorAvailability,
  SpecialisationType
} from '../models/doctor.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorApiService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/doctors`;

  constructor(private readonly http: HttpClient) {}

  getDoctors(specialisation?: SpecialisationType): Observable<Doctor[]> {
    let params = new HttpParams();

    if (specialisation) {
      params = params.set('specialisation', specialisation);
    }

    return this.http.get<Doctor[]>(this.apiUrl, { params });
  }

  getDoctorById(doctorId: number): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.apiUrl}/${doctorId}`);
  }

  getLoggedInDoctorProfile(): Observable<Doctor> {
    return this.http.get<Doctor>(`${this.apiUrl}/me`);
  }

  getDoctorAvailability(
    doctorId: number,
    date: string
  ): Observable<DoctorAvailability> {
    const params = new HttpParams().set('date', date);

    return this.http.get<DoctorAvailability>(
      `${this.apiUrl}/${doctorId}/availability`,
      { params }
    );
  }
}