import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  PatientCreateDto,
  PatientDto,
  PatientUpdateResponse
} from '../../dtos/patient.dto';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly profileApiUrl = `${API_CONFIG.baseUrl}/patients/profile`;

  constructor(private readonly http: HttpClient) {}

  getMyProfile(): Observable<PatientDto> {
    return this.http.get<PatientDto>(this.profileApiUrl);
  }

  updateMyProfile(payload: PatientCreateDto): Observable<PatientUpdateResponse> {
    return this.http.put<PatientUpdateResponse>(this.profileApiUrl, payload);
  }
}