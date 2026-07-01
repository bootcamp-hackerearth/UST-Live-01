import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../config/api.config';
import {
  AddHealthRecordRequest,
  HealthRecord
} from '../models/health-record.model';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordApiService {
  private readonly apiUrl = `${API_CONFIG.apiBaseUrl}/health-records`;

  constructor(private readonly http: HttpClient) {}

  getHealthRecordsByPatient(patientId: number): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${this.apiUrl}/${patientId}`);
  }

  getHealthRecordById(recordId: number): Observable<HealthRecord> {
    return this.http.get<HealthRecord>(`${this.apiUrl}/record/${recordId}`);
  }

  addHealthRecord(request: AddHealthRecordRequest): Observable<HealthRecord> {
    return this.http.post<HealthRecord>(this.apiUrl, request);
  }
}