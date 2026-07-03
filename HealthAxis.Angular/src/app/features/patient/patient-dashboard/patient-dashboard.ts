import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { HealthRecord } from '../../../core/models/health-record.model';
import { PagedResult } from '../../../core/models/paged-result.model';

import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboard implements OnInit {

  isLoadingAppointments = false;
  isLoadingHealthRecords = false;

  appointmentErrorMessage = '';
  healthRecordErrorMessage = '';

  appointments: Appointment[] = [];
  upcomingAppointments: Appointment[] = [];

  healthRecords: HealthRecord[] = [];
  recentHealthRecords: HealthRecord[] = [];

  totalAppointments = 0;
  upcomingCount = 0;
  completedCount = 0;
  cancelledCount = 0;
  healthRecordCount = 0;

  constructor(
    private readonly patientService: PatientService
  ) { }

  ngOnInit(): void {
    this.loadAppointments();
    this.loadHealthRecords();
  }

  loadAppointments(): void {
    this.isLoadingAppointments = true;
    this.appointmentErrorMessage = '';

    this.patientService.getMyAppointments().subscribe({

      next: (result: Appointment[] | PagedResult<Appointment>) => {

        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        this.prepareAppointmentData();
      },

      error: (error: HttpErrorResponse) => {

        console.log('Dashboard appointments error:', error);

        this.appointmentErrorMessage =
          error.error?.message ??
          `Unable to load appointments. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoadingAppointments = false;
      }

    });
  }

  loadHealthRecords(): void {

    this.isLoadingHealthRecords = true;
    this.healthRecordErrorMessage = '';

    this.patientService.getMyHealthRecords().subscribe({

      next: (records: HealthRecord[]) => {

        this.healthRecords = records ?? [];

        this.recentHealthRecords = [...this.healthRecords]
          .sort(
            (a, b) =>
              new Date(b.visitDate).getTime() -
              new Date(a.visitDate).getTime()
          )
          .slice(0, 3);

        this.healthRecordCount = this.healthRecords.length;
      },

      error: (error: HttpErrorResponse) => {

        console.log('Dashboard health records error:', error);

        this.healthRecordErrorMessage =
          error.error?.message ??
          `Unable to load health records. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoadingHealthRecords = false;
      }

    });

  }

  private prepareAppointmentData(): void {

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    this.totalAppointments = this.appointments.length;

    this.completedCount = this.appointments.filter(
      appointment =>
        this.getStatusText(appointment.status).toLowerCase() === 'completed'
    ).length;

    this.cancelledCount = this.appointments.filter(
      appointment =>
        this.getStatusText(appointment.status).toLowerCase() === 'cancelled'
    ).length;

    this.upcomingAppointments = this.appointments
      .filter(appointment => {

        const status =
          this.getStatusText(appointment.status).toLowerCase();

        const appointmentDate =
          new Date(appointment.scheduledDate);

        appointmentDate.setHours(0, 0, 0, 0);

        return (
          appointmentDate.getTime() >= today.getTime() &&
          status !== 'completed' &&
          status !== 'cancelled'
        );
      })
      .sort(
        (a, b) =>
          new Date(a.scheduledDate).getTime() -
          new Date(b.scheduledDate).getTime()
      )
      .slice(0, 3);

    this.upcomingCount = this.upcomingAppointments.length;
  }

  getDoctorDisplayName(appointment: Appointment): string {

    return (
      appointment.doctorName ??
      `Doctor ID ${appointment.doctorId ?? '-'}`
    );

  }

  getStatusText(status: number | string): string {

    if (typeof status === 'string') {
      return status;
    }

    const statusMap: Record<number, string> = {
      0: 'Scheduled',
      1: 'Scheduled',
      2: 'Confirmed',
      3: 'Cancelled',
      4: 'Completed'
    };

    return statusMap[status] ?? `Status ${status}`;
  }

  getStatusClass(status: number | string): string {

    const normalized =
      this.getStatusText(status).toLowerCase();

    switch (normalized) {

      case 'completed':
        return 'status-completed';

      case 'confirmed':
        return 'status-confirmed';

      case 'cancelled':
        return 'status-cancelled';

      default:
        return 'status-scheduled';
    }
  }

  formatDate(value: string): string {

    if (!value) {
      return '-';
    }

    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });

  }

}