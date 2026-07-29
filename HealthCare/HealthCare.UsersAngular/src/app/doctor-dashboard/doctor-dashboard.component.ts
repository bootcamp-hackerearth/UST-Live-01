import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DoctorSidebarComponent } from '../shared/doctor-sidebar/doctor-sidebar';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    DoctorSidebarComponent
  ],
  templateUrl: './doctor-dashboard.component.html',
  styleUrls: ['./doctor-dashboard.component.css']
})
export class DoctorDashboardComponent implements OnInit {

  upcomingAppointments = 0;
  patientsTreated = 0;
  upcomingLeaves = 0;

  todaySlots: string[] = [];

  constructor(private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<any>(
      '/api/doctors/dashboard',
      { headers }
    )
      .subscribe({
        next: (res) => {

          console.log('Doctor Dashboard:', res);

          this.upcomingAppointments = res.upcomingAppointments;
          this.patientsTreated = res.patientsTreated;
          this.upcomingLeaves = res.upcomingLeaves;
          this.todaySlots = res.todaysSchedule ?? [];
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error(err);
        }
      });
  }
}
