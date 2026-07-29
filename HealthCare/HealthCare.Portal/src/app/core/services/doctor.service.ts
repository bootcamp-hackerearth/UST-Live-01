import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import {
  CreateLeaveRequest,
  CreateLeaveResult,
  DoctorDashboardSummary,
  DoctorList,
  DoctorProfile
} from '../models/portal.models';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  private readonly apiBaseUrl = environment.apiBaseUrl;

  profile = signal<DoctorProfile | null>(null);
  availableDoctors = signal<DoctorList[]>([]);
  availableSlots = signal<string[]>([]);
  slotsLoaded = signal(false);

  dashboardSummary = signal<DoctorDashboardSummary>({
    upcomingAppointments: 0,
    completedAppointments: 0,
    upcomingLeaves: 0,
    todaysAppointments: 0
  });

  isLoading = signal(false);
  errorMessage = signal('');

  constructor(private readonly http: HttpClient) { }
  loadMyProfile(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<DoctorProfile>(`${this.apiBaseUrl}/api/doctors/profile`)
      .subscribe({
        next: (res) => {
          this.profile.set(res);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Failed to load doctor profile:', error);
          this.profile.set(null);
          this.errorMessage.set('Failed to load doctor profile.');
          this.isLoading.set(false);
        }
      });
  }

  loadAvailableDoctors(specialisation: string, date: string): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.availableDoctors.set([]);
    this.availableSlots.set([]);
    this.slotsLoaded.set(false);

    const params = new HttpParams()
      .set('specialisation', specialisation.trim())
      .set('date', date);

    this.http.get<DoctorList[]>(
      `${this.apiBaseUrl}/api/doctors/available`,
      { params }
    ).subscribe({
      next: (res) => {
        this.availableDoctors.set(res || []);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load available doctors:', error);
        this.availableDoctors.set([]);
        this.errorMessage.set('Failed to load available doctors.');
        this.isLoading.set(false);
      }
    });
  }

  loadAvailableSlots(doctorId: number, date: string): void {
    this.availableSlots.set([]);
    this.slotsLoaded.set(false);
    this.errorMessage.set('');

    const params = new HttpParams()
      .set('date', date)
      .set('doctorId', doctorId.toString());

    this.http.get<string[]>(
      `${this.apiBaseUrl}/api/appointments/available-slots`,
      { params }
    ).subscribe({
      next: (res) => {
        this.availableSlots.set(res || []);
        this.slotsLoaded.set(true);
      },
      error: (error) => {
        console.error('Failed to load available slots:', error);
        this.availableSlots.set([]);
        this.slotsLoaded.set(true);
        this.errorMessage.set(
          error?.error?.message || 'Failed to load available slots.'
        );
      }
    });
  }

  addLeaves(leaves: CreateLeaveRequest[]) {
    return this.http.post<CreateLeaveResult>(
      `${this.apiBaseUrl}/api/doctors/leaves`,
      leaves
    );
  }

  loadDashboardSummary(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.http.get<DoctorDashboardSummary>(
      `${this.apiBaseUrl}/api/doctors/dashboard-summary`
    ).subscribe({
      next: (res) => {
        this.dashboardSummary.set({
          upcomingAppointments: res?.upcomingAppointments ?? 0,
          completedAppointments: res?.completedAppointments ?? 0,
          upcomingLeaves: res?.upcomingLeaves ?? 0,
          todaysAppointments: res?.todaysAppointments ?? 0
        });

        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Failed to load doctor dashboard summary:', error);

        this.dashboardSummary.set({
          upcomingAppointments: 0,
          completedAppointments: 0,
          upcomingLeaves: 0,
          todaysAppointments: 0
        });

        this.errorMessage.set('Failed to load dashboard summary.');
        this.isLoading.set(false);
      }
    });
  }

  clearAvailableDoctors(): void {
    this.availableDoctors.set([]);
  }

  clearAvailableSlots(): void {
    this.availableSlots.set([]);
    this.slotsLoaded.set(false);
  }

  clearDashboardSummary(): void {
    this.dashboardSummary.set({
      upcomingAppointments: 0,
      completedAppointments: 0,
      upcomingLeaves: 0,
      todaysAppointments: 0
    });
  }
}
