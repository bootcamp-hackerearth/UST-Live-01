import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PatientService } from '../../../core/services/patient.service';

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
export class DashboardComponent implements OnInit {

  patientName = '';
  showBookingModal = false;

  stats: any[] = [];
  appointments: any[] = [];

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit() {
    this.loadProfile();
    this.loadAppointments();
    this.loadStats();
  }

  // ✅ Load patient profile
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

  // ✅ Load appointments
  loadAppointments() {
    this.patientService.getAppointments().subscribe({
      next: (res: any[]) => {
        console.log('Appointments:', res);
        this.appointments = res;
      },
      error: (err) => {
        console.error('Error loading appointments', err);
      }
    });
  }

  // ✅ Load stats (backend required)
  loadStats() {
    this.patientService.getDashboardStats().subscribe({
      next: (res: any) => {

        this.stats = [
          {
            title: 'Upcoming Appointments',
            value: res.upcoming,
            icon: 'bi-calendar-check',
            color: 'bg-blue-100 text-blue-600'
          },
          {
            title: 'Completed Visits',
            value: res.completed,
            icon: 'bi-check-circle',
            color: 'bg-green-100 text-green-600'
          },
          {
            title: 'Health Records',
            value: res.records,
            icon: 'bi-file-medical',
            color: 'bg-purple-100 text-purple-600'
          },
          {
            title: 'Prescriptions',
            value: res.prescriptions,
            icon: 'bi-prescription2',
            color: 'bg-yellow-100 text-yellow-600'
          }
        ];
      },
      error: (err) => {
        console.error('Error loading stats', err);
      }
    });
  }

}
