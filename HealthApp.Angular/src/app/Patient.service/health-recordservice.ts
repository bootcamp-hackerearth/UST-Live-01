import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { HealthRecord } from '../models/health-record/health-record.model';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {

  private baseUrl = 'https://localhost:7066/api/HealthRecordApi';

  constructor(private http: HttpClient) {}

  // User

  getMyRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${this.baseUrl}/me`);
  }

  // Doctor

  getDoctorRecords(): Observable<HealthRecord[]> {
    return this.http.get<HealthRecord[]>(`${this.baseUrl}/doctor/me`);
  }

}