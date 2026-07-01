export type AppointmentStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Cancelled'
  | 'Completed';

export interface Appointment {
  appointmentId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  scheduledDate: string;
  timeSlot: string;
  status: AppointmentStatus;
  cancellationReason?: string;
}

export interface BookAppointmentRequest {
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface UpdateAppointmentStatusRequest {
  status: AppointmentStatus;
  cancellationReason?: string;
}