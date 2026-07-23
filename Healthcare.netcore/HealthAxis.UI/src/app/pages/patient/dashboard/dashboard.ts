import { Component, OnInit, inject, NgZone, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly zone = inject(NgZone);
  private readonly cdr = inject(ChangeDetectorRef);

  patientName = 'Patient';

  totalDoctors = 0;
  appointmentCount = 0;
  healthRecordCount = 0;

  upcomingAppointmentText = 'No upcoming appointments';

  private readonly patientUrl = '/api/patients/me';
  private readonly doctorsUrl = '/api/doctors';
  private readonly appointmentsUrl = '/api/appointments';
  private readonly patientsUrl = '/api/patients';

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

    this.http.get<any>(this.patientUrl, { headers }).subscribe({
      next: (patient: any) => {
        console.log('Dashboard patient ✅:', patient);

        const patientId = Number(patient.patientId);

        localStorage.setItem('patientId', String(patientId));

        forkJoin({
          doctors: this.http.get<any>(this.doctorsUrl, { headers }),
          appointments: this.http.get<any[]>(this.appointmentsUrl, { headers }),
          records: this.http.get<any[]>(
            `${this.patientsUrl}/${patientId}/health-records`,
            { headers }
          )
        }).subscribe({
          next: (result: any) => {
            console.log('Dashboard data ✅:', result);

            const doctorsList =
              result.doctors?.items ||
              result.doctors?.data ||
              [];

            const allAppointments =
              result.appointments ||
              [];

            const records =
              result.records ||
              [];

            const patientAppointments = allAppointments.filter(
              (a: any) => Number(a.patientId) === Number(patientId)
            );

            const upcoming = patientAppointments.find((a: any) => {
              const appointmentDate = new Date(a.scheduledDate);
              const today = new Date();

              appointmentDate.setHours(0, 0, 0, 0);
              today.setHours(0, 0, 0, 0);

              return appointmentDate >= today;
            });

            this.zone.run(() => {
              this.patientName = patient.fullName || 'Patient';

              this.totalDoctors = doctorsList.length;
              this.appointmentCount = patientAppointments.length;
              this.healthRecordCount = records.length;

              this.upcomingAppointmentText = upcoming
                ? `${this.formatDate(upcoming.scheduledDate)} at ${upcoming.timeSlot}`
                : 'No upcoming appointments';

              this.cdr.detectChanges();
            });
          },
          error: (err: any) => {
            console.error('Dashboard summary load failed ❌:', err);
          }
        });
      },
      error: (err: any) => {
        console.error('Dashboard patient load failed ❌:', err);
      }
    });
  }

  private formatDate(value: string): string {
    if (!value) {
      return '';
    }

    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}