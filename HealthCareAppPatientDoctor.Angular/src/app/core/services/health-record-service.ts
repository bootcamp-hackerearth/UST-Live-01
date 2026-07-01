import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { environment } from '../../../environments/environment';

import { AddHealthRecord } from '../models/add-health-record';
import { HealthRecord } from '../models/health-record';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {

  private api =
    environment.apiUrl + '/HealthRecords';

  constructor(private http: HttpClient) { }

  addHealthRecord(request: AddHealthRecord) {

    return this.http.post(
      this.api,
      request
    );

  }

  getPatientHistory(
  appointmentId: number
) {

  return this.http.get<HealthRecord[]>(
    `${this.api}/appointment/${appointmentId}/history`
  );

}

getMyHealthRecords() {

  return this.http.get<HealthRecord[]>(
    `${this.api}/my`
  );

}

}