export interface Patient {
  patientId: number;
  fullName?: string;
  dateOfBirth?: Date;
  gender?: 'Male' | 'Female' | 'Other';
  phoneNumber?: string;
  email?: string;
  insuranceId?: string;
  createdDate?: Date;
  identityUserId?: string;
}