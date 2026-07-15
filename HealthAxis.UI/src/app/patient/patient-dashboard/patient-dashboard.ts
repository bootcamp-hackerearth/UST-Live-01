import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../core/models/appointment.model';
import { Patient } from '../../core/models/patient.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-patient-dashboard',
  imports: [
    RouterLink,
    DatePipe
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientDashboard {
  private readonly patientService = inject(PatientService);
  private readonly appointmentService = inject(AppointmentService);

  readonly patient = signal<Patient | null>(null);
  readonly appointments = signal<Appointment[]>([]);
  readonly errorMessage = signal('');

  readonly pendingCount = computed(() =>
    this.countByStatus('Pending')
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus('Confirmed')
  );

  readonly completedCount = computed(() =>
    this.countByStatus('Completed')
  );

  readonly cancelledCount = computed(() =>
    this.countByStatus('Cancelled')
  );

  readonly upcomingAppointment =
    computed<Appointment | null>(() => {
      const today = this.getTodayDateOnly();

      const appointment = this.appointments()
        .filter((item) =>
          this.isUpcomingAppointment(item, today)
        )
        .sort((first, second) =>
          this.getAppointmentDateValue(first) -
          this.getAppointmentDateValue(second)
        )[0];

      return appointment ?? null;
    });

  constructor() {
    this.loadDashboard();
  }

  getPatientFirstName(): string {
    const fullName = this.patient()?.fullName?.trim();

    if (!fullName) {
      return 'Patient';
    }

    return fullName.split(' ')[0];
  }

  private loadDashboard(): void {
    this.patientService.getMyProfile().subscribe({
      next: (patient) => {
        this.patient.set(patient);
      },
      error: (error: unknown) => {
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not load patient profile.'
          )
        );
      }
    });

    this.appointmentService.getMyAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
      },
      error: (error: unknown) => {
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not load appointments.'
          )
        );
      }
    });
  }

  private countByStatus(status: string): number {
    return this.appointments().filter(
      (appointment) =>
        appointment.status.toLowerCase() ===
        status.toLowerCase()
    ).length;
  }

  private isUpcomingAppointment(
    appointment: Appointment,
    today: Date
  ): boolean {
    const appointmentDate =
      new Date(appointment.scheduledDate);

    appointmentDate.setHours(0, 0, 0, 0);

    const status = appointment.status.toLowerCase();

    return appointmentDate >= today &&
      status !== 'cancelled' &&
      status !== 'completed';
  }

  private getAppointmentDateValue(
    appointment: Appointment
  ): number {
    return new Date(
      appointment.scheduledDate
    ).getTime();
  }

  private getTodayDateOnly(): Date {
    const today = new Date();

    today.setHours(0, 0, 0, 0);

    return today;
  }
}