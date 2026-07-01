export type GenderType = 'Male' | 'Female' | 'Transgender' | 'Other';

export interface Patient {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  email?: string;
  insuranceId?: string;
  createdDate: string;
}

export interface UpdatePatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  email: string;
  insuranceId: string;
}