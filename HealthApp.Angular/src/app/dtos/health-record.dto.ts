export interface HealthRecordCreateDto {
  patientId: number;
  doctorId: number;
  appointmentId?: number | null;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}

export interface HealthRecordDto {
  recordId: number;
  patientId: number;
  doctorId: number;
  appointmentId?: number | null;
  patientName: string;
  doctorName: string;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}