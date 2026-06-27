import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private api =
    'https://localhost:7225/api/appointments';

  constructor(private http: HttpClient) {}

  getAvailableSlots(
      doctorId: number,
      date: string
  ): Observable<string[]> {

    return this.http.get<string[]>(
      `${this.api}/available-slots?doctorId=${doctorId}&date=${date}`
    );
  }

  bookAppointment(data: any): Observable<any> {

    return this.http.post(
      this.api,
      data
    );
  }
}