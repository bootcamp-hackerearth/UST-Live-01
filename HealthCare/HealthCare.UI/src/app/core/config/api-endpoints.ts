
import { environment } from '../../../environments/environment';

export const API_ENDPOINTS = {
  PATIENT: `${environment.apiBaseUrl}api/patients`,
  DOCTOR: `${environment.apiBaseUrl}api/doctors`,
  APPOINTMENT: `${environment.apiBaseUrl}api/appointments`,
  HEALTH: `${environment.apiBaseUrl}api/healthrecords`,
  LOGIN: `${environment.apiBaseUrl}api/auth/login`,
  REGISTER_PATIENT: `${environment.apiBaseUrl}api/auth/register-patient`
};
