import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { HealthRecord } from '../models/health-record/health-record.model';
@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {

  private readonly baseUrl = '/api/HealthRecordApi';

  constructor(private readonly http: HttpClient) {}

  getMyRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${this.baseUrl}/me`);
  }

  getDoctorRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${this.baseUrl}/doctor/me`);
  }
}