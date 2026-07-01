import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Leave } from '../models/leave.model';
import { Doctor } from '../models/doctor.model';
import { HealthRecord } from '../models/health-record.model';

@Injectable({
  providedIn:'root'
})
export class DoctorService {

  private api =
    'https://localhost:7225/api/doctors';

  constructor(private http:HttpClient){}


getAvailableDoctors(specialization: string, date: string) {

  return this.http.get<any[]>(
    `${this.api}/available?specialisation=${specialization}&date=${date}`
  );
}


  getDoctorSchedule(date: string) {

  return this.http.get<any[]>(
    `${this.api}/doctor/0/schedule?date=${date}`
  );
  }

  addLeaves(leaves: Leave[]) {

    return this.http.post(
      `${this.api}/leaves`,
      leaves
    );
  }

  getMyLeaves() {
     return this.http.get<any[]>(
    `${this.api}/my-leaves`
  );
}

  getProfile() {

  return this.http.get<Doctor>(
    `${this.api}/my-profile`
  );
}

updateProfile(id: number, doctor: Doctor) {

  return this.http.put(
    `${this.api}/${id}`,
    doctor
  );
}

createHealthRecord(record: any) {

  return this.http.post(
    `https://localhost:7225/api/healthrecords`, 
    record
  );
}


getHealthRecordById(id: number) {

  return this.http.get<HealthRecord>(
    `${this.api}/$healthrecords/${id}`
  );

}

updateHealthRecord(
  id: number,
  data: any
) {

  return this.http.put(
    `${this.api}/$healthrecordshealthrecords/${id}`,
    data
  );

}

  

}