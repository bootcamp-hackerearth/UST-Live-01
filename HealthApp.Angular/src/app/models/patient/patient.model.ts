export interface Patient {
  patientId: number;
  fullName?: string;
  dateOfBirth?: string | Date | null;
  gender?: 'Male' | 'Female' | 'Other';
  phoneNumber?: string;
  email?: string;
  insuranceId?: string;
  createdDate?: Date;
  identityUserId?: string;
}