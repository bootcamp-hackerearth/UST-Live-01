import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin, timeout } from 'rxjs';

import { AppointmentDto } from '../../../shared/models/appointment.models';
import { DoctorLeaveDto } from '../../../shared/models/doctor-leave.models';
import { DoctorDto } from '../../../shared/models/doctor.models';
import { HealthRecordDto } from '../../../shared/models/health-record.models';

import { AuthService } from '../../../core/services/auth.service';
import { DoctorApiService } from '../../../core/services/doctor-api.service';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { HealthRecordApiService } from '../../../core/services/health-record-api.service';
import { DoctorLeaveApiService } from '../../../core/services/doctor-leave-api.service';

import { DoctorAppointmentList } from './components/appointment-list/doctor-appointment-list';
import { DoctorHealthRecords } from './components/health-records/doctor-health-records';
import { DoctorProfile } from './components/profile/doctor-profile';

type DoctorDashboardSection =
  | 'dashboard'
  | 'appointments'
  | 'records'
  | 'leave'
  | 'profile';

type ToastType = 'success' | 'info' | 'warning';

interface DoctorDashboardSummary {
  upcomingCount: number;
  pendingCount: number;
  confirmedCount: number;
  completedCount: number;
  healthRecordCount: number;
}

interface DoctorPasswordForm {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

interface DoctorLeaveForm {
  startDate: string;
  endDate: string;
  reason: string;
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
  doctorLeaves: DoctorLeaveDto[] = [];

  todayDate = '';

  isDashboardLoading = false;
  dashboardErrorMessage = '';

  isLoadingLeaves = false;
  isSubmittingLeave = false;
  isLeaveConfirmOpen = false;
  leaveMessage = '';

  isSidebarOpen = false;
  isLogoutModalOpen = false;

  mustChangeTemporaryPassword = false;
  isTempPasswordConfirmOpen = false;
  hasPasswordSubmitted = false;
  passwordMessage = '';
  isChangingTemporaryPassword = false;

  passwordForm: DoctorPasswordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  leaveForm: DoctorLeaveForm = {
    startDate: '',
    endDate: '',
    reason: ''
  };

  toastMessage = '';
  toastType: ToastType = 'info';

  private toastTimer?: ReturnType<typeof setTimeout>;

  constructor(
    private readonly authService: AuthService,
    private readonly doctorApiService: DoctorApiService,
    private readonly appointmentApiService: AppointmentApiService,
    private readonly healthRecordApiService: HealthRecordApiService,
    private readonly doctorLeaveApiService: DoctorLeaveApiService,
    private readonly router: Router
  ) {
    this.todayDate = this.formatDateForInput(new Date());
  }

  ngOnInit(): void {
    this.mustChangeTemporaryPassword = this.authService.getMustChangePassword();

    this.loadDashboardData();

    if (!this.mustChangeTemporaryPassword) {
      this.loadDoctorLeaves();
    }

    if (this.mustChangeTemporaryPassword) {
      this.showToast('Please change your temporary password to continue.', 'warning');
      return;
    }

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

  get latestDoctorLeave(): DoctorLeaveDto | undefined {
    return this.doctorLeaves[0];
  }

  get totalOverviewCount(): number {
    return (
      this.summary.upcomingCount +
      this.summary.pendingCount +
      this.summary.confirmedCount +
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

  get isCurrentPasswordInvalid(): boolean {
    return !this.passwordForm.currentPassword.trim();
  }

  get isNewPasswordInvalid(): boolean {
    const password = this.passwordForm.newPassword;
    const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    return !passwordPattern.test(password);
  }

  get isConfirmPasswordInvalid(): boolean {
    return !this.passwordForm.confirmPassword.trim();
  }

  get isPasswordMismatch(): boolean {
    return (
      this.passwordForm.confirmPassword.trim().length > 0 &&
      this.passwordForm.newPassword !== this.passwordForm.confirmPassword
    );
  }

  get isSamePasswordInvalid(): boolean {
    return (
      this.passwordForm.currentPassword.trim().length > 0 &&
      this.passwordForm.currentPassword === this.passwordForm.newPassword
    );
  }

  get isPasswordFormInvalid(): boolean {
    return (
      this.isCurrentPasswordInvalid ||
      this.isNewPasswordInvalid ||
      this.isConfirmPasswordInvalid ||
      this.isPasswordMismatch ||
      this.isSamePasswordInvalid
    );
  }

  get isLeaveDateRangeInvalid(): boolean {
    if (!this.leaveForm.startDate || !this.leaveForm.endDate) {
      return false;
    }

    const startDate = this.parseInputDate(this.leaveForm.startDate);
    const endDate = this.parseInputDate(this.leaveForm.endDate);

    if (!startDate || !endDate) {
      return true;
    }

    return endDate.getTime() < startDate.getTime();
  }

  get isLeaveStartDatePast(): boolean {
    if (!this.leaveForm.startDate) {
      return false;
    }

    const startDate = this.parseInputDate(this.leaveForm.startDate);
    const today = this.parseInputDate(this.todayDate);

    if (!startDate || !today) {
      return true;
    }

    return startDate.getTime() < today.getTime();
  }

  get isLeaveReasonInvalid(): boolean {
    return (
      this.leaveForm.reason.trim().length === 0 ||
      this.leaveForm.reason.trim().length > 300
    );
  }

  get isLeaveFormInvalid(): boolean {
    return (
      !this.leaveForm.startDate ||
      !this.leaveForm.endDate ||
      this.isLeaveStartDatePast ||
      this.isLeaveDateRangeInvalid ||
      this.isLeaveReasonInvalid
    );
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  closeSidebar(): void {
    this.isSidebarOpen = false;
  }

  setActiveSection(section: DoctorDashboardSection): void {
    if (this.mustChangeTemporaryPassword) {
      this.showToast('Change your temporary password first.', 'warning');
      return;
    }

    this.activeSection = section;
    this.closeSidebar();

    if (section === 'leave') {
      this.loadDoctorLeaves();
    }
  }

  submitTemporaryPasswordChange(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = true;

    if (this.isPasswordFormInvalid) {
      this.passwordMessage = 'Please correct the highlighted password fields.';
      return;
    }

    this.isTempPasswordConfirmOpen = true;
  }

  closeTempPasswordConfirm(): void {
    if (this.isChangingTemporaryPassword) {
      return;
    }

    this.isTempPasswordConfirmOpen = false;
  }

  confirmTemporaryPasswordChange(): void {
    this.passwordMessage = '';
    this.isChangingTemporaryPassword = true;

    this.authService.changePassword({
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword,
      confirmNewPassword: this.passwordForm.confirmPassword
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isChangingTemporaryPassword = false;
        this.isTempPasswordConfirmOpen = false;
        this.mustChangeTemporaryPassword = false;
        this.hasPasswordSubmitted = false;

        this.authService.markPasswordChangeCompleted();
        this.resetPasswordForm();
        this.loadDashboardData();
        this.loadDoctorLeaves();

        this.showToast('Password changed successfully. Dashboard unlocked ', 'success');
      },
      error: (error: unknown) => {
        console.log('Doctor temporary password change API error:', error);

        this.isChangingTemporaryPassword = false;
        this.isTempPasswordConfirmOpen = false;
        this.passwordMessage = this.getErrorMessage(error);
      }
    });
  }

  submitDoctorLeave(): void {
    this.leaveMessage = '';

    if (this.isLeaveFormInvalid) {
      this.leaveMessage = this.getLeaveValidationMessage();
      this.showToast(this.leaveMessage, 'warning');
      return;
    }

    this.isLeaveConfirmOpen = true;
  }

  closeLeaveConfirmModal(): void {
    if (this.isSubmittingLeave) {
      return;
    }

    this.isLeaveConfirmOpen = false;
  }

  confirmDoctorLeave(): void {
    this.leaveMessage = '';

    if (this.isLeaveFormInvalid) {
      this.isLeaveConfirmOpen = false;
      this.leaveMessage = this.getLeaveValidationMessage();
      this.showToast(this.leaveMessage, 'warning');
      return;
    }

    this.isSubmittingLeave = true;

    this.doctorLeaveApiService.createMyDoctorLeave({
      startDate: this.leaveForm.startDate,
      endDate: this.leaveForm.endDate,
      reason: this.leaveForm.reason.trim()
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isSubmittingLeave = false;
        this.isLeaveConfirmOpen = false;

        this.leaveMessage = 'Leave submitted successfully. Your availability has been updated.';

        this.showToast(this.leaveMessage, 'success');

        this.resetLeaveForm();
        this.loadDoctorLeaves();
        this.loadDashboardData();
      },
      error: (error: unknown) => {
        console.log('Doctor leave create API error:', error);

        this.isSubmittingLeave = false;
        this.isLeaveConfirmOpen = false;

        this.leaveMessage = this.getErrorMessage(error);

        this.showToast(this.leaveMessage, 'warning');
      }
    });
  }

  handleDoctorDataChanged(): void {
    this.loadDashboardData();
    this.loadDoctorLeaves();
  }

  handleDoctorToast(event: DoctorToastEvent): void {
    this.showToast(event.message, event.type);
  }

  openLogoutModal(): void {
    this.closeSidebar();

    this.isTempPasswordConfirmOpen = false;
    this.isLeaveConfirmOpen = false;

    setTimeout(() => {
      this.isLogoutModalOpen = true;
    }, 0);
  }

  closeLogoutModal(): void {
    this.isLogoutModalOpen = false;
  }

  confirmLogout(): void {
    this.isLogoutModalOpen = false;

    this.closeSidebar();

    this.authService.logout();

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

        const sortedUpcomingAppointments = [...result.upcomingAppointments];

        sortedUpcomingAppointments.sort(
          (a: AppointmentDto, b: AppointmentDto) =>
            new Date(a.scheduledDate).getTime() -
            new Date(b.scheduledDate).getTime()
        );

        this.upcomingAppointments = sortedUpcomingAppointments;

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

  private loadDoctorLeaves(): void {
    if (this.mustChangeTemporaryPassword) {
      return;
    }

    this.isLoadingLeaves = true;

    this.doctorLeaveApiService.getMyDoctorLeaves().pipe(
      timeout(15000)
    ).subscribe({
      next: (doctorLeaves: DoctorLeaveDto[]) => {
        this.doctorLeaves = doctorLeaves ?? [];
        this.isLoadingLeaves = false;
      },
      error: (error: unknown) => {
        console.log('Doctor leaves API error:', error);

        this.doctorLeaves = [];
        this.isLoadingLeaves = false;

        this.showToast(
          'Unable to load leave history. Please try again.',
          'warning'
        );
      }
    });
  }

  private calculatePercentage(value: number): number {
    return Math.round((value / this.chartTotalCount) * 100);
  }

  private resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  private resetLeaveForm(): void {
    this.leaveForm = {
      startDate: '',
      endDate: '',
      reason: ''
    };
  }

  private getLeaveValidationMessage(): string {
    if (!this.leaveForm.startDate) {
      return 'Please select leave start date.';
    }

    if (!this.leaveForm.endDate) {
      return 'Please select leave end date.';
    }

    if (this.isLeaveStartDatePast) {
      return 'Leave start date cannot be in the past.';
    }

    if (this.isLeaveDateRangeInvalid) {
      return 'Leave end date cannot be before start date.';
    }

    if (!this.leaveForm.reason.trim()) {
      return 'Please enter leave reason.';
    }

    if (this.leaveForm.reason.trim().length > 300) {
      return 'Leave reason cannot exceed 300 characters.';
    }

    return 'Please correct the leave details.';
  }

  private parseInputDate(dateValue: string): Date | null {
    const parts = dateValue.split('-');

    if (parts.length !== 3) {
      return null;
    }

    const year = Number(parts[0]);
    const month = Number(parts[1]);
    const day = Number(parts[2]);

    if (!year || !month || !day) {
      return null;
    }

    return new Date(year, month - 1, day);
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
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
          errors?: Record<string, string[]>;
          title?: string;
        } | string;
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (typeof apiError.error === 'string' && apiError.error.trim()) {
        return apiError.error;
      }

      if (typeof apiError.error === 'object' && apiError.error !== null) {
        if (apiError.error.message) {
          return apiError.error.message;
        }

        if (apiError.error.Message) {
          return apiError.error.Message;
        }

        if (apiError.error.title) {
          return apiError.error.title;
        }

        if (apiError.error.errors) {
          const firstError = Object.values(apiError.error.errors)[0]?.[0];

          if (firstError) {
            return firstError;
          }
        }
      }
    }

    return 'Something went wrong. Please try again.';
  }
}