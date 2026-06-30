export interface AppointmentCreateRequest {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}