export interface PortalNotification {
  notificationId: number;
  appointmentId: number | null;
  title: string;
  message: string;
  notificationType: string;
  isRead: boolean;
  createdDate: string;
  doctorName?: string | null;
  doctorSpecialisation?: string | null;
}

export interface UnreadNotificationCount {
  unreadCount: number;
}

export interface MarkAllNotificationsResponse {
  message: string;
  updatedCount: number;
}