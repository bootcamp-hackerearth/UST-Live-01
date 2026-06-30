import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { timeout } from 'rxjs';

import { AppointmentDto } from '../../../../../shared/models/appointment.models';

import {
  AppointmentApiService,
  AppointmentStatusText
} from '../../../../../core/services/appointment-api.service';

import { HealthRecordApiService } from '../../../../../core/services/health-record-api.service';

type ToastType = 'success' | 'info' | 'warning';

interface DoctorToastEvent {
  message: string;
  type: ToastType;
}

interface HealthRecordForm {
  diagnosis: string;
  prescription: string;
  notes: string;
}

@Component({
  selector: 'app-doctor-appointment-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './doctor-appointment-list.html',
  styleUrl: './doctor-appointment-list.css'
})
export class DoctorAppointmentList implements OnInit {
  appointments: AppointmentDto[] = [];

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

  @Output() refreshDashboard = new EventEmitter<void>();
  @Output() doctorToast = new EventEmitter<DoctorToastEvent>();

  constructor(
    private appointmentApiService: AppointmentApiService,
    private healthRecordApiService: HealthRecordApiService
  ) {
  }

  ngOnInit(): void {
    this.loadAppointments();
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
        this.refreshDashboard.emit();
        this.emitToast('Appointment confirmed successfully ✅', 'success');
      },
      error: (error: unknown) => {
        console.log('Doctor confirm appointment API error:', error);
        this.isConfirming = false;
        this.confirmMessage = this.getErrorMessage(error);
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
        this.refreshDashboard.emit();

        this.emitToast(
          'Health record saved and appointment completed ✅',
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