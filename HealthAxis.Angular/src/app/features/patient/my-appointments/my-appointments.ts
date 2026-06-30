import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-my-appointments',
  imports: [FormsModule, RouterLink],
  templateUrl: './my-appointments.html',
  styleUrls: ['./my-appointments.css']
})
export class MyAppointments implements OnInit {
  isLoading = false;
  isCancelling = false;

  errorMessage = '';
  successMessage = '';

  appointments: Appointment[] = [];

  selectedAppointmentId: number | null = null;
  cancellationReason = '';

  constructor(private patientService: PatientService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        console.log('Appointments API response:', result);

        if (Array.isArray(result)) {
          this.appointments = result;
        } else {
          this.appointments = result.items ?? [];
        }

        this.appointments = this.appointments.sort(
          (a, b) =>
            new Date(b.scheduledDate).getTime() -
            new Date(a.scheduledDate).getTime()
        );
      },

      error: (error: HttpErrorResponse) => {
        console.log('Appointments error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load appointments. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
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
    if (!this.cancellationReason.trim()) {
      this.errorMessage = 'Cancellation reason is required.';
      return;
    }

    this.isCancelling = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientService
      .cancelAppointment(appointmentId, this.cancellationReason.trim())
      .subscribe({
        next: () => {
          this.successMessage = 'Appointment cancelled successfully.';
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

  canCancelAppointment(status: number | string): boolean {
    const normalizedStatus =
      this.getStatusText(status).toLowerCase();

    return (
      normalizedStatus === 'scheduled' ||
      normalizedStatus === 'pending' ||
      normalizedStatus === 'confirmed'
    );
  }

  getStatusText(status: number | string): string {
    if (typeof status === 'string') {
      return status;
    }

    const statusMap: Record<number, string> = {
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