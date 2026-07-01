export type UserRole = 'Patient' | 'Doctor' | 'Admin';

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  message: string;
  expiresIn: number;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface ChangePasswordResponse {
  message: string;
}

export interface RegisterPatientDto {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceId?: string | null;
  password: string;
  confirmPassword: string;
}

export interface RegisterPatientResponse {
  message: string;
  userId: string;
}