export interface HealthRecord {
  healthRecordId: number;

  patientId: number;
  patientName?: string;

  doctorId: number;
  doctorName?: string;

  specialisation: number | string;

  appointmentId: number;

  visitDate: string;

  diagnosis: string;
  prescription: string;
  notes?: string;
}
