export interface HealthRecord {
  healthRecordId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  appointmentId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

export interface AddHealthRecordRequest {
  patientId: number;
  doctorId?: number;
  appointmentId: number;
  diagnosis: string;
  prescription: string;
  notes?: string;
  visitDate: string;
}