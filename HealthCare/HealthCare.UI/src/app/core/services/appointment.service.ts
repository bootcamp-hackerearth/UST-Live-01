import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DoctorAppointment } from '../models/doctor.appointment.model';
import { API_ENDPOINTS } from '../config/api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private http: HttpClient) { }


getAvailableSlots(doctorId: number, date: string) {

  return this.http.get<string[]>(
    `${API_ENDPOINTS.APPOINTMENT}/available-slots?doctorId=${doctorId}&date=${date}`
  );
}


  bookAppointment(data:any):Observable<any>{

    return this.http.post(API_ENDPOINTS.APPOINTMENT,data);
  }

 
getMyAppointments() {
  return this.http.get<any[]>(`${API_ENDPOINTS.APPOINTMENT}/my`   
  );
}

  getDoctorSchedule(date: string) {

  return this.http.get<DoctorAppointment[]>(
    `${API_ENDPOINTS.APPOINTMENT}/doctor/schedule?date=${date}`
  );
  }

  updateAppointmentStatus(
  id: number,
  status: string,
  cancellationReason?: string
) {

  return this.http.patch(`${API_ENDPOINTS.APPOINTMENT}/${id}/status`,
    {
      status,
      cancellationReason
    }
  );
}

}