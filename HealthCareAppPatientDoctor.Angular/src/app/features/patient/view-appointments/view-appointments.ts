import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { AppointmentService } from '../../../core/services/appointment-service';
import { Appointment } from '../../../core/models/appointment';
import { CancelAppointment } from '../../../core/models/cancel-appointment';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-view-appointments',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    FormsModule
  ],
  templateUrl: './view-appointments.html',
  styleUrl: './view-appointments.css'
})
export class ViewAppointments implements OnInit {

  private appointmentService = inject(AppointmentService);
  private cdr = inject(ChangeDetectorRef);

  appointments: Appointment[] = [];

  isLoading = false;

  selectedAppointmentId = 0;

  cancelReason = '';


  ngOnInit(): void {

    this.loadAppointments();


  }



  loadAppointments(): void {

    this.isLoading = true;

    this.appointmentService
      .getMyAppointments()
      .subscribe({

        next: (response) => {




          this.appointments = response;

          this.isLoading = false;
          this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;
          this.cdr.detectChanges();

        }

      });

  }

  getStatus(status: number): string {

    switch (status) {

      case 0:
        return 'Pending';

      case 1:
        return 'Confirmed';

      case 2:
        return 'Cancelled';

      case 3:
        return 'Completed';

      default:
        return 'Unknown';

    }

  }

  getStatusBadgeClass(status: number): string {

    switch (status) {

      case 0:
        return 'bg-warning text-dark';

      case 1:
        return 'bg-primary';

      case 2:
        return 'bg-danger';

      case 3:
        return 'bg-success';

      default:
        return 'bg-secondary';

    }

  }

  canCancel(status: number): boolean {

    return status === 0 || status === 1;

  }

  openCancelModal(appointment: Appointment): void {

  this.selectedAppointmentId = appointment.appointmentId;

  this.cancelReason = '';

}

confirmCancellation(): void {

  if (!this.cancelReason.trim()) {

    alert('Cancellation reason is required.');

    return;

  }

  const request = {

    appointmentId: this.selectedAppointmentId,

    reason: this.cancelReason.trim()

  };

  this.appointmentService
      .cancelAppointment(request)
      .subscribe({

        next: () => {

          alert('Appointment cancelled successfully.');

          this.loadAppointments();

          this.cancelReason = '';

        },

        error: err => {

          alert(err.error.message);

        }

      });

}

}