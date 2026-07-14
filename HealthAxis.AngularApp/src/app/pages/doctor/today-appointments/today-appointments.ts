import { Component, signal, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-today-appointments',
  standalone: false,
  templateUrl: './today-appointments.html',
  styleUrl: './today-appointments.css'
})
export class TodayAppointments implements OnInit {

  appointments = signal<any[]>([]);
  message = signal('');

  selectedAppointmentId = signal<number | null>(null);
  cancellationReason = signal('');

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      'https://localhost:7038/api/appointments/doctor/today',
      { headers }
    ).subscribe({
      next: (res) => {
        this.appointments.set(res);
      },

      error: (err) => {
        console.error(err);
        this.message.set('Failed to load appointments ❌');
      }
    });
  }

  updateStatus(id: number, status: string) {

    if (status === 'Cancelled') {
      this.selectedAppointmentId.set(id);
      return;
    }

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    const payload = {
      status,
      cancellationReason: null
    };

    this.http.put(
      `https://localhost:7038/api/appointments/${id}`,
      payload,
      { headers }
    ).subscribe({
      next: () => {

        // Completed → Open Health Record page
        if (status === 'Completed') {

          this.router.navigate(
            ['/doctor/create-health-record'],
            {
              state: {
                appointmentId: id
              }
            }
          );

          return;
        }

        // Confirmed
        this.message.set('Appointment confirmed ✅');

        this.loadAppointments();
      },

      error: (err) => {

        const msg =
          err?.error?.message ||
          'Update failed ❌';

        this.message.set(msg);
      }
    });
  }

  confirmCancellation() {

    if (!this.cancellationReason().trim()) {

      this.message.set(
        'Please enter a cancellation reason ❌'
      );

      return;
    }

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    const payload = {
      status: 'Cancelled',
      cancellationReason: this.cancellationReason()
    };

    this.http.put(
      `https://localhost:7038/api/appointments/${this.selectedAppointmentId()}`,
      payload,
      { headers }
    ).subscribe({
      next: () => {

        this.message.set(
          'Appointment cancelled ✅'
        );

        this.selectedAppointmentId.set(null);
        this.cancellationReason.set('');

        this.loadAppointments();
      },

      error: (err) => {

        const msg =
          err?.error?.message ||
          'Update failed ❌';

        this.message.set(msg);
      }
    });
  }

  addHealthRecord(appt: any) {

    this.router.navigate(
      ['/doctor/create-health-record'],
      {
        state: {
          appointmentId: appt.appointmentId
        }
      }
    );
  }
}
