import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  AppointmentList,
  CreateAppointmentRequest
} from '../models/portal.models';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  patientAppointments = signal<AppointmentList[]>([]);
  doctorAppointments = signal<AppointmentList[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  constructor(private readonly http: HttpClient) { }
  bookAppointment(request: CreateAppointmentRequest) {
    return this.http.post(
      `${this.apiBaseUrl}/api/appointments/book`,
      request
    );
  }

  loadPatientAppointments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<AppointmentList[]>(
      `${this.apiBaseUrl}/api/appointments/patient/upcoming`
    ).subscribe({
      next: (res) => {
        this.patientAppointments.set(res || []);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load patient appointments:', error);
        this.patientAppointments.set([]);
        this.errorMessage.set('Failed to load patient appointments.');
        this.isLoading.set(false);
      }
    });
  }

  loadDoctorAppointments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<AppointmentList[]>(
      `${this.apiBaseUrl}/api/appointments/doctor/upcoming`
    ).subscribe({
      next: (res) => {
        this.doctorAppointments.set(res || []);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load doctor appointments:', error);
        this.doctorAppointments.set([]);
        this.errorMessage.set('Failed to load doctor appointments.');
        this.isLoading.set(false);
      }
    });
  }

  updateAppointmentStatus(
    appointmentId: number,
    status: string,
    cancellationReason?: string
  ) {
    return this.http.put(
      `${this.apiBaseUrl}/api/appointments/${appointmentId}/status`,
      {
        status,
        cancellationReason
      }
    );
  }

  clearPatientAppointments(): void {
    this.patientAppointments.set([]);
    this.errorMessage.set('');
  }

  clearDoctorAppointments(): void {
    this.doctorAppointments.set([]);
    this.errorMessage.set('');
  }
}
