import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  Appointment,
  CreateAppointmentRequest,
  UpdateAppointmentStatusRequest
} from '../models/appointment.model';

const APPOINTMENT_API_PATH = '/appointments';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly http = inject(HttpClient);

  private readonly appointmentUrl =
    `${environment.apiBaseUrl}${APPOINTMENT_API_PATH}`;

  getAllAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(
      this.appointmentUrl
    );
  }

  createAppointment(
    data: CreateAppointmentRequest
  ): Observable<Appointment> {
    return this.http.post<Appointment>(
      this.appointmentUrl,
      data
    );
  }

  getMyAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(
      `${this.appointmentUrl}/my`
    );
  }

  getMyDoctorAppointments():
    Observable<Appointment[]> {
    return this.http.get<Appointment[]>(
      `${this.appointmentUrl}/doctor/my`
    );
  }

  getAppointmentById(
    id: number
  ): Observable<Appointment> {
    return this.http.get<Appointment>(
      `${this.appointmentUrl}/${id}`
    );
  }

  updateAppointmentStatus(
    id: number,
    data: UpdateAppointmentStatusRequest
  ): Observable<Appointment> {
    return this.http.put<Appointment>(
      `${this.appointmentUrl}/${id}/status`,
      data
    );
  }

  deleteAppointment(
    id: number
  ): Observable<object> {
    return this.http.delete<object>(
      `${this.appointmentUrl}/${id}`
    );
  }
}