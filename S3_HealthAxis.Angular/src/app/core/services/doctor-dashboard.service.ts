import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreateHealthRecordRequest,
  DoctorHealthRecord,
  DoctorPatientProfile,
  DoctorScheduleItem
} from '../../shared/models/doctor-dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class DoctorDashboardService {
  private readonly apiBaseUrl = 'https://localhost:7258/api';

  constructor(private http: HttpClient) {}

  getTodaySchedule(doctorId: number): Observable<DoctorScheduleItem[]> {
    return this.http.get<DoctorScheduleItem[]>(
      `${this.apiBaseUrl}/appointments/doctor/${doctorId}/today`
    );
  }

  getPatientProfile(patientId: number): Observable<DoctorPatientProfile> {
    return this.http.get<DoctorPatientProfile>(
      `${this.apiBaseUrl}/patients/${patientId}`
    );
  }

  getPatientHealthRecords(patientId: number): Observable<DoctorHealthRecord[]> {
    return this.http.get<DoctorHealthRecord[]>(
      `${this.apiBaseUrl}/patients/${patientId}/health-records`
    );
  }

  completeAppointment(appointmentId: number): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/appointments/${appointmentId}/complete`,
      {}
    );
  }

  createHealthRecord(request: CreateHealthRecordRequest): Observable<DoctorHealthRecord> {
    return this.http.post<DoctorHealthRecord>(
      `${this.apiBaseUrl}/healthrecords`,
      request
    );
  }
}

