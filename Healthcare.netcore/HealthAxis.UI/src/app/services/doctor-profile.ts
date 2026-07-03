import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorProfileService {

  private readonly baseUrl = 'https://localhost:7130/api/doctors';

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

  getCurrentDoctor() {
    return this.http.get<any>(
      `${this.baseUrl}/me`,
      {
        headers: this.getHeaders()
      }
    );
  }
}