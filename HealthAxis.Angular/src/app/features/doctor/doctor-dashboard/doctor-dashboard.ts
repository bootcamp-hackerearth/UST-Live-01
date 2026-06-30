import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [RouterLink],
  templateUrl: './doctor-dashboard.html',
  styleUrls: ['./doctor-dashboard.css']
})
export class DoctorDashboard implements OnInit {
  isLoading = false;
  isUpdating = false;

  errorMessage = '';
  successMessage = '';

  displayName = 'Doctor';
  userEmail = '';

  appointments: Appointment[] = [];
  todaysAppointments: Appointment[] = [];

  totalAppointments = 0;
  scheduledCount = 0;
  confirmedCount = 0;
  completedCount = 0;

  constructor(
    private doctorService: DoctorService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const email = this.tokenService.getUserEmail();

    if (email) {
      this.userEmail = email;
      this.displayName = this.getFirstNameFromEmail(email);
    }

    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        if (Array.isArray(result)) {
          this.appointments = result;
        } else {
          this.appointments = result.items ?? [];
        }

        this.prepareDashboardData();
      },

      error: (error: HttpErrorResponse) => {
        console.log('Doctor appointments error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load appointments. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  completeAppointment(appointmentId: number): void {
    this.isUpdating = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorService.completeAppointment(appointmentId).subscribe({
      next: () => {
        this.successMessage = 'Appointment completed successfully.';
        this.loadAppointments();
      },

      error: (error: HttpErrorResponse) => {
        console.log('Complete appointment error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to complete appointment. Status: ${error.status}`;
      },

      complete: () => {
        this.isUpdating = false;
      }
    });
  }

  logout(): void {
    this.tokenService.clearAuthData();
    this.router.navigate(['/login']);
  }

  canComplete(status: number | string): boolean {
    const normalizedStatus = this.getStatusText(status).toLowerCase();

    return (
      normalizedStatus === 'scheduled' ||
      normalizedStatus === 'confirmed'
    );
  }

  getStatusText(status: number | string): string {
    if (typeof status === 'string') {
      return status;
    }

    const statusMap: Record<number, string> = {
      1: 'Scheduled',
      2: 'Confirmed',
      3: 'Cancelled',
      4: 'Completed'
    };

    return statusMap[status] ?? `Status ${status}`;
  }

  getStatusClass(status: number | string): string {
    const normalizedStatus = this.getStatusText(status).toLowerCase();

    if (normalizedStatus === 'completed') {
      return 'status-completed';
    }

    if (normalizedStatus === 'confirmed') {
      return 'status-confirmed';
    }

    if (normalizedStatus === 'cancelled') {
      return 'status-cancelled';
    }

    return 'status-scheduled';
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

  getPatientDisplayName(appointment: Appointment): string {
    return appointment.patientName ??
      `Patient ID ${appointment.patientId ?? '-'}`;
  }

  private prepareDashboardData(): void {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    this.appointments = this.appointments.sort(
      (a, b) =>
        new Date(a.scheduledDate).getTime() -
        new Date(b.scheduledDate).getTime()
    );

    this.todaysAppointments = this.appointments.filter((appointment) => {
      const appointmentDate = new Date(appointment.scheduledDate);
      appointmentDate.setHours(0, 0, 0, 0);

      return appointmentDate.getTime() === today.getTime();
    });

    this.totalAppointments = this.appointments.length;

    this.scheduledCount = this.appointments.filter(
      appointment =>
        this.getStatusText(appointment.status).toLowerCase() === 'scheduled'
    ).length;

    this.confirmedCount = this.appointments.filter(
      appointment =>
        this.getStatusText(appointment.status).toLowerCase() === 'confirmed'
    ).length;

    this.completedCount = this.appointments.filter(
      appointment =>
        this.getStatusText(appointment.status).toLowerCase() === 'completed'
    ).length;
  }

  private getFirstNameFromEmail(email: string): string {
    const namePart = email.split('@')[0];

    const firstName =
      namePart
        .split(/[._-]/)
        .filter(Boolean)[0];

    return firstName || 'Doctor';
  }
}