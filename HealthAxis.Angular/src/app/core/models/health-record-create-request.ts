export interface HealthRecordCreateRequest {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes: string;
}