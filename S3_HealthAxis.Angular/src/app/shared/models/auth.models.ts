export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterPatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email: string;
  insuranceNumber?: string;
  password: string;
  confirmPassword: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  email: string;
  role: string;
  referenceId: number;
  mustChangePassword: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface ApiMessageResponse {
  message: string;
}
