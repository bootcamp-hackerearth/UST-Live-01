import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { API_CONFIG } from '../../config/api.config';
import {
  DoctorLeaveCreateDto,
  DoctorLeaveCreationResponse,
  DoctorLeaveDto,
  DoctorLeavePreviewDto
} from '../../dtos/doctor-leave.dto';

@Injectable({
  providedIn: 'root'
})
export class DoctorLeaveService {
  private readonly apiUrl = `${API_CONFIG.baseUrl}/doctors/leaves`;

  constructor(private readonly http: HttpClient) {}

  previewMyLeave(
    dto: DoctorLeaveCreateDto
  ): Observable<DoctorLeavePreviewDto> {
    return this.http.post<DoctorLeavePreviewDto>(
      `${this.apiUrl}/preview`,
      dto
    );
  }

  createMyLeave(
    dto: DoctorLeaveCreateDto
  ): Observable<DoctorLeaveCreationResponse> {
    return this.http.post<DoctorLeaveCreationResponse>(this.apiUrl, dto);
  }

  getMyLeaves(): Observable<DoctorLeaveDto[]> {
    return this.http.get<DoctorLeaveDto[]>(this.apiUrl);
  }
}
