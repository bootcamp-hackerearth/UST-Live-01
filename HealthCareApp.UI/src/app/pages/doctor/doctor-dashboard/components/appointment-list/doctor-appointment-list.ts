import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { timeout } from 'rxjs';

import { AppointmentDto } from '../../../../../shared/models/appointment.models';

import {
  AppointmentApiService,
  AppointmentStatusText
} from '../../../../../core/services/appointment-api.service';

import { HealthRecordApiService } from '../../../../../core/services/health-record-api.service';

type ToastType = 'success' | 'info' | 'warning';

type ReminderLevel =
  | 'six-hour'
  | 'three-hour'
  | 'one-hour'
  | 'overdue';

interface DoctorToastEvent {
  message: string;
  type: ToastType;
}

interface HealthRecordForm {
  diagnosis: string;
  prescription: string;
  notes: string;
}

interface PendingAppointmentReminder {
  appointment: AppointmentDto;
  minutesUntilStart: number;
  reminderLevel: ReminderLevel;
}

@Component({
  selector: 'app-doctor-appointment-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './doctor-appointment-list.html',
  styleUrl: './doctor-appointment-list.css'
})
export class DoctorAppointmentList implements OnInit, OnDestroy {
  appointments: AppointmentDto[] = [];

  pendingReminderAppointments: PendingAppointmentReminder[] = [];

  searchTerm = '';
  selectedStatus: AppointmentStatusText | '' = '';
  selectedDate = '';

  isLoading = false;
  errorMessage = '';

  pageNumber = 1;
  pageSize = 5;
  totalRecords = 0;
  totalPages = 0;

  pageSizeOptions: number[] = [5, 10, 15, 20];

  statusOptions: AppointmentStatusText[] = [
    'Pending',
    'Confirmed',
    'Completed',
    'Cancelled'
  ];

  todayDate = new Date().toISOString().split('T')[0];

  selectedAppointment?: AppointmentDto;

  isConfirmModalOpen = false;
  isConfirming = false;
  confirmMessage = '';

  isCancelModalOpen = false;
  isCancelling = false;
  cancelMessage = '';
  cancelReason = 'Doctor cancelled the appointment.';

  isHealthRecordModalOpen = false;
  isHealthRecordSaveConfirmOpen = false;
  isDiscardHealthRecordModalOpen = false;
  isSavingHealthRecord = false;

  hasRecordSubmitted = false;
  recordMessage = '';

  recordForm: HealthRecordForm = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  private readonly remindedAppointmentKeys = new Set<string>();

  private reminderPollingTimer?: ReturnType<typeof setInterval>;

  @Output() refreshDashboard = new EventEmitter<void>();
  @Output() doctorToast = new EventEmitter<DoctorToastEvent>();

  constructor(
    private readonly appointmentApiService: AppointmentApiService,
    private readonly healthRecordApiService: HealthRecordApiService
  ) {
  }

  ngOnInit(): void {
    this.loadAppointments();
    this.loadPendingAppointmentReminders();
    this.startReminderPolling();
  }

  ngOnDestroy(): void {
    if (this.reminderPollingTimer) {
      clearInterval(this.reminderPollingTimer);
    }
  }

  get hasActiveFilters(): boolean {
    return (
      this.searchTerm.trim().length > 0 ||
      !!this.selectedStatus ||
      !!this.selectedDate
    );
  }

  get canGoPrevious(): boolean {
    return this.pageNumber > 1;
  }

  get canGoNext(): boolean {
    return this.pageNumber < this.totalPages;
  }

  get isDiagnosisInvalid(): boolean {
    return this.recordForm.diagnosis.trim().length < 3;
  }

  get isPrescriptionInvalid(): boolean {
    return this.recordForm.prescription.trim().length < 3;
  }

  get isRecordFormInvalid(): boolean {
    return this.isDiagnosisInvalid || this.isPrescriptionInvalid;
  }

  get hasPendingAppointmentReminders(): boolean {
    return this.pendingReminderAppointments.length > 0;
  }

  get pendingReminderTitle(): string {
    const urgentReminder = this.pendingReminderAppointments.find(
      (reminder: PendingAppointmentReminder) =>
        reminder.reminderLevel === 'one-hour' ||
        reminder.reminderLevel === 'overdue'
    );

    if (urgentReminder) {
      return 'Urgent pending appointment action required';
    }

    return 'Pending appointment reminder';
  }

  get pendingReminderMessage(): string {
    const reminderCount = this.pendingReminderAppointments.length;

    if (reminderCount === 1) {
      const reminder = this.pendingReminderAppointments[0];

      return `${reminder.appointment.patientName} has a pending appointment at ${reminder.appointment.timeSlot}. Please confirm or cancel.`;
    }

    return `${reminderCount} pending appointments need your action. Please confirm or cancel them.`;
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.appointmentApiService.getMyAppointments({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      status: this.selectedStatus,
      scheduledDate: this.selectedDate
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: (response) => {
        this.appointments = response.items ?? [];
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalRecords = response.totalRecords;
        this.totalPages = response.totalPages;
        this.isLoading = false;
      },
      error: (error: unknown) => {
        console.log('Doctor appointments API error:', error);

        this.appointments = [];
        this.totalRecords = 0;
        this.totalPages = 0;
        this.isLoading = false;
        this.errorMessage = this.getErrorMessage(error);
      }
    });
  }

  applyFilters(): void {
    this.pageNumber = 1;
    this.loadAppointments();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedDate = '';
    this.pageNumber = 1;
    this.loadAppointments();
  }

  changePageSize(): void {
    this.pageNumber = 1;
    this.loadAppointments();
  }

  goToPreviousPage(): void {
    if (!this.canGoPrevious) {
      return;
    }

    this.pageNumber--;
    this.loadAppointments();
  }

  goToNextPage(): void {
    if (!this.canGoNext) {
      return;
    }

    this.pageNumber++;
    this.loadAppointments();
  }

  canAddHealthRecord(appointment: AppointmentDto): boolean {
    return (
      appointment.status === 'Confirmed' &&
      appointment.scheduledDate <= this.todayDate
    );
  }

  isFutureConfirmedAppointment(appointment: AppointmentDto): boolean {
    return (
      appointment.status === 'Confirmed' &&
      appointment.scheduledDate > this.todayDate
    );
  }

  canCancelAppointment(appointment: AppointmentDto): boolean {
    return appointment.status === 'Pending';
  }

  openConfirmModal(appointment: AppointmentDto): void {
    this.selectedAppointment = appointment;
    this.confirmMessage = '';
    this.isConfirmModalOpen = true;
  }

  closeConfirmModal(): void {
    if (this.isConfirming) {
      return;
    }

    this.selectedAppointment = undefined;
    this.confirmMessage = '';
    this.isConfirmModalOpen = false;
  }

  confirmAppointment(): void {
    this.confirmMessage = '';

    if (!this.selectedAppointment) {
      this.confirmMessage = 'Please select an appointment to confirm.';
      return;
    }

    this.isConfirming = true;

    this.appointmentApiService.confirmAppointment(
      this.selectedAppointment.appointmentId
    ).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isConfirming = false;
        this.closeConfirmModal();
        this.loadAppointments();
        this.loadPendingAppointmentReminders();
        this.refreshDashboard.emit();

        this.emitToast(
          'Appointment confirmed successfully ',
          'success'
        );
      },
      error: (error: unknown) => {
        console.log('Doctor confirm appointment API error:', error);

        this.isConfirming = false;
        this.confirmMessage = this.getErrorMessage(error);
      }
    });
  }

  openCancelModal(appointment: AppointmentDto): void {
    if (!this.canCancelAppointment(appointment)) {
      this.emitToast(
        'Only pending appointments can be cancelled.',
        'warning'
      );

      return;
    }

    this.selectedAppointment = appointment;
    this.cancelMessage = '';
    this.cancelReason = 'Doctor cancelled the appointment.';
    this.isCancelModalOpen = true;
  }

  closeCancelModal(): void {
    if (this.isCancelling) {
      return;
    }

    this.selectedAppointment = undefined;
    this.cancelMessage = '';
    this.cancelReason = 'Doctor cancelled the appointment.';
    this.isCancelModalOpen = false;
  }

  confirmCancelAppointment(): void {
    this.cancelMessage = '';

    if (!this.selectedAppointment) {
      this.cancelMessage = 'Please select an appointment to cancel.';
      return;
    }

    const reason = this.cancelReason.trim();

    if (!reason) {
      this.cancelMessage = 'Please enter a cancellation reason.';
      return;
    }

    this.isCancelling = true;

    this.appointmentApiService.cancelAppointment({
      appointmentId: this.selectedAppointment.appointmentId,
      reason
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isCancelling = false;
        this.closeCancelModal();
        this.loadAppointments();
        this.loadPendingAppointmentReminders();
        this.refreshDashboard.emit();

        this.emitToast(
          'Appointment cancelled successfully.',
          'warning'
        );
      },
      error: (error: unknown) => {
        console.log('Doctor cancel appointment API error:', error);

        this.isCancelling = false;
        this.cancelMessage = this.getErrorMessage(error);
      }
    });
  }

  openHealthRecordModal(appointment: AppointmentDto): void {
    if (!this.canAddHealthRecord(appointment)) {
      this.emitToast(
        'Health record can be added only on or after the appointment date.',
        'warning'
      );

      return;
    }

    this.selectedAppointment = appointment;
    this.hasRecordSubmitted = false;
    this.recordMessage = '';
    this.resetRecordForm();
    this.isHealthRecordModalOpen = true;
  }

  requestCloseHealthRecordModal(): void {
    if (this.isSavingHealthRecord) {
      return;
    }

    if (this.hasHealthRecordChanges()) {
      this.isDiscardHealthRecordModalOpen = true;
      return;
    }

    this.closeHealthRecordModal();
  }

  closeHealthRecordModal(): void {
    this.selectedAppointment = undefined;
    this.hasRecordSubmitted = false;
    this.recordMessage = '';
    this.resetRecordForm();
    this.isHealthRecordModalOpen = false;
    this.isHealthRecordSaveConfirmOpen = false;
    this.isDiscardHealthRecordModalOpen = false;
  }

  closeDiscardHealthRecordModal(): void {
    this.isDiscardHealthRecordModalOpen = false;
  }

  confirmDiscardHealthRecord(): void {
    this.closeHealthRecordModal();
  }

  submitHealthRecord(): void {
    this.recordMessage = '';
    this.hasRecordSubmitted = true;

    if (this.isRecordFormInvalid) {
      this.recordMessage = 'Please correct the highlighted health record fields.';
      return;
    }

    this.isHealthRecordSaveConfirmOpen = true;
  }

  closeHealthRecordSaveConfirm(): void {
    if (this.isSavingHealthRecord) {
      return;
    }

    this.isHealthRecordSaveConfirmOpen = false;
  }

  confirmSaveHealthRecord(): void {
    if (!this.selectedAppointment) {
      this.recordMessage = 'Please select an appointment.';
      this.isHealthRecordSaveConfirmOpen = false;
      return;
    }

    this.isSavingHealthRecord = true;
    this.recordMessage = '';

    this.healthRecordApiService.addHealthRecord({
      patientId: this.selectedAppointment.patientId,
      doctorId: this.selectedAppointment.doctorId,
      appointmentId: this.selectedAppointment.appointmentId,
      visitDate: this.selectedAppointment.scheduledDate,
      diagnosis: this.recordForm.diagnosis.trim(),
      prescription: this.recordForm.prescription.trim(),
      notes: this.recordForm.notes.trim()
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isSavingHealthRecord = false;
        this.closeHealthRecordModal();
        this.loadAppointments();
        this.loadPendingAppointmentReminders();
        this.refreshDashboard.emit();

        this.emitToast(
          'Health record saved and appointment completed ',
          'success'
        );
      },
      error: (error: unknown) => {
        console.log('Doctor add health record API error:', error);

        this.isSavingHealthRecord = false;
        this.isHealthRecordSaveConfirmOpen = false;
        this.recordMessage = this.getErrorMessage(error);
      }
    });
  }

  getStatusClass(status: string): string {
    return `da-status ${status.toLowerCase()}`;
  }

  formatDate(date: string): string {
    const parsedDate = new Date(date);

    if (Number.isNaN(parsedDate.getTime())) {
      return 'Not Available';
    }

    return parsedDate.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private loadPendingAppointmentReminders(): void {
    this.appointmentApiService.getMyAppointments({
      pageNumber: 1,
      pageSize: 50,
      status: 'Pending'
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: (response) => {
        const pendingAppointments = response.items ?? [];

        this.pendingReminderAppointments =
          this.buildPendingAppointmentReminders(pendingAppointments);

        this.emitPendingReminderToastIfNeeded();
      },
      error: (error: unknown) => {
        console.log('Pending appointment reminder API error:', error);

        this.pendingReminderAppointments = [];
      }
    });
  }

  private buildPendingAppointmentReminders(
    pendingAppointments: AppointmentDto[]
  ): PendingAppointmentReminder[] {
    const now = new Date();

    return pendingAppointments
      .map((appointment: AppointmentDto) => {
        const appointmentStart = this.getAppointmentStartDateTime(appointment);

        if (!appointmentStart) {
          return null;
        }

        const minutesUntilStart = Math.round(
          (appointmentStart.getTime() - now.getTime()) / 60000
        );

        const reminderLevel = this.getReminderLevel(minutesUntilStart);

        if (!reminderLevel) {
          return null;
        }

        return {
          appointment,
          minutesUntilStart,
          reminderLevel
        };
      })
      .filter(
        (reminder): reminder is PendingAppointmentReminder =>
          reminder !== null
      )
      .sort(
        (
          firstReminder: PendingAppointmentReminder,
          secondReminder: PendingAppointmentReminder
        ) =>
          firstReminder.minutesUntilStart -
          secondReminder.minutesUntilStart
      );
  }

  private getReminderLevel(minutesUntilStart: number): ReminderLevel | null {
    if (minutesUntilStart < 0 && minutesUntilStart >= -1440) {
      return 'overdue';
    }

    if (minutesUntilStart >= 0 && minutesUntilStart <= 60) {
      return 'one-hour';
    }

    if (minutesUntilStart > 60 && minutesUntilStart <= 180) {
      return 'three-hour';
    }

    if (minutesUntilStart > 180 && minutesUntilStart <= 360) {
      return 'six-hour';
    }

    return null;
  }

  private emitPendingReminderToastIfNeeded(): void {
    const newReminders = this.pendingReminderAppointments.filter(
      (reminder: PendingAppointmentReminder) => {
        const key = this.buildReminderKey(reminder);

        return !this.remindedAppointmentKeys.has(key);
      }
    );

    if (newReminders.length === 0) {
      return;
    }

    newReminders.forEach((reminder: PendingAppointmentReminder) => {
      this.remindedAppointmentKeys.add(this.buildReminderKey(reminder));
    });

    const urgentReminder = newReminders.find(
      (reminder: PendingAppointmentReminder) =>
        reminder.reminderLevel === 'one-hour' ||
        reminder.reminderLevel === 'overdue'
    );

    if (newReminders.length === 1) {
      const reminder = newReminders[0];

      this.emitToast(
        this.buildSingleReminderToastMessage(reminder),
        'warning'
      );

      return;
    }

    this.emitToast(
      urgentReminder
        ? `${newReminders.length} pending appointments need urgent action. Please confirm or cancel.`
        : `${newReminders.length} pending appointments start within the next 6 hours. Please confirm or cancel.`,
      'warning'
    );
  }

  private buildSingleReminderToastMessage(
    reminder: PendingAppointmentReminder
  ): string {
    const appointment = reminder.appointment;

    if (reminder.reminderLevel === 'overdue') {
      return `Overdue: Pending appointment with ${appointment.patientName} needs review. Please confirm or cancel.`;
    }

    if (reminder.reminderLevel === 'one-hour') {
      return `Urgent: Pending appointment with ${appointment.patientName} starts within 1 hour. Please confirm or cancel.`;
    }

    if (reminder.reminderLevel === 'three-hour') {
      return `Reminder: Pending appointment with ${appointment.patientName} starts within 3 hours. Please confirm or cancel.`;
    }

    return `Reminder: Pending appointment with ${appointment.patientName} starts within 6 hours. Please confirm or cancel.`;
  }

  private buildReminderKey(reminder: PendingAppointmentReminder): string {
    return `${reminder.appointment.appointmentId}-${reminder.reminderLevel}`;
  }

  private startReminderPolling(): void {
    this.reminderPollingTimer = setInterval(() => {
      this.loadPendingAppointmentReminders();
    }, 300000);
  }

  private getAppointmentStartDateTime(appointment: AppointmentDto): Date | null {
    if (!appointment.scheduledDate || !appointment.timeSlot) {
      return null;
    }

    const datePart = appointment.scheduledDate.split('T')[0];
    const dateParts = datePart.split('-');

    if (dateParts.length !== 3) {
      return null;
    }

    const year = Number(dateParts[0]);
    const month = Number(dateParts[1]);
    const day = Number(dateParts[2]);

    if (!year || !month || !day) {
      return null;
    }

    const startTime = appointment.timeSlot.split('-')[0]?.trim();

    if (!startTime) {
      return null;
    }

    const timeMatch = /^(\d{1,2}):(\d{2})\s?(AM|PM)$/i.exec(startTime);

    if (!timeMatch) {
      return null;
    }

    let hours = Number(timeMatch[1]);
    const minutes = Number(timeMatch[2]);
    const meridiem = timeMatch[3].toUpperCase();

    if (meridiem === 'PM' && hours !== 12) {
      hours += 12;
    }

    if (meridiem === 'AM' && hours === 12) {
      hours = 0;
    }

    return new Date(year, month - 1, day, hours, minutes, 0, 0);
  }

  private hasHealthRecordChanges(): boolean {
    return (
      this.recordForm.diagnosis.trim().length > 0 ||
      this.recordForm.prescription.trim().length > 0 ||
      this.recordForm.notes.trim().length > 0
    );
  }

  private resetRecordForm(): void {
    this.recordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };
  }

  private emitToast(message: string, type: ToastType): void {
    this.doctorToast.emit({
      message,
      type
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
          errors?: Record<string, string[]>;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (apiError.error?.message) {
        return apiError.error.message;
      }

      if (apiError.error?.Message) {
        return apiError.error.Message;
      }

      if (apiError.error?.errors) {
        const firstError = Object.values(apiError.error.errors)[0]?.[0];

        if (firstError) {
          return firstError;
        }
      }
    }

    return 'Something went wrong while updating the appointment.';
  }
}
