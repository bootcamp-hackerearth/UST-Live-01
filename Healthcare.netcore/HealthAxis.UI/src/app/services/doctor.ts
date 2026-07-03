import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private readonly baseUrl = 'https://localhost:7130/api/doctors';

  constructor(private readonly  http: HttpClient) {}

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
