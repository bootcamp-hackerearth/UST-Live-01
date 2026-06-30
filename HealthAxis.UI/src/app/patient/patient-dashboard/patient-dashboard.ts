import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Appointment } from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-patient-dashboard',
  imports: [RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientDashboard {
  private readonly patientService = inject(PatientService);
  private readonly appointmentService = inject(AppointmentService);

  readonly patient = signal<Patient | null>(null);
  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal('');

  readonly pendingCount = computed(() => this.countByStatus('Pending'));
  readonly confirmedCount = computed(() => this.countByStatus('Confirmed'));
  readonly completedCount = computed(() => this.countByStatus('Completed'));
  readonly cancelledCount = computed(() => this.countByStatus('Cancelled'));

readonly upcomingAppointment = computed<Appointment | null>(() => {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  const appointment = this.appointments()
    .filter((item) => {
      const appointmentDate = new Date(item.scheduledDate);
      appointmentDate.setHours(0, 0, 0, 0);

      return appointmentDate >= today &&
        item.status.toLowerCase() !== 'cancelled' &&
        item.status.toLowerCase() !== 'completed';
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

  loadDashboard(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    forkJoin({
      patient: this.patientService.getMyProfile(),
      appointments: this.appointmentService.getMyAppointments()
    }).subscribe({
      next: (result) => {
        this.patient.set(result.patient);
        this.appointments.set(result.appointments);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load patient dashboard.')
        );
      }
    });
  }

  private countByStatus(status: string): number {
    return this.appointments().filter(
      (appointment) => appointment.status.toLowerCase() === status.toLowerCase()
    ).length;
  }
}