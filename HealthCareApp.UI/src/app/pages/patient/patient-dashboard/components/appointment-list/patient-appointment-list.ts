import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppointmentDto } from '../../../../../shared/models/appointment.models';
import { PatientFakeDataService } from '../../../../../core/services/patient-fake-data.service';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-appointment-list.html',
  styleUrl: './patient-appointment-list.css'
})
export class PatientAppointmentList {

  appointments: AppointmentDto[] = [];

  isCancelModalOpen = false;
  cancellationReason = '';
  cancellationMessage = '';
  selectedAppointment?: AppointmentDto;

  @Output() refreshDashboard = new EventEmitter<void>();

  constructor(private service: PatientFakeDataService) {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.appointments = this.service.getAppointments().sort(
      (a: AppointmentDto, b: AppointmentDto) =>
        new Date(b.scheduledDate).getTime() -
        new Date(a.scheduledDate).getTime()
    );
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

    try {
      this.service.cancelAppointment({
        appointmentId: this.selectedAppointment.appointmentId,
        reason
      });

      this.closeCancelModal();
      this.loadAppointments();
      this.refreshDashboard.emit();
    } catch (error: unknown) {
      this.cancellationMessage = this.getErrorMessage(error);
    }
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
    if (error instanceof Error) {
      return error.message;
    }

    return 'Something went wrong while cancelling the appointment.';
  }
}