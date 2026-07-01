export interface PatientDto {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  phoneNumber: string;
  email: string;
  insuranceId?: string | null;
  createdDate?: string;
}

export interface PatientCreateDto {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  phoneNumber: string;
  email: string;
  insuranceId?: string | null;
}

export interface PatientUpdateResponse {
  message: string;
}