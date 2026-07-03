import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { AppointmentDto } from '../../shared/models/appointment.models';
import { PagedResponse } from '../../shared/models/paged-response.models';

export type AppointmentStatusText =
  | 'Pending'
  | 'Confirmed'
  | 'Cancelled'
  | 'Completed';

export interface BookAppointmentRequest {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface AppointmentQuery {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  status?: AppointmentStatusText | '';
  scheduledDate?: string;
  upcomingOnly?: boolean;
}

export interface CancelAppointmentRequest {
  appointmentId: number;
  reason: string;
}

interface ApiAppointmentDto {
  appointmentId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  scheduledDate: string;
  timeSlot: string;
  status: number | AppointmentStatusText;
  cancellationReason?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AppointmentApiService {
  private readonly apiUrl = 'https://localhost:7250/api/Appointments';

  constructor(private readonly http: HttpClient) {
  }

  bookAppointment(request: BookAppointmentRequest): Observable<AppointmentDto> {
    return this.http
      .post<ApiAppointmentDto>(this.apiUrl, request)
      .pipe(
        map((appointment: ApiAppointmentDto) =>
          this.mapAppointment(appointment)
        )
      );
  }

  getMyAppointments(
    query: AppointmentQuery
  ): Observable<PagedResponse<AppointmentDto>> {
    const params = this.buildAppointmentParams(query);

    return this.http
      .get<PagedResponse<ApiAppointmentDto>>(`${this.apiUrl}/my`, { params })
      .pipe(
        map((response: PagedResponse<ApiAppointmentDto>) => ({
          items: (response.items ?? []).map((appointment: ApiAppointmentDto) =>
            this.mapAppointment(appointment)
          ),
          pageNumber: response.pageNumber,
          pageSize: response.pageSize,
          totalRecords: response.totalRecords,
          totalPages: response.totalPages
        }))
      );
  }

  getMyUpcomingAppointments(): Observable<AppointmentDto[]> {
    return this.http
      .get<ApiAppointmentDto[]>(`${this.apiUrl}/my/upcoming`)
      .pipe(
        map((appointments: ApiAppointmentDto[]) =>
          (appointments ?? []).map((appointment: ApiAppointmentDto) =>
            this.mapAppointment(appointment)
          )
        )
      );
  }

  getMyPendingAppointments(): Observable<AppointmentDto[]> {
    return this.http
      .get<ApiAppointmentDto[]>(`${this.apiUrl}/my/pending`)
      .pipe(
        map((appointments: ApiAppointmentDto[]) =>
          (appointments ?? []).map((appointment: ApiAppointmentDto) =>
            this.mapAppointment(appointment)
          )
        )
      );
  }

  getMyTodayConfirmedAppointments(): Observable<AppointmentDto[]> {
    return this.http
      .get<ApiAppointmentDto[]>(`${this.apiUrl}/my/today-confirmed`)
      .pipe(
        map((appointments: ApiAppointmentDto[]) =>
          (appointments ?? []).map((appointment: ApiAppointmentDto) =>
            this.mapAppointment(appointment)
          )
        )
      );
  }

  confirmAppointment(appointmentId: number): Observable<AppointmentDto> {
    return this.http
      .put<ApiAppointmentDto>(`${this.apiUrl}/${appointmentId}/confirm`, {})
      .pipe(
        map((appointment: ApiAppointmentDto) =>
          this.mapAppointment(appointment)
        )
      );
  }

  completeAppointment(appointmentId: number): Observable<AppointmentDto> {
    return this.http
      .put<ApiAppointmentDto>(`${this.apiUrl}/${appointmentId}/complete`, {})
      .pipe(
        map((appointment: ApiAppointmentDto) =>
          this.mapAppointment(appointment)
        )
      );
  }

  cancelAppointment(request: CancelAppointmentRequest): Observable<AppointmentDto> {
    return this.http
      .put<ApiAppointmentDto>(`${this.apiUrl}/cancel`, request)
      .pipe(
        map((appointment: ApiAppointmentDto) =>
          this.mapAppointment(appointment)
        )
      );
  }

  private buildAppointmentParams(query: AppointmentQuery): HttpParams {
    let params = new HttpParams()
      .set('pageNumber', String(query.pageNumber ?? 1))
      .set('pageSize', String(query.pageSize ?? 10));

    if (query.searchTerm?.trim()) {
      params = params.set('searchTerm', query.searchTerm.trim());
    }

    if (query.status) {
      params = params.set('status', query.status);
    }

    if (query.scheduledDate) {
      params = params.set('scheduledDate', query.scheduledDate);
    }

    if (query.upcomingOnly !== undefined) {
      params = params.set('upcomingOnly', String(query.upcomingOnly));
    }

    return params;
  }

  private mapAppointment(appointment: ApiAppointmentDto): AppointmentDto {
    return {
      appointmentId: appointment.appointmentId,
      patientId: appointment.patientId,
      patientName: appointment.patientName ?? 'Patient',
      doctorId: appointment.doctorId,
      doctorName: appointment.doctorName ?? 'Doctor',
      specialisation: 'Consultation',
      scheduledDate: this.normalizeDate(appointment.scheduledDate),
      timeSlot: appointment.timeSlot,
      status: this.mapStatus(appointment.status),
      cancellationReason: appointment.cancellationReason ?? ''
    } as AppointmentDto;
  }

  private mapStatus(status: number | AppointmentStatusText): AppointmentStatusText {
    if (typeof status === 'string') {
      return status;
    }

    switch (status) {
      case 0:
        return 'Pending';

      case 1:
        return 'Confirmed';

      case 2:
        return 'Cancelled';

      case 3:
        return 'Completed';

      default:
        return 'Pending';
    }
  }

  private normalizeDate(dateValue: string): string {
    if (!dateValue) {
      return '';
    }
    

    return dateValue.split('T')[0];
  }
}