export interface CreateAppointmentRequest {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: number;
}

export interface CancelAppointmentRequest {
  cancellationReason: string;
}

