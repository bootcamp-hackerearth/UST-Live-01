export interface Doctor {
  doctorId: number;
  fullName?: string;
  specialisation?: string;
  practiceStartDate?: Date;
  consultationFee?: number;
  email?: string;
  doctorPhoneNumber: string;
  isActive?: boolean;
  identityUserId?: string;
}
