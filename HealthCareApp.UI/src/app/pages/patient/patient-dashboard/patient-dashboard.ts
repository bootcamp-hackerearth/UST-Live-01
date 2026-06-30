import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';

import { AppointmentDto } from '../../../shared/models/appointment.models';
import { HealthRecordDto } from '../../../shared/models/health-record.models';
import { PatientDto } from '../../../shared/models/patient.models';

import { AuthService } from '../../../core/services/auth.service';
import { PatientApiService } from '../../../core/services/patient-api.service';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { HealthRecordApiService } from '../../../core/services/health-record-api.service';

import { PatientBookAppointment } from './components/book-appointment/patient-book-appointment';
import { PatientAppointmentList } from './components/appointment-list/patient-appointment-list';
import { PatientHealthRecords } from './components/health-records/patient-health-records';
import { PatientProfile } from './components/profile/patient-profile';

type PatientDashboardSection =
  | 'dashboard'
  | 'book'
  | 'appointments'
  | 'records'
  | 'profile';

type ToastType = 'success' | 'info' | 'warning';

interface DashboardSummary {
  upcomingCount: number;
  pendingCount: number;
  completedCount: number;
  healthRecordCount: number;
}

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    PatientBookAppointment,
    PatientAppointmentList,
    PatientHealthRecords,
    PatientProfile
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard implements OnInit, OnDestroy {
  activeSection: PatientDashboardSection = 'dashboard';

  patient?: PatientDto;

  summary: DashboardSummary = {
    upcomingCount: 0,
    pendingCount: 0,
    completedCount: 0,
    healthRecordCount: 0
  };

  upcomingAppointments: AppointmentDto[] = [];
  healthRecords: HealthRecordDto[] = [];

  isDashboardLoading = false;
  dashboardErrorMessage = '';

  toastMessage = '';
  toastType: ToastType = 'info';

  isSidebarOpen = false;
  isLogoutModalOpen = false;

  private toastTimer?: ReturnType<typeof setTimeout>;

  constructor(
    private authService: AuthService,
    private patientApiService: PatientApiService,
    private appointmentApiService: AppointmentApiService,
    private healthRecordApiService: HealthRecordApiService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.loadDashboardData();
    this.showToast('Welcome to your HealthAxis patient portal.', 'success');
  }

  ngOnDestroy(): void {
    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
    }
  }

  get patientFirstName(): string {
    if (!this.patient?.patientName) {
      return 'Patient';
    }

    return this.patient.patientName.split(' ')[0];
  }

  get nextAppointment(): AppointmentDto | undefined {
    return this.upcomingAppointments[0];
  }

  get latestHealthRecord(): HealthRecordDto | undefined {
    return this.healthRecords[0];
  }

 get totalOverviewCount(): number {
  return (
    this.summary.upcomingCount +
    this.summary.pendingCount +
    this.summary.completedCount +
    this.summary.healthRecordCount
  );
}

get chartTotalCount(): number {
  return this.totalOverviewCount > 0 ? this.totalOverviewCount : 1;
}


  get upcomingPercentage(): number {
    return this.calculatePercentage(this.summary.upcomingCount);
  }

  get pendingPercentage(): number {
    return this.calculatePercentage(this.summary.pendingCount);
  }

  get completedPercentage(): number {
    return this.calculatePercentage(this.summary.completedCount);
  }

  get recordPercentage(): number {
    return this.calculatePercentage(this.summary.healthRecordCount);
  }

  get chartBackground(): string {
    const upcomingEnd = this.upcomingPercentage;
    const pendingEnd = upcomingEnd + this.pendingPercentage;
    const completedEnd = pendingEnd + this.completedPercentage;

    return `
      conic-gradient(
        #2563eb 0% ${upcomingEnd}%,
        #f59e0b ${upcomingEnd}% ${pendingEnd}%,
        #16a34a ${pendingEnd}% ${completedEnd}%,
        #7c3aed ${completedEnd}% 100%
      )
    `;
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  closeSidebar(): void {
    this.isSidebarOpen = false;
  }

  setActiveSection(section: PatientDashboardSection): void {
    this.activeSection = section;
    this.closeSidebar();
  }

  handleBookingSuccess(): void {
    this.loadDashboardData();
    this.activeSection = 'dashboard';
    this.showToast('Appointment booked successfully ✅', 'success');
  }

  handleAppointmentCancel(): void {
    this.loadDashboardData();
    this.showToast('Appointment cancelled successfully ⚠️', 'warning');
  }

  handleProfileUpdated(): void {
    this.loadDashboardData();
    this.showToast('Profile updated successfully ✅', 'success');
  }

  handlePasswordChanged(): void {
    this.showToast('Password changed successfully ✅', 'success');
  }

  showToast(message: string, type: ToastType): void {
    this.toastMessage = message;
    this.toastType = type;

    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
    }

    this.toastTimer = setTimeout(() => {
      this.toastMessage = '';
    }, 3000);
  }

  dismissToast(): void {
    this.toastMessage = '';

    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
    }
  }

  logout(): void {
    this.isLogoutModalOpen = true;
    this.closeSidebar();
  }

  closeLogoutModal(): void {
    this.isLogoutModalOpen = false;
  }

  confirmLogout(): void {
    this.authService.logout();

    this.isLogoutModalOpen = false;
    this.router.navigate(['/']);
  }

  formatDate(dateValue: string): string {
    const parsedDate = new Date(dateValue);

    if (Number.isNaN(parsedDate.getTime())) {
      return 'Not Available';
    }

    return parsedDate.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getStatusClass(status: string): string {
    return `pd-status ${status.toLowerCase()}`;
  }

  retryDashboardLoad(): void {
    this.loadDashboardData();
  }

 private calculatePercentage(value: number): number {
  return Math.round((value / this.chartTotalCount) * 100);
}


  private loadDashboardData(): void {
    this.isDashboardLoading = true;
    this.dashboardErrorMessage = '';

    forkJoin({
      patient: this.patientApiService.getMyProfile(),

      upcomingAppointments: this.appointmentApiService.getMyUpcomingAppointments(),

      pendingAppointments: this.appointmentApiService.getMyAppointments({
        pageNumber: 1,
        pageSize: 1,
        status: 'Pending'
      }),

      completedAppointments: this.appointmentApiService.getMyAppointments({
        pageNumber: 1,
        pageSize: 1,
        status: 'Completed'
      }),

      healthRecords: this.healthRecordApiService.getMyHealthRecords({
        pageNumber: 1,
        pageSize: 1
      })
    }).subscribe({
      next: (result) => {
        this.patient = result.patient;

        this.upcomingAppointments = result.upcomingAppointments.sort(
          (a: AppointmentDto, b: AppointmentDto) =>
            new Date(a.scheduledDate).getTime() -
            new Date(b.scheduledDate).getTime()
        );

        this.healthRecords = result.healthRecords.items;

        this.summary = {
          upcomingCount: this.upcomingAppointments.length,
          pendingCount: result.pendingAppointments.totalRecords,
          completedCount: result.completedAppointments.totalRecords,
          healthRecordCount: result.healthRecords.totalRecords
        };

        this.isDashboardLoading = false;
      },
      error: (error: unknown) => {
        this.isDashboardLoading = false;
        this.dashboardErrorMessage = this.getErrorMessage(error);
      }
    });
  }

  private getErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
        };
      };

      return (
        apiError.error?.message ??
        apiError.error?.Message ??
        'Unable to load patient dashboard data.'
      );
    }

    return 'Unable to load patient dashboard data.';
  }
}