import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { DoctorDashboardService } from '../../core/services/doctor-dashboard.service';
import { TokenService } from '../../core/services/token.service';

import {
  CreateHealthRecordRequest,
  DoctorScheduleItem
} from '../../shared/models/doctor-dashboard.models';

import {
  AppointmentStatus,
  AppointmentTimeSlot
} from '../../shared/models/patient-dashboard.models';

type DoctorScheduleAction = 'confirm' | 'complete';

@Component({
  selector: 'app-doctor-schedule',
  imports: [
    DatePipe,
    FormsModule
  ],
  templateUrl: './doctor-schedule.html',
  styleUrl: './doctor-schedule.css'
})
export class DoctorSchedule implements OnInit {
  doctorId: number | null = null;

  activeView: 'today' | 'week' = 'today';

  loading = true;
  actionLoading = false;
  savingHealthRecord = false;
  cancellingAppointment = false;

  errorMessage = '';
  successMessage = '';

  schedule: DoctorScheduleItem[] = [];

  selectedAppointment?: DoctorScheduleItem;
  selectedReasonAppointment?: DoctorScheduleItem;
  selectedCancelAppointment?: DoctorScheduleItem;

  showHealthRecordPanel = false;
  showReasonPanel = false;
  showCancelPanel = false;

  showActionConfirmModal = false;
  selectedActionAppointment?: DoctorScheduleItem;
  pendingAction?: DoctorScheduleAction;

  cancellationReason = '';

  healthRecordForm = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  constructor(
    private tokenService: TokenService,
    private doctorDashboardService: DoctorDashboardService
  ) { }

  ngOnInit(): void {
    this.doctorId = this.tokenService.getReferenceId();

    if (!this.doctorId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    this.loadSchedule();
  }

  switchView(view: 'today' | 'week'): void {
    this.activeView = view;
    this.closeInlinePanels();
    this.loadSchedule();
  }

  loadSchedule(): void {
    if (!this.doctorId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    if (this.activeView === 'today') {
      this.doctorDashboardService.getTodaySchedule(this.doctorId).subscribe({
        next: (schedule) => {
          this.schedule = this.sortSchedule(schedule ?? []);
          this.loading = false;
        },
        error: (error) => this.handleLoadError(error)
      });

      return;
    }

    const range = this.getCurrentWeekRange();

    this.doctorDashboardService
      .getWeekSchedule(this.doctorId, range.startDate, range.endDate)
      .subscribe({
        next: (schedule) => {
          this.schedule = this.sortSchedule(schedule ?? []);
          this.loading = false;
        },
        error: (error) => this.handleLoadError(error)
      });
  }

  openActionConfirmModal(
    appointment: DoctorScheduleItem,
    action: DoctorScheduleAction
  ): void {
    this.selectedActionAppointment = appointment;
    this.pendingAction = action;
    this.showActionConfirmModal = true;
    this.errorMessage = '';
    this.successMessage = '';
  }

  closeActionConfirmModal(): void {
    if (this.actionLoading) {
      return;
    }

    this.showActionConfirmModal = false;
    this.selectedActionAppointment = undefined;
    this.pendingAction = undefined;
  }

  confirmSelectedAction(): void {
    if (!this.selectedActionAppointment || !this.pendingAction) {
      this.closeActionConfirmModal();
      return;
    }

    if (this.pendingAction === 'confirm') {
      this.confirmAppointment(this.selectedActionAppointment);
      return;
    }

    if (this.pendingAction === 'complete') {
      this.completeAppointment(this.selectedActionAppointment);
      return;
    }
  }

  private confirmAppointment(appointment: DoctorScheduleItem): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.actionLoading = true;

    this.doctorDashboardService.confirmAppointment(appointment.appointmentId).subscribe({
      next: () => {
        this.actionLoading = false;
        this.showActionConfirmModal = false;
        this.selectedActionAppointment = undefined;
        this.pendingAction = undefined;

        this.successMessage = 'Appointment confirmed successfully.';
        this.loadSchedule();
      },
      error: (error) =>
        this.handleActionError(
          error,
          'Could not confirm appointment. Please try again.'
        )
    });
  }

  private completeAppointment(appointment: DoctorScheduleItem): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.actionLoading = true;

    this.doctorDashboardService.completeAppointment(appointment.appointmentId).subscribe({
      next: () => {
        this.actionLoading = false;
        this.showActionConfirmModal = false;
        this.selectedActionAppointment = undefined;
        this.pendingAction = undefined;

        this.successMessage =
          'Appointment marked as completed. You can now add a health record.';
        this.loadSchedule();
      },
      error: (error) =>
        this.handleActionError(
          error,
          'Could not complete appointment. Please try again.'
        )
    });
  }

  toggleHealthRecordPanel(appointment: DoctorScheduleItem): void {
    const isSameAppointment =
      this.selectedAppointment?.appointmentId === appointment.appointmentId;

    if (isSameAppointment && this.showHealthRecordPanel) {
      this.closeHealthRecordPanel();
      return;
    }

    this.selectedReasonAppointment = undefined;
    this.selectedCancelAppointment = undefined;

    this.showReasonPanel = false;
    this.showCancelPanel = false;

    this.selectedAppointment = appointment;
    this.showHealthRecordPanel = true;

    this.healthRecordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    this.errorMessage = '';
    this.successMessage = '';
  }

  closeHealthRecordPanel(): void {
    if (this.savingHealthRecord) {
      return;
    }

    this.showHealthRecordPanel = false;
    this.selectedAppointment = undefined;

    this.healthRecordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };
  }

  submitHealthRecord(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.doctorId) {
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    if (!this.selectedAppointment) {
      this.errorMessage = 'No appointment selected.';
      return;
    }

    if (this.selectedAppointment.status !== AppointmentStatus.Completed) {
      this.errorMessage =
        'Health record can only be added after completing the appointment.';
      return;
    }

    if (this.selectedAppointment.hasHealthRecord) {
      this.errorMessage = 'Health record has already been added for this appointment.';
      return;
    }

    if (!this.healthRecordForm.diagnosis.trim()) {
      this.errorMessage = 'Diagnosis is required.';
      return;
    }

    if (!this.healthRecordForm.prescription.trim()) {
      this.errorMessage = 'Prescription is required.';
      return;
    }

    const request: CreateHealthRecordRequest = {
      appointmentId: this.selectedAppointment.appointmentId,
      patientId: this.selectedAppointment.patientId,
      doctorId: this.doctorId,
      diagnosis: this.healthRecordForm.diagnosis.trim(),
      prescription: this.healthRecordForm.prescription.trim(),
      notes: this.healthRecordForm.notes.trim() || undefined
    };

    this.savingHealthRecord = true;

    this.doctorDashboardService.createHealthRecord(request).subscribe({
      next: () => {
        this.savingHealthRecord = false;
        this.showHealthRecordPanel = false;
        this.selectedAppointment = undefined;

        this.successMessage = 'Health record added successfully.';

        this.healthRecordForm = {
          diagnosis: '',
          prescription: '',
          notes: ''
        };

        this.loadSchedule();
      },
      error: (error) => {
        this.savingHealthRecord = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to add health records.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage =
            'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not add health record. Please try again.';
      }
    });
  }

  toggleReasonPanel(appointment: DoctorScheduleItem): void {
    const isSameAppointment =
      this.selectedReasonAppointment?.appointmentId === appointment.appointmentId;

    if (isSameAppointment && this.showReasonPanel) {
      this.closeReasonPanel();
      return;
    }

    this.selectedAppointment = undefined;
    this.selectedCancelAppointment = undefined;

    this.showHealthRecordPanel = false;
    this.showCancelPanel = false;

    this.selectedReasonAppointment = appointment;
    this.showReasonPanel = true;
  }

  closeReasonPanel(): void {
    this.showReasonPanel = false;
    this.selectedReasonAppointment = undefined;
  }

  toggleCancelPanel(appointment: DoctorScheduleItem): void {
    const isSameAppointment =
      this.selectedCancelAppointment?.appointmentId === appointment.appointmentId;

    if (isSameAppointment && this.showCancelPanel) {
      this.closeCancelPanel();
      return;
    }

    this.selectedAppointment = undefined;
    this.selectedReasonAppointment = undefined;

    this.showHealthRecordPanel = false;
    this.showReasonPanel = false;

    this.selectedCancelAppointment = appointment;
    this.cancellationReason = '';
    this.showCancelPanel = true;

    this.errorMessage = '';
    this.successMessage = '';
  }

  closeCancelPanel(): void {
    if (this.cancellingAppointment) {
      return;
    }

    this.showCancelPanel = false;
    this.selectedCancelAppointment = undefined;
    this.cancellationReason = '';
  }

  submitCancelAppointment(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedCancelAppointment) {
      this.errorMessage = 'No appointment selected for cancellation.';
      return;
    }

    if (!this.cancellationReason.trim()) {
      this.errorMessage = 'Cancellation reason is required.';
      return;
    }

    this.cancellingAppointment = true;

    this.doctorDashboardService
      .cancelAppointment(
        this.selectedCancelAppointment.appointmentId,
        this.cancellationReason.trim()
      )
      .subscribe({
        next: () => {
          this.cancellingAppointment = false;
          this.showCancelPanel = false;
          this.selectedCancelAppointment = undefined;
          this.cancellationReason = '';

          this.successMessage = 'Appointment cancelled successfully.';
          this.loadSchedule();
        },
        error: (error) => {
          this.cancellingAppointment = false;

          if (error.status === 400 && typeof error.error === 'string') {
            this.errorMessage = error.error;
            return;
          }

          if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You are not authorized to cancel this appointment.';
            return;
          }

          if (error.status === 0) {
            this.errorMessage =
              'Could not connect to the API. Please make sure the API is running.';
            return;
          }

          this.errorMessage = 'Could not cancel appointment. Please try again.';
        }
      });
  }

  canConfirmAppointment(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Pending;
  }

  canCompleteAppointment(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Confirmed;
  }

  canAddHealthRecord(appointment: DoctorScheduleItem): boolean {
    return (
      appointment.status === AppointmentStatus.Completed &&
      !appointment.hasHealthRecord
    );
  }

  hasHealthRecordAdded(appointment: DoctorScheduleItem): boolean {
    return (
      appointment.status === AppointmentStatus.Completed &&
      appointment.hasHealthRecord
    );
  }

  canCancelAppointment(appointment: DoctorScheduleItem): boolean {
    return (
      appointment.status === AppointmentStatus.Pending ||
      appointment.status === AppointmentStatus.Confirmed
    );
  }

  isCancelled(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Cancelled;
  }

  get actionTitle(): string {
    if (this.pendingAction === 'confirm') {
      return 'Confirm Appointment';
    }

    if (this.pendingAction === 'complete') {
      return 'Complete Appointment';
    }

    return 'Confirm Action';
  }

  get actionMessage(): string {
    if (!this.selectedActionAppointment) {
      return '';
    }

    if (this.pendingAction === 'confirm') {
      return `Are you sure you want to confirm appointment #${this.selectedActionAppointment.appointmentId}?`;
    }

    if (this.pendingAction === 'complete') {
      return `Are you sure you want to mark appointment #${this.selectedActionAppointment.appointmentId} as completed?`;
    }

    return 'Are you sure you want to continue?';
  }

  get actionButtonText(): string {
    if (this.pendingAction === 'confirm') {
      return 'Yes, Confirm';
    }

    if (this.pendingAction === 'complete') {
      return 'Yes, Complete';
    }

    return 'Yes, Continue';
  }

  statusText(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'Pending';

      case AppointmentStatus.Confirmed:
        return 'Confirmed';

      case AppointmentStatus.Cancelled:
        return 'Cancelled';

      case AppointmentStatus.Completed:
        return 'Completed';

      default:
        return 'Unknown';
    }
  }

  statusClass(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'pending';

      case AppointmentStatus.Confirmed:
        return 'confirmed';

      case AppointmentStatus.Cancelled:
        return 'cancelled';

      case AppointmentStatus.Completed:
        return 'completed';

      default:
        return 'unknown';
    }
  }

  timeSlotText(timeSlot: number): string {
    switch (timeSlot) {
      case AppointmentTimeSlot.TenAM:
        return '10:00 AM - 10:30 AM';

      case AppointmentTimeSlot.TenThirtyAM:
        return '10:30 AM - 11:00 AM';

      case AppointmentTimeSlot.ElevenAM:
        return '11:00 AM - 11:30 AM';

      case AppointmentTimeSlot.ElevenThirtyAM:
        return '11:30 AM - 12:00 PM';

      case AppointmentTimeSlot.TwelvePM:
        return '12:00 PM - 12:30 PM';

      case AppointmentTimeSlot.TwelveThirtyPM:
        return '12:30 PM - 01:00 PM';

      case AppointmentTimeSlot.OnePM:
        return '01:00 PM - 01:30 PM';

      case AppointmentTimeSlot.OneThirtyPM:
        return '01:30 PM - 02:00 PM';

      case AppointmentTimeSlot.TwoPM:
        return '02:00 PM - 02:30 PM';

      case AppointmentTimeSlot.TwoThirtyPM:
        return '02:30 PM - 03:00 PM';

      case AppointmentTimeSlot.ThreePM:
        return '03:00 PM - 03:30 PM';

      case AppointmentTimeSlot.ThreeThirtyPM:
        return '03:30 PM - 04:00 PM';

      default:
        return `Slot ${timeSlot}`;
    }
  }

  private closeInlinePanels(): void {
    this.showHealthRecordPanel = false;
    this.showReasonPanel = false;
    this.showCancelPanel = false;

    this.selectedAppointment = undefined;
    this.selectedReasonAppointment = undefined;
    this.selectedCancelAppointment = undefined;

    this.cancellationReason = '';
  }

  private handleLoadError(error: any): void {
    this.loading = false;

    if (error.status === 401 || error.status === 403) {
      this.errorMessage =
        'You are not authorized to view schedule. Please login again.';
      return;
    }

    if (error.status === 0) {
      this.errorMessage =
        'Could not connect to the API. Please make sure the API is running.';
      return;
    }

    this.errorMessage = 'Could not load schedule. Please try again.';
  }

  private handleActionError(error: any, fallbackMessage: string): void {
    this.actionLoading = false;
    this.showActionConfirmModal = false;

    if (error.status === 400 && typeof error.error === 'string') {
      this.errorMessage = error.error;
      return;
    }

    if (error.status === 401 || error.status === 403) {
      this.errorMessage = 'You are not authorized to perform this action.';
      return;
    }

    if (error.status === 0) {
      this.errorMessage =
        'Could not connect to the API. Please make sure the API is running.';
      return;
    }

    this.errorMessage = fallbackMessage;
  }

  private sortSchedule(schedule: DoctorScheduleItem[]): DoctorScheduleItem[] {
    return [...schedule].sort((a, b) => {
      const dateA = new Date(a.scheduledDate).getTime();
      const dateB = new Date(b.scheduledDate).getTime();

      if (dateA !== dateB) {
        return dateA - dateB;
      }

      return a.timeSlot - b.timeSlot;
    });
  }

  private getCurrentWeekRange(): { startDate: string; endDate: string } {
    const today = new Date();
    const day = today.getDay();

    const diffToMonday = day === 0 ? -6 : 1 - day;

    const monday = new Date(today);
    monday.setDate(today.getDate() + diffToMonday);

    const sunday = new Date(monday);
    sunday.setDate(monday.getDate() + 6);

    return {
      startDate: this.formatDate(monday),
      endDate: this.formatDate(sunday)
    };
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}

