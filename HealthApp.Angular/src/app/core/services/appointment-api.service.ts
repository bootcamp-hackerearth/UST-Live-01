import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import {
  Appointment,
  BookAppointmentRequest,
  UpdateAppointmentStatusRequest
} from '../models/appointment.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentApiService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/appointments`;

  constructor(private readonly http: HttpClient) {}

  getAppointments(patientId?: number, doctorId?: number): Observable<Appointment[]> {
    let params = new HttpParams();

    if (patientId) {
      params = params.set('patientId', patientId);
    }

    if (doctorId) {
      params = params.set('doctorId', doctorId);
    }

    return this.http.get<Appointment[]>(this.apiUrl, { params });
  }

  getAppointmentById(appointmentId: number): Observable<Appointment> {
    return this.http.get<Appointment>(`${this.apiUrl}/${appointmentId}`);
  }

  bookAppointment(request: BookAppointmentRequest): Observable<Appointment> {
    return this.http.post<Appointment>(this.apiUrl, request);
  }

  updateAppointmentStatus(
    appointmentId: number,
    request: UpdateAppointmentStatusRequest
  ): Observable<Appointment> {
    return this.http.put<Appointment>(
      `${this.apiUrl}/${appointmentId}/status`,
      request
    );
  }

  cancelAppointment(
    appointmentId: number,
    reason: string
  ): Observable<Appointment> {
    const params = new HttpParams().set('reason', reason);

    return this.http.delete<Appointment>(`${this.apiUrl}/${appointmentId}`, {
      params
    });
  }
}