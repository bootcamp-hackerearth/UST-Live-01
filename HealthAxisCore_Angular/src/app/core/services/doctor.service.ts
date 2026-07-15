import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpParams
} from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { DoctorDto } from '../models/doctor.model';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly apiUrl = `${environment.apiBaseUrl}/doctors`;

  constructor(private readonly httpClient: HttpClient) {
  }

  getDoctors(specialisation?: string): Observable<DoctorDto[]> {
    let params = new HttpParams();

    if (specialisation) {
      params = params.set('specialisation', specialisation);
    }

    return this.httpClient.get<DoctorDto[]>(
      this.apiUrl,
      {
        params
      }
    );
  }

  getById(doctorId: number): Observable<DoctorDto> {
    return this.httpClient.get<DoctorDto>(
      `${this.apiUrl}/${doctorId}`
    );
  }

  updateOwnStatus(
    doctorId: number,
    isActive: boolean
  ): Observable<DoctorDto> {
    return this.httpClient.put<DoctorDto>(
      `${this.apiUrl}/${doctorId}/status`,
      null,
      {
        params: {
          isActive
        }
      }
    );
  }

  getAvailability(
    doctorId: number,
    date: string
  ): Observable<string[]> {
    const params = new HttpParams().set('date', date);

    return this.httpClient.get<string[]>(
      `${this.apiUrl}/${doctorId}/availability`,
      {
        params
      }
    );
  }
}
