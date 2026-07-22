export interface NotificationDto {
  notificationId: number;
  patientId: number | null;
  doctorId: number | null;
  appointmentId: number | null;
  title: string;
  message: string;
  notificationType: string;
  isRead: boolean;
  createdDate: string;
}
