import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import {
  Appointment,
  AppointmentStatusCode
} from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const STATUS_FILTERS = [
  'All',
  'Pending',
  'Confirmed',
  'Completed',
  'Cancelled'
] as const;

type StatusFilter = typeof STATUS_FILTERS[number];

@Component({
  selector: 'app-my-appointments',
  imports: [DatePipe, RouterLink],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyAppointments {
  private readonly appointmentService = inject(AppointmentService);

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly cancelling = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly searchText = signal('');
  readonly selectedStatus = signal<StatusFilter>('All');
  readonly cancelTarget = signal<Appointment | null>(null);
  readonly cancellationReason = signal('');

  readonly statusFilters = STATUS_FILTERS;

  readonly pendingCount = computed(() => this.countByStatus('Pending'));
  readonly confirmedCount = computed(() => this.countByStatus('Confirmed'));
  readonly completedCount = computed(() => this.countByStatus('Completed'));
  readonly cancelledCount = computed(() => this.countByStatus('Cancelled'));

  readonly filteredAppointments = computed(() => {
    const searchValue = this.searchText().trim().toLowerCase();
    const status = this.selectedStatus();

    return this.appointments()
      .filter((appointment) =>
        this.matchesFilter(appointment, status, searchValue)
      )
      .sort((first, second) =>
        new Date(second.scheduledDate).getTime() -
        new Date(first.scheduledDate).getTime()
      );
  });

  readonly nextAppointment = computed<Appointment | null>(() => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const appointment = this.appointments()
      .filter((item) => this.isUpcomingAppointment(item, today))
      .sort((first, second) =>
        new Date(first.scheduledDate).getTime() -
        new Date(second.scheduledDate).getTime()
      )[0];

    return appointment ?? null;
  });

  constructor() {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getMyAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load your appointments.')
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
  }

  onCancellationReasonInput(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    this.cancellationReason.set(textarea.value);
  }

  selectStatus(status: StatusFilter): void {
    this.selectedStatus.set(status);
  }

  openCancelDialog(appointment: Appointment): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.cancelTarget.set(appointment);
    this.cancellationReason.set('');
  }

  closeCancelDialog(): void {
    if (this.cancelling()) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
  }

  confirmCancelAppointment(): void {
    const appointment = this.cancelTarget();

    if (!appointment) {
      this.errorMessage.set('Please select an appointment to cancel.');
      return;
    }

    if (!this.canCancel(appointment)) {
      this.errorMessage.set('Only pending or confirmed appointments can be cancelled.');
      return;
    }

    this.cancelling.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateAppointmentStatus(
      appointment.appointmentId,
      {
        status: AppointmentStatusCode.Cancelled,
        cancellationReason: this.getOptionalCancellationReason()
      }
    ).subscribe({
      next: () => {
        this.cancelling.set(false);
        this.cancelTarget.set(null);
        this.cancellationReason.set('');
        this.successMessage.set('Appointment cancelled successfully.');
        this.loadAppointments();
      },
      error: (error: unknown) => {
        this.cancelling.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not cancel appointment. Please try again.'
          )
        );
      }
    });
  }

  canCancel(appointment: Appointment): boolean {
    const status = appointment.status.toLowerCase();

    return status === 'pending' || status === 'confirmed';
  }

  getStatusClass(status: string): string {
    return `${status.trim().toLowerCase()}-badge`;
  }

  private getOptionalCancellationReason(): string | null {
    const reason = this.cancellationReason().trim();

    if (!reason) {
      return null;
    }

    return reason;
  }

  private countByStatus(status: string): number {
    return this.appointments().filter(
      (appointment) => appointment.status.toLowerCase() === status.toLowerCase()
    ).length;
  }

  private matchesFilter(
    appointment: Appointment,
    status: StatusFilter,
    searchValue: string
  ): boolean {
    const matchesStatus =
      status === 'All' ||
      appointment.status.toLowerCase() === status.toLowerCase();

    const searchableText = [
      appointment.doctorName,
      appointment.specialisation,
      appointment.status,
      appointment.timeSlot,
      appointment.scheduledDate,
      appointment.appointmentId.toString()
    ]
      .join(' ')
      .toLowerCase();

    return matchesStatus && (!searchValue || searchableText.includes(searchValue));
  }

  private isUpcomingAppointment(
    appointment: Appointment,
    today: Date
  ): boolean {
    const appointmentDate = new Date(appointment.scheduledDate);
    appointmentDate.setHours(0, 0, 0, 0);

    const status = appointment.status.toLowerCase();

    return appointmentDate >= today &&
      status !== 'cancelled' &&
      status !== 'completed';
  }
}