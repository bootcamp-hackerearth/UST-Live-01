import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import {
  Appointment,
  AppointmentStatusCode
} from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const FILTERS = [
  'All',
  'Today',
  'Pending',
  'Confirmed'
] as const;

type AppointmentFilter = typeof FILTERS[number];

@Component({
  selector: 'app-upcoming-appointments',
  imports: [DatePipe, RouterLink],
  templateUrl: './upcoming-appointments.html',
  styleUrl: './upcoming-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpcomingAppointments {
  private readonly appointmentService = inject(AppointmentService);

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly confirmingAppointmentId = signal<number | null>(null);

  readonly selectedFilter = signal<AppointmentFilter>('All');
  readonly searchText = signal('');
  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly filters = FILTERS;

  readonly upcomingAppointments = computed(() =>
    this.appointments()
      .filter((appointment) => this.isUpcomingStatus(appointment.status))
      .sort((first, second) =>
        new Date(first.scheduledDate).getTime() -
        new Date(second.scheduledDate).getTime()
      )
  );

  readonly filteredAppointments = computed(() => {
    const filter = this.selectedFilter();
    const searchValue = this.searchText().trim().toLowerCase();

    return this.upcomingAppointments().filter((appointment) =>
      this.matchesFilter(appointment, filter, searchValue)
    );
  });

  readonly todayCount = computed(() =>
    this.upcomingAppointments().filter((appointment) =>
      this.isToday(appointment.scheduledDate)
    ).length
  );

  readonly pendingCount = computed(() =>
    this.countByStatus('pending')
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus('confirmed')
  );

  constructor() {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load doctor appointments.')
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
  }

  selectFilter(filter: AppointmentFilter): void {
    this.selectedFilter.set(filter);
  }

  confirmAppointment(appointment: Appointment): void {
    if (!this.canConfirm(appointment)) {
      this.errorMessage.set('Only pending appointments can be confirmed.');
      return;
    }

    this.confirmingAppointmentId.set(appointment.appointmentId);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateAppointmentStatus(
      appointment.appointmentId,
      {
        status: AppointmentStatusCode.Confirmed
      }
    ).subscribe({
      next: () => {
        this.confirmingAppointmentId.set(null);
        this.successMessage.set('Appointment confirmed successfully.');
        this.loadAppointments();
      },
      error: (error: unknown) => {
        this.confirmingAppointmentId.set(null);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not confirm appointment.')
        );
      }
    });
  }

  canConfirm(appointment: Appointment): boolean {
    return this.getStatusText(appointment.status) === 'pending';
  }

  getStatusLabel(status: string | number): string {
    const normalizedStatus = this.getStatusText(status);

    if (normalizedStatus === 'pending') {
      return 'Pending';
    }

    if (normalizedStatus === 'confirmed') {
      return 'Confirmed';
    }

    if (normalizedStatus === 'completed') {
      return 'Completed';
    }

    if (normalizedStatus === 'cancelled') {
      return 'Cancelled';
    }

    return String(status);
  }

  getStatusClass(status: string | number): string {
    return `${this.getStatusText(status)}-badge`;
  }

  private matchesFilter(
    appointment: Appointment,
    filter: AppointmentFilter,
    searchValue: string
  ): boolean {
    const status = this.getStatusText(appointment.status);

    const matchesFilter =
      filter === 'All' ||
      (filter === 'Today' && this.isToday(appointment.scheduledDate)) ||
      status === filter.toLowerCase();

    const searchableText = [
      appointment.patientName,
      appointment.scheduledDate,
      appointment.timeSlot,
      this.getStatusLabel(appointment.status),
      appointment.appointmentId.toString()
    ]
      .join(' ')
      .toLowerCase();

    return matchesFilter && (!searchValue || searchableText.includes(searchValue));
  }

  private countByStatus(status: string): number {
    return this.upcomingAppointments().filter(
      (appointment) => this.getStatusText(appointment.status) === status
    ).length;
  }

  private isUpcomingStatus(status: string | number): boolean {
    const normalizedStatus = this.getStatusText(status);

    return normalizedStatus === 'pending' ||
      normalizedStatus === 'confirmed';
  }

  private isToday(dateValue: string): boolean {
    const appointmentDate = new Date(dateValue);
    const today = new Date();

    return appointmentDate.getFullYear() === today.getFullYear() &&
      appointmentDate.getMonth() === today.getMonth() &&
      appointmentDate.getDate() === today.getDate();
  }

  private getStatusText(status: string | number): string {
    const value = String(status).trim().toLowerCase();

    if (value === '1') {
      return 'pending';
    }

    if (value === '2') {
      return 'confirmed';
    }

    if (value === '3') {
      return 'cancelled';
    }

    if (value === '4') {
      return 'completed';
    }

    return value;
  }
}