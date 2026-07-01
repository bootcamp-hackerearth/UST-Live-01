export type GenderType = 'Male' | 'Female' | 'Transgender' | 'Other';

export interface RegisterPatientRequest {
  fullName: string;
  email: string;
  password: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  insuranceId?: string;
}