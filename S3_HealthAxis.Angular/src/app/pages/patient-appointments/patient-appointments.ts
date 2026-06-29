import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { AppointmentService } from '../../core/services/appointment.service';
import { TokenService } from '../../core/services/token.service';

import {
  AppointmentStatus,
  AppointmentTimeSlot,
  PatientAppointment
} from '../../shared/models/patient-dashboard.models';

@Component({
  selector: 'app-patient-appointments',
  imports: [
    DatePipe,
    FormsModule,
    RouterLink
  ],
  templateUrl: './patient-appointments.html',
  styleUrl: './patient-appointments.css'
})
export class PatientAppointments implements OnInit {
  loading = true;
  errorMessage = '';
  successMessage = '';

  appointments: PatientAppointment[] = [];

  showCancelModal = false;
  cancelling = false;
  selectedAppointment?: PatientAppointment;
  cancellationReason = '';

  constructor(
    private appointmentService: AppointmentService,
    private tokenService: TokenService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const booked = this.route.snapshot.queryParamMap.get('booked');

    if (booked === 'true') {
      this.successMessage = 'Appointment booked successfully.';
    }

    this.loadAppointments();
  }

  loadAppointments(): void {
    this.loading = true;
    this.errorMessage = '';

    const patientId = this.tokenService.getReferenceId();

    if (!patientId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      return;
    }

    this.appointmentService.getPatientAppointments(patientId).subscribe({
      next: (appointments) => {
        this.appointments = this.sortAppointments(appointments ?? []);
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view these appointments. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load appointments. Please try again.';
      }
    });
  }

  get totalAppointments(): number {
    return this.appointments.length;
  }

  get pendingAppointments(): number {
    return this.appointments.filter(a => a.status === AppointmentStatus.Pending).length;
  }

  get confirmedAppointments(): number {
    return this.appointments.filter(a => a.status === AppointmentStatus.Confirmed).length;
  }

  get completedAppointments(): number {
    return this.appointments.filter(a => a.status === AppointmentStatus.Completed).length;
  }

  get cancelledAppointments(): number {
    return this.appointments.filter(a => a.status === AppointmentStatus.Cancelled).length;
  }

  canCancel(appointment: PatientAppointment): boolean {
    return (
      appointment.status === AppointmentStatus.Pending ||
      appointment.status === AppointmentStatus.Confirmed
    );
  }

  openCancelModal(appointment: PatientAppointment): void {
    this.selectedAppointment = appointment;
    this.cancellationReason = '';
    this.errorMessage = '';
    this.successMessage = '';
    this.showCancelModal = true;
  }

  closeCancelModal(): void {
    if (this.cancelling) {
      return;
    }

    this.showCancelModal = false;
    this.selectedAppointment = undefined;
    this.cancellationReason = '';
  }

  confirmCancel(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedAppointment) {
      this.errorMessage = 'No appointment selected for cancellation.';
      this.showCancelModal = false;
      return;
    }

    if (!this.cancellationReason.trim()) {
      this.errorMessage = 'Please enter a cancellation reason.';
      return;
    }

    this.cancelling = true;

    this.appointmentService
      .cancelAppointment(this.selectedAppointment.appointmentId, {
        cancellationReason: this.cancellationReason.trim()
      })
      .subscribe({
        next: () => {
          this.cancelling = false;
          this.showCancelModal = false;
          this.successMessage = 'Appointment cancelled successfully.';
          this.selectedAppointment = undefined;
          this.cancellationReason = '';
          this.loadAppointments();
        },
        error: (error) => {
          this.cancelling = false;
          this.showCancelModal = false;

          if (error.status === 400 && typeof error.error === 'string') {
            this.errorMessage = error.error;
            return;
          }

          if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You are not authorized to cancel this appointment.';
            return;
          }

          if (error.status === 0) {
            this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
            return;
          }

          this.errorMessage = 'Could not cancel appointment. Please try again.';
        }
      });
  }

  statusText(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'Pending';

      case AppointmentStatus.Confirmed:
        return 'Confirmed';

      case AppointmentStatus.Cancelled:
        return 'Cancelled';

      case AppointmentStatus.Completed:
        return 'Completed';

      default:
        return 'Unknown';
    }
  }

  statusClass(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'pending';

      case AppointmentStatus.Confirmed:
        return 'confirmed';

      case AppointmentStatus.Cancelled:
        return 'cancelled';

      case AppointmentStatus.Completed:
        return 'completed';

      default:
        return 'unknown';
    }
  }

  timeSlotText(timeSlot: number): string {
    switch (timeSlot) {
      case AppointmentTimeSlot.TenAM:
        return '10:00 AM - 10:30 AM';

      case AppointmentTimeSlot.TenThirtyAM:
        return '10:30 AM - 11:00 AM';

      case AppointmentTimeSlot.ElevenAM:
        return '11:00 AM - 11:30 AM';

      case AppointmentTimeSlot.ElevenThirtyAM:
        return '11:30 AM - 12:00 PM';

      case AppointmentTimeSlot.TwelvePM:
        return '12:00 PM - 12:30 PM';

      case AppointmentTimeSlot.TwelveThirtyPM:
        return '12:30 PM - 01:00 PM';

      case AppointmentTimeSlot.OnePM:
        return '01:00 PM - 01:30 PM';

      case AppointmentTimeSlot.OneThirtyPM:
        return '01:30 PM - 02:00 PM';

      case AppointmentTimeSlot.TwoPM:
        return '02:00 PM - 02:30 PM';

      case AppointmentTimeSlot.TwoThirtyPM:
        return '02:30 PM - 03:00 PM';

      case AppointmentTimeSlot.ThreePM:
        return '03:00 PM - 03:30 PM';

      case AppointmentTimeSlot.ThreeThirtyPM:
        return '03:30 PM - 04:00 PM';

      default:
        return `Slot ${timeSlot}`;
    }
  }

  private sortAppointments(appointments: PatientAppointment[]): PatientAppointment[] {
    return [...appointments].sort((a, b) => {
      const dateA = new Date(a.scheduledDate).getTime();
      const dateB = new Date(b.scheduledDate).getTime();

      if (dateB !== dateA) {
        return dateB - dateA;
      }

      return b.timeSlot - a.timeSlot;
    });
  }
}

