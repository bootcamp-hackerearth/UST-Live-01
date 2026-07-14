import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { catchError, finalize, forkJoin, of } from 'rxjs';

import { AppointmentDto } from '../../../dtos/appointment.dto';
import { HealthRecordDto } from '../../../dtos/health-record.dto';
import { PatientNotificationDto } from '../../../dtos/patient-notification.dto';

import { AppointmentService } from '../../../core/services/appointment.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { HealthRecordService } from '../../../core/services/health-record.service';
import { PatientNotificationService } from '../../../core/services/patient-notification.service';
import { PatientService } from '../../../core/services/patient.service';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

interface DashboardStat {
  label: string;
  value: number;
  icon: string;
  cardClass: string;
}

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard implements OnInit {
  patientName = 'Patient';
  isLoading = false;
  isAcknowledgingNotification = false;

  appointments: AppointmentDto[] = [];
  healthRecords: HealthRecordDto[] = [];
  unreadCancellationNotifications: PatientNotificationDto[] = [];

  activeDoctorCount = 0;
  dashboardStats: DashboardStat[] = [];

  constructor(
    private readonly patientService: PatientService,
    private readonly appointmentService: AppointmentService,
    private readonly healthRecordService: HealthRecordService,
    private readonly doctorService: DoctorService,
    private readonly patientNotificationService: PatientNotificationService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  get currentCancellationNotification(): PatientNotificationDto | null {
    return this.unreadCancellationNotifications[0] ?? null;
  }

  get remainingCancellationCount(): number {
    return Math.max(0, this.unreadCancellationNotifications.length - 1);
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    forkJoin({
      profile: this.patientService.getMyProfile(),
      appointments: this.appointmentService.getMyAppointments(false),
      healthRecords: this.healthRecordService.getMyHealthRecords(),
      doctors: this.doctorService.getDoctors(undefined, undefined, true),
      notifications: this.patientNotificationService
        .getUnreadDoctorLeaveNotifications()
        .pipe(catchError(() => of([] as PatientNotificationDto[])))
    })
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: result => {
          this.patientName = result.profile?.fullName || 'Patient';
          this.appointments = result.appointments ?? [];
          this.healthRecords = result.healthRecords ?? [];
          this.activeDoctorCount = result.doctors?.length ?? 0;
          this.unreadCancellationNotifications = result.notifications ?? [];

          this.buildDashboardStats();
          this.cdr.markForCheck();
        },
        error: () => {
          this.patientName = 'Patient';
          this.appointments = [];
          this.healthRecords = [];
          this.activeDoctorCount = 0;
          this.unreadCancellationNotifications = [];

          this.buildDashboardStats();
          this.cdr.markForCheck();
        }
      });
  }

  get upcomingAppointments(): AppointmentDto[] {
    return this.appointments
      .filter(
        appointment =>
          appointment.status === 'Pending' ||
          appointment.status === 'Confirmed'
      )
      .sort(
        (a, b) =>
          this.getAppointmentDateTime(a).getTime() -
          this.getAppointmentDateTime(b).getTime()
      )
      .slice(0, 3);
  }

  get recentHealthRecords(): HealthRecordDto[] {
    return [...this.healthRecords]
      .sort(
        (a, b) =>
          new Date(b.visitDate).getTime() -
          new Date(a.visitDate).getTime()
      )
      .slice(0, 3);
  }

  get hasNoPreviewData(): boolean {
    return (
      this.upcomingAppointments.length === 0 &&
      this.recentHealthRecords.length === 0
    );
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Confirmed':
        return 'status-confirmed';
      case 'Pending':
        return 'status-pending';
      case 'Completed':
        return 'status-completed';
      case 'Cancelled':
        return 'status-cancelled';
      default:
        return 'status-pending';
    }
  }

  refreshDashboard(): void {
    this.loadDashboard();
  }

  acknowledgeCurrentNotification(): void {
    this.acknowledgeCurrent(false);
  }

  bookAnotherAppointment(): void {
    this.acknowledgeCurrent(true);
  }

  private acknowledgeCurrent(navigateToBooking: boolean): void {
    const notification = this.currentCancellationNotification;

    if (!notification || this.isAcknowledgingNotification) {
      return;
    }

    this.isAcknowledgingNotification = true;
    this.cdr.markForCheck();

    this.patientNotificationService
      .acknowledgeNotification(notification.notificationId)
      .pipe(
        finalize(() => {
          this.isAcknowledgingNotification = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: () => {
          this.unreadCancellationNotifications =
            this.unreadCancellationNotifications.filter(
              item => item.notificationId !== notification.notificationId
            );

          if (navigateToBooking) {
            this.router.navigate(['/patient/book-appointment']);
            return;
          }

          this.cdr.markForCheck();
        },
        error: () => {
          // The global error interceptor displays the error toast.
        }
      });
  }

  private buildDashboardStats(): void {
    const upcomingCount = this.appointments.filter(
      appointment =>
        appointment.status === 'Pending' ||
        appointment.status === 'Confirmed'
    ).length;

    const completedCount = this.appointments.filter(
      appointment => appointment.status === 'Completed'
    ).length;

    this.dashboardStats = [
      {
        label: 'Upcoming Appointments',
        value: upcomingCount,
        icon: 'bi bi-calendar-event',
        cardClass: 'metric-teal'
      },
      {
        label: 'Completed Appointments',
        value: completedCount,
        icon: 'bi bi-calendar-check',
        cardClass: 'metric-blue'
      },
      {
        label: 'Doctors Available',
        value: this.activeDoctorCount,
        icon: 'bi bi-person-badge',
        cardClass: 'metric-cyan'
      },
      {
        label: 'Health Records',
        value: this.healthRecords.length,
        icon: 'bi bi-clipboard-pulse',
        cardClass: 'metric-amber'
      }
    ];
  }

  private getAppointmentDateTime(appointment: AppointmentDto): Date {
    return new Date(appointment.scheduledDate);
  }
}