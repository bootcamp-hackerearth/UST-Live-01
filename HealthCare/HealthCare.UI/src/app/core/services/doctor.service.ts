import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Leave } from '../models/leave.model';
import { Doctor } from '../models/doctor.model';
import { HealthRecord } from '../models/health-record.model';
import { API_ENDPOINTS } from '../config/api-endpoints';

@Injectable({
  providedIn:'root'
})
export class DoctorService {

  constructor(private http:HttpClient,
  ){}


getAvailableDoctors(specialization: string, date: string) {

  return this.http.get<any[]>(
    `${API_ENDPOINTS.DOCTOR}/available?specialisation=${specialization}&date=${date}`
  );
}


  getDoctorSchedule(date: string) {

  return this.http.get<any[]>(
    `${API_ENDPOINTS.DOCTOR}/doctor/0/schedule?date=${date}`
  );
  }

  addLeaves(leaves: Leave[]) {

    return this.http.post(
      `${API_ENDPOINTS.DOCTOR}/leaves`,
      leaves
    );
  }

  getMyLeaves() {
     return this.http.get<any[]>(
    `${API_ENDPOINTS.DOCTOR}/my-leaves`
  );
}

  getProfile() {

  return this.http.get<Doctor>(
    `${API_ENDPOINTS.DOCTOR}/my-profile`
  );
}

updateProfile(id: number, doctor: Doctor) {

  return this.http.put(
    `${API_ENDPOINTS.DOCTOR}/${id}`,
    doctor
  );
}

createHealthRecord(record: any) {

  return this.http.post(
    `${API_ENDPOINTS.HEALTH}`, 
    record
  );
}


getHealthRecordById(id: number) {

  return this.http.get<HealthRecord>(
    `${API_ENDPOINTS.HEALTH}/${id}`
  );

}

updateHealthRecord(
  id: number,
  data: any
) {

  return this.http.put(
    `${API_ENDPOINTS.HEALTH}/${id}`,
    data
  );

}

  

}