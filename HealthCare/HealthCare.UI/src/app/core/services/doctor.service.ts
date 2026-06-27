import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  private api =
    'https://localhost:7225/api/doctors';

  constructor(private http: HttpClient) {}

  getSpecialisations(): Observable<string[]> {

    return this.http.get<string[]>(
      `${this.api}/specialisations`
    );
  }

  getAvailableDoctors(
      specialisation: string,
      date: string
  ): Observable<any[]> {

    return this.http.get<any[]>(
      `${this.api}/available?specialisation=${specialisation}&date=${date}`
    );
  }
}