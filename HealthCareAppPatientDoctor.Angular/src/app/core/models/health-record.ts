export interface HealthRecord {

  healthRecordId: number;

  patientId: number;

  patientName: string;

  doctorId?: number;

  doctorName?: string;

  appointmentId: number;

  visitDate: string;

  diagnosis: string;

  prescription: string;

  notes?: string;

}