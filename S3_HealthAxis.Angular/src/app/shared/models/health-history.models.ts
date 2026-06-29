export interface HealthHistoryRecord {
  healthRecordId: number;
  appointmentId: number;
  patientId: number;
  doctorId: number;
  doctorName: string;
  doctorSpecialisation: number;
  createdOn: string;
  diagnosis?: string;
  prescription?: string;
  notes?: string;
}

