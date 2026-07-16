import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private readonly baseUrl = 'http://localhost:5066/api/doctors';

  constructor(private readonly http: HttpClient) {}

  getMyProfile() {
    return this.http.get<any>(`${this.baseUrl}/me`, {
      headers: this.getHeaders()
    });
  }

  private getHeaders(): { Authorization: string } {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    };
  }
}