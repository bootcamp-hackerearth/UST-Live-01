import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

export interface DoctorAvailability {
  doctorId: number;
  fullName: string;
  isActive: boolean;
  date: string;
  availableSlots: string[];
}

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private readonly baseUrl = '/api/doctors';

  constructor(private readonly http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = globalThis.window === undefined
    ? null
    : globalThis.window.localStorage.getItem('token');


    if (token) {
      return new HttpHeaders({
        Authorization: `Bearer ${token}`
      });
    }

    return new HttpHeaders();
  }

  getDoctors(pageNumber: number = 1, pageSize: number = 10) {
    return this.http.get<any>(
      this.baseUrl,
      {
        headers: this.getHeaders(),
        params: {
          pageNumber,
          pageSize
        }
      }
    );
  }

  getDoctorAvailability(doctorId: number, date: string) {
    return this.http.get<DoctorAvailability>(
      `${this.baseUrl}/${doctorId}/availability`,
      {
        headers: this.getHeaders(),
        params: {
          date
        }
      }
    );
  }
}