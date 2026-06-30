import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import {
  Appointment,
  CreateAppointmentRequest,
  UpdateAppointmentStatusRequest
} from '../models/appointment.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly http = inject(HttpClient);
  private readonly appointmentUrl = `${environment.apiBaseUrl}/appointments`;

  getAllAppointments() {
    return this.http.get<Appointment[]>(this.appointmentUrl);
  }

  createAppointment(data: CreateAppointmentRequest) {
    return this.http.post<Appointment>(this.appointmentUrl, data);
  }

  getMyAppointments() {
    return this.http.get<Appointment[]>(`${this.appointmentUrl}/my`);
  }

  getAppointmentById(id: number) {
    return this.http.get<Appointment>(`${this.appointmentUrl}/${id}`);
  }

  updateAppointmentStatus(
    id: number,
    data: UpdateAppointmentStatusRequest
  ) {
    return this.http.put<Appointment>(
      `${this.appointmentUrl}/${id}/status`,
      data
    );
  }

  deleteAppointment(id: number) {
    return this.http.delete(`${this.appointmentUrl}/${id}`);
  }
}