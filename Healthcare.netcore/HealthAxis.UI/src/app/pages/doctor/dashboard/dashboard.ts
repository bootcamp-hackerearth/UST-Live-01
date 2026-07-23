import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  doctorName = 'Doctor';

  todayAppointments = 0;
  pendingAppointments = 0;
  completedAppointments = 0;

  private readonly doctorsUrl = '/api/doctors';
  private readonly appointmentsUrl = '/api/appointments';

  ngOnInit() {
    if (globalThis.window !== undefined) {
      this.loadDashboard();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadDashboard() {
    const headers = this.getHeaders();

    forkJoin({
      doctor: this.http.get<any>(`${this.doctorsUrl}/me`, { headers }),
      appointments: this.http.get<any[]>(this.appointmentsUrl, { headers })
    }).subscribe({
      next: (result: any) => {
        console.log('Doctor dashboard data ✅:', result);

        const doctor = result.doctor;
        const doctorId = Number(doctor.doctorId);

        localStorage.setItem('doctorId', String(doctorId));

        const allAppointments = result.appointments || [];

        const doctorAppointments = allAppointments.filter(
          (a: any) => Number(a.doctorId) === doctorId
        );

        const today = new Date();
        today.setHours(0, 0, 0, 0);

        this.doctorName = doctor.fullName || 'Doctor';

        this.todayAppointments = doctorAppointments.filter((a: any) => {
          const appointmentDate = new Date(a.scheduledDate);
          appointmentDate.setHours(0, 0, 0, 0);

          return appointmentDate.getTime() === today.getTime();
        }).length;

        this.pendingAppointments = doctorAppointments.filter(
          (a: any) => Number(a.status) === 0
        ).length;

        this.completedAppointments = doctorAppointments.filter(
          (a: any) => Number(a.status) === 2
        ).length;

        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Doctor dashboard load failed ❌:', err);
      }
    });
  }
}