import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private baseUrl = 'https://localhost:7130/api/doctors';

  constructor(private http: HttpClient) {}

  getDoctors() {

    const token = localStorage.getItem('token');

    console.log("TOKEN ✅:", token);

    return this.http.get<any>(
      this.baseUrl,
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
  }
}
