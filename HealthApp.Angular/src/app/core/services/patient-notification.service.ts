import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import { PatientNotificationDto } from '../../dtos/patient-notification.dto';

@Injectable({
  providedIn: 'root'
})
export class PatientNotificationService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/notifications`;

  constructor(private readonly http: HttpClient) {}

  getUnreadDoctorLeaveNotifications(): Observable<PatientNotificationDto[]> {
    return this.http.get<PatientNotificationDto[]>(
      `${this.apiUrl}/my/unread-doctor-leave`
    );
  }

  acknowledgeNotification(notificationId: number): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${notificationId}/acknowledge`,
      {}
    );
  }
}