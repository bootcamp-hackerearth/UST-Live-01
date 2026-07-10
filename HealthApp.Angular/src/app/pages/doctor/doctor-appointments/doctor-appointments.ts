import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { NotificationService } from '../../../core/services/notification.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { HealthRecordService } from '../../../core/services/health-record.service';

import {
  AppointmentStatus,
  CancelAppointmentDto,
  DoctorAppointmentViewDto,
} from '../../../dtos/appointment.dto';

import { HealthRecordCreateDto } from '../../../dtos/health-record.dto';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css',
})
export class DoctorAppointments implements OnInit {
  searchText = '';
  selectedStatus = 'All';
  selectedDate = '';

  isLoading = false;
  isConfirming = false;
  isCancelling = false;
  isCompleting = false;
  isLoadingHealthRecord = false;

  appointments: DoctorAppointmentViewDto[] = [];

  selectedAppointmentForRecord: DoctorAppointmentViewDto | null = null;
  selectedAppointmentForViewRecord: DoctorAppointmentViewDto | null = null;
  selectedAppointmentForCancel: DoctorAppointmentViewDto | null = null;

  cancellationReason = '';

  healthRecordForm: HealthRecordCreateDto = {
    patientId: 0,
    doctorId: 0,
    appointmentId: null,
    visitDate: '',
    diagnosis: '',
    prescription: '',
    notes: '',
  };

  statusOptions = ['All', 'Pending', 'Confirmed', 'Cancelled', 'Completed'];

  constructor(
    private readonly notificationService: NotificationService,
    private readonly appointmentService: AppointmentService,
    private readonly healthRecordService: HealthRecordService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    this.appointmentService
      .getMyAppointments(false)
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (appointments) => {
          this.appointments = appointments ?? [];
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  get filteredAppointments(): DoctorAppointmentViewDto[] {
    const search = this.searchText.trim().toLowerCase();

    return this.appointments.filter((appointment) => {
      const matchesSearch =
        !search ||
        appointment.appointmentId.toString().includes(search) ||
        appointment.patientName.toLowerCase().includes(search) ||
        appointment.doctorName.toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'All' || appointment.status === this.selectedStatus;

      const matchesDate =
        !this.selectedDate || appointment.scheduledDate.substring(0, 10) === this.selectedDate;

      return matchesSearch && matchesStatus && matchesDate;
    });
  }

  get totalAppointments(): number {
    return this.appointments.length;
  }

  get pendingCount(): number {
    return this.appointments.filter((x) => x.status === 'Pending').length;
  }

  get confirmedCount(): number {
    return this.appointments.filter((x) => x.status === 'Confirmed').length;
  }

  get completedCount(): number {
    return this.appointments.filter((x) => x.status === 'Completed').length;
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedStatus = 'All';
    this.selectedDate = '';
  }

  confirmAppointment(appointment: DoctorAppointmentViewDto): void {
    if (appointment.status !== 'Pending') {
      this.notificationService.warning('Only pending appointments can be confirmed.');
      return;
    }

    this.isConfirming = true;
    this.cdr.markForCheck();

    this.appointmentService
      .confirmAppointment(appointment.appointmentId)
      .pipe(
        finalize(() => {
          this.isConfirming = false;
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (response) => {
          appointment.status = 'Confirmed';

          this.notificationService.success(
            response.message || `Appointment #APT-${appointment.appointmentId} confirmed.`,
          );

          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  openCancelModal(appointment: DoctorAppointmentViewDto): void {
    if (!this.canCancel(appointment.status)) {
      this.notificationService.warning('Only pending or confirmed appointments can be cancelled.');
      return;
    }

    this.selectedAppointmentForCancel = appointment;
    this.cancellationReason = '';
    this.cdr.markForCheck();
  }

  closeCancelModal(): void {
    if (this.isCancelling) {
      return;
    }

    this.selectedAppointmentForCancel = null;
    this.cancellationReason = '';
    this.cdr.markForCheck();
  }

  confirmCancellation(): void {
    if (!this.selectedAppointmentForCancel) {
      this.notificationService.error('No appointment selected.');
      return;
    }

    const reason = this.cancellationReason.trim();

    if (!reason) {
      this.notificationService.warning('Cancellation reason is required.');
      return;
    }

    if (reason.length > 250) {
      this.notificationService.warning('Cancellation reason cannot exceed 250 characters.');
      return;
    }

    const appointment = this.selectedAppointmentForCancel;

    const dto: CancelAppointmentDto = {
      cancellationReason: reason,
    };

    this.isCancelling = true;
    this.cdr.markForCheck();

    this.appointmentService
      .cancelAppointment(appointment.appointmentId, dto)
      .pipe(
        finalize(() => {
          this.isCancelling = false;
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (response) => {
          appointment.status = 'Cancelled';
          appointment.cancellationReason = reason;

          this.notificationService.success(
            response.message || `Appointment #APT-${appointment.appointmentId} cancelled.`,
          );

          this.selectedAppointmentForCancel = null;
          this.cancellationReason = '';
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  openHealthRecordModal(appointment: DoctorAppointmentViewDto): void {
    if (appointment.status !== 'Confirmed') {
      this.notificationService.warning('Only confirmed appointments can be completed.');
      return;
    }

    this.selectedAppointmentForRecord = appointment;

    this.healthRecordForm = {
      patientId: appointment.patientId,
      doctorId: appointment.doctorId,
      appointmentId: appointment.appointmentId,
      visitDate: appointment.scheduledDate.substring(0, 10),
      diagnosis: '',
      prescription: '',
      notes: '',
    };

    this.cdr.markForCheck();
  }

  closeHealthRecordModal(forceClose = false): void {
    if (this.isCompleting && !forceClose) {
      return;
    }

    this.selectedAppointmentForRecord = null;

    this.healthRecordForm = {
      patientId: 0,
      doctorId: 0,
      appointmentId: null,
      visitDate: '',
      diagnosis: '',
      prescription: '',
      notes: '',
    };

    this.cdr.markForCheck();
  }

  saveHealthRecord(): void {
    if (!this.selectedAppointmentForRecord) {
      this.notificationService.error('No appointment selected.');
      return;
    }

    if (!this.healthRecordForm.visitDate) {
      this.notificationService.warning('Visit date is required.');
      return;
    }

    const diagnosis = this.healthRecordForm.diagnosis.trim();
    const prescription = this.healthRecordForm.prescription.trim();
    const notes = this.healthRecordForm.notes?.trim() || null;

    if (!diagnosis) {
      this.notificationService.warning('Diagnosis is required.');
      return;
    }

    if (diagnosis.length < 3) {
      this.notificationService.warning('Diagnosis must be at least 3 characters.');
      return;
    }

    if (!prescription) {
      this.notificationService.warning('Prescription is required.');
      return;
    }

    if (prescription.length < 3) {
      this.notificationService.warning('Prescription must be at least 3 characters.');
      return;
    }

    if (notes && notes.length > 500) {
      this.notificationService.warning('Notes cannot exceed 500 characters.');
      return;
    }

    const appointment = this.selectedAppointmentForRecord;

    const payload: HealthRecordCreateDto = {
      patientId: appointment.patientId,
      doctorId: appointment.doctorId,
      appointmentId: appointment.appointmentId,
      visitDate: this.healthRecordForm.visitDate,
      diagnosis,
      prescription,
      notes,
    };

    this.isCompleting = true;
    this.cdr.markForCheck();

    this.healthRecordService
      .createHealthRecord(payload)
      .pipe(
        finalize(() => {
          this.isCompleting = false;
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (response) => {
          appointment.status = 'Completed';

          this.notificationService.success(
            response.message ||
              `Health record created and appointment #APT-${appointment.appointmentId} completed.`,
          );

          this.closeHealthRecordModal(true);
          this.loadAppointments();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  openViewHealthRecordModal(appointment: DoctorAppointmentViewDto): void {
    if (appointment.status !== 'Completed') {
      this.notificationService.warning(
        'Health record is available only for completed appointments.',
      );
      return;
    }

    if (appointment.healthRecord) {
      this.selectedAppointmentForViewRecord = appointment;
      this.cdr.markForCheck();
      return;
    }

    this.isLoadingHealthRecord = true;
    this.cdr.markForCheck();

    this.healthRecordService
      .getHealthRecordByAppointmentId(appointment.appointmentId)
      .pipe(
        finalize(() => {
          this.isLoadingHealthRecord = false;
          this.cdr.markForCheck();
        }),
      )
      .subscribe({
        next: (record) => {
          appointment.healthRecord = record;
          this.selectedAppointmentForViewRecord = appointment;
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        },
      });
  }

  closeViewHealthRecordModal(): void {
    this.selectedAppointmentForViewRecord = null;
    this.cdr.markForCheck();
  }

  canConfirm(status: AppointmentStatus): boolean {
    return status === 'Pending';
  }

  canCancel(status: AppointmentStatus): boolean {
    return status === 'Pending' || status === 'Confirmed';
  }

  canComplete(status: AppointmentStatus): boolean {
    return status === 'Confirmed';
  }

  canViewHealthRecord(appointment: DoctorAppointmentViewDto): boolean {
    return appointment.status === 'Completed';
  }

  canViewPatientRecords(appointment: DoctorAppointmentViewDto): boolean {
    return appointment.status === 'Confirmed' || appointment.status === 'Completed';
  }

  hasNoActions(appointment: DoctorAppointmentViewDto): boolean {
    return appointment.status === 'Cancelled';
  }

  getStatusClass(status: AppointmentStatus): string {
    switch (status) {
      case 'Pending':
        return 'appointment-status-pending';

      case 'Confirmed':
        return 'appointment-status-confirmed';

      case 'Cancelled':
        return 'appointment-status-cancelled';

      case 'Completed':
        return 'appointment-status-completed';

      default:
        return 'appointment-status-pending';
    }
  }

  getStatusIcon(status: AppointmentStatus): string {
    switch (status) {
      case 'Pending':
        return 'bi bi-hourglass-split';

      case 'Confirmed':
        return 'bi bi-check-circle';

      case 'Cancelled':
        return 'bi bi-x-circle';

      case 'Completed':
        return 'bi bi-calendar-check';

      default:
        return 'bi bi-info-circle';
    }
  }

  viewPatientRecords(appointment: DoctorAppointmentViewDto): void {
    this.router.navigate(['/doctor/health-records', appointment.patientId]);
  }
}
