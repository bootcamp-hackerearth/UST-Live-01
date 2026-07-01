import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private baseUrl = 'https://localhost:7066/api/appointments';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getMyDoctorAppointments() {
    return this.http.get<any[]>(`${this.baseUrl}/doctor/me`, {
      headers: this.getHeaders()
    });
  }

  confirmAppointment(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/confirm`, {}, {
      headers: this.getHeaders()
    });
  }

  completeAppointment(id: number) {
    return this.http.put(`${this.baseUrl}/${id}/complete`, {}, {
      headers: this.getHeaders()
    });
  }
}