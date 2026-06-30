export interface HealthRecord {

  recorId: number;

  patientId:number;

  doctorId:number;

  doctorName: string;

  visitDate: string;

  diagnosis: string;

  prescription: string;

  notes?: string;

}