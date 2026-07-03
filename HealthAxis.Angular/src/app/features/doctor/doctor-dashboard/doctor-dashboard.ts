import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
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

  confirmAppointment(appointmentId: number): void {

    this.isUpdating = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorService.confirmAppointment(appointmentId).subscribe({

      next: () => {
        this.successMessage = 'Appointment confirmed successfully.';
        this.loadAppointments();
      },

      error: (error: HttpErrorResponse) => {

        console.log('Confirm appointment error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to confirm appointment. Status: ${error.status}`;
      },

      complete: () => {
        this.isUpdating = false;
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

  canConfirm(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'scheduled';
  }

  canComplete(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'confirmed';
  }

  canAddHealthRecord(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'completed';
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

    const value = this.getStatusText(status).toLowerCase();

    switch (value) {

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

  getPatientDisplayName(appointment: Appointment): string {
    return appointment.patientName ??
      `Patient ID ${appointment.patientId ?? '-'}`;
  }

  private prepareDashboardData(): void {

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    this.appointments.sort(
      (a, b) =>
        new Date(a.scheduledDate).getTime() -
        new Date(b.scheduledDate).getTime()
    );

    this.todaysAppointments = this.appointments.filter(appointment => {

      const appointmentDate = new Date(appointment.scheduledDate);
      appointmentDate.setHours(0, 0, 0, 0);

      return appointmentDate.getTime() === today.getTime();

    });

    this.totalAppointments = this.appointments.length;

    this.scheduledCount = this.appointments.filter(
      a => this.getStatusText(a.status).toLowerCase() === 'scheduled'
    ).length;

    this.confirmedCount = this.appointments.filter(
      a => this.getStatusText(a.status).toLowerCase() === 'confirmed'
    ).length;

    this.completedCount = this.appointments.filter(
      a => this.getStatusText(a.status).toLowerCase() === 'completed'
    ).length;
  }

  private getFirstNameFromEmail(email: string): string {

    const firstName = email
      .split('@')[0]
      .split(/[._-]/)
      .filter(Boolean)[0];

    return firstName || 'Doctor';
  }
}