import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';

import { DoctorDto } from '../../shared/models/doctor.models';

interface ApiDoctorDto {
  doctorId: number;
  fullName?: string;
  doctorName?: string;
  specialisation: string | number;
  email?: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorApiService {
  private readonly apiUrl = 'https://localhost:7250/api/Doctors';

  constructor(private http: HttpClient) {
  }

  getMyProfile(): Observable<DoctorDto> {
    return this.http
      .get<ApiDoctorDto>(`${this.apiUrl}/me`)
      .pipe(
        map((doctor: ApiDoctorDto) => this.mapDoctor(doctor))
      );
  }

  getAllActiveDoctors(): Observable<DoctorDto[]> {
    return this.http
      .get<ApiDoctorDto[]>(this.apiUrl)
      .pipe(
        map((doctors: ApiDoctorDto[]) =>
          doctors.map((doctor: ApiDoctorDto) => this.mapDoctor(doctor))
        )
      );
  }

  getDoctorById(doctorId: number): Observable<DoctorDto> {
    return this.http
      .get<ApiDoctorDto>(`${this.apiUrl}/${doctorId}`)
      .pipe(
        map((doctor: ApiDoctorDto) => this.mapDoctor(doctor))
      );
  }

  getDoctorAvailability(doctorId: number): Observable<string[]> {
    return this.http.get<string[]>(
      `${this.apiUrl}/${doctorId}/availability`
    );
  }

  private mapDoctor(doctor: ApiDoctorDto): DoctorDto {
    return {
      doctorId: doctor.doctorId,
      doctorName: doctor.doctorName ?? doctor.fullName ?? 'Doctor',
      specialisation: this.mapSpecialisation(doctor.specialisation),
      yearsOfExperience: doctor.yearsOfExperience,
      consultationFee: doctor.consultationFee,
      isActive: doctor.isActive
    } as DoctorDto;
  }

  private mapSpecialisation(specialisation: string | number): string {
    if (typeof specialisation === 'string') {
      return this.formatSpecialisationText(specialisation);
    }

    switch (specialisation) {
      case 0:
        return 'Endocrinologist';

      case 1:
        return 'Oncologist';

      case 2:
        return 'Gynecologist';

      case 3:
        return 'Orthopedic Surgeon';

      case 4:
        return 'Psychiatrist';

      case 5:
        return 'Pediatrician';

      case 6:
        return 'Neurologist';

      case 7:
        return 'Dermatologist';

      case 8:
        return 'Cardiologist';

      case 9:
        return 'General Practitioner';

      default:
        return 'General Practitioner';
    }
  }

  private formatSpecialisationText(value: string): string {
    return value
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace('GeneralPractitioner', 'General Practitioner')
      .replace('OrthopedicSurgeon', 'Orthopedic Surgeon');
  }
}