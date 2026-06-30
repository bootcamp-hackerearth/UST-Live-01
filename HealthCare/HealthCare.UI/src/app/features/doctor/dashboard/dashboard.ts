import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppointmentService }from '../../../core/services/appointment.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DoctorDashboardComponent implements OnInit {

  doctorName = 'Doctor';

  todayAppointments = 0;

  pendingAppointments = 0;

  completedAppointments = 0;

  todaySchedule: any[] = [];

  constructor(
    private appointmentService: AppointmentService,
  ) {}

  ngOnInit(): void {

    this.loadSchedule();

  }

  loadSchedule() {

    const today =
      new Date().toISOString().split('T')[0];

    this.appointmentService
      .getDoctorSchedule(today)
      .subscribe({

        next: (res: any[]) => {

          this.todaySchedule = res;

          this.todayAppointments = res.length;

          this.pendingAppointments =
            res.filter(x =>
              x.status === 'Pending').length;

          this.completedAppointments =
            res.filter(x =>
              x.status === 'Completed').length;
        },

        error: (err: any) => console.log(err)

      });
  }

}