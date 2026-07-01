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
  updatedDate?: string | null;
}

export interface CreateHealthRecordRequest {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes: string;
}

export interface UpdateHealthRecordRequest {
  diagnosis: string;
  prescription: string;
  notes: string;
}