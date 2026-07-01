import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppointmentService } from '../../../core/services/appointment.service';
import { AppointmentDto } from '../../../core/models/appointment.model';
import { AuthService } from '../../../core/services/auth.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-appointments',
  imports: [
    FormsModule,
    CommonModule,
    RouterLink
  ],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css'
})
export class MyAppointments {
  appointments = signal<AppointmentDto[]>([]);

  selectedAppointment = signal<AppointmentDto | null>(null);

  cancellationReason = signal('');

  cancellationSubmitted = signal(false);

  isLoading = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService
  ) {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.appointmentService.getAppointments().subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  canCancelAppointment(appointment: AppointmentDto): boolean {
    return appointment.status === 'Pending' ||
      appointment.status === 'Confirmed';
  }

  openCancelModal(appointment: AppointmentDto): void {
    if (!this.canCancelAppointment(appointment)) {
      return;
    }

    this.selectedAppointment.set(appointment);
    this.cancellationReason.set('');
    this.cancellationSubmitted.set(false);
  }

  closeCancelModal(): void {
    this.selectedAppointment.set(null);
    this.cancellationReason.set('');
    this.cancellationSubmitted.set(false);
  }

  confirmCancellation(): void {
    this.cancellationSubmitted.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    const appointment = this.selectedAppointment();

    if (!appointment) {
      return;
    }

    if (!this.cancellationReason().trim()) {
      return;
    }

    this.appointmentService.updateStatus(appointment.appointmentId, {
      status: 'Cancelled',
      cancellationReason: this.cancellationReason().trim()
    }).subscribe({
      next: () => {
        this.successMessage.set('Appointment cancelled successfully.');
        this.closeCancelModal();
        this.loadAppointments();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }
}
