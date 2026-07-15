import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  PatientDto,
  UpdatePatientRequest
} from '../models/patient.model';
import { HealthRecordDto } from '../models/health-record.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly apiUrl = `${environment.apiBaseUrl}/patients`;

  constructor(private readonly httpClient: HttpClient) {
  }

  getById(patientId: number): Observable<PatientDto> {
    return this.httpClient.get<PatientDto>(
      `${this.apiUrl}/${patientId}`
    );
  }

  update(
    patientId: number,
    request: UpdatePatientRequest
  ): Observable<PatientDto> {
    return this.httpClient.put<PatientDto>(
      `${this.apiUrl}/${patientId}`,
      request
    );
  }

  getHealthRecords(patientId: number): Observable<HealthRecordDto[]> {
    return this.httpClient.get<HealthRecordDto[]>(
      `${this.apiUrl}/${patientId}/health-records`
    );
  }
}
