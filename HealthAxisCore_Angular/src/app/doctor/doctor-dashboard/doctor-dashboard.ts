import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DoctorSession } from '../../services/doctor-session';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [
    RouterLink
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard {
  doctorName = '';

  dashboardCards = [
    {
      title: 'Today’s Appointments',
      value: '6',
      description: 'View your consultation schedule.',
      route: '/doctor/schedule'
    },
    {
      title: 'Patients',
      value: '24',
      description: 'View patient profiles and history.',
      route: '/doctor/patients'
    },
    {
      title: 'Completed Today',
      value: '2',
      description: 'Consultations completed today.',
      route: '/doctor/schedule'
    }
  ];

  upcomingAppointments = [
    {
      appointmentId: 1,
      patientName: 'Arun Menon',
      timeSlot: '09:30 AM',
      status: 'Confirmed'
    },
    {
      appointmentId: 2,
      patientName: 'Meera Thomas',
      timeSlot: '10:30 AM',
      status: 'Pending'
    }
  ];

  constructor(private doctorSession: DoctorSession) {
    const doctor = this.doctorSession.getCurrentDoctor();

    this.doctorName = doctor.doctorName;
  }
}
