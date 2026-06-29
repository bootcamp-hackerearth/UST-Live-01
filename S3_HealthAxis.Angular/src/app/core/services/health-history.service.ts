import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { HealthHistoryRecord } from '../../shared/models/health-history.models';

@Injectable({
  providedIn: 'root'
})
export class HealthHistoryService {
  private readonly apiBaseUrl = 'https://localhost:7258/api';

  constructor(private http: HttpClient) {}

  getPatientHealthRecords(patientId: number): Observable<HealthHistoryRecord[]> {
    return this.http.get<HealthHistoryRecord[]>(
      `${this.apiBaseUrl}/patients/${patientId}/health-records`
    );
  }
}

