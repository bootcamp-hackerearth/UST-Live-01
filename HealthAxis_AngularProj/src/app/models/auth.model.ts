export interface RegisterDTO {
  email: string;
  password: string;
  confirmPassword: string;
  role: string;
  patientName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
}

export interface LoginDTO {
  email: string;
  password: string;
}

export interface AuthResponseDTO {
  token: string;
  refreshToken: string;
  email: string;
  role: string;
  referenceId: number;
  isFirstLogin: boolean;
}