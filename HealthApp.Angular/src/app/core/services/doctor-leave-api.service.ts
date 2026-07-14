import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import {
  CreateDoctorLeaveRequest,
  DoctorLeave,
  DoctorLeaveCreationResult,
  DoctorLeaveImpact,
  DoctorLeaveStatus
} from '../models/doctor-leave.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorLeaveApiService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/doctor-leaves`;

  constructor(private readonly http: HttpClient) {}

  previewMyLeaveImpact(
    request: CreateDoctorLeaveRequest
  ): Observable<DoctorLeaveImpact> {
    return this.http.post<DoctorLeaveImpact>(
      `${this.apiUrl}/me/preview`,
      request
    );
  }

  createMyLeave(
    request: CreateDoctorLeaveRequest
  ): Observable<DoctorLeaveCreationResult> {
    return this.http.post<DoctorLeaveCreationResult>(
      `${this.apiUrl}/me`,
      request
    );
  }

  getMyLeaveHistory(): Observable<DoctorLeave[]> {
    return this.http.get<DoctorLeave[]>(`${this.apiUrl}/me`);
  }

  getDoctorLeaveHistory(doctorId: number): Observable<DoctorLeave[]> {
    return this.http.get<DoctorLeave[]>(
      `${this.apiUrl}/doctor/${doctorId}`
    );
  }

  getDoctorLeaveStatus(
    doctorId: number,
    date: string
  ): Observable<DoctorLeaveStatus> {
    const params = new HttpParams().set('date', date);

    return this.http.get<DoctorLeaveStatus>(
      `${this.apiUrl}/doctor/${doctorId}/status`,
      { params }
    );
  }
}
