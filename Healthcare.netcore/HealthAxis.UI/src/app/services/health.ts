import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HealthService {

  private readonly baseUrl = 'https://localhost:7130/api/patients';

  constructor(private readonly http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = typeof window !== 'undefined'
      ? localStorage.getItem('token')
      : null;

    if (token) {
      return new HttpHeaders({
        Authorization: `Bearer ${token}`
      });
    }

    return new HttpHeaders();
  }

  getHealthRecordsByPatient(patientId: number) {
    return this.http.get<any[]>(
      `${this.baseUrl}/${patientId}/health-records`,
      {
        headers: this.getHeaders()
      }
    );
  }
}
