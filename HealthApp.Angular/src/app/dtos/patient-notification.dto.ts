export interface PatientNotificationDto {
  notificationId: number;
  notificationType: string;
  title: string;
  message: string;
  relatedEntityId: number | null;
  relatedEntityType: string | null;
  isRead: boolean;
  createdAt: string;
}
