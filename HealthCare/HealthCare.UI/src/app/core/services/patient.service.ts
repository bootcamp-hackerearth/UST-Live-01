import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { API_ENDPOINTS } from "../config/api-endpoints";


@Injectable({
  providedIn: 'root'
})
export class PatientService {

  // private patientApi = 'https://localhost:7225/api/patients';
  // private healthApi = 'https://localhost:7225/api/healthrecords';

  constructor(private http: HttpClient) {}

  //  PROFILE
  getProfile() {
    return this.http.get(`${API_ENDPOINTS.PATIENT}/profile`);
  }

  updateProfile(data: any) {
    return this.http.put(`${API_ENDPOINTS.PATIENT}/profile`, data);
  }

  //  HEALTH RECORDS API
  getHealthRecords() {
    return this.http.get<any[]>(`${API_ENDPOINTS.HEALTH}/my-records`);
  }

  //  APPOINTMENTS
  getAppointments() {
    return this.http.get<any[]>(
      `${API_ENDPOINTS.PATIENT}/appointments/my`
    );
  }

  //  DASHBOARD STATS
  getDashboardStats() {
    return this.http.get<any>(
      `${API_ENDPOINTS.PATIENT}/dashboard/stats`
    );
  }
}
