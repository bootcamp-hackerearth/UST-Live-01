import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DoctorAppointment } from '../models/doctor.appointment.model';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  private api =
    'https://localhost:7225/api/appointments';

  constructor(private http: HttpClient) { }

  getAvailableSlots(
    doctorId:number,
    date:string
  ):Observable<string[]>{

    return this.http.get<string[]>(
      `${this.api}/available-slots?doctorId=${doctorId}&date=${date}`
    );
  }

  bookAppointment(data:any):Observable<any>{

    return this.http.post(
      this.api,
      data
    );
  }

  getMyAppointments(): Observable<any[]> {

  return this.http.get<any[]>(
    `${this.api}/my`
  );
  }

  getDoctorSchedule(date: string) {

  return this.http.get<DoctorAppointment[]>(
    `${this.api}/doctor/0/schedule?date=${date}`
  );
  }

  updateAppointmentStatus(
  id: number,
  status: string,
  cancellationReason?: string
) {

  return this.http.patch(
    `${this.api}/${id}/status`,
    {
      status,
      cancellationReason
    }
  );
}

}