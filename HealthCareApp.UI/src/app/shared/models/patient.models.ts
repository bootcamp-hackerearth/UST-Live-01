export interface PatientDto {
  patientId: number;
  patientName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceId?: string;
}

export interface UpdatePatientDto {
  patientName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceId?: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}