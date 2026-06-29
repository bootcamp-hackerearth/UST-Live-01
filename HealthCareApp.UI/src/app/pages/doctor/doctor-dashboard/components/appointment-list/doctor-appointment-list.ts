import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppointmentDto } from '../../../../../shared/models/appointment.models';
import { DoctorFakeDataService } from '../../../../../core/services/doctor-fake-data.service';

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
export class DoctorAppointmentList {
  appointments: AppointmentDto[] = [];

  searchTerm = '';
  selectedStatus = '';
  selectedTimeSlot = '';
  selectedDate = '';

  todayDate = new Date().toISOString().split('T')[0];

  selectedAppointment?: AppointmentDto;

  isConfirmModalOpen = false;
  confirmMessage = '';

  isHealthRecordModalOpen = false;
  isHealthRecordSaveConfirmOpen = false;
  isDiscardHealthRecordModalOpen = false;

  hasRecordSubmitted = false;
  recordMessage = '';

  recordForm: HealthRecordForm = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  @Output() refreshDashboard = new EventEmitter<void>();
  @Output() doctorToast = new EventEmitter<DoctorToastEvent>();

  constructor(private service: DoctorFakeDataService) {
    this.loadAppointments();
  }

  get filteredAppointments(): AppointmentDto[] {
    const term = this.searchTerm.trim().toLowerCase();

    return this.appointments.filter((appointment: AppointmentDto) =>
      this.matchesSearchTerm(appointment, term) &&
      this.matchesStatusFilter(appointment) &&
      this.matchesTimeSlotFilter(appointment) &&
      this.matchesDateFilter(appointment)
    );
  }

  get statusOptions(): string[] {
    return Array.from(
      new Set(
        this.appointments.map(
          (appointment: AppointmentDto) => appointment.status
        )
      )
    );
  }

  get timeSlotOptions(): string[] {
    return Array.from(
      new Set(
        this.appointments.map(
          (appointment: AppointmentDto) => appointment.timeSlot
        )
      )
    );
  }

  get hasActiveFilters(): boolean {
    return (
      this.searchTerm.trim().length > 0 ||
      !!this.selectedStatus ||
      !!this.selectedTimeSlot ||
      !!this.selectedDate
    );
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
    this.appointments = this.service.getDoctorAppointments().sort(
      (a: AppointmentDto, b: AppointmentDto) =>
        new Date(b.scheduledDate).getTime() -
        new Date(a.scheduledDate).getTime()
    );
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedStatus = '';
    this.selectedTimeSlot = '';
    this.selectedDate = '';
  }

  canAddHealthRecord(appointment: AppointmentDto): boolean {
    return (
      appointment.status === 'Confirmed' &&
      appointment.scheduledDate === this.todayDate
    );
  }

  isFutureConfirmedAppointment(appointment: AppointmentDto): boolean {
    return (
      appointment.status === 'Confirmed' &&
      appointment.scheduledDate > this.todayDate
    );
  }

  isPastConfirmedAppointment(appointment: AppointmentDto): boolean {
    return (
      appointment.status === 'Confirmed' &&
      appointment.scheduledDate < this.todayDate
    );
  }

  openConfirmModal(appointment: AppointmentDto): void {
    this.selectedAppointment = appointment;
    this.confirmMessage = '';
    this.isConfirmModalOpen = true;
  }

  closeConfirmModal(): void {
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

    try {
      this.service.confirmAppointment(this.selectedAppointment.appointmentId);

      this.closeConfirmModal();
      this.loadAppointments();
      this.refreshDashboard.emit();
      this.emitToast('Appointment confirmed successfully ✅', 'success');
    } catch (error: unknown) {
      this.confirmMessage = this.getErrorMessage(error);
    }
  }

  openHealthRecordModal(appointment: AppointmentDto): void {
    if (!this.canAddHealthRecord(appointment)) {
      this.emitToast(
        'Health record can be added only on the appointment date.',
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
    this.isHealthRecordSaveConfirmOpen = false;
  }

  confirmSaveHealthRecord(): void {
    if (!this.selectedAppointment) {
      this.recordMessage = 'Please select an appointment.';
      this.isHealthRecordSaveConfirmOpen = false;
      return;
    }

    try {
      /*
        Important flow:
        1. Add health record first.
        2. Only if saving health record succeeds, mark appointment as Completed.
        3. If saving fails, appointment remains Confirmed.
      */
      this.service.addHealthRecord({
        patientId: this.selectedAppointment.patientId,
        patientName: this.selectedAppointment.patientName,
        doctorId: this.selectedAppointment.doctorId,
        doctorName: this.selectedAppointment.doctorName,
        appointmentId: this.selectedAppointment.appointmentId,
        visitDate: this.selectedAppointment.scheduledDate,
        diagnosis: this.recordForm.diagnosis,
        prescription: this.recordForm.prescription,
        notes: this.recordForm.notes
      });

      this.service.completeAppointment(this.selectedAppointment.appointmentId);

      this.closeHealthRecordModal();
      this.loadAppointments();
      this.refreshDashboard.emit();

      this.emitToast(
        'Health record saved and appointment completed ✅',
        'success'
      );
    } catch (error: unknown) {
      this.isHealthRecordSaveConfirmOpen = false;
      this.recordMessage = this.getErrorMessage(error);
    }
  }

  getStatusClass(status: string): string {
    return `da-status ${status.toLowerCase()}`;
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
      appointment.patientName.toLowerCase().includes(term) ||
      appointment.appointmentId.toString().includes(term)
    );
  }

  private matchesStatusFilter(appointment: AppointmentDto): boolean {
    if (!this.selectedStatus) {
      return true;
    }

    return appointment.status === this.selectedStatus;
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
    if (error instanceof Error) {
      return error.message;
    }

    return 'Something went wrong while updating the appointment.';
  }
}