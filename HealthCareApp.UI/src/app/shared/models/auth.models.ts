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

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface ChangePasswordResponse {
  message: string;
}