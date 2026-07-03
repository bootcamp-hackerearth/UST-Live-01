import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private readonly baseUrl = 'https://localhost:7130/api/appointments';

  constructor(private readonly http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    let token: string | null = null;

    if (typeof window !== 'undefined') {
      token = localStorage.getItem('token');
    }

    if (!token) {
      return new HttpHeaders();
    }

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  getAppointments() {
    return this.http.get<any[]>(
      this.baseUrl,
      {
        headers: this.getHeaders()
      }
    );
  }

  bookAppointment(data: any) {
    return this.http.post<any>(
      this.baseUrl,
      data,
      {
        headers: this.getHeaders()
      }
    );
  }

  deleteAppointment(id: number) {
    return this.http.delete<any>(
      `${this.baseUrl}/${id}`,
      {
        headers: this.getHeaders()
      }
    );
  }
}