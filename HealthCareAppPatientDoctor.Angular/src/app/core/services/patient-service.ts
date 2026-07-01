import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Patient } from '../models/patient';
import { PatientProfile } from '../models/patient-profile';
import { UpdatePatient } from '../models/update-patient';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMyProfile(): Observable<Patient> {

    return this.http.get<Patient>(
      `${this.apiUrl}/Patients/me`
    );

  }

  getMyPatientProfile() {

  return this.http.get<PatientProfile>(
    `${this.apiUrl}/Patients/me`
  );

}

updateMyProfile(request: UpdatePatient) {

  return this.http.put<Patient>(
    `${this.apiUrl}/Patients/me`,
    request
  );

}

}