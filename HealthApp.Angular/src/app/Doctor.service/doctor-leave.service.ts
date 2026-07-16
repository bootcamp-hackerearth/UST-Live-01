import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DoctorLeave, DoctorLeaveCreate } from '../models/doctor-leave/doctor-leave.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorLeaveService {

  private apiUrl = 'http://localhost:5066/api/doctor-leaves';

  constructor(private http: HttpClient) {}

  createMyLeave(data: DoctorLeaveCreate) {
    return this.http.post<DoctorLeave>(`${this.apiUrl}/my`, data);
  }

  getMyLeaves() {
    return this.http.get<DoctorLeave[]>(`${this.apiUrl}/my`);
  }

  getDoctorLeaves(doctorId: number) {
    return this.http.get<DoctorLeave[]>(`${this.apiUrl}/doctor/${doctorId}`);
  }
}