import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin, timeout } from 'rxjs';

import { AppointmentDto } from '../../../shared/models/appointment.models';
import { DoctorDto } from '../../../shared/models/doctor.models';
import { HealthRecordDto } from '../../../shared/models/health-record.models';

import { AuthService } from '../../../core/services/auth.service';
import { DoctorApiService } from '../../../core/services/doctor-api.service';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { HealthRecordApiService } from '../../../core/services/health-record-api.service';

import { DoctorAppointmentList } from './components/appointment-list/doctor-appointment-list';
import { DoctorHealthRecords } from './components/health-records/doctor-health-records';
import { DoctorProfile } from './components/profile/doctor-profile';

type DoctorDashboardSection =
  | 'dashboard'
  | 'appointments'
  | 'records'
  | 'profile';

type ToastType = 'success' | 'info' | 'warning';

interface DoctorDashboardSummary {
  upcomingCount: number;
  pendingCount: number;
  confirmedCount: number;
  completedCount: number;
  healthRecordCount: number;
}

interface DoctorToastEvent {
  message: string;
  type: ToastType;
}

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [
    FormsModule,
    DoctorAppointmentList,
    DoctorHealthRecords,
    DoctorProfile
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard implements OnInit, OnDestroy {
  activeSection: DoctorDashboardSection = 'dashboard';

  doctor?: DoctorDto;

  summary: DoctorDashboardSummary = {
    upcomingCount: 0,
    pendingCount: 0,
    confirmedCount: 0,
    completedCount: 0,
    healthRecordCount: 0
  };

  upcomingAppointments: AppointmentDto[] = [];
  recentHealthRecords: HealthRecordDto[] = [];

  isDashboardLoading = false;
  dashboardErrorMessage = '';

  isSidebarOpen = false;
  isLogoutModalOpen = false;

  toastMessage = '';
  toastType: ToastType = 'info';

  private toastTimer?: ReturnType<typeof setTimeout>;

  constructor(
    private authService: AuthService,
    private doctorApiService: DoctorApiService,
    private appointmentApiService: AppointmentApiService,
    private healthRecordApiService: HealthRecordApiService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.loadDashboardData();
    this.showToast('Welcome to your HealthAxis doctor portal.', 'success');
  }

  ngOnDestroy(): void {
    if (this.toastTimer) {
      clearTimeout(this.toastTimer);
    }
  }

  get doctorFirstName(): string {
    if (!this.doctor?.doctorName) {
      return 'Doctor';
    }

    return this.doctor.doctorName.replace('Dr. ', '').split(' ')[0];
  }

  get nextAppointment(): AppointmentDto | undefined {
    return this.upcomingAppointments[0];
  }

  get latestHealthRecord(): HealthRecordDto | undefined {
    return this.recentHealthRecords[0];
  }

  get totalOverviewCount(): number {
    const total =
      this.summary.upcomingCount +
      this.summary.pendingCount +
      this.summary.confirmedCount +
      this.summary.completedCount +
      this.summary.healthRecordCount;

    return total > 0 ? total : 1;
  }

  get upcomingPercentage(): number {
    return this.calculatePercentage(this.summary.upcomingCount);
  }

  get pendingPercentage(): number {
    return this.calculatePercentage(this.summary.pendingCount);
  }

  get confirmedPercentage(): number {
    return this.calculatePercentage(this.summary.confirmedCount);
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
    const confirmedEnd = pendingEnd + this.confirmedPercentage;
    const completedEnd = confirmedEnd + this.completedPercentage;

    return `
      conic-gradient(
        #2563eb 0% ${upcomingEnd}%,
        #f59e0b ${upcomingEnd}% ${pendingEnd}%,
        #0ea5e9 ${pendingEnd}% ${confirmedEnd}%,
        #16a34a ${confirmedEnd}% ${completedEnd}%,
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

  setActiveSection(section: DoctorDashboardSection): void {
    this.activeSection = section;
    this.closeSidebar();
  }

  handleDoctorDataChanged(): void {
    this.loadDashboardData();
  }

  handleDoctorToast(event: DoctorToastEvent): void {
    this.showToast(event.message, event.type);
  }

  openLogoutModal(): void {
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

  retryDashboardLoad(): void {
    this.loadDashboardData();
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
    return `dd-status ${status.toLowerCase()}`;
  }

  private loadDashboardData(): void {
    this.isDashboardLoading = true;
    this.dashboardErrorMessage = '';

    forkJoin({
      doctor: this.doctorApiService.getMyProfile(),

      upcomingAppointments: this.appointmentApiService.getMyUpcomingAppointments(),

      pendingAppointments: this.appointmentApiService.getMyAppointments({
        pageNumber: 1,
        pageSize: 1,
        status: 'Pending'
      }),

      confirmedAppointments: this.appointmentApiService.getMyAppointments({
        pageNumber: 1,
        pageSize: 1,
        status: 'Confirmed'
      }),

      completedAppointments: this.appointmentApiService.getMyAppointments({
        pageNumber: 1,
        pageSize: 1,
        status: 'Completed'
      }),

      healthRecords: this.healthRecordApiService.getMyHealthRecords({
        pageNumber: 1,
        pageSize: 3
      })
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: (result) => {
        this.doctor = result.doctor;

        this.upcomingAppointments = result.upcomingAppointments.sort(
          (a: AppointmentDto, b: AppointmentDto) =>
            new Date(a.scheduledDate).getTime() -
            new Date(b.scheduledDate).getTime()
        );

        this.recentHealthRecords = result.healthRecords.items;

        this.summary = {
          upcomingCount: this.upcomingAppointments.length,
          pendingCount: result.pendingAppointments.totalRecords,
          confirmedCount: result.confirmedAppointments.totalRecords,
          completedCount: result.completedAppointments.totalRecords,
          healthRecordCount: result.healthRecords.totalRecords
        };

        this.isDashboardLoading = false;
      },
      error: (error: unknown) => {
        console.log('Doctor dashboard API error:', error);
        this.isDashboardLoading = false;
        this.dashboardErrorMessage = this.getErrorMessage(error);
      }
    });
  }

  private calculatePercentage(value: number): number {
    return Math.round((value / this.totalOverviewCount) * 100);
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
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      return (
        apiError.error?.message ??
        apiError.error?.Message ??
        'Unable to load doctor dashboard data.'
      );
    }

    return 'Unable to load doctor dashboard data.';
  }
}