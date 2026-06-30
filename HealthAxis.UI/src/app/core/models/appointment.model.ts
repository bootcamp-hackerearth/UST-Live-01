export interface Appointment {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  patientName: string;
  doctorName: string;
  specialisation: string;
  scheduledDate: string;
  timeSlot: string;
  status: string;
  cancellationReason?: string | null;
}

export interface CreateAppointmentRequest {
  patientId?: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface UpdateAppointmentStatusRequest {
  status: number;
  cancellationReason?: string | null;
}

export const AppointmentStatusCode = {
  Pending: 1,
  Confirmed: 2,
  Cancelled: 3,
  Completed: 4
} as const;