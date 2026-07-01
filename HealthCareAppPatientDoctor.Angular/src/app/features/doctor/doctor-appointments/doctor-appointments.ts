import { ChangeDetectorRef,Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

import { DoctorService } from '../../../core/services/doctor-service';
import { Appointment } from '../../../core/models/appointment';
import { AppointmentService } from '../../../core/services/appointment-service';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css'
})
export class DoctorAppointments implements OnInit {

  private doctorService = inject(DoctorService);
   private cdr = inject(ChangeDetectorRef);
   private appointmentService = inject(AppointmentService);
   private router = inject(Router);


  appointments: Appointment[] = [];

  isLoading = false;

  ngOnInit(): void {

    this.loadAppointments();

  }

  loadAppointments(): void {

    this.isLoading = true;

    this.doctorService
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

        }

      });

  }

  confirmAppointment(appointmentId: number): void {

  if (!confirm('Confirm this appointment?')) {

    return;

  }

  this.appointmentService
    .confirmAppointment(appointmentId)
    .subscribe({

      next: () => {

        alert('Appointment confirmed successfully.');

        this.loadAppointments();

      },

      error: (err) => {

        console.log(err);

        alert(err.error.message);

      }

    });

}

goToHealthRecord(appointment: Appointment): void {

  this.router.navigate([
    '/doctor/appointments',
    appointment.appointmentId,
    'health-record'
  ]);

}

viewHistory(appointmentId: number): void {

  this.router.navigate([
    '/doctor/appointments',
    appointmentId,
    'history'
  ]);

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

  getBadge(status: number): string {

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

}