import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { BookAppointments } from '../models/book-appointment';
import { Appointment } from '../models/appointment';
import { CancelAppointment } from '../models/cancel-appointment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private api = environment.apiUrl + '/Appointments';

  constructor(private http: HttpClient) {}

  getAvailableSlots(
    doctorId: number,
    date: string
  ): Observable<string[]> {

    return this.http.get<string[]>(
      `${this.api}/available-slots?doctorId=${doctorId}&date=${date}`
    );

  }

  bookAppointment(request: BookAppointments) {

  return this.http.post(
    `${this.api}`,
    request
  );

}

getMyAppointments() {

  return this.http.get<Appointment[]>(
    `${this.api}/my`
  );

}

cancelAppointment(request: CancelAppointment) {

  return this.http.put(

    `${this.api}/cancel`,

    request

  );

}

confirmAppointment(appointmentId: number) {

  return this.http.put(
    `${this.api}/${appointmentId}/confirm`,
    {}
  );

}

getAppointmentById(appointmentId: number) {

  return this.http.get<Appointment>(
    `${this.api}/${appointmentId}`
  );

}

}