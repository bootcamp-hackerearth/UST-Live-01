import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { Doctor } from '../models/doctor';
import { DoctorProfile } from '../models/doctor-profile';
import { Appointment } from '../models/appointment';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private api = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getAllDoctors(): Observable<Doctor[]> {

    return this.http.get<Doctor[]>(`${this.api}/Doctors`);

  }

  getMyProfile() {

  return this.http.get<DoctorProfile>(
    `${this.api}/Doctors/me`
  );

}

getMyAppointments() {

  return this.http.get<Appointment[]>(
    `${this.api}/Appointments/my`
  );

}

}