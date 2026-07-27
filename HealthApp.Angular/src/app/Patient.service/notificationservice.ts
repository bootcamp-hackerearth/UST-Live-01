import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Observable } from 'rxjs';
import { Notification } from '../models/notification/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private baseUrl = '/api/notifications';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token || ''}`,
      'Content-Type': 'application/json'
    });
  }

  getMyUnreadNotifications(): Observable<Notification[]> {
    return this.http.get<Notification[]>(
      `${this.baseUrl}/my-unread`,
      { headers: this.getHeaders() }
    );
  }

  markAsRead(notificationId: string): Observable<Notification> {
    return this.http.put<Notification>(
      `${this.baseUrl}/${notificationId}/read`,
      {},
      { headers: this.getHeaders() }
    );
  }
}