export interface PatientDto {
  patientId: number;
  patientName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceID?: string | null;
  isActive: boolean;
}

export interface UpdatePatientRequest {
  patientName: string;
  dateOfBirth: string;
  gender: string;
  phoneNumber: string;
  insuranceID?: string | null;
}
