import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PatientService } from '../../../core/services/patient.service';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {

  patientName = '';
  showBookingModal = false;
  stats: any[] = [];
  loading = signal(false);
  appointments = signal<any[]>([]);
  totalAppointments = computed(() =>this.appointments().length);
  upcomingCount = computed(() => {
  const today = new Date();
  return this.appointments().filter(a => {
    const d = new Date(a.scheduledDate);

    return (
      d >= today &&
      a.status !== 'Cancelled'
    );
  }).length;

});

completedCount = computed(() =>this.appointments().filter(a => a.status === 'Completed').length);
cancelledCount = computed(() =>this.appointments().filter(a => a.status === 'Cancelled').length
);

  constructor(
    private patientService: PatientService,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit() {
    this.loadProfile();
    this.loadAppointments();
  }

  //  Load patient profile
  loadProfile() {
    this.patientService.getProfile().subscribe({
      next: (res: any) => {
        console.log('Profile:', res);
        this.patientName = res.fullName;
      },
      error: (err) => {
        console.error('Error loading profile', err);
      }
    });
  }

  //  Load appointments
 loadAppointments() {

  this.loading.set(true);

  this.appointmentService
    .getMyAppointments()
    .subscribe({

      next: (res: any[]) => {

        console.log('Dashboard Appointments', res);

        this.appointments.set(res);
        this.loading.set(false);

      },

      error: (err) => {

        console.error('Error', err);

        this.loading.set(false);
      }

    });
}

//Upcoming Appointments
upcomingAppointments = computed(() => {

  const today = new Date();

  return this.appointments()
    .filter(a => {

      const apptDate = new Date(a.scheduledDate);

      return (
        apptDate >= today &&
        a.status !== 'Completed' &&
        a.status !== 'Cancelled'
      );

    })
    .sort((a, b) =>
      new Date(a.scheduledDate).getTime() -
      new Date(b.scheduledDate).getTime()
    )
    .slice(0, 5);

});

}
