import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PatientSidebarComponent } from '../../shared/patient-sidebar/patient-sidebar.component';
import { Router} from '@angular/router';



@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule, PatientSidebarComponent],
  templateUrl: './patient-appointments.component.html',
  styleUrls: ['./patient-appointments.component.css']
})
export class MyAppointmentsComponent implements OnInit {

  appointments: any[] = [];
  loading: boolean = true;

  constructor(private readonly http: HttpClient, private readonly router: Router, private readonly cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.getAppointments();
  }

  getAppointments() {

    this.loading = true; 
    this.appointments = []; 

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<any[]>(
      '/api/appointments/patient/upcoming',
      { headers }
    ).subscribe({
      next: (res) => {
        console.log("Appointments:", res);
        this.appointments = [...res];
        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  // ✅ Optional for display
  convertTime(time: string): string {
    const [h, m] = time.split(':').map(Number);
    const suffix = h >= 12 ? 'PM' : 'AM';
    const hour = h % 12 || 12;
    return `${hour}:${m.toString().padStart(2, '0')} ${suffix}`;
  }
}
