export interface CreateHealthRecord {

  appointmentId: number;

  patientId: number;

  visitDate: string;

  diagnosis: string;

  prescription: string;

  notes?: string;

}