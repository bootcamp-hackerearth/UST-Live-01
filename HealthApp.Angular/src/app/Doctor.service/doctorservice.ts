import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class doctorservice {

  private baseUrl = 'https://localhost:7066/api/doctors';

  constructor(private http: HttpClient) {}

  getMyProfile() {
    return this.http.get<any>(`${this.baseUrl}/me`, {
      headers: this.getHeaders()
    });
  }

  private getHeaders() {
    return {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    };
  }
}