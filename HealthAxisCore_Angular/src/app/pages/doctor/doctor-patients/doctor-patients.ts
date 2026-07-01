import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppointmentService } from '../../../core/services/appointment.service';
import { AppointmentDto } from '../../../core/models/appointment.model';
import { AuthService } from '../../../core/services/auth.service';

interface DoctorPatientView {
  patientId: number;
  patientName: string;
  completedVisits: number;
  totalValidAppointments: number;
  lastVisit: string | null;
  nextAppointment: string | null;
}

@Component({
  selector: 'app-doctor-patients',
  imports: [
    CommonModule,
    RouterLink,
    FormsModule
  ],
  templateUrl: './doctor-patients.html',
  styleUrl: './doctor-patients.css'
})
export class DoctorPatients {
  appointments = signal<AppointmentDto[]>([]);

  searchText = signal('');

  isLoading = signal(false);

  errorMessage = signal('');

  today = new Date().toISOString().split('T')[0];

  patients = computed(() => {
    const patientMap = new Map<number, DoctorPatientView>();

    const validAppointments = this.appointments()
      .filter(appointment => appointment.status !== 'Cancelled');

    for (const appointment of validAppointments) {
      const appointmentDate = this.getDateOnly(appointment.scheduledDate);

      const existingPatient = patientMap.get(appointment.patientId);

      if (!existingPatient) {
        patientMap.set(appointment.patientId, {
          patientId: appointment.patientId,
          patientName: appointment.patientName,
          completedVisits: appointment.status === 'Completed' ? 1 : 0,
          totalValidAppointments: 1,
          lastVisit: appointment.status === 'Completed'
            ? appointment.scheduledDate
            : null,
          nextAppointment: this.isUpcomingAppointment(appointment)
            ? appointment.scheduledDate
            : null
        });

        continue;
      }

      existingPatient.totalValidAppointments++;

      if (appointment.status === 'Completed') {
        existingPatient.completedVisits++;

        if (
          !existingPatient.lastVisit ||
          appointmentDate > this.getDateOnly(existingPatient.lastVisit)
        ) {
          existingPatient.lastVisit = appointment.scheduledDate;
        }
      }

      if (this.isUpcomingAppointment(appointment)) {
        if (
          !existingPatient.nextAppointment ||
          appointmentDate < this.getDateOnly(existingPatient.nextAppointment)
        ) {
          existingPatient.nextAppointment = appointment.scheduledDate;
        }
      }
    }

    return Array.from(patientMap.values())
      .sort((firstPatient, secondPatient) =>
        firstPatient.patientName.localeCompare(secondPatient.patientName)
      );
  });

  filteredPatients = computed(() => {
    const search = this.searchText().trim().toLowerCase();

    return this.patients().filter(patient =>
      !search ||
      patient.patientName.toLowerCase().includes(search)
    );
  });

  constructor(
    private appointmentService: AppointmentService,
    private authService: AuthService
  ) {
    this.loadPatients();
  }

  loadPatients(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.appointmentService.getAppointments().subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  private isUpcomingAppointment(appointment: AppointmentDto): boolean {
    const appointmentDate = this.getDateOnly(appointment.scheduledDate);

    return (
      appointmentDate >= this.today &&
      (
        appointment.status === 'Pending' ||
        appointment.status === 'Confirmed'
      )
    );
  }

  private getDateOnly(dateValue: string): string {
    return dateValue.split('T')[0];
  }
}
