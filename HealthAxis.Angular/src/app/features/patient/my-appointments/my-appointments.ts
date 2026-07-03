import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Appointment } from '../../../core/models/appointment.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { PatientService } from '../../../core/services/patient.service';
import { Pagination } from '../../../shared/pagination/pagination';

@Component({
  selector: 'app-my-appointments',
  imports: [FormsModule, Pagination],
  templateUrl: './my-appointments.html',
  styleUrls: ['./my-appointments.css']
})
export class MyAppointments implements OnInit {
  isLoading = false;
  isCancelling = false;

  errorMessage = '';
  successMessage = '';

  appointments: Appointment[] = [];
  pagedAppointments: Appointment[] = [];

  selectedAppointmentId: number | null = null;
  cancellationReason = '';

  currentPage = 1;
  pageSize = 5;
  totalItems = 0;

  constructor(private readonly patientService: PatientService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        this.appointments = this.appointments.sort(
          (a, b) =>
            new Date(b.scheduledDate).getTime() -
            new Date(a.scheduledDate).getTime()
        );

        this.totalItems = this.appointments.length;
        this.updatePagedAppointments();
      },

      error: (error: HttpErrorResponse) => {
        console.log('My appointments error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load appointments. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onPageChanged(page: number): void {
    this.currentPage = page;
    this.updatePagedAppointments();
  }

  private updatePagedAppointments(): void {
    const startIndex =
      (this.currentPage - 1) * this.pageSize;

    const endIndex =
      startIndex + this.pageSize;

    this.pagedAppointments =
      this.appointments.slice(startIndex, endIndex);
  }

  showCancelBox(appointmentId: number): void {
    this.selectedAppointmentId = appointmentId;
    this.cancellationReason = '';
    this.errorMessage = '';
    this.successMessage = '';
  }

  hideCancelBox(): void {
    this.selectedAppointmentId = null;
    this.cancellationReason = '';
  }

  submitCancellation(appointmentId: number): void {
    this.isCancelling = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientService
      .cancelAppointment(
        appointmentId,
        this.cancellationReason.trim()
      )
      .subscribe({
        next: () => {
          this.successMessage =
            'Appointment cancelled successfully.';

          this.hideCancelBox();
          this.loadAppointments();
        },

        error: (error: HttpErrorResponse) => {
          console.log('Cancel appointment error:', error);

          this.errorMessage =
            error.error?.message ??
            `Unable to cancel appointment. Status: ${error.status}`;
        },

        complete: () => {
          this.isCancelling = false;
        }
      });
  }

  canCancel(status: number | string): boolean {
  const normalizedStatus =
    this.getStatusText(status).toLowerCase();

  return (
    normalizedStatus === 'scheduled' ||
    normalizedStatus === 'confirmed'
  );
}

canCancelAppointment(status: number | string): boolean {
  return this.canCancel(status);
}

  getStatusText(status: number | string): string {
    if (typeof status === 'string') {
      return status;
    }

    const statusMap: Record<number, string> = {
      0: 'Scheduled',
      1: 'Scheduled',
      2: 'Confirmed',
      3: 'Cancelled',
      4: 'Completed'
    };

    return statusMap[status] ?? `Status ${status}`;
  }

  getStatusClass(status: number | string): string {
    const normalizedStatus =
      this.getStatusText(status).toLowerCase();

    if (normalizedStatus === 'completed') {
      return 'status-completed';
    }

    if (normalizedStatus === 'confirmed') {
      return 'status-confirmed';
    }

    if (normalizedStatus === 'cancelled') {
      return 'status-cancelled';
    }

    return 'status-scheduled';
  }

  formatDate(value: string): string {
    if (!value) {
      return '-';
    }

    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getDoctorDisplayName(appointment: Appointment): string {
    return appointment.doctorName ??
      `Doctor ID ${appointment.doctorId ?? '-'}`;
  }
}