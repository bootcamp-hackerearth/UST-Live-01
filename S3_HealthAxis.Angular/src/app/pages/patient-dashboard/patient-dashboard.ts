import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { TokenService } from '../../core/services/token.service';
import { PatientPortalService } from '../../core/services/patient-portal.service';

import {
  AppointmentStatus,
  AppointmentTimeSlot,
  PatientAppointment,
  PatientHealthRecord,
  PatientProfile
} from '../../shared/models/patient-dashboard.models';

@Component({
  selector: 'app-patient-dashboard',
  imports: [
    RouterLink,
    DatePipe
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard implements OnInit {
  loading = true;
  errorMessage = '';

  patient?: PatientProfile;
  appointments: PatientAppointment[] = [];
  healthRecords: PatientHealthRecord[] = [];

  constructor(
    private tokenService: TokenService,
    private patientPortalService: PatientPortalService
  ) {}

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;
    this.errorMessage = '';

    const patientId = this.tokenService.getReferenceId();

    if (!patientId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      return;
    }

    forkJoin({
      patient: this.patientPortalService.getPatientProfile(patientId),
      appointments: this.patientPortalService.getPatientAppointments(patientId)
    }).subscribe({
      next: ({ patient, appointments }) => {
        this.patient = patient;
        this.appointments = this.sortAppointments(appointments ?? []);

        this.patientPortalService
          .getHealthRecordsForAppointments(this.appointments)
          .subscribe({
            next: (records) => {
              this.healthRecords = records ?? [];
              this.loading = false;
            },
            error: () => {
              this.healthRecords = [];
              this.loading = false;
            }
          });
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view this dashboard. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load patient dashboard. Please try again.';
      }
    });
  }

  get patientName(): string {
    return this.patient?.fullName ?? 'Patient';
  }

  get patientEmail(): string {
    return this.patient?.email ?? this.tokenService.getEmail() ?? 'Email not available';
  }

  get patientPhone(): string {
    return this.patient?.phoneNumber ?? 'Phone not available';
  }

  get totalAppointments(): number {
    return this.appointments.length;
  }

  get pendingAppointments(): number {
    return this.appointments.filter(
      appointment => appointment.status === AppointmentStatus.Pending
    ).length;
  }

  get confirmedAppointments(): number {
    return this.appointments.filter(
      appointment => appointment.status === AppointmentStatus.Confirmed
    ).length;
  }

  get cancelledAppointments(): number {
    return this.appointments.filter(
      appointment => appointment.status === AppointmentStatus.Cancelled
    ).length;
  }

  get completedAppointments(): number {
    return this.appointments.filter(
      appointment => appointment.status === AppointmentStatus.Completed
    ).length;
  }

  get upcomingAppointment(): PatientAppointment | undefined {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const upcomingAppointments = this.appointments
      .filter(appointment => {
        const appointmentDate = this.getAppointmentDate(appointment.scheduledDate);

        if (!appointmentDate) {
          return false;
        }

        appointmentDate.setHours(0, 0, 0, 0);

        return (
          appointmentDate >= today &&
          appointment.status !== AppointmentStatus.Cancelled &&
          appointment.status !== AppointmentStatus.Completed
        );
      })
      .sort((a, b) => {
        const dateA = this.getAppointmentDate(a.scheduledDate)?.getTime() ?? 0;
        const dateB = this.getAppointmentDate(b.scheduledDate)?.getTime() ?? 0;

        if (dateA !== dateB) {
          return dateA - dateB;
        }

        return a.timeSlot - b.timeSlot;
      });

    return upcomingAppointments[0];
  }

  get recentAppointments(): PatientAppointment[] {
    return this.appointments.slice(0, 4);
  }

  get recentHealthRecords(): PatientHealthRecord[] {
    return this.healthRecords.slice(0, 3);
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

  recordDoctorName(record: PatientHealthRecord): string {
    return (
      record.doctorName ??
      record.doctorFullName ??
      'Doctor'
    );
  }

  recordSpecialisation(record: PatientHealthRecord): string {
    return (
      record.specialisation ??
      record.doctorSpecialisation ??
      'Specialist'
    );
  }

  recordVisitDate(record: PatientHealthRecord): string | undefined {
    return record.visitDate ?? record.createdDate;
  }

  recordMedication(record: PatientHealthRecord): string {
    return (
      record.prescription ??
      record.prescribedMedication ??
      record.medication ??
      'Medication not available'
    );
  }

  private sortAppointments(appointments: PatientAppointment[]): PatientAppointment[] {
    return [...appointments].sort((a, b) => {
      const dateA = this.getAppointmentDate(a.scheduledDate)?.getTime() ?? 0;
      const dateB = this.getAppointmentDate(b.scheduledDate)?.getTime() ?? 0;

      if (dateB !== dateA) {
        return dateB - dateA;
      }

      return b.timeSlot - a.timeSlot;
    });
  }

  private getAppointmentDate(value: string | undefined | null): Date | null {
    if (!value) {
      return null;
    }

    const date = new Date(value);

    return Number.isNaN(date.getTime()) ? null : date;
  }
}

