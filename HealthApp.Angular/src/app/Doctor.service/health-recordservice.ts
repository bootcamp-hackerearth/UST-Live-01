import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HealthRecordService {

  private readonly baseUrl = 'http://localhost:5066/api/HealthRecordApi';

  constructor(private readonly http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token || ''}`,
      'Content-Type': 'application/json'
    });
  }

  createRecord(data: any) {
    return this.http.post(this.baseUrl, data, {
      headers: this.getHeaders()
    });
  }

  getById(id: number) {
    return this.http.get<any>(`${this.baseUrl}/${id}`, {
      headers: this.getHeaders()
    });
  }

  getDoctorRecords() {
    return this.http.get<any[]>(`${this.baseUrl}/doctor/me`, {
      headers: this.getHeaders()
    });
  }
}