import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateHealthRecordRequest,
  HealthRecordDto
} from '../models/health-record.model';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly apiUrl = `${environment.apiBaseUrl}/health-records`;

  constructor(private readonly httpClient: HttpClient) {
  }

  getByPatientId(patientId: number): Observable<HealthRecordDto[]> {
    return this.httpClient.get<HealthRecordDto[]>(
      `${this.apiUrl}/${patientId}`
    );
  }

  existsForAppointment(appointmentId: number): Observable<boolean> {
    return this.httpClient.get<boolean>(
      `${this.apiUrl}/appointment/${appointmentId}/exists`
    );
  }

  create(request: CreateHealthRecordRequest): Observable<HealthRecordDto> {
    return this.httpClient.post<HealthRecordDto>(
      this.apiUrl,
      request
    );
  }
}
