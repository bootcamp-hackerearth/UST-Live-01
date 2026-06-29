export type AppointmentStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Completed'
  | 'Cancelled';

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
  cancellationReason?: string;
}

export interface BookAppointmentDto {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface CancelAppointmentDto {
  appointmentId: number;
  reason: string;
}