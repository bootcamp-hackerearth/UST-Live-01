import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HealthRecord } from '../models/health-record.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly http = inject(HttpClient);
  private readonly healthRecordUrl = `${environment.apiBaseUrl}/health-records`;

  getHealthRecordsByPatientId(patientId: number) {
    return this.http.get<HealthRecord[]>(`${this.healthRecordUrl}/patient/${patientId}`);
  }

  getHealthRecordById(id: number) {
    return this.http.get<HealthRecord>(`${this.healthRecordUrl}/${id}`);
  }

  createHealthRecord(data: HealthRecord) {
    return this.http.post<HealthRecord>(this.healthRecordUrl, data);
  }
}