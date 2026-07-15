import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { Appointment } from '../../../core/models/appointment.model';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { AuthService } from '../../../core/services/auth.service';
import { Pagination } from '../../../shared/pagination/pagination';

@Component({
  selector: 'app-patient-appointments',
  imports: [RouterLink, FormsModule, Pagination],
  templateUrl: './patient-appointments.html',
  styleUrl: './patient-appointments.css',
})
export class PatientAppointments implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly authService = inject(AuthService);
  private readonly appointmentApi = inject(AppointmentApiService);
  private readonly router = inject(Router);

  appointments: Appointment[] = [];
  selectedAppointment?: Appointment;
  cancellationReason = '';

  message = '';
  isError = false;

  currentPage = 1;
  pageSize = 4;

  ngOnInit(): void {
    if (this.authService.currentRole() !== 'Patient') {
      this.router.navigate(['/login']);
      return;
    }

    this.loadAppointments();
  }

  loadAppointments(): void {
    this.appointmentApi.getAppointments().subscribe({
      next: appointments => {
        this.appointments = appointments;
        this.currentPage = 1;
        this.cdr.detectChanges();
      },
      error: error => {
        this.showError(this.authService.getErrorMessage(error));
        this.cdr.detectChanges();
      }
    });
  }
  get pagedAppointments(): Appointment[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.appointments.slice(startIndex, startIndex + this.pageSize);
  }

  onPageChange(page: number): void {
    this.currentPage = page;
  }

  canCancel(appointment: Appointment): boolean {
    return appointment.status === 'Pending' || appointment.status === 'Confirmed';
  }

  openCancelModal(appointment: Appointment): void {
    this.selectedAppointment = appointment;
    this.cancellationReason = '';
    this.clearMessage();
  }

  closeCancelModal(): void {
    this.selectedAppointment = undefined;
    this.cancellationReason = '';
  }

  confirmCancelAppointment(): void {
    if (!this.selectedAppointment) {
      return;
    }

    this.appointmentApi.updateAppointmentStatus(this.selectedAppointment.appointmentId, {
      status: 'Cancelled',
      cancellationReason: this.cancellationReason
    }).subscribe({
    next: () => {
      this.showSuccess('Appointment cancelled successfully.');
      this.closeCancelModal();
      this.loadAppointments();
      this.cdr.detectChanges();
    },
    error: error => {
      this.showError(this.authService.getErrorMessage(error));
      this.cdr.detectChanges();
    }    
  });
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getDoctorSpecialisation(): string {
    return '';
  }

  private showSuccess(message: string): void {
    this.message = message;
    this.isError = false;
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }

  private clearMessage(): void {
    this.message = '';
    this.isError = false;
  }
}