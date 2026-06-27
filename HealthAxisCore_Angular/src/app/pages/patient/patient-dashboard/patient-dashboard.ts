import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { PatientSession } from '../../../services/patient-session';

@Component({
  selector: 'app-patient-dashboard',
  imports: [
    RouterLink
  ],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard {
  patientName = '';

  dashboardCards = [
    {
      title: 'Find Doctors',
      description: 'Search specialists and book appointments.',
      route: '/patient/doctors',
      value: '10+'
    },
    {
      title: 'My Appointments',
      description: 'View and manage your booked appointments.',
      route: '/patient/appointments',
      value: '3'
    },
    {
      title: 'Health History',
      description: 'Review past visits and health records.',
      route: '/patient/health-history',
      value: '8'
    }
  ];

  upcomingAppointments = [
    {
      doctorName: 'Dr. Isha Nair',
      specialisation: 'Dermatologist',
      date: 'Today',
      time: '04:30 PM',
      status: 'Confirmed'
    },
    {
      doctorName: 'Dr. Aravind Menon',
      specialisation: 'Cardiologist',
      date: 'Tomorrow',
      time: '10:00 AM',
      status: 'Pending'
    }
  ];

  constructor(private patientSession: PatientSession) {
    const patient = this.patientSession.getCurrentPatient();

    this.patientName = patient.fullName;
  }
}
