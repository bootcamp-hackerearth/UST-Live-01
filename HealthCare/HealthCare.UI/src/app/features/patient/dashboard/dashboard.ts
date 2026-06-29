import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent {

  patientName = 'John Doe';

  stats = [
    {
      title: 'Upcoming Appointments',
      value: 2,
      icon: 'bi-calendar-check',
      color: 'bg-blue-100 text-blue-600'
    },
    {
      title: 'Completed Visits',
      value: 12,
      icon: 'bi-check-circle',
      color: 'bg-green-100 text-green-600'
    },
    {
      title: 'Health Records',
      value: 8,
      icon: 'bi-file-medical',
      color: 'bg-purple-100 text-purple-600'
    },
    {
      title: 'Prescriptions',
      value: 5,
      icon: 'bi-prescription2',
      color: 'bg-yellow-100 text-yellow-600'
    }
  ];

  appointments = [

    {
      doctor: 'Dr. Sarah Johnson',
      speciality: 'Cardiologist',
      date: '20 May 2026',
      time: '10:00 AM',
      status: 'Confirmed'
    },

    {
      doctor: 'Dr. Emily Carter',
      speciality: 'Dermatologist',
      date: '25 May 2026',
      time: '02:30 PM',
      status: 'Pending'
    }

  ];

}