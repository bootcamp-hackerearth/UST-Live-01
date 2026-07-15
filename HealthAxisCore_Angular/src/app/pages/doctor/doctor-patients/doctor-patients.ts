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
    const patientMap = this.buildPatientMap();

    return Array.from(patientMap.values())
      .sort((firstPatient, secondPatient) =>
        firstPatient.patientName.localeCompare(secondPatient.patientName)
      );
  });

  filteredPatients = computed(() => {
    const search = this.searchText().trim().toLowerCase();

    return this.patients().filter(patient =>
      this.matchesSearch(patient, search)
    );
  });

  constructor(
    private readonly appointmentService: AppointmentService,
    private readonly authService: AuthService
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

  private buildPatientMap(): Map<number, DoctorPatientView> {
    const patientMap = new Map<number, DoctorPatientView>();

    const validAppointments = this.appointments()
      .filter(appointment => this.isValidAppointment(appointment));

    for (const appointment of validAppointments) {
      this.addAppointmentToPatientMap(
        patientMap,
        appointment
      );
    }

    return patientMap;
  }

  private addAppointmentToPatientMap(
    patientMap: Map<number, DoctorPatientView>,
    appointment: AppointmentDto
  ): void {
    const existingPatient = patientMap.get(appointment.patientId);

    if (!existingPatient) {
      patientMap.set(
        appointment.patientId,
        this.createPatientView(appointment)
      );

      return;
    }

    this.updatePatientView(
      existingPatient,
      appointment
    );
  }

  private createPatientView(
    appointment: AppointmentDto
  ): DoctorPatientView {
    return {
      patientId: appointment.patientId,
      patientName: appointment.patientName,
      completedVisits: this.isCompletedAppointment(appointment) ? 1 : 0,
      totalValidAppointments: 1,
      lastVisit: this.isCompletedAppointment(appointment)
        ? appointment.scheduledDate
        : null,
      nextAppointment: this.isUpcomingAppointment(appointment)
        ? appointment.scheduledDate
        : null
    };
  }

  private updatePatientView(
    patient: DoctorPatientView,
    appointment: AppointmentDto
  ): void {
    patient.totalValidAppointments++;

    this.updateCompletedVisitDetails(
      patient,
      appointment
    );

    this.updateNextAppointmentDetails(
      patient,
      appointment
    );
  }

  private updateCompletedVisitDetails(
    patient: DoctorPatientView,
    appointment: AppointmentDto
  ): void {
    if (!this.isCompletedAppointment(appointment)) {
      return;
    }

    patient.completedVisits++;

    if (this.isMoreRecentVisit(appointment, patient.lastVisit)) {
      patient.lastVisit = appointment.scheduledDate;
    }
  }

  private updateNextAppointmentDetails(
    patient: DoctorPatientView,
    appointment: AppointmentDto
  ): void {
    if (!this.isUpcomingAppointment(appointment)) {
      return;
    }

    if (this.isEarlierUpcomingAppointment(appointment, patient.nextAppointment)) {
      patient.nextAppointment = appointment.scheduledDate;
    }
  }

  private isValidAppointment(
    appointment: AppointmentDto
  ): boolean {
    return appointment.status !== 'Cancelled';
  }

  private isCompletedAppointment(
    appointment: AppointmentDto
  ): boolean {
    return appointment.status === 'Completed';
  }

  private isUpcomingAppointment(
    appointment: AppointmentDto
  ): boolean {
    const appointmentDate = this.getDateOnly(appointment.scheduledDate);

    return appointmentDate >= this.today &&
      (
        appointment.status === 'Pending' ||
        appointment.status === 'Confirmed'
      );
  }

  private isMoreRecentVisit(
    appointment: AppointmentDto,
    currentLastVisit: string | null
  ): boolean {
    if (!currentLastVisit) {
      return true;
    }

    return this.getDateOnly(appointment.scheduledDate) >
      this.getDateOnly(currentLastVisit);
  }

  private isEarlierUpcomingAppointment(
    appointment: AppointmentDto,
    currentNextAppointment: string | null
  ): boolean {
    if (!currentNextAppointment) {
      return true;
    }

    return this.getDateOnly(appointment.scheduledDate) <
      this.getDateOnly(currentNextAppointment);
  }

  private matchesSearch(
    patient: DoctorPatientView,
    search: string
  ): boolean {
    return !search ||
      patient.patientName.toLowerCase().includes(search);
  }

  private getDateOnly(dateValue: string): string {
    return dateValue.split('T')[0];
  }
}
