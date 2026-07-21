import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  MarkAllNotificationsResponse,
  PortalNotification,
  UnreadNotificationCount
} from '../models/notification.model';

interface NotificationActionResponse {
  message: string;
}

const NOTIFICATION_API_PATH = '/notifications';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly http = inject(HttpClient);

  private readonly notificationApiUrl =
    `${environment.apiBaseUrl}${NOTIFICATION_API_PATH}`;

  getMyNotifications(): Observable<PortalNotification[]> {
    return this.http.get<PortalNotification[]>(
      `${this.notificationApiUrl}/my`
    );
  }

  getUnreadCount(): Observable<UnreadNotificationCount> {
    return this.http.get<UnreadNotificationCount>(
      `${this.notificationApiUrl}/unread-count`
    );
  }

  markAsRead(
    notificationId: number
  ): Observable<NotificationActionResponse> {
    return this.http.put<NotificationActionResponse>(
      `${this.notificationApiUrl}/${notificationId}/read`,
      {}
    );
  }

  markAllAsRead():
    Observable<MarkAllNotificationsResponse> {
    return this.http.put<MarkAllNotificationsResponse>(
      `${this.notificationApiUrl}/read-all`,
      {}
    );
  }
}