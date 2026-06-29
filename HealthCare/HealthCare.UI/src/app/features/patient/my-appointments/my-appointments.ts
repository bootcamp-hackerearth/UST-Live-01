import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppointmentService }
from '../../../core/services/appointment.service';

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css'
})
export class MyAppointmentsComponent
implements OnInit {

  appointments: any[] = [];

  loading = true;

  constructor(
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {

    this.appointmentService
      .getMyAppointments()
      .subscribe({

        next: (res: any[]) => {

          this.appointments = res;

          this.loading = false;
        },

        error: () => {

          this.loading = false;
        }

      });
  }

  getStatusClass(status: string) {

    switch (status) {

      case 'Pending':
        return 'bg-yellow-100 text-yellow-700';

      case 'Confirmed':
        return 'bg-blue-100 text-blue-700';

      case 'Completed':
        return 'bg-green-100 text-green-700';

      case 'Cancelled':
        return 'bg-red-100 text-red-700';

      default:
        return 'bg-gray-100 text-gray-700';
    }
  }

}