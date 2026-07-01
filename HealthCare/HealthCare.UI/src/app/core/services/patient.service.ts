import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private patientApi = 'https://localhost:7225/api/patients';
  private healthApi = 'https://localhost:7225/api/healthrecords';

  constructor(private http: HttpClient) {}

  //  PROFILE
  getProfile() {
    return this.http.get(`${this.patientApi}/profile`);
  }

  updateProfile(data: any) {
    return this.http.put(`${this.patientApi}/profile`, data);
  }

  //  HEALTH RECORDS API
  getHealthRecords() {
    return this.http.get<any[]>(`${this.healthApi}/my-records`);
  }

  //  APPOINTMENTS
  getAppointments() {
    return this.http.get<any[]>(
      `${this.patientApi}/appointments/my`
    );
  }

  //  DASHBOARD STATS
  getDashboardStats() {
    return this.http.get<any>(
      `${this.patientApi}/dashboard/stats`
    );
  }
}
