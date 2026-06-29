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

  searchTerm = '';
  selectedStatus = '';
  selectedSpecialisation = '';
  selectedTimeSlot = '';
  selectedDate = '';

  isCancelModalOpen = false;
  cancellationReason = '';
  cancellationMessage = '';
  selectedAppointment?: AppointmentDto;

  @Output() refreshDashboard = new EventEmitter<void>();

  constructor(private service: PatientFakeDataService) {
    this.loadAppointments();
  }

  get filteredAppointments(): AppointmentDto[] {
    const term = this.searchTerm.trim().toLowerCase();

    return this.appointments.filter((appointment: AppointmentDto) =>
      this.matchesSearchTerm(appointment, term) &&
      this.matchesStatusFilter(appointment) &&
      this.matchesSpecialisationFilter(appointment) &&
      this.matchesTimeSlotFilter(appointment) &&
      this.matchesDateFilter(appointment)
    );
  }

  get statusOptions(): string[] {
    return Array.from(
      new Set(this.appointments.map((appointment: AppointmentDto) => appointment.status))
    );
  }

  get specialisationOptions(): string[] {
    return Array.from(
      new Set(this.appointments.map((appointment: AppointmentDto) => appointment.specialisation))
    );
  }

  get timeSlotOptions(): string[] {
    return Array.from(
      new Set(this.appointments.map((appointment: AppointmentDto) => appointment.timeSlot))
    );
  }

  get hasActiveFilters(): boolean {
    return (
      this.searchTerm.trim().length > 0 ||
      !!this.selectedStatus ||
      !!this.selectedSpecialisation ||
      !!this.selectedTimeSlot ||
      !!this.selectedDate
    );
  }

  loadAppointments(): void {
    this.appointments = this.service.getAppointments().sort(
      (a: AppointmentDto, b: AppointmentDto) =>
        new Date(b.scheduledDate).getTime() -
        new Date(a.scheduledDate).getTime()
    );
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedSpecialisation = '';
    this.selectedTimeSlot = '';
    this.selectedDate = '';
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

  private matchesSearchTerm(
    appointment: AppointmentDto,
    term: string
  ): boolean {
    if (!term) {
      return true;
    }

    return (
      appointment.doctorName.toLowerCase().includes(term) ||
      appointment.appointmentId.toString().includes(term)
    );
  }

  private matchesStatusFilter(appointment: AppointmentDto): boolean {
    if (!this.selectedStatus) {
      return true;
    }

    return appointment.status === this.selectedStatus;
  }

  private matchesSpecialisationFilter(appointment: AppointmentDto): boolean {
    if (!this.selectedSpecialisation) {
      return true;
    }

    return appointment.specialisation === this.selectedSpecialisation;
  }

  private matchesTimeSlotFilter(appointment: AppointmentDto): boolean {
    if (!this.selectedTimeSlot) {
      return true;
    }

    return appointment.timeSlot === this.selectedTimeSlot;
  }

  private matchesDateFilter(appointment: AppointmentDto): boolean {
    if (!this.selectedDate) {
      return true;
    }

    return appointment.scheduledDate === this.selectedDate;
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof Error) {
      return error.message;
    }

    return 'Something went wrong while cancelling the appointment.';
  }
}