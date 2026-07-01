import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreateHealthRecordRequest,
  DoctorHealthRecord,
  DoctorPatient,
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

  getWeekSchedule(
    doctorId: number,
    startDate: string,
    endDate: string
  ): Observable<DoctorScheduleItem[]> {
    const params = new HttpParams()
      .set('startDate', startDate)
      .set('endDate', endDate);

    return this.http.get<DoctorScheduleItem[]>(
      `${this.apiBaseUrl}/appointments/doctor/${doctorId}/week`,
      { params }
    );
  }

  getDoctorPatients(doctorId: number): Observable<DoctorPatient[]> {
    return this.http.get<DoctorPatient[]>(
      `${this.apiBaseUrl}/doctors/${doctorId}/patients`
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

  confirmAppointment(appointmentId: number): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/appointments/${appointmentId}/confirm`,
      {}
    );
  }

  completeAppointment(appointmentId: number): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/appointments/${appointmentId}/complete`,
      {}
    );
  }

  cancelAppointment(
    appointmentId: number,
    cancellationReason: string
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiBaseUrl}/appointments/${appointmentId}/cancel`,
      {
        cancellationReason
      }
    );
  }

  createHealthRecord(
    request: CreateHealthRecordRequest
  ): Observable<DoctorHealthRecord> {
    return this.http.post<DoctorHealthRecord>(
      `${this.apiBaseUrl}/healthrecords`,
      request
    );
  }
}

