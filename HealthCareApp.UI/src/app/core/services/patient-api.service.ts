import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { PatientDto } from '../../shared/models/patient.models';

export interface UpdatePatientRequest {
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email: string;
  insuranceId: string;
}

interface ApiPatientDto {
  patientId: number;
  fullName?: string;
  patientName?: string;
  dateOfBirth: string;
  gender: string | number;
  email: string;
  phoneNumber: string;
  insuranceId?: string;
  insuranceID?: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientApiService {
  private readonly apiUrl = '/api/Patients';

  constructor(private readonly http: HttpClient) {
  }

  getMyProfile(): Observable<PatientDto> {
    return this.http
      .get<ApiPatientDto>(`${this.apiUrl}/me`)
      .pipe(
        map((patient: ApiPatientDto) => this.mapPatient(patient))
      );
  }

  updateMyProfile(request: UpdatePatientRequest): Observable<PatientDto> {
    return this.http
      .put<ApiPatientDto>(`${this.apiUrl}/me`, request)
      .pipe(
        map((patient: ApiPatientDto) => this.mapPatient(patient))
      );
  }

  private mapPatient(patient: ApiPatientDto): PatientDto {
    return {
      patientId: patient.patientId,
      patientName: patient.patientName ?? patient.fullName ?? 'Patient',
      dateOfBirth: this.normalizeDate(patient.dateOfBirth),
      gender: this.mapGender(patient.gender),
      email: patient.email,
      phoneNumber: patient.phoneNumber,
      insuranceId: patient.insuranceId ?? patient.insuranceID ?? ''
    } as PatientDto;
  }

  private mapGender(gender: string | number): string {
    if (typeof gender === 'string') {
      return gender;
    }

    switch (gender) {
      case 0:
        return 'Male';

      case 1:
        return 'Female';

      case 2:
        return 'Transgender';

      case 3:
        return 'Other';

      default:
        return 'Other';
    }
  }

  private normalizeDate(dateValue: string): string {
    if (!dateValue) {
      return '';
    }

    return dateValue.split('T')[0];
  }
}