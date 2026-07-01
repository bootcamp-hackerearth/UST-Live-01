export type UserRole = 'Admin' | 'Doctor' | 'Patient';

export interface CurrentUser {
  userId: string;
  email: string;
  role: UserRole;
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  patientId?: number;
  doctorId?: number;
}