export interface Notification {
  notificationId: string;
  userId: string;
  title: string;
  message: string;
  eventType: string;
  isRead: boolean;
  createdAt: string | Date;
}