import { Component, computed, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { AppointmentDto } from '../../../core/models/appointment.model';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [
    RouterLink
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard {
  doctorName = computed(() =>
    this.authService.currentUser()?.fullName ?? 'Doctor'
  );

  appointments = signal<AppointmentDto[]>([]);

  isLoading = signal(false);

  errorMessage = signal('');

  today = new Date().toISOString().split('T')[0];

  constructor(
    private authService: AuthService,
    private appointmentService: AppointmentService
  ) {
    this.loadTodayAppointments();
  }

  totalToday = computed(() => this.appointments().length);

  pendingCount = computed(() =>
    this.appointments().filter(a => a.status === 'Pending').length
  );

  confirmedCount = computed(() =>
    this.appointments().filter(a => a.status === 'Confirmed').length
  );

  completedCount = computed(() =>
    this.appointments().filter(a => a.status === 'Completed').length
  );

  loadTodayAppointments(): void {
    this.isLoading.set(true);

    this.appointmentService.getAppointments({
      date: this.today
    }).subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }
}
