import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-my-appointments',
  standalone: false,
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css'
})
export class MyAppointments implements OnInit {

  appointments = signal<any[]>([]);
  message = signal('');

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      'https://localhost:7038/api/appointments/my',
      { headers }
    ).subscribe({
      next: (res) => {
        this.appointments.set(res);
      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load appointments ❌'
        );

      }
    });
  }
}
