import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  DoctorAvailabilityDto,
  DoctorDto,
  DoctorStatusUpdateResponse
} from '../../dtos/doctor.dto';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/doctors`;

  constructor(private readonly http: HttpClient) {}

  getDoctors(
    search?: string,
    specialisation?: string,
    isActive?: boolean
  ): Observable<DoctorDto[]> {
    let params = new HttpParams();

    if (typeof isActive === 'boolean') {
      params = params.set('isActive', String(isActive));
    }

    if (search?.trim()) {
      params = params.set('search', search.trim());
    }

    if (specialisation?.trim() && specialisation !== 'All') {
      params = params.set('specialisation', specialisation.trim());
    }

    return this.http.get<DoctorDto[]>(this.apiUrl, { params });
  }

  getDoctorById(id: number): Observable<DoctorDto> {
    return this.http.get<DoctorDto>(`${this.apiUrl}/${id}`);
  }

  getDoctorsBySpecialisation(
    specialisation: string
  ): Observable<DoctorDto[]> {
    return this.http.get<DoctorDto[]>(
      `${this.apiUrl}/specialisation/${specialisation}`
    );
  }

  getMyProfile(): Observable<DoctorDto> {
    return this.http.get<DoctorDto>(`${this.apiUrl}/profile`);
  }

  getMyAvailability(date: string): Observable<DoctorAvailabilityDto> {
    return this.http.get<DoctorAvailabilityDto>(
      `${this.apiUrl}/profile/availability`,
      {
        params: { date }
      }
    );
  }

  getDoctorAvailability(
    doctorId: number,
    date: string
  ): Observable<DoctorAvailabilityDto> {
    return this.http.get<DoctorAvailabilityDto>(
      `${this.apiUrl}/${doctorId}/availability`,
      {
        params: { date }
      }
    );
  }

  changeMyStatus(
    isActive: boolean
  ): Observable<DoctorStatusUpdateResponse> {
    return this.http.patch<DoctorStatusUpdateResponse>(
      `${this.apiUrl}/profile/status`,
      null,
      {
        params: { isActive }
      }
    );
  }
}