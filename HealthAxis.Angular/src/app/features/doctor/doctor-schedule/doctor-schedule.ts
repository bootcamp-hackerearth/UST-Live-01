import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';

import { Appointment } from '../../../core/models/appointment.model';
import { HealthRecord } from '../../../core/models/health-record.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { Pagination } from '../../../shared/pagination/pagination';

@Component({
  selector: 'app-doctor-schedule',
  imports: [FormsModule, RouterLink, Pagination],
  templateUrl: './doctor-schedule.html',
  styleUrls: ['./doctor-schedule.css']
})
export class DoctorSchedule implements OnInit {
  isLoading = false;
  isUpdating = false;

  errorMessage = '';
  successMessage = '';

  appointments: Appointment[] = [];
  filteredAppointments: Appointment[] = [];
  pagedAppointments: Appointment[] = [];

  healthRecordAppointmentIds = new Set<number>();

  searchText = '';
  selectedStatus = '';

  currentPage = 1;
  pageSize = 5;
  totalItems = 0;

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        this.appointments = this.appointments.sort(
          (a, b) =>
            new Date(a.scheduledDate).getTime() -
            new Date(b.scheduledDate).getTime()
        );

        this.loadExistingHealthRecordFlags().subscribe({
          next: () => {
            this.applyFilters();
          },

          error: () => {
            this.applyFilters();
          },

          complete: () => {
            this.isLoading = false;
          }
        });
      },

      error: (error: HttpErrorResponse) => {
        console.log('Doctor schedule error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load schedule. Status: ${error.status}`;

        this.isLoading = false;
      }
    });
  }

  private loadExistingHealthRecordFlags(): Observable<void> {
    this.healthRecordAppointmentIds.clear();

    const completedAppointments =
      this.appointments.filter(appointment =>
        this.canAddHealthRecordByStatusOnly(appointment.status) &&
        !!appointment.patientId
      );

    const uniquePatientIds =
      Array.from(
        new Set(
          completedAppointments.map(appointment =>
            appointment.patientId as number
          )
        )
      );

    if (uniquePatientIds.length === 0) {
      return of(void 0);
    }

    const requests =
      uniquePatientIds.map(patientId =>
        this.doctorService.getPatientHealthRecords(patientId).pipe(
          catchError(() => of([] as HealthRecord[]))
        )
      );

    return forkJoin(requests).pipe(
      map((recordGroups: HealthRecord[][]) => {
        recordGroups
          .flat()
          .forEach((record) => {
            if (record.appointmentId) {
              this.healthRecordAppointmentIds.add(record.appointmentId);
            }
          });

        this.appointments =
          this.appointments.map(appointment => ({
            ...appointment,
            hasHealthRecord:
              this.healthRecordAppointmentIds.has(appointment.appointmentId)
          }));
      })
    );
  }

  applyFilters(): void {
    const search =
      this.searchText.trim().toLowerCase();

    this.filteredAppointments =
      this.appointments.filter((appointment) => {
        const patientName =
          appointment.patientName?.toLowerCase() ?? '';

        const appointmentId =
          appointment.appointmentId.toString();

        const statusText =
          this.getStatusText(appointment.status).toLowerCase();

        const matchesSearch =
          !search ||
          patientName.includes(search) ||
          appointmentId.includes(search);

        const matchesStatus =
          !this.selectedStatus ||
          statusText === this.selectedStatus;

        return matchesSearch && matchesStatus;
      });

    this.currentPage = 1;
    this.totalItems = this.filteredAppointments.length;
    this.updatePagedAppointments();
  }

  onPageChanged(page: number): void {
    this.currentPage = page;
    this.updatePagedAppointments();
  }

  private updatePagedAppointments(): void {
    const startIndex =
      (this.currentPage - 1) * this.pageSize;

    const endIndex =
      startIndex + this.pageSize;

    this.pagedAppointments =
      this.filteredAppointments.slice(startIndex, endIndex);
  }

  confirmAppointment(appointmentId: number): void {
    this.isUpdating = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorService.confirmAppointment(appointmentId).subscribe({
      next: () => {
        this.successMessage =
          'Appointment confirmed successfully.';

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
        this.successMessage =
          'Appointment completed successfully.';

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

  canConfirm(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'scheduled';
  }

  canComplete(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'confirmed';
  }

  canAddHealthRecord(appointment: Appointment): boolean {
    return (
      this.canAddHealthRecordByStatusOnly(appointment.status) &&
      !appointment.hasHealthRecord
    );
  }

  hasHealthRecord(appointment: Appointment): boolean {
    return !!appointment.hasHealthRecord;
  }

  private canAddHealthRecordByStatusOnly(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'completed';
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
    const normalizedStatus =
      this.getStatusText(status).toLowerCase();

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
}
