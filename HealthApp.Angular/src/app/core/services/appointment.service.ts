import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  AppointmentActionResponse,
  AppointmentBookingDto,
  AppointmentDto,
  CancelAppointmentDto,
  DoctorAppointmentViewDto
} from '../../dtos/appointment.dto';

@Injectable({
  providedIn: 'root'
})

export class AppointmentService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/appointments`;

  constructor(private readonly http: HttpClient) {}

  getMyAppointments(onlyUpcoming = false): Observable<DoctorAppointmentViewDto[]> {
    return this.http.get<DoctorAppointmentViewDto[]>(`${this.apiUrl}/my`, {
      params: {
        onlyUpcoming
      }
    });
  }

  getMyUpcomingAppointments(): Observable<DoctorAppointmentViewDto[]> {
    return this.http.get<DoctorAppointmentViewDto[]>(
      `${this.apiUrl}/my/upcoming`
    );
  }

  getAppointmentById(id: number): Observable<AppointmentDto> {
    return this.http.get<AppointmentDto>(`${this.apiUrl}/${id}`);
  }

  bookAppointment(payload: AppointmentBookingDto): Observable<AppointmentDto> {
    return this.http.post<AppointmentDto>(this.apiUrl, payload);
  }

  confirmAppointment(id: number): Observable<AppointmentActionResponse> {
    return this.http.post<AppointmentActionResponse>(
      `${this.apiUrl}/${id}/confirm`,
      null
    );
  }

  cancelAppointment(
    id: number,
    payload: CancelAppointmentDto
  ): Observable<AppointmentActionResponse> {
    return this.http.post<AppointmentActionResponse>(
      `${this.apiUrl}/${id}/cancel`,
      payload
    );
  }

  completeAppointment(id: number): Observable<AppointmentActionResponse> {
    return this.http.post<AppointmentActionResponse>(
      `${this.apiUrl}/${id}/complete`,
      null
    );
  }
}