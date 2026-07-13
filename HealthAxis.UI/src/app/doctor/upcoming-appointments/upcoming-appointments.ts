import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
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
  'Confirmed',
  'Cancelled'
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
  private readonly cancellationCutoffHours = 2;

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);

  readonly processingAppointmentId = signal<number | null>(null);
  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly cancelTarget = signal<Appointment | null>(null);
  readonly cancellationReason = signal('');
  readonly cancellationReasonTouched = signal(false);

  readonly selectedFilter = signal<AppointmentFilter>('All');
  readonly searchText = signal('');
  readonly currentPage = signal(1);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly filters = FILTERS;

  readonly doctorAppointments = computed(() =>
    this.appointments()
      .filter((appointment) => this.isDoctorVisibleStatus(appointment.status))
      .sort((first, second) =>
        this.getAppointmentStartDateTime(first).getTime() -
        this.getAppointmentStartDateTime(second).getTime()
      )
  );

  readonly filteredAppointments = computed(() => {
    const filter = this.selectedFilter();
    const searchValue = this.searchText().trim().toLowerCase();

    return this.doctorAppointments().filter((appointment) =>
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
    this.doctorAppointments().filter((appointment) =>
      this.isToday(appointment.scheduledDate)
    ).length
  );

  readonly pendingCount = computed(() => this.countByStatus('pending'));

  readonly confirmedCount = computed(() => this.countByStatus('confirmed'));

  readonly cancelledCount = computed(() => this.countByStatus('cancelled'));

  readonly canSubmitCancellation = computed(() =>
    this.cancellationReason().trim().length > 0
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
    this.cancellationReasonTouched.set(true);
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
      this.errorMessage.set(this.getConfirmRestrictionMessage(appointment));
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
      this.errorMessage.set(this.getCancelRestrictionMessage(appointment));
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.selectedAppointment.set(null);
    this.cancelTarget.set(appointment);
    this.cancellationReason.set('');
    this.cancellationReasonTouched.set(false);
  }

  closeCancelDialog(): void {
    if (this.processingAppointmentId()) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
    this.cancellationReasonTouched.set(false);
  }

  confirmCancelAppointment(): void {
    const appointment = this.cancelTarget();

    if (!appointment) {
      this.errorMessage.set('Please select an appointment to cancel.');
      return;
    }

    if (!this.canCancel(appointment)) {
      this.errorMessage.set(this.getCancelRestrictionMessage(appointment));
      return;
    }

    if (!this.canSubmitCancellation()) {
      this.cancellationReasonTouched.set(true);
      return;
    }

    this.updateAppointmentStatus(
      appointment,
      AppointmentStatusCode.Cancelled,
      'Appointment cancelled successfully.',
      'Could not cancel appointment.',
      this.getCancellationReason()
    );
  }

  canConfirm(appointment: Appointment): boolean {
    return this.getStatusText(appointment.status) === 'pending' &&
      !this.hasAppointmentStarted(appointment);
  }

  canCancel(appointment: Appointment): boolean {
    const status = this.getStatusText(appointment.status);

    if (status === 'pending') {
      return true;
    }

    if (status === 'confirmed') {
      return !this.isWithinCancellationCutoff(appointment);
    }

    return false;
  }

  canAddHealthRecord(appointment: Appointment): boolean {
    return this.getStatusText(appointment.status) === 'confirmed';
  }

  isProcessing(appointment: Appointment): boolean {
    return this.processingAppointmentId() === appointment.appointmentId;
  }

  shouldShowCancelButton(appointment: Appointment): boolean {
    const status = this.getStatusText(appointment.status);

    return status === 'pending' || status === 'confirmed';
  }

  getCancelRestrictionMessage(appointment: Appointment): string {
    const status = this.getStatusText(appointment.status);

    if (status === 'confirmed' && this.isWithinCancellationCutoff(appointment)) {
      return 'Confirmed appointments cannot be cancelled within 2 hours of the scheduled time. Please contact the patient or admin.';
    }

    if (status === 'completed') {
      return 'Completed appointment cannot be cancelled.';
    }

    if (status === 'cancelled') {
      return 'Appointment is already cancelled.';
    }

    return 'Only pending or confirmed appointments can be cancelled.';
  }

  getConfirmRestrictionMessage(appointment: Appointment): string {
    if (this.hasAppointmentStarted(appointment)) {
      return 'Pending appointment cannot be confirmed because the scheduled time has already passed.';
    }

    return 'Only pending appointments can be confirmed.';
  }

  getCancellationReasonErrorMessage(): string {
    if (!this.cancellationReasonTouched()) {
      return '';
    }

    if (this.cancellationReason().trim().length === 0) {
      return 'Cancellation reason is required.';
    }

    return '';
  }

  getStatusLabel(status: string): string {
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

    return status;
  }

  getStatusClass(status: string): string {
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
    status: number,
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
        this.cancellationReasonTouched.set(false);
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
      appointment.appointmentId.toString(),
      appointment.cancellationReason
    ]
      .join(' ')
      .toLowerCase();

    return matchesFilter &&
      (!searchValue || searchableText.includes(searchValue));
  }

  private countByStatus(status: string): number {
    return this.doctorAppointments().filter(
      (appointment) => this.getStatusText(appointment.status) === status
    ).length;
  }

  private isDoctorVisibleStatus(status: string): boolean {
    const normalizedStatus = this.getStatusText(status);

    return normalizedStatus === 'pending' ||
      normalizedStatus === 'confirmed' ||
      normalizedStatus === 'cancelled';
  }

  private isToday(dateValue: string): boolean {
    const appointmentDate = new Date(dateValue);
    const today = new Date();

    return appointmentDate.getFullYear() === today.getFullYear() &&
      appointmentDate.getMonth() === today.getMonth() &&
      appointmentDate.getDate() === today.getDate();
  }

  private getCancellationReason(): string {
    return this.cancellationReason().trim();
  }

  private hasAppointmentStarted(appointment: Appointment): boolean {
    return this.getAppointmentStartDateTime(appointment).getTime() <=
      new Date().getTime();
  }

  private isWithinCancellationCutoff(appointment: Appointment): boolean {
    const appointmentStart = this.getAppointmentStartDateTime(appointment);
    const cutoffTime = new Date();

    cutoffTime.setHours(cutoffTime.getHours() + this.cancellationCutoffHours);

    return appointmentStart.getTime() <= cutoffTime.getTime();
  }

  private getAppointmentStartDateTime(appointment: Appointment): Date {
    const appointmentDate = new Date(appointment.scheduledDate);
    const startTimeText = appointment.timeSlot.split('-')[0]?.trim();

    if (!startTimeText) {
      return appointmentDate;
    }

    const timeMatch = /^(\d{1,2}):(\d{2})\s?(AM|PM)$/i.exec(startTimeText);

    if (!timeMatch) {
      return appointmentDate;
    }

    const hourValue = Number(timeMatch[1]);
    const minuteValue = Number(timeMatch[2]);
    const meridian = timeMatch[3].toUpperCase();

    let hour = hourValue;

    if (meridian === 'PM' && hour !== 12) {
      hour += 12;
    }

    if (meridian === 'AM' && hour === 12) {
      hour = 0;
    }

    appointmentDate.setHours(hour, minuteValue, 0, 0);

    return appointmentDate;
  }

  private getStatusText(status: string): string {
    const value = status.trim().toLowerCase();

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