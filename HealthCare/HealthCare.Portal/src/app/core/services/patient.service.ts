import { HttpClient } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  HealthRecordResponse,
  PatientProfile,
  UpdatePatientProfileRequest
} from '../models/portal.models';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  profile = signal<PatientProfile | null>(null);
  records = signal<HealthRecordResponse[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  constructor(private readonly http: HttpClient) { }
  loadProfile(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<PatientProfile | null>(`${this.apiBaseUrl}/api/patients/profile`)
      .subscribe({
        next: (res) => {
          if (res) {
            this.profile.set(res);
          } else {
            this.profile.set(null);
            this.errorMessage.set('Patient profile not found.');
          }

          this.isLoading.set(false);
        },

        error: (error) => {
          console.error('Failed to load patient profile:', error);

          this.profile.set(null);

          if (error.status === 404) {
            this.errorMessage.set('Patient profile not found.');
          } else if (error.status === 401) {
            this.errorMessage.set('Session expired. Please login again.');
          } else if (error.status === 403) {
            this.errorMessage.set('You are not allowed to view this profile.');
          } else {
            this.errorMessage.set('Failed to load patient profile.');
          }

          this.isLoading.set(false);
        }
      });
  }

  updateProfile(request: UpdatePatientProfileRequest) {
    return this.http.put(
      `${this.apiBaseUrl}/api/patients/profile`,
      request
    );
  }

  loadRecords(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<HealthRecordResponse[]>(`${this.apiBaseUrl}/api/records/my-records`)
      .subscribe({
        next: (res) => {
          this.records.set(res || []);
          this.isLoading.set(false);
        },

        error: (error) => {
          console.error('Failed to load health records:', error);

          this.records.set([]);

          if (error.status === 404) {
            this.errorMessage.set('No health records found.');
          } else if (error.status === 401) {
            this.errorMessage.set('Session expired. Please login again.');
          } else {
            this.errorMessage.set('Failed to load health records.');
          }

          this.isLoading.set(false);
        }
      });
  }

  clearProfile(): void {
    this.profile.set(null);
    this.errorMessage.set('');
  }

  clearRecords(): void {
    this.records.set([]);
    this.errorMessage.set('');
  }
}
