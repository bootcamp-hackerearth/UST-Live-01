import { Component, signal, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { Router } from '@angular/router';

@Component({
  selector: 'app-doctor-appointments',
  standalone: false,
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css'
})
export class DoctorAppointments implements OnInit {

  appointments = signal<any[]>([]);
  message = signal('');


  selectedAppointmentId = signal<number | null>(null);
  cancellationReason = signal('');

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit() {
    this.loadAppointments();
  }

  loadAppointments() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      'https://localhost:7038/api/appointments/doctor/my',
      { headers }
    ).subscribe({
      next: (res) => {
        this.appointments.set(res);
      },
      error: () => {
        console.error("Failed to load");
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


        this.message.set(
          status === 'Completed'
            ? 'Appointment completed ✅'
            : 'Appointment confirmed ✅'
        );

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

        this.message.set('Appointment cancelled ✅');

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
          appointment: appt
        }
      }
    );

  }

}
