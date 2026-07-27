import {
  HttpClient,
  HttpErrorResponse
} from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import {
  Observable,
  catchError,
  of,
  throwError
} from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateHealthRecordRequest,
  HealthRecord,
  UpdateHealthRecordRequest
} from '../models/health-record.model';

const HEALTH_RECORD_API_PATH =
  '/health-records';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly http = inject(HttpClient);

  private readonly healthRecordUrl =
    `${environment.apiBaseUrl}${HEALTH_RECORD_API_PATH}`;

  getHealthRecordsByPatientId(
    patientId: number
  ): Observable<HealthRecord[]> {
    return this.http
      .get<HealthRecord[]>(
        `${this.healthRecordUrl}/patient/${patientId}`
      )
      .pipe(
        catchError(
          HealthRecordService.handleListError
        )
      );
  }

  getMyDoctorHealthRecords():
    Observable<HealthRecord[]> {
    return this.http
      .get<HealthRecord[]>(
        `${this.healthRecordUrl}/doctor/my`
      )
      .pipe(
        catchError(
          HealthRecordService.handleListError
        )
      );
  }

  getPatientHistoryByAppointmentId(
    appointmentId: number
  ): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.healthRecordUrl}/appointment/${appointmentId}/history`
    );
  }

  getHealthRecordById(
    id: number
  ): Observable<HealthRecord> {
    return this.http.get<HealthRecord>(
      `${this.healthRecordUrl}/${id}`
    );
  }

  createHealthRecord(
    data: CreateHealthRecordRequest
  ): Observable<HealthRecord> {
    return this.http.post<HealthRecord>(
      this.healthRecordUrl,
      data
    );
  }

  updateHealthRecord(
    id: number,
    data: UpdateHealthRecordRequest
  ): Observable<HealthRecord> {
    return this.http.put<HealthRecord>(
      `${this.healthRecordUrl}/${id}`,
      data
    );
  }

  private static handleListError(
    error: HttpErrorResponse
  ): Observable<HealthRecord[]> {
    if (error.status === 404) {
      return of([]);
    }

    return throwError(() => error);
  }
}