import { HealthRecordDto } from './health-record.dto';

export type AppointmentStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Cancelled'
  | 'Completed';

export interface AppointmentDto {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  patientName: string;
  doctorName: string;
  scheduledDate: string;
  timeSlot: string;
  status: AppointmentStatus;
  cancellationReason?: string | null;
}

export interface AppointmentCreateDto {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface AppointmentBookingDto {
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface CancelAppointmentDto {
  cancellationReason: string;
}

export interface DoctorAppointmentViewDto extends AppointmentDto {
  healthRecord?: HealthRecordDto | null;
}

export interface AppointmentActionResponse {
  message: string;
}