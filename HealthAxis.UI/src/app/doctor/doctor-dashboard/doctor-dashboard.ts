import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { catchError, forkJoin, of } from 'rxjs';

import { Appointment } from '../../core/models/appointment.model';
import { Doctor } from '../../core/models/doctor.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { DoctorService } from '../../core/services/doctor.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorDashboard {
  private readonly doctorService =
    inject(DoctorService);

  private readonly appointmentService =
    inject(AppointmentService);

  private readonly doctorStatusState =
    inject(DoctorStatusStateService);

  readonly doctor = signal<Doctor | null>(null);
  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly todayAppointments = computed(() => {
    const today = new Date();

    return this.appointments()
      .filter((appointment) =>
        this.isSameDate(
          this.createLocalDate(
            appointment.scheduledDate
          ),
          today
        )
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(
            first
          ).getTime() -
          this.getAppointmentStartDateTime(
            second
          ).getTime()
      );
  });

  readonly weekAppointments = computed(() =>
    this.appointments()
      .filter((appointment) =>
        this.isWithinNextSevenDays(
          this.createLocalDate(
            appointment.scheduledDate
          )
        )
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(
            first
          ).getTime() -
          this.getAppointmentStartDateTime(
            second
          ).getTime()
      )
  );

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

  readonly nextAppointment =
    computed<Appointment | null>(() => {
      const appointment = this.appointments()
        .filter((item) => {
          const status =
            this.normalizeStatus(item.status);

          return (
            (
              status === 'pending' ||
              status === 'confirmed'
            ) &&
            this.getAppointmentStartDateTime(
              item
            ).getTime() >= Date.now()
          );
        })
        .sort(
          (first, second) =>
            this.getAppointmentStartDateTime(
              first
            ).getTime() -
            this.getAppointmentStartDateTime(
              second
            ).getTime()
        )[0];

      return appointment ?? null;
    });

  readonly isDoctorAvailable = computed(() => {
    const sharedStatus =
      this.doctorStatusState.isActive();

    if (sharedStatus !== null) {
      return sharedStatus;
    }

    return Boolean(this.doctor()?.isActive);
  });

  constructor() {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    let profileError = '';
    let appointmentsError = '';

    const doctorRequest = this.doctorService
      .getMyDoctorProfile()
      .pipe(
        catchError((error: unknown) => {
          profileError =
            getFriendlyErrorMessage(
              error,
              'Could not load doctor profile.'
            );

          return of(null);
        })
      );

    const appointmentsRequest =
      this.appointmentService
        .getMyDoctorAppointments()
        .pipe(
          catchError((error: unknown) => {
            appointmentsError =
              getFriendlyErrorMessage(
                error,
                'Could not load doctor appointments.'
              );

            return of([] as Appointment[]);
          })
        );

    forkJoin({
      doctor: doctorRequest,
      appointments: appointmentsRequest
    }).subscribe({
      next: ({ doctor, appointments }) => {
        this.doctor.set(doctor);
        this.appointments.set(appointments);

        if (doctor) {
          this.doctorStatusState.setStatus(
            Boolean(doctor.isActive)
          );
        }

        const errors = [
          profileError,
          appointmentsError
        ].filter(Boolean);

        this.errorMessage.set(
          errors.join(' ')
        );

        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);

        this.errorMessage.set(
          'Could not load the doctor dashboard.'
        );
      }
    });
  }

  getDoctorFirstName(): string {
    const fullName = this.removeDoctorTitle(
      this.doctor()?.fullName
    );

    if (!fullName) {
      return 'Doctor';
    }

    return fullName.split(/\s+/)[0];
  }

  getDoctorDisplayName(): string {
    const fullName =
      this.doctor()?.fullName?.trim();

    if (!fullName) {
      return 'Doctor';
    }

    const normalizedName =
      fullName.toLowerCase();

    if (
      normalizedName.startsWith('dr.') ||
      normalizedName.startsWith('dr ')
    ) {
      return fullName;
    }

    return `Dr. ${fullName}`;
  }

  getDoctorAvailabilityText(): string {
    return this.isDoctorAvailable()
      ? 'Available for appointments'
      : 'Currently unavailable';
  }

  getStatusClass(status: string): string {
    switch (this.normalizeStatus(status)) {
      case 'pending':
        return 'status-badge pending-status';

      case 'confirmed':
        return 'status-badge confirmed-status';

      case 'completed':
        return 'status-badge completed-status';

      case 'cancelled':
        return 'status-badge cancelled-status';

      default:
        return 'status-badge default-status';
    }
  }

  getStatusLabel(status: string): string {
    const cleanStatus = status.trim();

    return cleanStatus || 'Pending';
  }

  getPatientName(
    name: string | null | undefined
  ): string {
    const cleanName = (name ?? '').trim();

    return cleanName || 'Patient';
  }

  getTimeSlot(
    timeSlot: string | null | undefined
  ): string {
    const cleanTimeSlot =
      (timeSlot ?? '').trim();

    return cleanTimeSlot ||
      'Time not assigned';
  }

  private countByStatus(
    status: string
  ): number {
    const normalizedStatus =
      this.normalizeStatus(status);

    return this.appointments()
      .filter(
        (appointment) =>
          this.normalizeStatus(
            appointment.status
          ) === normalizedStatus
      )
      .length;
  }

  private isSameDate(
    firstDate: Date,
    secondDate: Date
  ): boolean {
    return (
      firstDate.getFullYear() ===
        secondDate.getFullYear() &&
      firstDate.getMonth() ===
        secondDate.getMonth() &&
      firstDate.getDate() ===
        secondDate.getDate()
    );
  }

  private isWithinNextSevenDays(
    date: Date
  ): boolean {
    const startDate =
      this.startOfDay(new Date());

    const endDate =
      this.startOfDay(new Date());

    endDate.setDate(
      startDate.getDate() + 6
    );

    endDate.setHours(
      23,
      59,
      59,
      999
    );

    return (
      date >= startDate &&
      date <= endDate
    );
  }

  private getAppointmentStartDateTime(
    appointment: Appointment
  ): Date {
    const appointmentDate =
      this.createLocalDate(
        appointment.scheduledDate
      );

    const timeParts =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)/i.exec(
        appointment.timeSlot.trim()
      );

    if (!timeParts) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    let hours = Number(timeParts[1]);
    const minutes = Number(timeParts[2]);
    const period =
      timeParts[3].toUpperCase();

    if (
      period === 'PM' &&
      hours !== 12
    ) {
      hours += 12;
    }

    if (
      period === 'AM' &&
      hours === 12
    ) {
      hours = 0;
    }

    appointmentDate.setHours(
      hours,
      minutes,
      0,
      0
    );

    return appointmentDate;
  }

  private createLocalDate(
    dateValue: string
  ): Date {
    const dateOnly =
      dateValue.split('T')[0];

    const parts = dateOnly
      .split('-')
      .map(Number);

    if (
      parts.length === 3 &&
      parts.every((part) =>
        Number.isFinite(part)
      )
    ) {
      const [year, month, day] = parts;

      return new Date(
        year,
        month - 1,
        day
      );
    }

    return new Date(dateValue);
  }

  private startOfDay(date: Date): Date {
    const cleanDate = new Date(date);

    cleanDate.setHours(
      0,
      0,
      0,
      0
    );

    return cleanDate;
  }

  private normalizeStatus(
    status: string | number
  ): string {
    return String(status)
      .trim()
      .toLowerCase();
  }

  private removeDoctorTitle(
    name: string | null | undefined
  ): string {
    return (name ?? '')
      .trim()
      .replace(
        /^dr\.?\s+/i,
        ''
      );
  }
}

