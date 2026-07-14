import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreateDoctorLeaveRequest,
  DoctorLeaveDto
} from '../../shared/models/doctor-leave.models';

@Injectable({
  providedIn: 'root'
})
export class DoctorLeaveApiService {
  private readonly apiUrl = 'https://localhost:7250/api/DoctorLeaves';

  constructor(private readonly http: HttpClient) {
  }

  createMyLeave(request: CreateDoctorLeaveRequest): Observable<DoctorLeaveDto> {
    return this.http.post<DoctorLeaveDto>(
      `${this.apiUrl}/my`,
      request
    );
  }

  getMyLeaves(): Observable<DoctorLeaveDto[]> {
    return this.http.get<DoctorLeaveDto[]>(
      `${this.apiUrl}/my`
    );
  }

  getDoctorLeaves(doctorId: number): Observable<DoctorLeaveDto[]> {
    return this.http.get<DoctorLeaveDto[]>(
      `${this.apiUrl}/doctor/${doctorId}`
    );
  }
}