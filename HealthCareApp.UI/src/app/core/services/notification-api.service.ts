import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { NotificationDto } from '../../shared/models/notification.models';

interface ApiNotificationDto {
  notificationId?: number;
  NotificationId?: number;

  patientId?: number | null;
  PatientId?: number | null;

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

  createdDate?: string;
  CreatedDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationApiService {
  private readonly apiUrl = '/api/api/Notifications';

  constructor(private readonly http: HttpClient) {
  }

  getMyUnreadNotifications(): Observable<NotificationDto[]> {
    return this.http
      .get<ApiNotificationDto[]>(`${this.apiUrl}/my-unread`)
      .pipe(
        map((notifications: ApiNotificationDto[]) =>
          (notifications ?? []).map((notification: ApiNotificationDto) =>
            this.mapNotification(notification)
          )
        )
      );
  }

  markNotificationAsRead(notificationId: number): Observable<NotificationDto> {
    return this.http
      .put<ApiNotificationDto>(`${this.apiUrl}/${notificationId}/read`, {})
      .pipe(
        map((notification: ApiNotificationDto) =>
          this.mapNotification(notification)
        )
      );
  }

  private mapNotification(notification: ApiNotificationDto): NotificationDto {
    return {
      notificationId:
        notification.notificationId ?? notification.NotificationId ?? 0,

      patientId:
        notification.patientId ?? notification.PatientId ?? null,

      doctorId:
        notification.doctorId ?? notification.DoctorId ?? null,

      appointmentId:
        notification.appointmentId ?? notification.AppointmentId ?? null,

      title:
        notification.title ?? notification.Title ?? 'Notification',

      message:
        notification.message ?? notification.Message ?? '',

      notificationType:
        notification.notificationType ?? notification.NotificationType ?? '',

      isRead:
        notification.isRead ?? notification.IsRead ?? false,

      createdDate:
        notification.createdDate ?? notification.CreatedDate ?? ''
    };
  }
}