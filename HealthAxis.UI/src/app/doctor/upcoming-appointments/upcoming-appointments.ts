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
  private readonly pageSize = 8;

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);

  readonly processingAppointmentId = signal<number | null>(null);
  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly cancelTarget = signal<Appointment | null>(null);
  readonly cancellationReason = signal('');

  readonly selectedFilter = signal<AppointmentFilter>('All');
  readonly searchText = signal('');
  readonly currentPage = signal(1);

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

  readonly totalPages = computed(() => {
    const pages = Math.ceil(this.filteredAppointments().length / this.pageSize);

    return pages > 0 ? pages : 1;
  });

  readonly pagedAppointments = computed(() => {
    const startIndex = (this.currentPage() - 1) * this.pageSize;

    return this.filteredAppointments().slice(
      startIndex,
      startIndex + this.pageSize
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
        this.currentPage.set(1);
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
    this.currentPage.set(1);
  }

  onCancellationReasonInput(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    this.cancellationReason.set(textarea.value);
  }

  selectFilter(filter: AppointmentFilter): void {
    this.selectedFilter.set(filter);
    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedFilter.set('All');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update((page) => page - 1);
    }
  }

  goToNextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((page) => page + 1);
    }
  }

  openAppointmentDetails(appointment: Appointment): void {
    this.selectedAppointment.set(appointment);
  }

  closeAppointmentDetails(): void {
    this.selectedAppointment.set(null);
  }

  confirmAppointment(appointment: Appointment): void {
    if (!this.canConfirm(appointment)) {
      this.errorMessage.set('Only pending appointments can be confirmed.');
      return;
    }

    this.updateAppointmentStatus(
      appointment,
      AppointmentStatusCode.Confirmed,
      'Appointment confirmed successfully.',
      'Could not confirm appointment.'
    );
  }

  openCancelDialog(appointment: Appointment): void {
    if (!this.canCancel(appointment)) {
      this.errorMessage.set('Only pending or confirmed appointments can be cancelled.');
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.cancelTarget.set(appointment);
    this.cancellationReason.set('');
  }

  closeCancelDialog(): void {
    if (this.processingAppointmentId()) {
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

    this.updateAppointmentStatus(
      appointment,
      AppointmentStatusCode.Cancelled,
      'Appointment cancelled successfully.',
      'Could not cancel appointment.',
      this.getOptionalCancellationReason()
    );
  }

  canConfirm(appointment: Appointment): boolean {
    return this.getStatusText(appointment.status) === 'pending';
  }

  canCancel(appointment: Appointment): boolean {
    const status = this.getStatusText(appointment.status);

    return status === 'pending' || status === 'confirmed';
  }

  isProcessing(appointment: Appointment): boolean {
    return this.processingAppointmentId() === appointment.appointmentId;
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

  getPatientName(appointment: Appointment): string {
    return appointment.patientName?.trim() || 'Patient';
  }

  getSafeText(value: string | null | undefined, fallback: string): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  private updateAppointmentStatus(
    appointment: Appointment,
    status: AppointmentStatusCode,
    successMessage: string,
    failureMessage: string,
    cancellationReason?: string | null
  ): void {
    this.processingAppointmentId.set(appointment.appointmentId);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateAppointmentStatus(
      appointment.appointmentId,
      {
        status,
        cancellationReason
      }
    ).subscribe({
      next: () => {
        this.processingAppointmentId.set(null);
        this.cancelTarget.set(null);
        this.cancellationReason.set('');
        this.successMessage.set(successMessage);
        this.loadAppointments();
      },
      error: (error: unknown) => {
        this.processingAppointmentId.set(null);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, failureMessage)
        );
      }
    });
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

  private getOptionalCancellationReason(): string | null {
    const reason = this.cancellationReason().trim();

    return reason || null;
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