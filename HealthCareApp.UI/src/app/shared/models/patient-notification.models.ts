export interface PatientNotificationDto {
  patientNotificationId: number;
  patientId: number;
  doctorId?: number | null;
  appointmentId?: number | null;
  title: string;
  message: string;
  notificationType: string;
  isRead: boolean;
  createdDateUtc: string;
}

export const PatientNotificationTypes = {
  DoctorLeave: 'DoctorLeave',
  AppointmentCancelled: 'AppointmentCancelled',
  AppointmentRebook: 'AppointmentRebook',
  System: 'System'
} as const;
