
import { environment } from '../../../environments/environment';

export const API_ENDPOINTS = {
  PATIENT: `${environment.apiBaseUrl}/patients`,
  DOCTOR: `${environment.apiBaseUrl}/doctors`,
  APPOINTMENT: `${environment.apiBaseUrl}/appointments`,
  HEALTH: `${environment.apiBaseUrl}/healthrecords`
};
