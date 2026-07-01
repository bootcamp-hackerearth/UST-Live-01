export interface LoginResponse {
  success: boolean;
  message: string;
  accessToken: string;
  expiresIn: number;
  role: string;
}