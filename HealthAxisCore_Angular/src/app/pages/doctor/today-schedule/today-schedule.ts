import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AppointmentService } from '../../../core/services/appointment.service';
import { HealthRecordService } from '../../../core/services/health-record.service';
import { AppointmentDto } from '../../../core/models/appointment.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-today-schedule',
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  templateUrl: './today-schedule.html',
  styleUrl: './today-schedule.css'
})
export class TodaySchedule {
  appointments = signal<AppointmentDto[]>([]);

  selectedAppointment = signal<AppointmentDto | null>(null);

  cancellationReason = signal('');

  cancellationSubmitted = signal(false);

  isLoading = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  today = new Date().toISOString().split('T')[0];

  constructor(
    private appointmentService: AppointmentService,
    private healthRecordService: HealthRecordService,
    private authService: AuthService
  ) {
    this.loadSchedule();
  }

  loadSchedule(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getAppointments({
      date: this.today
    }).subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.loadHealthRecordFlags(appointments);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  private loadHealthRecordFlags(appointments: AppointmentDto[]): void {
    const completedAppointments = appointments.filter(appointment =>
      appointment.status === 'Completed'
    );

    if (completedAppointments.length === 0) {
      this.isLoading.set(false);
      return;
    }

    let completedChecks = 0;

    completedAppointments.forEach(appointment => {
      this.healthRecordService.existsForAppointment(
        appointment.appointmentId
      ).subscribe({
        next: exists => {
          appointment.hasHealthRecord = exists;

          completedChecks++;

          if (completedChecks === completedAppointments.length) {
            this.appointments.set([...appointments]);
            this.isLoading.set(false);
          }
        },
        error: () => {
          appointment.hasHealthRecord = false;

          completedChecks++;

          if (completedChecks === completedAppointments.length) {
            this.appointments.set([...appointments]);
            this.isLoading.set(false);
          }
        }
      });
    });
  }

  canAddHealthRecord(appointment: AppointmentDto): boolean {
    return appointment.status === 'Completed' &&
      !appointment.hasHealthRecord;
  }

  getAddRecordText(appointment: AppointmentDto): string {
    if (appointment.status !== 'Completed') {
      return 'Add Record';
    }

    if (appointment.hasHealthRecord) {
      return 'Record Added';
    }

    return 'Add Record';
  }

  canConfirmAppointment(appointment: AppointmentDto): boolean {
    return appointment.status === 'Pending';
  }

  canCompleteAppointment(appointment: AppointmentDto): boolean {
    return appointment.status === 'Confirmed';
  }

  canCancelAppointment(appointment: AppointmentDto): boolean {
    return appointment.status === 'Pending' ||
      appointment.status === 'Confirmed';
  }

  markConfirmed(appointment: AppointmentDto): void {
    if (!this.canConfirmAppointment(appointment)) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateStatus(appointment.appointmentId, {
      status: 'Confirmed',
      cancellationReason: ''
    }).subscribe({
      next: () => {
        this.successMessage.set('Appointment confirmed successfully.');
        this.loadSchedule();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  markCompleted(appointment: AppointmentDto): void {
    if (!this.canCompleteAppointment(appointment)) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateStatus(appointment.appointmentId, {
      status: 'Completed',
      cancellationReason: ''
    }).subscribe({
      next: () => {
        this.successMessage.set('Appointment completed successfully.');
        this.loadSchedule();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
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

    const appointment = this.selectedAppointment();

    if (!appointment) {
      return;
    }

    if (!this.cancellationReason().trim()) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateStatus(appointment.appointmentId, {
      status: 'Cancelled',
      cancellationReason: this.cancellationReason().trim()
    }).subscribe({
      next: () => {
        this.successMessage.set('Appointment cancelled successfully.');
        this.closeCancelModal();
        this.loadSchedule();
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
