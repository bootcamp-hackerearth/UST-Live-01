import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  getMyAppointments(): Observable<any[]> {
    return this.http.get<any[]>(
      `${this.baseUrl}/me`,
      { headers: this.getHeaders() }
    );
  }

  bookAppointment(data: {
    doctorId: number;
    scheduledDate: string;
    timeSlot: string;
  }): Observable<any> {

    console.log("BOOK PAYLOAD:", data);

    return this.http.post(
      this.baseUrl,
      data,
      { headers: this.getHeaders() }
    );
  }

  cancelAppointment(id: number, reason: string): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/${id}/cancel?reason=${encodeURIComponent(reason)}`,
      {},
      { headers: this.getHeaders() }
    );
  }

  checkDoctorAvailability(doctorId: number, date: Date): Observable<string[]> {
    return this.http.get<string[]>(
      `${this.baseUrl}/doctor/${doctorId}/availability?date=${date.toISOString()}`,
      { headers: this.getHeaders() }
    );
  }






}