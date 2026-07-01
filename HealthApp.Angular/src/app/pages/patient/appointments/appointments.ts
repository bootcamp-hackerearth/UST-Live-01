import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { NotificationService } from '../../../core/services/notification.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { HealthRecordService } from '../../../core/services/health-record.service';

import {
  AppointmentDto,
  AppointmentStatus,
  CancelAppointmentDto
} from '../../../dtos/appointment.dto';

import { HealthRecordDto } from '../../../dtos/health-record.dto';

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './appointments.html',
  styleUrl: './appointments.css'
})
export class MyAppointments implements OnInit {
  searchText = '';
  selectedStatus = 'All';
  selectedDate = '';

  isLoading = false;
  isCancelling = false;
  isLoadingHealthRecord = false;

  appointments: AppointmentDto[] = [];
  healthRecords: HealthRecordDto[] = [];

  selectedAppointmentForCancel: AppointmentDto | null = null;
  selectedAppointmentForViewRecord: AppointmentDto | null = null;
  selectedHealthRecord: HealthRecordDto | null = null;

  cancellationReason = '';

  statusOptions = [
    'All',
    'Pending',
    'Confirmed',
    'Cancelled',
    'Completed'
  ];

  constructor(
    private readonly appointmentService: AppointmentService,
    private readonly healthRecordService: HealthRecordService,
    private readonly notificationService: NotificationService,
    private readonly cdr: ChangeDetectorRef
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
        })
      )
      .subscribe({
        next: appointments => {
          this.appointments = appointments ?? [];
          this.cdr.markForCheck();
        },
        error: () => {
          this.appointments = [];
          this.cdr.markForCheck();
        }
      });
  }

  get filteredAppointments(): AppointmentDto[] {
    const search = this.searchText.trim().toLowerCase();

    return this.appointments.filter(appointment => {
      const matchesSearch =
        !search ||
        appointment.appointmentId.toString().includes(search) ||
        appointment.doctorName.toLowerCase().includes(search) ||
        appointment.patientName.toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'All' ||
        appointment.status === this.selectedStatus;

      const matchesDate =
        !this.selectedDate ||
        appointment.scheduledDate.substring(0, 10) === this.selectedDate;

      return matchesSearch && matchesStatus && matchesDate;
    });
  }

  get totalAppointments(): number {
    return this.appointments.length;
  }

  get pendingCount(): number {
    return this.appointments.filter(appointment => appointment.status === 'Pending').length;
  }

  get confirmedCount(): number {
    return this.appointments.filter(appointment => appointment.status === 'Confirmed').length;
  }

  get completedCount(): number {
    return this.appointments.filter(appointment => appointment.status === 'Completed').length;
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedStatus = 'All';
    this.selectedDate = '';
    this.cdr.markForCheck();
  }

  canCancel(status: AppointmentStatus): boolean {
    return status === 'Pending' || status === 'Confirmed';
  }

  canViewHealthRecord(status: AppointmentStatus): boolean {
    return status === 'Completed';
  }

  hasNoActions(status: AppointmentStatus): boolean {
    return status === 'Cancelled';
  }

  openCancelModal(appointment: AppointmentDto): void {
    if (!this.canCancel(appointment.status)) {
      this.notificationService.warning(
        'Only pending or confirmed appointments can be cancelled.'
      );
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
      this.notificationService.warning(
        'Cancellation reason cannot exceed 250 characters.'
      );
      return;
    }

    const appointment = this.selectedAppointmentForCancel;

    const payload: CancelAppointmentDto = {
      cancellationReason: reason
    };

    this.isCancelling = true;
    this.cdr.markForCheck();

    this.appointmentService
      .cancelAppointment(appointment.appointmentId, payload)
      .pipe(
        finalize(() => {
          this.isCancelling = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: response => {
          appointment.status = 'Cancelled';
          appointment.cancellationReason = reason;

          this.notificationService.success(
            response.message ||
              `Appointment #APT-${appointment.appointmentId} cancelled successfully.`
          );

          this.selectedAppointmentForCancel = null;
          this.cancellationReason = '';
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  openViewHealthRecordModal(appointment: AppointmentDto): void {
    if (appointment.status !== 'Completed') {
      this.notificationService.warning(
        'Health record is available only for completed appointments.'
      );
      return;
    }

    const cachedRecord = this.findHealthRecordForAppointment(appointment.appointmentId);

    if (cachedRecord) {
      this.selectedAppointmentForViewRecord = appointment;
      this.selectedHealthRecord = cachedRecord;
      this.cdr.markForCheck();
      return;
    }

    this.isLoadingHealthRecord = true;
    this.cdr.markForCheck();

    this.healthRecordService
      .getMyHealthRecords()
      .pipe(
        finalize(() => {
          this.isLoadingHealthRecord = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: records => {
          this.healthRecords = records ?? [];

          const record = this.findHealthRecordForAppointment(appointment.appointmentId);

          if (!record) {
            this.notificationService.warning(
              'No health record found for this appointment yet.'
            );
            return;
          }

          this.selectedAppointmentForViewRecord = appointment;
          this.selectedHealthRecord = record;
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  closeViewHealthRecordModal(): void {
    this.selectedAppointmentForViewRecord = null;
    this.selectedHealthRecord = null;
    this.cdr.markForCheck();
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

  private findHealthRecordForAppointment(appointmentId: number): HealthRecordDto | null {
    return this.healthRecords.find(record => record.appointmentId === appointmentId) ?? null;
  }
}
