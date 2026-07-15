import {
  Component,
  OnDestroy,
  OnInit,
  computed,
  inject,
  signal
} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { Appointment, AppointmentStatus } from '../../../core/models/appointment.model';
import { AddHealthRecordRequest } from '../../../core/models/health-record.model';
import { AuthService } from '../../../core/services/auth.service';
import { DoctorPortalStateService } from '../../../core/services/doctor-portal-state.service';
import { Pagination } from '../../../shared/pagination/pagination';

type DateFilter = 'All' | 'Today' | 'Upcoming' | 'Past';
type StatusFilter = AppointmentStatus | 'All';
type ReminderSeverity = 'info' | 'warning' | 'urgent' | 'critical' | 'overdue';

interface PendingAppointmentReminder {
  appointment: Appointment;
  scheduledStart: Date;
  minutesRemaining: number;
  severity: ReminderSeverity;
  title: string;
  message: string;
  timeLabel: string;
}

@Component({
  selector: 'app-doctor-appointments',
  imports: [FormsModule, Pagination],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css',
})
export class DoctorAppointments implements OnInit, OnDestroy {
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

  readonly currentTime = signal(new Date());
  readonly showAllReminders = signal(false);
  private reminderTimerId: ReturnType<typeof setInterval> | null = null;

  readonly pendingReminders = computed<PendingAppointmentReminder[]>(() => {
    const now = this.currentTime();

    return this.doctorState.appointments()
      .filter(appointment => appointment.status === 'Pending')
      .map(appointment => this.createPendingReminder(appointment, now))
      .filter((reminder): reminder is PendingAppointmentReminder => reminder !== null)
      .sort((a, b) => {
        const priority =
          this.severityPriority(a.severity) -
          this.severityPriority(b.severity);

        return priority === 0
          ? a.scheduledStart.getTime() -
              b.scheduledStart.getTime()
          : priority;
      });
  });

  readonly visiblePendingReminders = computed(() =>
    this.showAllReminders()
      ? this.pendingReminders()
      : this.pendingReminders().slice(0, 3)
  );

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

    if (status === 'Pending' || status === 'Confirmed' || status === 'Completed' || status === 'Cancelled') {
      this.statusFilter.set(status);
    }

    this.doctorState.loadDoctorPortal();
    this.currentTime.set(new Date());
    this.reminderTimerId = setInterval(() => this.currentTime.set(new Date()), 60_000);
  }

  ngOnDestroy(): void {
    if (this.reminderTimerId !== null) {
      clearInterval(this.reminderTimerId);
      this.reminderTimerId = null;
    }
  }

  updateSearchText(value: string): void { this.searchText.set(value); this.resetPage(); }
  updateStatusFilter(value: string): void { this.statusFilter.set(this.toStatusFilter(value)); this.resetPage(); }
  updateDateFilter(value: string): void { this.dateFilter.set(this.toDateFilter(value)); this.resetPage(); }

  clearFilters(): void {
    this.searchText.set('');
    this.statusFilter.set('All');
    this.dateFilter.set('All');
    this.currentPage.set(1);
    this.showAllReminders.set(false);
    this.router.navigate(['/doctor/appointments']);
  }

  showAllPendingReminders(): void {
    this.showAllReminders.set(true);
    this.searchText.set('');
    this.statusFilter.set('Pending');
    this.dateFilter.set('All');
    this.currentPage.set(1);
    this.router.navigate(['/doctor/appointments'], { queryParams: { status: 'Pending' } });
  }

  onPageChange(page: number): void { this.currentPage.set(page); }
  openPatientModal(appointment: Appointment): void { this.doctorState.loadPatientContext(appointment.patientId); }
  closePatientModal(): void { this.doctorState.closePatientContext(); }

  confirmAppointment(appointment: Appointment): void {
    const start = this.getAppointmentStartDate(appointment);

    if (start && start.getTime() <= this.currentTime().getTime()) {
      this.showError('This appointment has already reached its scheduled start time and cannot be confirmed.');
      return;
    }

    this.doctorState.confirmAppointment(appointment.appointmentId).subscribe({
      next: () => { this.showSuccess('Appointment confirmed successfully.'); this.currentTime.set(new Date()); },
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
    if (!appointment) return;

    const reason = this.cancellationReason().trim();
    if (!reason) { this.showError('Cancellation reason is required.'); return; }

    this.doctorState.cancelAppointment(appointment.appointmentId, reason).subscribe({
      next: () => { this.showSuccess('Appointment cancelled successfully.'); this.closeCancelModal(); this.currentTime.set(new Date()); },
      error: error => this.showError(this.authService.getErrorMessage(error))
    });
  }

  openAddRecordPanel(appointment: Appointment): void {
    this.selectedRecordAppointment.set(appointment);
    this.diagnosis.set(''); this.prescription.set(''); this.notes.set(''); this.clearMessage();
  }

  closeAddRecordPanel(): void {
    this.selectedRecordAppointment.set(null);
    this.diagnosis.set(''); this.prescription.set(''); this.notes.set('');
  }

  addHealthRecord(): void {
    const appointment = this.selectedRecordAppointment();
    if (!appointment) return;

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
      next: () => { this.showSuccess('Health record added successfully.'); this.closeAddRecordPanel(); },
      error: error => this.showError(this.authService.getErrorMessage(error))
    });
  }

  openRecordModal(appointment: Appointment): void { this.doctorState.loadRecordByAppointment(appointment); }
  closeRecordModal(): void { this.doctorState.closeRecord(); }
  openCancelledDetailsModal(appointment: Appointment): void { this.selectedCancelledAppointment.set(appointment); }
  closeCancelledDetailsModal(): void { this.selectedCancelledAppointment.set(null); }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  private createPendingReminder(appointment: Appointment, now: Date): PendingAppointmentReminder | null {
    const start = this.getAppointmentStartDate(appointment);
    if (!start) return null;

    const minutes = Math.ceil((start.getTime() - now.getTime()) / 60_000);
    if (minutes > 360 || minutes < -1_440) return null;

    const severity = this.reminderSeverity(minutes);
    return {
      appointment,
      scheduledStart: start,
      minutesRemaining: minutes,
      severity,
      title: this.reminderTitle(severity),
      message: this.reminderMessage(appointment, minutes, severity),
      timeLabel: this.timeLabel(minutes)
    };
  }

  private reminderSeverity(minutes: number): ReminderSeverity {
    if (minutes < 0) return 'overdue';
    if (minutes <= 30) return 'critical';
    if (minutes <= 60) return 'urgent';
    if (minutes <= 180) return 'warning';
    return 'info';
  }

  private severityPriority(value: ReminderSeverity): number {
    return { overdue: 0, critical: 1, urgent: 2, warning: 3, info: 4 }[value];
  }

  private reminderTitle(value: ReminderSeverity): string {
    return {
      overdue: 'Overdue Pending Appointment',
      critical: 'Immediate Action Required',
      urgent: 'Urgent Confirmation Required',
      warning: 'Confirmation Pending',
      info: 'Appointment Confirmation Required'
    }[value];
  }

  private reminderMessage(appointment: Appointment, minutes: number, severity: ReminderSeverity): string {
    const patient = appointment.patientName || 'the patient';
    const remaining = this.timeLabel(minutes);

    if (severity === 'overdue') {
      return `The appointment for ${patient} has passed and is still pending. Please review and cancel it if the consultation did not proceed.`;
    }

    if (severity === 'critical') return `The appointment for ${patient} starts in ${remaining}. Confirm or cancel it immediately.`;
    if (severity === 'urgent') return `The appointment for ${patient} starts in ${remaining} and is still pending.`;
    if (severity === 'warning') return `The appointment for ${patient} starts in ${remaining}. Please confirm or cancel it soon.`;
    return `The appointment for ${patient} starts in ${remaining}. Please review its confirmation status.`;
  }

  private timeLabel(minutes: number): string {
    if (minutes < 0) {
      return this.overdueTimeLabel(Math.abs(minutes));
    }

    return this.upcomingTimeLabel(minutes);
  }

  private overdueTimeLabel(minutes: number): string {
    if (minutes < 60) {
      return `${this.formatUnit(minutes, 'minute')} overdue`;
    }

    return `${this.formatHoursAndMinutes(minutes)} overdue`;
  }

  private upcomingTimeLabel(minutes: number): string {
    if (minutes === 0) {
      return 'starting now';
    }

    if (minutes < 60) {
      return this.formatUnit(minutes, 'minute');
    }

    return this.formatHoursAndMinutes(minutes);
  }

  private formatHoursAndMinutes(totalMinutes: number): string {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;

    if (minutes === 0) {
      return this.formatUnit(hours, 'hour');
    }

    return `${hours}h ${minutes}m`;
  }

  private formatUnit(
    value: number,
    unit: 'minute' | 'hour'
  ): string {
    const suffix = value === 1 ? '' : 's';

    return `${value} ${unit}${suffix}`;
  }

  private getAppointmentStartDate(
    appointment: Appointment
  ): Date | null {
    const dateOnly = this.toDateOnly(
      appointment.scheduledDate
    );

    const startText =
      appointment.timeSlot
        .split('-')[0]
        ?.trim();

    if (!startText) {
      return null;
    }

    const timePattern =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)$/i;

    const match =
      timePattern.exec(startText);

    if (!match) {
      return null;
    }

    let hours = Number(match[1]);
    const minutes = Number(match[2]);
    const period = match[3].toUpperCase();

    if (
      hours < 1 ||
      hours > 12 ||
      minutes < 0 ||
      minutes > 59
    ) {
      return null;
    }

    if (
      period === 'AM' &&
      hours === 12
    ) {
      hours = 0;
    }

    if (
      period === 'PM' &&
      hours !== 12
    ) {
      hours += 12;
    }

    const dateParts =
      dateOnly
        .split('-')
        .map(Number);

    const year = dateParts[0];
    const month = dateParts[1];
    const day = dateParts[2];

    if (!year || !month || !day) {
      return null;
    }

    return new Date(
      year,
      month - 1,
      day,
      hours,
      minutes,
      0,
      0
    );
  }

  private resetPage(): void { this.currentPage.set(1); }
  private showSuccess(value: string): void { this.message.set(value); this.isError.set(false); }
  private showError(value: string): void { this.message.set(value); this.isError.set(true); }
  private clearMessage(): void { this.message.set(''); this.isError.set(false); }

  private toStatusFilter(value: string): StatusFilter {
    return value === 'Pending' || value === 'Confirmed' || value === 'Completed' || value === 'Cancelled'
      ? value
      : 'All';
  }

  private toDateFilter(value: string): DateFilter {
    return value === 'Today' || value === 'Upcoming' || value === 'Past' ? value : 'All';
  }

  private toDateOnly(value: string | Date): string {
    const date = new Date(value);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
