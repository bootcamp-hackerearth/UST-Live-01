import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { Doctor } from '../../core/models/doctor.model';
import { DoctorService } from '../../core/services/doctor.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [DatePipe, RouterLink],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorDashboard {
  private readonly doctorService = inject(DoctorService);
  private readonly appointmentService = inject(AppointmentService);

  readonly doctor = signal<Doctor | null>(null);
  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly todayAppointments = computed(() =>
    this.appointments().filter((appointment) =>
      this.isSameDate(new Date(appointment.scheduledDate), new Date())
    )
  );

  readonly weekAppointments = computed(() =>
    this.appointments().filter((appointment) =>
      this.isWithinCurrentWeek(new Date(appointment.scheduledDate))
    )
  );

  readonly pendingCount = computed(() => this.countByStatus('Pending'));
  readonly confirmedCount = computed(() => this.countByStatus('Confirmed'));
  readonly completedCount = computed(() => this.countByStatus('Completed'));
  readonly cancelledCount = computed(() => this.countByStatus('Cancelled'));

  readonly nextAppointment = computed<Appointment | null>(() => {
    const now = new Date();

    const appointment = this.appointments()
      .filter((item) => {
        const appointmentDate = new Date(item.scheduledDate);
        const status = this.getStatusText(item.status);

        return appointmentDate >= this.startOfToday(now) &&
          status !== 'cancelled' &&
          status !== 'completed';
      })
      .sort((first, second) =>
        new Date(first.scheduledDate).getTime() -
        new Date(second.scheduledDate).getTime()
      )[0];

    return appointment ?? null;
  });

  constructor() {
    this.loadDashboard();
  }

  private loadDashboard(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.doctorService.getMyDoctorProfile().subscribe({
      next: (doctor) => {
        this.doctor.set(doctor);
      },
      error: (error: unknown) => {
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load doctor profile.')
        );
      }
    });

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load doctor appointments.')
        );
      }
    });
  }

  private countByStatus(status: string): number {
    return this.appointments().filter(
      (appointment) => this.getStatusText(appointment.status) === status.toLowerCase()
    ).length;
  }

  private isSameDate(firstDate: Date, secondDate: Date): boolean {
    return firstDate.getFullYear() === secondDate.getFullYear() &&
      firstDate.getMonth() === secondDate.getMonth() &&
      firstDate.getDate() === secondDate.getDate();
  }

  private isWithinCurrentWeek(date: Date): boolean {
    const today = new Date();
    const startOfWeek = this.startOfToday(today);
    const endOfWeek = this.startOfToday(today);

    endOfWeek.setDate(startOfWeek.getDate() + 6);

    return date >= startOfWeek && date <= endOfWeek;
  }

  private startOfToday(date: Date): Date {
    const cleanDate = new Date(date);
    cleanDate.setHours(0, 0, 0, 0);

    return cleanDate;
  }

  private getStatusText(status: string | number): string {
    return String(status).trim().toLowerCase();
  }
}