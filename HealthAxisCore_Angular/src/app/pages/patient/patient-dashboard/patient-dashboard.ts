import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { PatientService } from '../../../core/services/patient.service';

import { AppointmentDto } from '../../../core/models/appointment.model';
import { DoctorDto } from '../../../core/models/doctor.model';
import { HealthRecordDto } from '../../../core/models/health-record.model';

@Component({
  selector: 'app-patient-dashboard',
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard {
  patientName = computed(() =>
    this.authService.currentUser()?.fullName ?? 'Patient'
  );

  appointments = signal<AppointmentDto[]>([]);

  doctors = signal<DoctorDto[]>([]);

  healthRecords = signal<HealthRecordDto[]>([]);

  isLoading = signal(false);

  errorMessage = signal('');

  todayDate = new Date().toISOString().split('T')[0];

  upcomingAppointments = computed(() => {
    return this.appointments()
      .filter(appointment =>
        appointment.status !== 'Cancelled' &&
        appointment.status !== 'Completed' &&
        appointment.scheduledDate.split('T')[0] >= this.todayDate
      )
      .sort((first, second) => {
        const firstDateTime = `${first.scheduledDate} ${first.timeSlot}`;
        const secondDateTime = `${second.scheduledDate} ${second.timeSlot}`;

        return firstDateTime.localeCompare(secondDateTime);
      })
      .slice(0, 3);
  });

  upcomingAppointmentCount = computed(() =>
    this.appointments().filter(appointment =>
      appointment.status !== 'Cancelled' &&
      appointment.status !== 'Completed' &&
      appointment.scheduledDate.split('T')[0] >= this.todayDate
    ).length
  );

  activeDoctorCount = computed(() =>
    this.doctors().filter(doctor => doctor.isActive).length
  );

  healthRecordCount = computed(() =>
    this.healthRecords().length
  );

  dashboardCards = computed(() => [
    {
      title: 'Find Doctors',
      description: 'Search specialists and book appointments.',
      route: '/patient/doctors',
      value: this.activeDoctorCount().toString()
    },
    {
      title: 'My Appointments',
      description: 'View and manage your upcoming appointments.',
      route: '/patient/appointments',
      value: this.upcomingAppointmentCount().toString()
    },
    {
      title: 'Health History',
      description: 'Review past visits and health records.',
      route: '/patient/health-history',
      value: this.healthRecordCount().toString()
    }
  ]);

  constructor(
    private readonly authService: AuthService,
    private readonly appointmentService: AppointmentService,
    private readonly doctorService: DoctorService,
    private readonly patientService: PatientService
  ) {
    this.loadDashboardData();
  }

  loadDashboardData(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.loadAppointments();
    this.loadDoctors();
    this.loadHealthRecords();
  }

  private loadAppointments(): void {
    this.appointmentService.getAppointments().subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.stopLoadingIfReady();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  private loadDoctors(): void {
    this.doctorService.getDoctors().subscribe({
      next: doctors => {
        this.doctors.set(doctors);
        this.stopLoadingIfReady();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  private loadHealthRecords(): void {
    const patientId = this.authService.patientId();

    if (!patientId) {
      this.errorMessage.set('Patient ID missing. Please login again.');
      this.isLoading.set(false);
      return;
    }

    this.patientService.getHealthRecords(patientId).subscribe({
      next: records => {
        this.healthRecords.set(records);
        this.stopLoadingIfReady();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  private stopLoadingIfReady(): void {
    this.isLoading.set(false);
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }
}
