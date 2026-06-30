export interface HealthRecord {
  healthRecordId?: number;
  recordId?: number;
  appointmentId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  specialisation?: string;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes: string;
}