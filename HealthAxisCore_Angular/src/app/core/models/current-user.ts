export interface CurrentUser {
  userId: string;
  patientId?: number | null;
  doctorId?: number | null;
  fullName: string;
  email: string;
  role: string;
  firstLogin: boolean;
}
