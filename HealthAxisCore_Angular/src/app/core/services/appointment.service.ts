import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  AppointmentDto,
  CreateAppointmentRequest,
  UpdateAppointmentStatusRequest
} from '../models/appointment.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly apiUrl = `${environment.apiBaseUrl}/appointments`;

  constructor(private readonly httpClient: HttpClient) {
  }

  getAppointments(filters?: {
    patientId?: number;
    doctorId?: number;
    date?: string;
  }): Observable<AppointmentDto[]> {
    let params = new HttpParams();

    if (filters?.patientId) {
      params = params.set('patientId', filters.patientId);
    }

    if (filters?.doctorId) {
      params = params.set('doctorId', filters.doctorId);
    }

    if (filters?.date) {
      params = params.set('date', filters.date);
    }

    return this.httpClient.get<AppointmentDto[]>(
      this.apiUrl,
      {
        params
      }
    );
  }

  create(request: CreateAppointmentRequest): Observable<AppointmentDto> {
    return this.httpClient.post<AppointmentDto>(
      this.apiUrl,
      request
    );
  }

  updateStatus(
    appointmentId: number,
    request: UpdateAppointmentStatusRequest
  ): Observable<AppointmentDto> {
    return this.httpClient.put<AppointmentDto>(
      `${this.apiUrl}/${appointmentId}/status`,
      request
    );
  }
}
