export interface HealthRecord {
  recordId: number;
  patientId?: number;
  doctorId?: number;
  patientName: string;
  doctorName: string;
  visitDate?: Date;
  diagnosis?: string;
  prescription?: string;
  notes?: string;
}