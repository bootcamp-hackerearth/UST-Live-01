export interface Patient {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  phoneNumber: string;
  email: string;
  age?: number;
  createdDate?: string;
}