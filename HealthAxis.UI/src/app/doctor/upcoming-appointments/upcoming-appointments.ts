import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

import {
  Appointment,
  AppointmentStatusCode
} from '../../core/models/appointment.model';
import { HealthRecord } from '../../core/models/health-record.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { HealthRecordService } from '../../core/services/health-record.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const FILTERS = [
  'All',
  'Today',
  'Pending',
  'Confirmed',
  'Cancelled'
] as const;

const PAGE_SIZE = 8;
const CANCELLATION_CUTOFF_HOURS = 2;

type AppointmentFilter = typeof FILTERS[number];

@Component({
  selector: 'app-upcoming-appointments',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './upcoming-appointments.html',
  styleUrl: './upcoming-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UpcomingAppointments {
  private readonly appointmentService =
    inject(AppointmentService);

  private readonly healthRecordService =
    inject(HealthRecordService);

  private readonly route = inject(ActivatedRoute);

  private readonly historyRequestAppointmentId =
    signal<number | null>(null);

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);

  readonly processingAppointmentId =
    signal<number | null>(null);

  readonly selectedAppointment =
    signal<Appointment | null>(null);

  readonly cancelTarget =
    signal<Appointment | null>(null);

  readonly cancellationReason = signal('');
  readonly cancellationReasonTouched = signal(false);

  readonly selectedFilter =
    signal<AppointmentFilter>('All');

  readonly searchText = signal('');
  readonly currentPage = signal(1);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly historyLoading = signal(false);
  readonly historyErrorMessage = signal('');

  readonly historyAppointment =
    signal<Appointment | null>(null);

  readonly historyRecords = signal<HealthRecord[]>([]);

  readonly selectedHistoryRecord =
    signal<HealthRecord | null>(null);

  readonly historySearchText = signal('');

  readonly filters = FILTERS;

  readonly doctorAppointments = computed(() => {
    return this.appointments()
      .filter((appointment) =>
        this.isDoctorVisibleStatus(
          appointment.status
        )
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(first).getTime() -
          this.getAppointmentStartDateTime(second).getTime()
      );
  });

  readonly filteredAppointments = computed(() => {
    const filter = this.selectedFilter();
    const searchValue = this.searchText()
      .trim()
      .toLowerCase();

    return this.doctorAppointments().filter(
      (appointment) =>
        this.matchesFilter(
          appointment,
          filter,
          searchValue
        )
    );
  });

  readonly totalPages = computed(() => {
    const pages = Math.ceil(
      this.filteredAppointments().length /
      PAGE_SIZE
    );

    return Math.max(pages, 1);
  });

  readonly pagedAppointments = computed(() => {
    const startIndex =
      (this.currentPage() - 1) * PAGE_SIZE;

    return this.filteredAppointments().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly todayCount = computed(() =>
    this.doctorAppointments().filter(
      (appointment) =>
        this.isToday(appointment.scheduledDate)
    ).length
  );

  readonly pendingCount = computed(() =>
    this.countByStatus('pending')
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus('confirmed')
  );

  readonly cancelledCount = computed(() =>
    this.countByStatus('cancelled')
  );

  readonly canSubmitCancellation = computed(() => {
    const reasonLength =
      this.cancellationReason().trim().length;

    return reasonLength > 0 && reasonLength <= 200;
  });

  readonly cancellationReasonLength = computed(() =>
    this.cancellationReason().length
  );

  readonly sortedHistoryRecords = computed(() => {
    return [...this.historyRecords()].sort(
      (first, second) =>
        this.getRecordDateTime(second) -
        this.getRecordDateTime(first)
    );
  });

  readonly filteredHistoryRecords = computed(() => {
    const searchValue = this.historySearchText()
      .trim()
      .toLowerCase();

    if (!searchValue) {
      return this.sortedHistoryRecords();
    }

    return this.sortedHistoryRecords().filter((record) => {
      const searchableText = [
        record.doctorName,
        record.specialisation,
        record.visitDate,
        record.diagnosis,
        record.prescription,
        record.notes,
        record.appointmentId.toString()
      ]
        .join(' ')
        .toLowerCase();

      return searchableText.includes(searchValue);
    });
  });

  constructor() {
    this.applyFilterFromQuery();
    this.loadAppointments();
  }

  loadAppointments(clearMessages = true): void {
    this.loading.set(true);

    if (clearMessages) {
      this.errorMessage.set('');
      this.successMessage.set('');
    }

    this.appointmentService
      .getMyDoctorAppointments()
      .subscribe({
        next: (appointments) => {
          this.appointments.set(appointments);
          this.ensureCurrentPageIsValid();
          this.closeHistoryWhenAccessEnds(appointments);
          this.loading.set(false);
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load doctor appointments.'
            )
          );
        }
      });
  }

  selectFilter(filter: AppointmentFilter): void {
    this.selectedFilter.set(filter);
    this.currentPage.set(1);
  }

  isFilterSelected(
    filter: AppointmentFilter
  ): boolean {
    return this.selectedFilter() === filter;
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedFilter.set('All');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() <= 1) {
      return;
    }

    this.currentPage.update(
      (page) => page - 1
    );
  }

  goToNextPage(): void {
    if (
      this.currentPage() >=
      this.totalPages()
    ) {
      return;
    }

    this.currentPage.update(
      (page) => page + 1
    );
  }

  openAppointmentDetails(
    appointment: Appointment
  ): void {
    this.errorMessage.set('');
    this.selectedAppointment.set(appointment);
  }

  closeAppointmentDetails(): void {
    this.selectedAppointment.set(null);
  }

  openPatientHistory(
    appointment: Appointment
  ): void {
    if (!this.canViewPatientHistory(appointment)) {
      this.errorMessage.set(
        this.getPatientHistoryRestrictionMessage()
      );

      return;
    }

    const appointmentId = appointment.appointmentId;

    this.errorMessage.set('');
    this.successMessage.set('');
    this.selectedAppointment.set(null);
    this.cancelTarget.set(null);
    this.historyLoading.set(true);
    this.historyErrorMessage.set('');
    this.historyAppointment.set(appointment);
    this.historyRecords.set([]);
    this.selectedHistoryRecord.set(null);
    this.historySearchText.set('');
    this.historyRequestAppointmentId.set(appointmentId);

    this.healthRecordService
      .getPatientHistoryByAppointmentId(appointmentId)
      .subscribe({
        next: (records) => {
          if (
            this.historyRequestAppointmentId() !==
            appointmentId
          ) {
            return;
          }

          this.historyRecords.set(records);
          this.historyLoading.set(false);
        },
        error: (error: unknown) => {
          if (
            this.historyRequestAppointmentId() !==
            appointmentId
          ) {
            return;
          }

          this.historyLoading.set(false);
          this.historyErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load this patient health history.'
            )
          );
        }
      });
  }

  closePatientHistory(): void {
    this.historyRequestAppointmentId.set(null);
    this.historyLoading.set(false);
    this.historyErrorMessage.set('');
    this.historyAppointment.set(null);
    this.historyRecords.set([]);
    this.selectedHistoryRecord.set(null);
    this.historySearchText.set('');
  }

  onHistorySearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.historySearchText.set(input.value);
  }

  clearHistorySearch(): void {
    this.historySearchText.set('');
  }

  openHistoryRecord(record: HealthRecord): void {
    this.selectedHistoryRecord.set(record);
  }

  closeHistoryRecord(): void {
    this.selectedHistoryRecord.set(null);
  }

  confirmAppointment(
    appointment: Appointment
  ): void {
    if (!this.canConfirm(appointment)) {
      this.errorMessage.set(
        this.getConfirmRestrictionMessage(
          appointment
        )
      );

      return;
    }

    this.updateAppointmentStatus(
      appointment,
      AppointmentStatusCode.Confirmed,
      'Appointment confirmed successfully.',
      'Could not confirm appointment.'
    );
  }

  openCancelDialog(
    appointment: Appointment
  ): void {
    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        this.getCancelRestrictionMessage(
          appointment
        )
      );

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
    if (
      this.processingAppointmentId() !== null
    ) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
    this.cancellationReasonTouched.set(false);
  }

  onCancellationReasonInput(
    event: Event
  ): void {
    const textarea =
      event.target as HTMLTextAreaElement;

    this.cancellationReason.set(
      textarea.value
    );

    if (this.cancellationReasonTouched()) {
      this.cancellationReasonTouched.set(true);
    }
  }

  markCancellationReasonTouched(): void {
    this.cancellationReasonTouched.set(true);
  }

  confirmCancelAppointment(): void {
    const appointment = this.cancelTarget();

    if (!appointment) {
      this.errorMessage.set(
        'Please select an appointment to cancel.'
      );

      return;
    }

    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        this.getCancelRestrictionMessage(
          appointment
        )
      );

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

  canConfirm(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(
        appointment.status
      ) === 'pending' &&
      !this.hasAppointmentStarted(
        appointment
      )
    );
  }

  canCancel(
    appointment: Appointment
  ): boolean {
    const status = this.getStatusText(
      appointment.status
    );

    if (status === 'pending') {
      return true;
    }

    if (status === 'confirmed') {
      return !this.isWithinCancellationCutoff(
        appointment
      );
    }

    return false;
  }

  canAddHealthRecord(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(
        appointment.status
      ) === 'confirmed' &&
      this.hasAppointmentStarted(appointment)
    );
  }

  canViewPatientHistory(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(appointment.status) ===
      'confirmed'
    );
  }

  isProcessing(
    appointment: Appointment
  ): boolean {
    return (
      this.processingAppointmentId() ===
      appointment.appointmentId
    );
  }

  shouldShowCancelButton(
    appointment: Appointment
  ): boolean {
    const status = this.getStatusText(
      appointment.status
    );

    return (
      status === 'pending' ||
      status === 'confirmed'
    );
  }

  isCancelled(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(
        appointment.status
      ) === 'cancelled'
    );
  }

  isPastPending(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(
        appointment.status
      ) === 'pending' &&
      this.hasAppointmentStarted(appointment)
    );
  }

  getCancelRestrictionMessage(
    appointment: Appointment
  ): string {
    const status = this.getStatusText(
      appointment.status
    );

    if (
      status === 'confirmed' &&
      this.hasAppointmentStarted(appointment)
    ) {
      return (
        'The confirmed appointment time has passed. ' +
        'Complete the consultation by adding a health record.'
      );
    }

    if (
      status === 'confirmed' &&
      this.isWithinCancellationCutoff(
        appointment
      )
    ) {
      return (
        'Confirmed appointments cannot be cancelled ' +
        'within 2 hours of the scheduled time.'
      );
    }

    if (status === 'completed') {
      return (
        'Completed appointments cannot be cancelled.'
      );
    }

    if (status === 'cancelled') {
      return (
        'This appointment is already cancelled.'
      );
    }

    return (
      'Only pending or confirmed appointments ' +
      'can be cancelled.'
    );
  }

  getConfirmRestrictionMessage(
    appointment: Appointment
  ): string {
    if (
      this.hasAppointmentStarted(appointment)
    ) {
      return (
        'This pending appointment cannot be confirmed ' +
        'because its scheduled time has passed.'
      );
    }

    return (
      'Only pending appointments can be confirmed.'
    );
  }

  getHealthRecordRestrictionMessage(
    appointment: Appointment
  ): string {
    if (
      this.getStatusText(
        appointment.status
      ) !== 'confirmed'
    ) {
      return (
        'A health record can be added only to ' +
        'a confirmed appointment.'
      );
    }

    if (
      !this.hasAppointmentStarted(appointment)
    ) {
      return (
        'The health record can be added after ' +
        'the scheduled appointment time.'
      );
    }

    return '';
  }

  getCancellationReasonErrorMessage(): string {
    if (!this.cancellationReasonTouched()) {
      return '';
    }

    if (
      this.cancellationReason().trim().length === 0
    ) {
      return (
        'Cancellation reason is required.'
      );
    }

    return '';
  }

  getAppointmentStatusLabel(
    appointment: Appointment
  ): string {
    return this.getStatusLabel(
      appointment.status
    );
  }

  getAppointmentStatusClass(
    appointment: Appointment
  ): string {
    return this.getStatusClass(
      appointment.status
    );
  }

  getStatusLabel(status: string): string {
    switch (this.getStatusText(status)) {
      case 'pending':
        return 'Pending';

      case 'confirmed':
        return 'Confirmed';

      case 'completed':
        return 'Completed';

      case 'cancelled':
        return 'Cancelled';

      default:
        return status;
    }
  }

  getStatusClass(status: string): string {
    const normalizedStatus =
      this.getStatusText(status);

    return `${normalizedStatus}-badge`;
  }

  getPatientName(
    appointment: Appointment
  ): string {
    return appointment.patientName?.trim() ||
      'Patient';
  }

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  getHistoryDoctorName(
    record: HealthRecord
  ): string {
    return this.getSafeText(
      record.doctorName,
      'Doctor not available'
    );
  }

  getHistorySpecialisation(
    record: HealthRecord
  ): string {
    return this.getSafeText(
      record.specialisation,
      'Not specified'
    );
  }

  getPatientHistoryRestrictionMessage(): string {
    return (
      'Patient history is available only while ' +
      'the appointment is confirmed.'
    );
  }

  private applyFilterFromQuery(): void {
    const requestedFilter =
      this.route.snapshot.queryParamMap.get(
        'status'
      );

    if (!requestedFilter) {
      return;
    }

    const matchingFilter = FILTERS.find(
      (filter) =>
        filter.toLowerCase() ===
        requestedFilter.toLowerCase()
    );

    if (matchingFilter) {
      this.selectedFilter.set(
        matchingFilter
      );
    }
  }

  private updateAppointmentStatus(
    appointment: Appointment,
    status: number,
    successMessage: string,
    failureMessage: string,
    cancellationReason?: string | null
  ): void {
    this.processingAppointmentId.set(
      appointment.appointmentId
    );

    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService
      .updateAppointmentStatus(
        appointment.appointmentId,
        {
          status,
          cancellationReason
        }
      )
      .subscribe({
        next: () => {
          this.processingAppointmentId.set(null);
          this.selectedAppointment.set(null);
          this.cancelTarget.set(null);
          this.cancellationReason.set('');
          this.cancellationReasonTouched.set(false);

          if (
            this.historyRequestAppointmentId() ===
            appointment.appointmentId
          ) {
            this.closePatientHistory();
          }

          this.successMessage.set(successMessage);
          this.loadAppointments(false);
        },
        error: (error: unknown) => {
          this.processingAppointmentId.set(null);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              failureMessage
            )
          );
        }
      });
  }

  private matchesFilter(
    appointment: Appointment,
    filter: AppointmentFilter,
    searchValue: string
  ): boolean {
    const status = this.getStatusText(
      appointment.status
    );

    const matchesFilter =
      filter === 'All' ||
      (
        filter === 'Today' &&
        this.isToday(
          appointment.scheduledDate
        )
      ) ||
      status === filter.toLowerCase();

    const searchableText = [
      appointment.patientName,
      appointment.scheduledDate,
      appointment.timeSlot,
      this.getAppointmentStatusLabel(
        appointment
      ),
      this.isPastPending(appointment)
        ? 'Scheduled time passed'
        : '',
      appointment.appointmentId.toString(),
      appointment.cancellationReason ?? ''
    ]
      .join(' ')
      .toLowerCase();

    return (
      matchesFilter &&
      (
        !searchValue ||
        searchableText.includes(searchValue)
      )
    );
  }

  private countByStatus(
    status: string
  ): number {
    return this.doctorAppointments().filter(
      (appointment) =>
        this.getStatusText(
          appointment.status
        ) === status
    ).length;
  }

  private isDoctorVisibleStatus(
    status: string
  ): boolean {
    const normalizedStatus =
      this.getStatusText(status);

    return (
      normalizedStatus === 'pending' ||
      normalizedStatus === 'confirmed' ||
      normalizedStatus === 'cancelled'
    );
  }

  private isToday(dateValue: string): boolean {
    const appointmentDate =
      this.createLocalDate(dateValue);

    const today = new Date();

    return (
      appointmentDate.getFullYear() ===
        today.getFullYear() &&
      appointmentDate.getMonth() ===
        today.getMonth() &&
      appointmentDate.getDate() ===
        today.getDate()
    );
  }

  private getCancellationReason(): string {
    return this.cancellationReason().trim();
  }

  private hasAppointmentStarted(
    appointment: Appointment
  ): boolean {
    return (
      this.getAppointmentStartDateTime(
        appointment
      ).getTime() <= Date.now()
    );
  }

  private isWithinCancellationCutoff(
    appointment: Appointment
  ): boolean {
    const appointmentStart =
      this.getAppointmentStartDateTime(
        appointment
      );

    const cutoffTime = new Date();

    cutoffTime.setHours(
      cutoffTime.getHours() +
      CANCELLATION_CUTOFF_HOURS
    );

    return (
      appointmentStart.getTime() <=
      cutoffTime.getTime()
    );
  }

  private getAppointmentStartDateTime(
    appointment: Appointment
  ): Date {
    const appointmentDate =
      this.createLocalDate(
        appointment.scheduledDate
      );

    const startTimeText =
      appointment.timeSlot
        .split('-')[0]
        ?.trim();

    if (!startTimeText) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    const timeMatch =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)$/i
        .exec(startTimeText);

    if (!timeMatch) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    let hour = Number(timeMatch[1]);
    const minute = Number(timeMatch[2]);
    const meridian =
      timeMatch[3].toUpperCase();

    if (
      meridian === 'PM' &&
      hour !== 12
    ) {
      hour += 12;
    }

    if (
      meridian === 'AM' &&
      hour === 12
    ) {
      hour = 0;
    }

    appointmentDate.setHours(
      hour,
      minute,
      0,
      0
    );

    return appointmentDate;
  }

  private createLocalDate(
    dateValue: string
  ): Date {
    const dateOnly = dateValue.split('T')[0];
    const parts = dateOnly
      .split('-')
      .map(Number);

    if (
      parts.length === 3 &&
      parts.every(
        (part) => Number.isFinite(part)
      )
    ) {
      const [year, month, day] = parts;

      return new Date(
        year,
        month - 1,
        day
      );
    }

    return new Date(dateValue);
  }

  private ensureCurrentPageIsValid(): void {
    if (
      this.currentPage() >
      this.totalPages()
    ) {
      this.currentPage.set(
        this.totalPages()
      );
    }

    if (this.currentPage() < 1) {
      this.currentPage.set(1);
    }
  }

  private closeHistoryWhenAccessEnds(
    appointments: Appointment[]
  ): void {
    const appointmentId =
      this.historyRequestAppointmentId();

    if (appointmentId === null) {
      return;
    }

    const stillConfirmed = appointments.some(
      (appointment) =>
        appointment.appointmentId === appointmentId &&
        this.canViewPatientHistory(appointment)
    );

    if (!stillConfirmed) {
      this.closePatientHistory();
    }
  }

  private getRecordDateTime(
    record: HealthRecord
  ): number {
    const visitDate = new Date(
      record.visitDate
    ).getTime();

    return Number.isNaN(visitDate)
      ? 0
      : visitDate;
  }

  private getStatusText(
    status: string | number
  ): string {
    const value = String(status)
      .trim()
      .toLowerCase();

    switch (value) {
      case '1':
        return 'pending';

      case '2':
        return 'confirmed';

      case '3':
        return 'cancelled';

      case '4':
        return 'completed';

      default:
        return value;
    }
  }
}