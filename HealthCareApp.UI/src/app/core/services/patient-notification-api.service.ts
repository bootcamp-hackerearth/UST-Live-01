import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

import { PatientNotificationDto } from '../../shared/models/patient-notification.models';

interface ApiPatientNotificationDto {
  patientNotificationId?: number;
  PatientNotificationId?: number;

  patientId?: number;
  PatientId?: number;

  doctorId?: number | null;
  DoctorId?: number | null;

  appointmentId?: number | null;
  AppointmentId?: number | null;

  title?: string;
  Title?: string;

  message?: string;
  Message?: string;

  notificationType?: string;
  NotificationType?: string;

  isRead?: boolean;
  IsRead?: boolean;

  createdDateUtc?: string;
  CreatedDateUtc?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientNotificationApiService {
  private readonly apiUrl = 'https://localhost:7250/api/PatientNotifications';

  constructor(private readonly http: HttpClient) {
  }

  getMyUnreadNotifications(): Observable<PatientNotificationDto[]> {
    return this.http
      .get<ApiPatientNotificationDto[]>(`${this.apiUrl}/my-unread`)
      .pipe(
        map((notifications: ApiPatientNotificationDto[]) =>
          (notifications ?? []).map((notification: ApiPatientNotificationDto) =>
            this.mapNotification(notification)
          )
        )
      );
  }

  markAsRead(patientNotificationId: number): Observable<PatientNotificationDto> {
    return this.http
      .put<ApiPatientNotificationDto>(
        `${this.apiUrl}/${patientNotificationId}/read`,
        {}
      )
      .pipe(
        map((notification: ApiPatientNotificationDto) =>
          this.mapNotification(notification)
        )
      );
  }

  private mapNotification(
    notification: ApiPatientNotificationDto
  ): PatientNotificationDto {
    return {
      patientNotificationId:
        notification.patientNotificationId ??
        notification.PatientNotificationId ??
        0,

      patientId:
        notification.patientId ??
        notification.PatientId ??
        0,

      doctorId:
        notification.doctorId ??
        notification.DoctorId ??
        null,

      appointmentId:
        notification.appointmentId ??
        notification.AppointmentId ??
        null,

      title:
        notification.title ??
        notification.Title ??
        '',

      message:
        notification.message ??
        notification.Message ??
        '',

      notificationType:
        notification.notificationType ??
        notification.NotificationType ??
        '',

      isRead:
        notification.isRead ??
        notification.IsRead ??
        false,

      createdDateUtc:
        notification.createdDateUtc ??
        notification.CreatedDateUtc ??
        ''
    };
  }
}