export interface ApiError {
  message?: string;
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]>;
}