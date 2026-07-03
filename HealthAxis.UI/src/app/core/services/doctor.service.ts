import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Doctor } from '../models/doctor.model';
import { Patient } from '../models/patient.model';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';


export interface DoctorStatusResponse {
  message: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly http = inject(HttpClient);
  private readonly doctorUrl = `${environment.apiBaseUrl}/doctors`;

  getAllDoctors() {
    return this.http.get<Doctor[]>(this.doctorUrl);
  }

  getMyDoctorProfile() {
    return this.http.get<Doctor>(`${this.doctorUrl}/me`);
  }

  getDoctorById(id: number) {
    return this.http.get<Doctor>(`${this.doctorUrl}/${id}`);
  }

  getDoctorAvailability(id: number) {
    return this.http.get<any>(`${this.doctorUrl}/${id}/availability`);
  }

  getMyPatients() {
    return this.http.get<Patient[]>(`${this.doctorUrl}/me/patients`);
  }

  getMyPatientById(patientId: number) {
    return this.http.get<Patient>(`${this.doctorUrl}/me/patients/${patientId}`);
 
 }
 updateMyStatus(isActive: boolean): Observable<DoctorStatusResponse> {
  return this.http.put<DoctorStatusResponse>(
    `${this.doctorUrl}/me/status`,
    { isActive }
  );
}
}