import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AppointmentService }
from '../../../core/services/appointment.service';

import { DoctorAppointment }
from '../../../core/models/doctor.appointment.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-schedule',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './schedule.html',
  styleUrl: './schedule.css'
})
export class ScheduleComponent implements OnInit {

  appointments = signal<DoctorAppointment[]>([]);
  loading = signal(false);

  selectedDate = new Date().toISOString().split('T')[0];

  constructor(private appointmentService: AppointmentService,private toastr:ToastrService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {

    this.loading.set(true);

    this.appointmentService
      .getDoctorSchedule(this.selectedDate)
      .subscribe({

        next: (res) => {

          console.log('Schedule data:', res);

          this.appointments.set(res); 

          this.loading.set(false);
        },

        error: (err) => {
          console.error(err);
          this.loading.set(false);
        }

      });
  }


  updateStatus(
    appointmentId: number,
    status: string
  ): void {

    let reason = '';

    if (status === 'Cancelled') {

      reason =
        prompt('Enter cancellation reason') || '';
    }

    this.appointmentService
      .updateAppointmentStatus(
        appointmentId,
        status,
        reason
      )
      .subscribe({

        next: () => {
       this.toastr.success(
       'Status updated successfully',
       'Success');
       this.loadAppointments();
        },

        error: (err) => {

          console.log(err);
          this.toastr.error('Unable to update status','Error');
        }

      });
  }

  badgeClass(status: string): string {

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
        return 'bg-slate-100 text-slate-700';
    }
  }
}