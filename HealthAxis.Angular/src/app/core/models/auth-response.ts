export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  message: string;
  expiresIn: number;
  userId: string;
  email: string;
  role: string;
  referenceId: number;
}
