import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Appointment } from '../models/appointment.model';
import { HealthRecord } from '../models/health-record.model';
import { DoctorInfo } from '../models/doctor-info.model';
import { HealthRecordCreateRequest } from '../models/health-record-create-request';
import { PagedResult } from '../models/paged-result.model';
import { Patient } from '../models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly apiBaseUrl = 'https://localhost:7224/api';

  constructor(private http: HttpClient) {}

    getMyInformation(doctorId: number): Observable<DoctorInfo> {
    return this.http.get<DoctorInfo>(
        `${this.apiBaseUrl}/doctors/${doctorId}`
    );
    }
    getMyAppointments(): Observable<Appointment[] | PagedResult<Appointment>> {
        return this.http.get<Appointment[] | PagedResult<Appointment>>(
        `${this.apiBaseUrl}/appointments`
        );
    }

    confirmAppointment(appointmentId: number): Observable<Appointment> {
    return this.http.put<Appointment>(
        `${this.apiBaseUrl}/appointments/${appointmentId}/status`,
        {
        status: 2,
        cancellationReason: ''
        }
    );
    }
    completeAppointment(appointmentId: number): Observable<Appointment> {
        return this.http.put<Appointment>(
        `${this.apiBaseUrl}/appointments/${appointmentId}/status`,
        {
            status: 4,
            cancellationReason: ''
        }
        );
    }

    getPatientProfile(patientId: number): Observable<Patient> {
        return this.http.get<Patient>(
        `${this.apiBaseUrl}/doctors/patients/${patientId}`
        );
    }

    getPatientHealthRecords(patientId: number): Observable<HealthRecord[]> {
        return this.http.get<HealthRecord[]>(
        `${this.apiBaseUrl}/doctors/patients/${patientId}/health-records`
        );
    }

    updateMyActiveStatus(
        doctorId: number,
        isActive: boolean
        ): Observable<DoctorInfo> {
        return this.http.patch<DoctorInfo>(
            `${this.apiBaseUrl}/doctors/${doctorId}/active-status`,
            {
            isActive
            }
        );
    }

    addHealthRecord(
        request: HealthRecordCreateRequest
    ): Observable<HealthRecord> {
        return this.http.post<HealthRecord>(
        `${this.apiBaseUrl}/health-records`,
        request
        );
    }
}