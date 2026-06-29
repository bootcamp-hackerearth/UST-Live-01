import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';

import {
  AppointmentStatus,
  PatientAppointment,
  PatientHealthRecord,
  PatientProfile
} from '../../shared/models/patient-dashboard.models';

@Injectable({
  providedIn: 'root'
})
export class PatientPortalService {
  private readonly apiBaseUrl = 'https://localhost:7258/api';

  constructor(private http: HttpClient) {}

  getPatientProfile(patientId: number): Observable<PatientProfile> {
    return this.http.get<PatientProfile>(
      `${this.apiBaseUrl}/patients/${patientId}`
    );
  }

  getPatientAppointments(patientId: number): Observable<PatientAppointment[]> {
    return this.http.get<PatientAppointment[]>(
      `${this.apiBaseUrl}/appointments/patient/${patientId}`
    );
  }

  getHealthRecordByAppointment(appointmentId: number): Observable<PatientHealthRecord | null> {
    return this.http
      .get<PatientHealthRecord>(
        `${this.apiBaseUrl}/healthrecords/appointment/${appointmentId}`
      )
      .pipe(
        catchError(() => of(null))
      );
  }

  getHealthRecordsForAppointments(
    appointments: PatientAppointment[]
  ): Observable<PatientHealthRecord[]> {
    const completedAppointments = appointments.filter(
      appointment => appointment.status === AppointmentStatus.Completed
    );

    if (!completedAppointments.length) {
      return of([]);
    }

    const requests = completedAppointments.map(appointment =>
      this.getHealthRecordByAppointment(appointment.appointmentId)
    );

    return forkJoin(requests).pipe(
      map(records =>
        records.filter((record): record is PatientHealthRecord => record !== null)
      )
    );
  }
}
