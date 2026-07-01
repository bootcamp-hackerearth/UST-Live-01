export interface AuthResponse {
  message: string;
  userId: string;
  email: string;
  role: string;
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAt: string;
  mustChangePassword: boolean;
  patientId?: number;
  doctorId?: number;
}