export interface DoctorInfo {
  doctorId: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  specialisation: number | string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}