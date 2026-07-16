import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

import { DoctorAvailabilityResponse } from '../models/appointment/doctor-availability.model';
import { DoctorSlot } from '../models/appointment/doctor-availability.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private baseUrl = 'http://localhost:5066/api/appointments';


  availabilityMessage = '';
  isDoctorOnLeave = false;
  showBookingModal = false;
  selectedDoctorId: number | null = null;
    selectedDate = '';
    selectedSlot = '';
    selectedDoctor: any = null;

availableSlots: DoctorSlot[] = [];
  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token || ''}`
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

    console.log('BOOK PAYLOAD:', data);

    return this.http.post<any>(
      this.baseUrl,
      data,
      { headers: this.getHeaders() }
    );
  }

  cancelAppointment(id: number, reason: string): Observable<any> {
    return this.http.put<any>(
      `${this.baseUrl}/${id}/cancel?reason=${encodeURIComponent(reason)}`,
      {},
      { headers: this.getHeaders() }
    );
  }

  checkDoctorAvailability(
    doctorId: number,
    date: string
  ): Observable<DoctorAvailabilityResponse> {

    return this.http.get<DoctorAvailabilityResponse>(
      `${this.baseUrl}/doctor/${doctorId}/availability?date=${date}`,
      { headers: this.getHeaders() }
    );
  }
}