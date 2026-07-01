export type AppointmentStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Cancelled'
  | 'Completed';

export interface AppointmentDto {
  appointmentId: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  specialisation: string;
  scheduledDate: string;
  timeSlot: string;
  status: AppointmentStatus;
  cancellationReason: string;
  hasHealthRecord?: boolean;
}

export interface CreateAppointmentRequest {
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface UpdateAppointmentStatusRequest {
  status: AppointmentStatus;
  cancellationReason?: string;
}
