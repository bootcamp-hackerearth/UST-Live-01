import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';

@Component({
  selector: 'app-doctor-schedule',
  imports: [FormsModule, RouterLink],
  templateUrl: './doctor-schedule.html',
  styleUrls: ['./doctor-schedule.css']
})
export class DoctorSchedule implements OnInit {
  isLoading = false;
  errorMessage = '';

  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];

  searchText = '';
  selectedStatus = '';

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        this.applyFilters();
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to load schedule. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  applyFilters(): void {
    const search = this.searchText.trim().toLowerCase();

    this.filteredAppointments = this.appointments.filter((appointment) => {
      const patientName = appointment.patientName?.toLowerCase() ?? '';
      const appointmentId = appointment.appointmentId.toString();
      const statusText = this.getStatusText(appointment.status).toLowerCase();

      const matchesSearch =
        !search ||
        patientName.includes(search) ||
        appointmentId.includes(search);

      const matchesStatus =
        !this.selectedStatus ||
        statusText === this.selectedStatus;

      return matchesSearch && matchesStatus;
    });
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
    const text = this.getStatusText(status).toLowerCase();

    if (text === 'completed') return 'status-completed';
    if (text === 'confirmed') return 'status-confirmed';
    if (text === 'cancelled') return 'status-cancelled';

    return 'status-scheduled';
  }

  formatDate(value: string): string {
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
}