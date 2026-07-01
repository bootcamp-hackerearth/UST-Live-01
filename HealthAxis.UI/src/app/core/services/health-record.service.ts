import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreateHealthRecordRequest,
  HealthRecord,
  UpdateHealthRecordRequest
} from '../models/health-record.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly http = inject(HttpClient);
  private readonly healthRecordUrl = `${environment.apiBaseUrl}/health-records`;

  getHealthRecordsByPatientId(patientId: number): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(
      `${this.healthRecordUrl}/patient/${patientId}`
    );
  }

  getHealthRecordById(id: number): Observable<HealthRecord> {
    return this.http.get<HealthRecord>(`${this.healthRecordUrl}/${id}`);
  }

  createHealthRecord(
    data: CreateHealthRecordRequest
  ): Observable<HealthRecord> {
    return this.http.post<HealthRecord>(this.healthRecordUrl, data);
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
}