export interface RegisterPatientRequest {
  patientName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceID?: string;
  password: string;
}
