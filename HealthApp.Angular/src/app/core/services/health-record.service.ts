import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  HealthRecordCreateDto,
  HealthRecordDto
} from '../../dtos/health-record.dto';

export interface HealthRecordActionResponse {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/health-records`;

  constructor(private readonly http: HttpClient) {}

  createHealthRecord(payload: HealthRecordCreateDto): Observable<HealthRecordActionResponse> {
    return this.http.post<HealthRecordActionResponse>(this.apiUrl, payload);
  }

  getHealthRecordById(id: number): Observable<HealthRecordDto> {
    return this.http.get<HealthRecordDto>(`${this.apiUrl}/${id}`);
  }

  getHealthRecordsByPatientId(patientId: number): Observable<HealthRecordDto[]> {
    return this.http.get<HealthRecordDto[]>(`${this.apiUrl}/patient/${patientId}`);
  }

  getPatientHealthRecords(patientId: number): Observable<HealthRecordDto[]> {
    return this.http.get<HealthRecordDto[]>(
      `${API_CONFIG.baseUrl}/patients/${patientId}/health-records`
    );
  }

  getMyHealthRecords(): Observable<HealthRecordDto[]> {
    return this.http.get<HealthRecordDto[]>(
      `${API_CONFIG.baseUrl}/patients/profile/health-records`
    );
  }

  healthRecordExistsByAppointmentId(appointmentId: number): Observable<boolean> {
    return this.http.get<boolean>(
      `${API_CONFIG.baseUrl}/appointments/${appointmentId}/healthrecord`
    );
  }

  getHealthRecordByAppointmentId(appointmentId: number): Observable<HealthRecordDto> {
    return this.http.get<HealthRecordDto>(
      `${API_CONFIG.baseUrl}/appointments/${appointmentId}/healthrecord/details`
    );
  }
}