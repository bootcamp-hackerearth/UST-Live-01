import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Doctor } from '../../shared/models/doctor.models';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly apiBaseUrl = 'https://localhost:7258/api';

  constructor(private http: HttpClient) {}

  getDoctors(sortBy = 'name', specialisation?: number | null): Observable<Doctor[]> {
    let params = new HttpParams().set('sortBy', sortBy);

    if (specialisation && specialisation > 0) {
      params = params.set('specialisation', specialisation);
    }

    return this.http.get<Doctor[]>(
      `${this.apiBaseUrl}/doctors`,
      { params }
    );
  }

  getDoctorById(doctorId: number): Observable<Doctor> {
    return this.http.get<Doctor>(
      `${this.apiBaseUrl}/doctors/${doctorId}`
    );
  }

  getDoctorAvailability(doctorId: number, date: string): Observable<number[]> {
    const params = new HttpParams().set('date', date);

    return this.http.get<number[]>(
      `${this.apiBaseUrl}/doctors/${doctorId}/availability`,
      { params }
    );
  }
}

