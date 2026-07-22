import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import {
  CreateMyDoctorLeaveDto,
  DoctorLeaveDto
} from '../../shared/models/doctor-leave.models';

interface ApiDoctorLeaveDto {
  doctorLeaveId?: number;
  DoctorLeaveId?: number;

  doctorId?: number;
  DoctorId?: number;

  doctorName?: string;
  DoctorName?: string;

  startDate?: string;
  StartDate?: string;

  endDate?: string;
  EndDate?: string;

  reason?: string;
  Reason?: string;

  createdDate?: string;
  CreatedDate?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorLeaveApiService {
  private readonly apiUrl = '/api/DoctorLeaves';

  constructor(private readonly http: HttpClient) {
  }

  createMyDoctorLeave(
    request: CreateMyDoctorLeaveDto
  ): Observable<DoctorLeaveDto> {
    return this.http
      .post<ApiDoctorLeaveDto>(`${this.apiUrl}/my-leaves`, request)
      .pipe(
        map((doctorLeave: ApiDoctorLeaveDto) =>
          this.mapDoctorLeave(doctorLeave)
        )
      );
  }

  getMyDoctorLeaves(): Observable<DoctorLeaveDto[]> {
    return this.http
      .get<ApiDoctorLeaveDto[]>(`${this.apiUrl}/my-leaves`)
      .pipe(
        map((doctorLeaves: ApiDoctorLeaveDto[]) =>
          (doctorLeaves ?? []).map((doctorLeave: ApiDoctorLeaveDto) =>
            this.mapDoctorLeave(doctorLeave)
          )
        )
      );
  }

  private mapDoctorLeave(doctorLeave: ApiDoctorLeaveDto): DoctorLeaveDto {
    return {
      doctorLeaveId:
        doctorLeave.doctorLeaveId ?? doctorLeave.DoctorLeaveId ?? 0,

      doctorId:
        doctorLeave.doctorId ?? doctorLeave.DoctorId ?? 0,

      doctorName:
        doctorLeave.doctorName ?? doctorLeave.DoctorName ?? 'Doctor',

      startDate:
        this.normalizeDate(doctorLeave.startDate ?? doctorLeave.StartDate ?? ''),

      endDate:
        this.normalizeDate(doctorLeave.endDate ?? doctorLeave.EndDate ?? ''),

      reason:
        doctorLeave.reason ?? doctorLeave.Reason ?? '',

      createdDate:
        this.normalizeDate(doctorLeave.createdDate ?? doctorLeave.CreatedDate ?? '')
    };
  }

  private normalizeDate(dateValue: string): string {
    if (!dateValue) {
      return '';
    }

    return dateValue.split('T')[0];
  }
}
