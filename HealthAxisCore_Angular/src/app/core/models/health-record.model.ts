export interface HealthRecordDto {
  healthRecordId: number;
  patientId?: number | null;
  patientName: string;
  doctorId?: number | null;
  doctorName: string;
  appointmentId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}

export interface CreateHealthRecordRequest {
  patientId: number;
  appointmentId: number;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}
