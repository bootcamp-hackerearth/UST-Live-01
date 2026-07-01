import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin, map, of } from 'rxjs';

import { PatientHealthRecord } from '../../shared/models/patient-dashboard.models';
import { HealthHistoryRecord } from '../../shared/models/health-history.models';
import { getDoctorSpecialisationText } from '../../shared/models/doctor.models';

export interface PatientProfileDetails {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email: string;
  insuranceNumber?: string;
  insuranceId?: string;
  isActive?: boolean;
}

export interface UpdatePatientProfileRequest {
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email: string;
  insuranceNumber?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientPortalService {
  private baseUrl = 'https://localhost:7258/api';

  private patientUrl = `${this.baseUrl}/patients`;
  private appointmentUrl = `${this.baseUrl}/appointments`;

  constructor(private http: HttpClient) {}

  getPatientProfile(patientId: number): Observable<PatientProfileDetails> {
    return this.http.get<PatientProfileDetails>(
      `${this.patientUrl}/${patientId}`
    );
  }

  updatePatientProfile(
    patientId: number,
    request: UpdatePatientProfileRequest
  ): Observable<any> {
    return this.http.put(
      `${this.patientUrl}/${patientId}`,
      request
    );
  }

  getPatientAppointments(patientId: number): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.appointmentUrl}/patient/${patientId}`
    );
  }

  getPatientHealthRecords(patientId: number): Observable<HealthHistoryRecord[]> {
    return this.http.get<HealthHistoryRecord[]>(
      `${this.patientUrl}/${patientId}/health-records`
    );
  }

  getHealthRecordsForAppointments(
    appointments: any[]
  ): Observable<PatientHealthRecord[]> {
    if (!appointments || appointments.length === 0) {
      return of([]);
    }

    const patientIds = [
      ...new Set(
        appointments
          .map(appointment => appointment.patientId)
          .filter(patientId => patientId !== null && patientId !== undefined)
      )
    ];

    if (patientIds.length === 0) {
      return of([]);
    }

    const requests = patientIds.map(patientId =>
      this.http.get<HealthHistoryRecord[]>(
        `${this.patientUrl}/${patientId}/health-records`
      )
    );

    return forkJoin(requests).pipe(
      map(results =>
        results
          .flat()
          .map(record => this.mapToPatientHealthRecord(record))
      )
    );
  }

  private mapToPatientHealthRecord(
    record: HealthHistoryRecord
  ): PatientHealthRecord {
    return {
      healthRecordId: record.healthRecordId,
      appointmentId: record.appointmentId,
      doctorName: record.doctorName,
      doctorSpecialisation: getDoctorSpecialisationText(record.doctorSpecialisation),
      diagnosis: record.diagnosis,
      prescription: record.prescription,
      notes: record.notes
    };
  }
}
