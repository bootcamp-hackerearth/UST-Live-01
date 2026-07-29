import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CreateHealthRecordRequest, HealthRecordResponse, HealthRecordListDto } from '../models/portal.models';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) { }
  createRecord(request: CreateHealthRecordRequest) {
    return this.http.post(
      `${this.apiBaseUrl}/api/records/create`,
      request
    );
  }

  getRecordsByPatient(patientId: number) {
    return this.http.get<HealthRecordListDto[]>(
      `${this.apiBaseUrl}/api/records/by-patient/${patientId}`
    );
  }

  getMyRecords() {
    return this.http.get<HealthRecordResponse[]>(
      `${this.apiBaseUrl}/api/records/my-records`
    );
  }
}
