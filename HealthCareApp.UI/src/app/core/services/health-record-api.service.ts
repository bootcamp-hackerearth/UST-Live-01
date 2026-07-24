import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { HealthRecordDto } from '../../shared/models/health-record.models';
import { PagedResponse } from '../../shared/models/paged-response.models';

interface HealthRecordQuery {
  pageNumber?: number;
  pageSize?: number;
  searchTerm?: string;
  visitDate?: string;
}

export interface AddHealthRecordRequest {
  patientId: number;
  doctorId?: number;
  appointmentId: number;
  diagnosis: string;
  prescription: string;
  notes: string;
  visitDate: string;
}

@Injectable({
  providedIn: 'root'
})
export class HealthRecordApiService {
  private readonly apiUrl = '/api/HealthRecords';

  constructor(private readonly http: HttpClient) {
  }

  getMyHealthRecords(query: HealthRecordQuery): Observable<PagedResponse<HealthRecordDto>> {
    const params = this.buildHealthRecordParams(query);

    return this.http.get<PagedResponse<HealthRecordDto>>(
      `${this.apiUrl}/my`,
      { params }
    );
  }

  getHealthRecordById(healthRecordId: number): Observable<HealthRecordDto> {
    return this.http.get<HealthRecordDto>(
      `${this.apiUrl}/${healthRecordId}`
    );
  }

  getHealthRecordsByAppointmentId(appointmentId: number): Observable<HealthRecordDto[]> {
    return this.http.get<HealthRecordDto[]>(
      `${this.apiUrl}/appointment/${appointmentId}`
    );
  }

  addHealthRecord(request: AddHealthRecordRequest): Observable<HealthRecordDto> {
    return this.http.post<HealthRecordDto>(
      this.apiUrl,
      request
    );
  }

  private buildHealthRecordParams(query: HealthRecordQuery): HttpParams {
    let params = new HttpParams()
      .set('pageNumber', String(query.pageNumber ?? 1))
      .set('pageSize', String(query.pageSize ?? 10));

    if (query.searchTerm?.trim()) {
      params = params.set('searchTerm', query.searchTerm.trim());
    }

    if (query.visitDate) {
      params = params.set('visitDate', query.visitDate);
    }

    return params;
  }
}