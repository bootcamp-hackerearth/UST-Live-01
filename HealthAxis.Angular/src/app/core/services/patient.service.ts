import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Patient } from '../models/patient.model';
import { Doctor } from '../models/doctor.model';
import { PagedResult } from '../models/paged-result.model';
import { PatientUpdateRequest } from '../models/patient-update-request';
import { Appointment } from '../models/appointment.model';
import { AppointmentCreateRequest } from '../models/appointment-create-request';
import { HealthRecord } from '../models/health-record.model';

import { UpdateAppointmentStatusRequest } from '../models/update-appointment-status-request';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly apiBaseUrl = 'https://localhost:7224/api';

  constructor(private http: HttpClient) {}

  getMyInformation(patientId: number): Observable<Patient> {
  return this.http.get<Patient>(
    `${this.apiBaseUrl}/patients/${patientId}`
  );
}

updateMyInformation(
  patientId: number,
  request: PatientUpdateRequest
): Observable<Patient> {
  return this.http.put<Patient>(
    `${this.apiBaseUrl}/patients/${patientId}`,
    request
  );
}

  getDoctors(
    pageNumber: number,
    pageSize: number,
    searchTerm: string,
    specialisation: string
  ): Observable<Doctor[] | PagedResult<Doctor>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (searchTerm.trim()) {
      params = params.set('searchTerm', searchTerm.trim());
    }

    if (specialisation) {
      params = params.set('specialisation', specialisation);
    }

    return this.http.get<Doctor[] | PagedResult<Doctor>>(
      `${this.apiBaseUrl}/doctors`,
      { params }
    );
  }

  getMyAppointments(): Observable<Appointment[] | PagedResult<Appointment>> {
    return this.http.get<Appointment[] | PagedResult<Appointment>>(
      `${this.apiBaseUrl}/appointments`
    );
  }
  getMyHealthRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.apiBaseUrl}/patients/me/health-records`
    );
  }

  cancelAppointment(
    appointmentId: number,
    cancellationReason: string
  ): Observable<Appointment> {
    const request: UpdateAppointmentStatusRequest = {
      status: 4,
      cancellationReason: cancellationReason
    };

    return this.http.put<Appointment>(
      `${this.apiBaseUrl}/appointments/${appointmentId}/status`,
      request
    );
  }
    getDoctorById(doctorId: number): Observable<Doctor> {
    return this.http.get<Doctor>(
    `${this.apiBaseUrl}/doctors/${doctorId}`
    );
  }

  getDoctorAvailability(
  doctorId: number,
  date: string
): Observable<string[]> {
  const formattedDate =
    new Date(date).toISOString().split('T')[0];

  const params =
    new HttpParams()
      .set('date', formattedDate);

  return this.http.get<string[]>(
    `${this.apiBaseUrl}/doctors/${doctorId}/availability`,
    { params }
  );
}

  bookAppointment(
    request: AppointmentCreateRequest
  ): Observable<Appointment> {
    return this.http.post<Appointment>(
      `${this.apiBaseUrl}/appointments`,
      request
    );
  }
}