export interface RegisterPatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email: string;
  password: string;
  confirmPassword: string;
}