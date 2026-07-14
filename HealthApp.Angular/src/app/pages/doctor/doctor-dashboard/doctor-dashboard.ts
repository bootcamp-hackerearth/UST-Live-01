import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { finalize, forkJoin } from 'rxjs';

import { AppointmentDto } from '../../../dtos/appointment.dto';
import { DoctorDto } from '../../../dtos/doctor.dto';

import { AppointmentService } from '../../../core/services/appointment.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

interface DoctorDashboardStat {
  label: string;
  value: number;
  icon: string;
  cardClass: string;
}

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink, LoadingSpinnerComponent],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard implements OnInit {
  doctorName = 'Doctor';
  doctorProfile: DoctorDto | null = null;

  isLoading = false;
  appointments: AppointmentDto[] = [];
  stats: DoctorDashboardStat[] = [];

  constructor(
    private readonly doctorService: DoctorService,
    private readonly appointmentService: AppointmentService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    forkJoin({
      profile: this.doctorService.getMyProfile(),
      appointments: this.appointmentService.getMyAppointments(false)
    })
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: result => {
          this.doctorProfile = result.profile;
          this.doctorName = result.profile?.fullName || 'Doctor';
          this.appointments = result.appointments ?? [];

          this.buildStats();
          this.cdr.markForCheck();
        },
        error: () => {
          this.doctorProfile = null;
          this.doctorName = 'Doctor';
          this.appointments = [];

          this.buildStats();
          this.cdr.markForCheck();
        }
      });
  }

  get todayAppointments(): AppointmentDto[] {
    const today = this.getTodayDate();

    return this.appointments
      .filter(
        appointment => appointment.scheduledDate.substring(0, 10) === today
      )
      .sort((a, b) => a.timeSlot.localeCompare(b.timeSlot))
      .slice(0, 5);
  }

  get isDoctorAvailable(): boolean {
    return !!this.doctorProfile?.isActive;
  }

  get availabilityText(): string {
    return this.isDoctorAvailable ? 'Available' : 'Unavailable';
  }

  refreshDashboard(): void {
    this.loadDashboard();
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'Confirmed':
        return 'status-confirmed';

      case 'Completed':
        return 'status-completed';

      case 'Cancelled':
        return 'status-cancelled';

      case 'Pending':
      default:
        return 'status-pending';
    }
  }

  private buildStats(): void {
    const today = this.getTodayDate();

    const todaysCount = this.appointments.filter(
      appointment => appointment.scheduledDate.substring(0, 10) === today
    ).length;

    const confirmedCount = this.appointments.filter(
      appointment => appointment.status === 'Confirmed'
    ).length;

    const completedCount = this.appointments.filter(
      appointment => appointment.status === 'Completed'
    ).length;

    const pendingRecordsCount = this.appointments.filter(
      appointment => appointment.status === 'Confirmed'
    ).length;

    this.stats = [
      {
        label: "Today's Appointments",
        value: todaysCount,
        icon: 'bi bi-calendar-event',
        cardClass: 'metric-teal'
      },
      {
        label: 'Confirmed',
        value: confirmedCount,
        icon: 'bi bi-check-circle',
        cardClass: 'metric-blue'
      },
      {
        label: 'Completed',
        value: completedCount,
        icon: 'bi bi-calendar-check',
        cardClass: 'metric-green'
      },
      {
        label: 'Pending Records',
        value: pendingRecordsCount,
        icon: 'bi bi-journal-plus',
        cardClass: 'metric-amber'
      }
    ];
  }

  private getTodayDate(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}