import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Doctor } from '../models/doctor/doctor.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private baseUrl = 'https://localhost:7066/api/doctors';

  constructor(private http: HttpClient) {}


  getActiveDoctors(pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get(
      `${this.baseUrl}/activedoctors?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  searchBySpecialisation(type: string, pageNumber: number = 1, pageSize: number = 10): Observable<any> {
    return this.http.get(
      `${this.baseUrl}/specialisation/${type}?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

}