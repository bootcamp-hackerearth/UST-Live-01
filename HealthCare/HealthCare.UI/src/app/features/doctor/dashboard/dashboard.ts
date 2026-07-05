import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AppointmentService }from '../../../core/services/appointment.service';
import { DoctorService } from '../../../core/services/doctor.service';


@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})

export class DoctorDashboardComponent implements OnInit {

  doctorName = 'Doctor';
  leaves = signal<any[]>([]);
  todayAppointments = computed(() => this.todaySchedule().length);
  pendingAppointments = computed(() => this.todaySchedule().filter(x => x.status === 'Pending').length);
  completedAppointments = computed(() => this.todaySchedule().filter(x => x.status === 'Completed').length);


  todaySchedule = signal<any[]>([]);

  constructor(
    private readonly appointmentService: AppointmentService,
    private readonly doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.loadSchedule();
    this.loadLeaves()


  }

loadSchedule() {

  const today = new Date().toISOString().split('T')[0];

  this.appointmentService
    .getDoctorSchedule(today)
    .subscribe({

      next: (res: any[]) => {

        console.log('Schedule:', res);

        this.todaySchedule.set(res); 

      },
      error: (err) => console.error(err)

    });
}

loadLeaves() {

  this.doctorService.getMyLeaves().subscribe({

    next: (res: any[]) => {

      const today = new Date().toISOString().split('T')[0];

      //  only future leaves
      const upcoming = res.filter(l =>
        l.leaveDate >= today
      );

      this.leaves.set(upcoming);

    }

  });
}


}