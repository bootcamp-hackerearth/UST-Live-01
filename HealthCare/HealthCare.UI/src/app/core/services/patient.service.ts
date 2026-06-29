import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HealthRecord } from '../models/health-record.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private api =
    'https://localhost:7225/api/patients';

  constructor(private http: HttpClient) {}

  getProfile() {

    return this.http.get(
      `${this.api}/Profile`
    );
  }

  updateProfile(data: any) {

    return this.http.put(
      `${this.api}/Profile`,
      data
    );
  }

  getHealthRecords() {

  return this.http.get<HealthRecord[]>(
    `${this.api}/healthrecords/my-records`
  );

}

}