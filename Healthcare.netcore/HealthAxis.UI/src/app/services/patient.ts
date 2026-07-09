import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private readonly patientBaseUrl = 'https://localhost:7130/api/patients';
  private readonly authBaseUrl = 'https://localhost:7130/api/auth';

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

  getPatients(pageNumber: number = 1, pageSize: number = 10) {
    return this.http.get<any>(
      this.patientBaseUrl,
      {
        headers: this.getHeaders(),
        params: {
          pageNumber,
          pageSize
        }
      }
    );
  }

  getDoctorPatients(pageNumber: number = 1, pageSize: number = 10) {
    return this.http.get<any>(
      `${this.patientBaseUrl}/doctor`,
      {
        headers: this.getHeaders(),
        params: {
          pageNumber,
          pageSize
        }
      }
    );
  }

  getCurrentPatient() {
    return this.http.get<any>(
      `${this.patientBaseUrl}/me`,
      {
        headers: this.getHeaders()
      }
    );
  }

  updatePatient(patientId: number, data: any) {
    return this.http.put<any>(
      `${this.patientBaseUrl}/${patientId}`,
      data,
      {
        headers: this.getHeaders()
      }
    );
  }

  changePassword(data: any) {
    return this.http.post<any>(
      `${this.authBaseUrl}/change-password`,
      data,
      {
        headers: this.getHeaders()
      }
    );
  }
}