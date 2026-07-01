import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { Appointment, AppointmentStatus } from '../../../core/models/appointment.model';
import { AddHealthRecordRequest } from '../../../core/models/health-record.model';
import { AuthService } from '../../../core/services/auth.service';
import { DoctorPortalStateService } from '../../../core/services/doctor-portal-state.service';
import { Pagination } from '../../../shared/pagination/pagination';

type DateFilter = 'All' | 'Today' | 'Upcoming' | 'Past';
type StatusFilter = AppointmentStatus | 'All';

@Component({
  selector: 'app-doctor-appointments',
  imports: [FormsModule, Pagination],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css',
})
export class DoctorAppointments implements OnInit {
  readonly doctorState = inject(DoctorPortalStateService);

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly searchText = signal('');
  readonly statusFilter = signal<StatusFilter>('All');
  readonly dateFilter = signal<DateFilter>('All');

  readonly currentPage = signal(1);
  readonly pageSize = 5;

  readonly selectedCancelAppointment = signal<Appointment | null>(null);
  readonly selectedRecordAppointment = signal<Appointment | null>(null);
  readonly selectedCancelledAppointment = signal<Appointment | null>(null);

  readonly cancellationReason = signal('');
  readonly diagnosis = signal('');
  readonly prescription = signal('');
  readonly notes = signal('');

  readonly message = signal('');
  readonly isError = signal(false);

  readonly filteredAppointments = computed(() => {
    let result = [...this.doctorState.appointments()];

    const search = this.searchText().trim().toLowerCase();

    if (search) {
      result = result.filter(appointment =>
        appointment.appointmentId.toString().includes(search) ||
        appointment.patientId.toString().includes(search) ||
        appointment.patientName?.toLowerCase().includes(search) ||
        appointment.timeSlot.toLowerCase().includes(search)
      );
    }

    if (this.statusFilter() !== 'All') {
      result = result.filter(appointment => appointment.status === this.statusFilter());
    }

    const today = this.toDateOnly(new Date());

    if (this.dateFilter() === 'Today') {
      result = result.filter(appointment => this.toDateOnly(appointment.scheduledDate) === today);
    }

    if (this.dateFilter() === 'Upcoming') {
      result = result.filter(appointment => this.toDateOnly(appointment.scheduledDate) > today);
    }

    if (this.dateFilter() === 'Past') {
      result = result.filter(appointment => this.toDateOnly(appointment.scheduledDate) < today);
    }

    return result;
  });

  readonly pagedFilteredAppointments = computed(() => {
    const startIndex = (this.currentPage() - 1) * this.pageSize;

    return this.filteredAppointments().slice(startIndex, startIndex + this.pageSize);
  });

  ngOnInit(): void {
    if (this.authService.currentRole() !== 'Doctor') {
      this.router.navigate(['/login']);
      return;
    }

    const status = this.route.snapshot.queryParamMap.get('status');

    if (
      status === 'Pending' ||
      status === 'Confirmed' ||
      status === 'Completed' ||
      status === 'Cancelled'
    ) {
      this.statusFilter.set(status);
    }

    this.doctorState.loadDoctorPortal();
  }

  updateSearchText(value: string): void {
    this.searchText.set(value);
    this.resetPage();
  }

  updateStatusFilter(value: string): void {
    this.statusFilter.set(this.toStatusFilter(value));
    this.resetPage();
  }

  updateDateFilter(value: string): void {
    this.dateFilter.set(this.toDateFilter(value));
    this.resetPage();
  }

  clearFilters(): void {
    this.searchText.set('');
    this.statusFilter.set('All');
    this.dateFilter.set('All');
    this.currentPage.set(1);
    this.router.navigate(['/doctor/appointments']);
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
  }

  openPatientModal(appointment: Appointment): void {
    this.doctorState.loadPatientContext(appointment.patientId);
  }

  closePatientModal(): void {
    this.doctorState.closePatientContext();
  }

  confirmAppointment(appointment: Appointment): void {
    this.doctorState.confirmAppointment(appointment.appointmentId).subscribe({
      next: () => this.showSuccess('Appointment confirmed successfully.'),
      error: error => this.showError(this.authService.getErrorMessage(error))
    });
  }

  openCancelModal(appointment: Appointment): void {
    this.selectedCancelAppointment.set(appointment);
    this.cancellationReason.set('');
    this.clearMessage();
  }

  closeCancelModal(): void {
    this.selectedCancelAppointment.set(null);
    this.cancellationReason.set('');
  }

  cancelAppointment(): void {
    const appointment = this.selectedCancelAppointment();

    if (!appointment) {
      return;
    }

    this.doctorState.cancelAppointment(
      appointment.appointmentId,
      this.cancellationReason()
    ).subscribe({
      next: () => {
        this.showSuccess('Appointment cancelled successfully.');
        this.closeCancelModal();
      },
      error: error => this.showError(this.authService.getErrorMessage(error))
    });
  }

  openAddRecordPanel(appointment: Appointment): void {
    this.selectedRecordAppointment.set(appointment);
    this.diagnosis.set('');
    this.prescription.set('');
    this.notes.set('');
    this.clearMessage();
  }

  closeAddRecordPanel(): void {
    this.selectedRecordAppointment.set(null);
    this.diagnosis.set('');
    this.prescription.set('');
    this.notes.set('');
  }

  addHealthRecord(): void {
    const appointment = this.selectedRecordAppointment();

    if (!appointment) {
      return;
    }

    const request: AddHealthRecordRequest = {
      patientId: appointment.patientId,
      doctorId: appointment.doctorId,
      appointmentId: appointment.appointmentId,
      diagnosis: this.diagnosis(),
      prescription: this.prescription(),
      notes: this.notes(),
      visitDate: appointment.scheduledDate
    };

    this.doctorState.addHealthRecord(request).subscribe({
      next: () => {
        this.showSuccess('Health record added successfully.');
        this.closeAddRecordPanel();
      },
      error: error => this.showError(this.authService.getErrorMessage(error))
    });
  }

  openRecordModal(appointment: Appointment): void {
    this.doctorState.loadRecordByAppointment(appointment);
  }

  closeRecordModal(): void {
    this.doctorState.closeRecord();
  }

  openCancelledDetailsModal(appointment: Appointment): void {
    this.selectedCancelledAppointment.set(appointment);
  }

  closeCancelledDetailsModal(): void {
    this.selectedCancelledAppointment.set(null);
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private resetPage(): void {
    this.currentPage.set(1);
  }

  private showSuccess(message: string): void {
    this.message.set(message);
    this.isError.set(false);
  }

  private showError(message: string): void {
    this.message.set(message);
    this.isError.set(true);
  }

  private clearMessage(): void {
    this.message.set('');
    this.isError.set(false);
  }

  private toStatusFilter(value: string): StatusFilter {
    if (
      value === 'Pending' ||
      value === 'Confirmed' ||
      value === 'Completed' ||
      value === 'Cancelled'
    ) {
      return value;
    }

    return 'All';
  }

  private toDateFilter(value: string): DateFilter {
    if (
      value === 'Today' ||
      value === 'Upcoming' ||
      value === 'Past'
    ) {
      return value;
    }

    return 'All';
  }

  private toDateOnly(value: string | Date): string {
    return new Date(value).toISOString().split('T')[0];
  }
}