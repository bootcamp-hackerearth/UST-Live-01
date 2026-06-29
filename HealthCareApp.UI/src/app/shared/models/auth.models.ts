export type UserRole = 'Admin' | 'Patient' | 'Doctor';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  message: string;
  expiresIn: number;
}

export interface DecodedToken {
  userId: string;
  email: string;
  role: UserRole | '';
  expiresAt: number;
}