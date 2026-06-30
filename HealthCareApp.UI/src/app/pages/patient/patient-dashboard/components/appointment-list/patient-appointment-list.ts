import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppointmentDto } from '../../../../../shared/models/appointment.models';
import {
  AppointmentApiService,
  AppointmentStatusText
} from '../../../../../core/services/appointment-api.service';
import { finalize, timeout } from 'rxjs';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-appointment-list.html',
  styleUrl: './patient-appointment-list.css'
})
export class PatientAppointmentList {
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

  isCancelModalOpen = false;
  cancellationReason = '';
  cancellationMessage = '';
  selectedAppointment?: AppointmentDto;

  @Output() refreshDashboard = new EventEmitter<void>();

  constructor(private appointmentApiService: AppointmentApiService) {
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
    timeout(15000),
    finalize(() => {
      this.isLoading = false;
    })
  ).subscribe({
    next: (response) => {
      this.appointments = response.items;
      this.pageNumber = response.pageNumber;
      this.pageSize = response.pageSize;
      this.totalRecords = response.totalRecords;
      this.totalPages = response.totalPages;
    },
    error: (error: unknown) => {
      console.log('Patient appointments API error:', error);
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

  openCancelModal(appointment: AppointmentDto): void {
    this.selectedAppointment = appointment;
    this.cancellationReason = '';
    this.cancellationMessage = '';
    this.isCancelModalOpen = true;
  }

  closeCancelModal(): void {
    this.isCancelModalOpen = false;
    this.selectedAppointment = undefined;
    this.cancellationReason = '';
    this.cancellationMessage = '';
  }

  confirmCancellation(): void {
    this.cancellationMessage = '';

    if (!this.selectedAppointment) {
      this.cancellationMessage = 'Please select an appointment to cancel.';
      return;
    }

    const reason = this.cancellationReason.trim();

    if (reason.length < 5) {
      this.cancellationMessage =
        'Please enter a cancellation reason with at least 5 characters.';
      return;
    }

    this.appointmentApiService.cancelAppointment({
      appointmentId: this.selectedAppointment.appointmentId,
      reason
    }).subscribe({
      next: () => {
        this.closeCancelModal();
        this.loadAppointments();
        this.refreshDashboard.emit();
      },
      error: (error: unknown) => {
        this.cancellationMessage = this.getErrorMessage(error);
      }
    });
  }

  getStatusClass(status: string): string {
    return `pd-status ${status.toLowerCase()}`;
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
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
        };
      };

      return (
        apiError.error?.message ??
        apiError.error?.Message ??
        'Something went wrong while loading appointments.'
      );
    }

    return 'Something went wrong while loading appointments.';
  }
}