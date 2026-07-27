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

const PENDING_STATUS = 'pending';
const CONFIRMED_STATUS = 'confirmed';
const COMPLETED_STATUS = 'completed';
const CANCELLED_STATUS = 'cancelled';

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


  readonly patientDisplayName = computed(() => {
    const fullName = this.patient()?.fullName?.trim();
    return fullName || 'Patient';
  });

  readonly pendingCount = computed(() =>
    this.countByStatus(PENDING_STATUS)
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus(CONFIRMED_STATUS)
  );

  readonly completedCount = computed(() =>
    this.countByStatus(COMPLETED_STATUS)
  );

  readonly cancelledCount = computed(() =>
    this.countByStatus(CANCELLED_STATUS)
  );

  readonly upcomingAppointment =
    computed<Appointment | null>(() => {
      const currentTime = Date.now();

      const appointment = this.appointments()
        .filter((item) =>
          this.isUpcomingAppointment(item, currentTime)
        )
        .sort((first, second) =>
          this.getAppointmentStartValue(first) -
          this.getAppointmentStartValue(second)
        )[0];

      return appointment ?? null;
    });

  constructor() {
    this.loadDashboard();
  }

  getStatusClass(status: string): string {
    switch (this.normalizeStatus(status)) {
      case CONFIRMED_STATUS:
        return 'status-confirmed';

      case COMPLETED_STATUS:
        return 'status-completed';

      case CANCELLED_STATUS:
        return 'status-cancelled';

      default:
        return 'status-pending';
    }
  }

  private loadDashboard(): void {
    this.errorMessage.set('');

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
    const currentTime = Date.now();

    return this.appointments().filter(
      (appointment) => {
        const normalizedStatus =
          this.normalizeStatus(
            appointment.status
          );

        if (normalizedStatus !== status) {
          return false;
        }

        if (status === PENDING_STATUS) {
          return this.getAppointmentStartValue(
            appointment
          ) > currentTime;
        }

        return true;
      }
    ).length;
  }

  private isUpcomingAppointment(
    appointment: Appointment,
    currentTime: number
  ): boolean {
    const status =
      this.normalizeStatus(appointment.status);

    const isActiveStatus =
      status === PENDING_STATUS ||
      status === CONFIRMED_STATUS;

    return isActiveStatus &&
      this.getAppointmentStartValue(appointment) >=
        currentTime;
  }

  private getAppointmentStartValue(
    appointment: Appointment
  ): number {
    const appointmentDate =
      new Date(appointment.scheduledDate);

    if (Number.isNaN(appointmentDate.getTime())) {
      return Number.MAX_SAFE_INTEGER;
    }

    const startTime =
      this.parseTimeSlotStart(appointment.timeSlot);

    if (startTime) {
      appointmentDate.setHours(
        startTime.hours,
        startTime.minutes,
        0,
        0
      );
    } else {
      appointmentDate.setHours(0, 0, 0, 0);
    }

    return appointmentDate.getTime();
  }

  private parseTimeSlotStart(
    timeSlot: string | null | undefined
  ): {
    hours: number;
    minutes: number;
  } | null {
    const startTimeText =
      timeSlot?.split('-')[0]?.trim();

    if (!startTimeText) {
      return null;
    }

    const match =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)$/i.exec(
        startTimeText
      );

    if (!match) {
      return null;
    }

    const [, hourText, minuteText, periodText] =
      match;

    let hours = Number.parseInt(hourText, 10);
    const minutes =
      Number.parseInt(minuteText, 10);

    if (
      hours < 1 ||
      hours > 12 ||
      minutes < 0 ||
      minutes > 59
    ) {
      return null;
    }

    const period = periodText.toUpperCase();

    if (period === 'AM' && hours === 12) {
      hours = 0;
    } else if (period === 'PM' && hours !== 12) {
      hours += 12;
    }

    return {
      hours,
      minutes
    };
  }

  private normalizeStatus(
    status: string | null | undefined
  ): string {
    return status?.trim().toLowerCase() ?? '';
  }

}