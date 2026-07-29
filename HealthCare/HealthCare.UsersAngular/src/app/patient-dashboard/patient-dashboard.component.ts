import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { PatientSidebarComponent }
  from '../shared/patient-sidebar/patient-sidebar.component';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    PatientSidebarComponent
  ],
  templateUrl: './patient-dashboard.component.html',
  styleUrls: ['./patient-dashboard.component.css']
})
export class PatientDashboardComponent implements OnInit {

  dashboard: any = {
    upcomingAppointmentsCount: 0,
    latestRecordsCount: 0
  };

  constructor(private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.getDashboardData();
  }

  getDashboardData() {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get(
      '/api/patients/dashboard',
      { headers }
    ).subscribe({
      next: (res: any) => {

        console.log('Dashboard Response:', res);

        this.dashboard = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}
