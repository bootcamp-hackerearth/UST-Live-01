export interface AuthResponse {
  userId: string;
  patientId?: number | null;
  doctorId?: number | null;
  fullName: string;
  email: string;
  role: string;
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  firstLogin: boolean;
}
