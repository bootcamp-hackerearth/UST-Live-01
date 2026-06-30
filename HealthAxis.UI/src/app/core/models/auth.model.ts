import { Gender } from './gender.enum';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  message: string;
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface RegisterPatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: Gender;
  phoneNumber: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface RefreshTokenRequest {
  accessToken: string;
  refreshToken: string;
}

export interface RefreshTokenResponse {
  message: string;
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}