import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { PatientApiService } from '../../../core/services/patient-api.service';
import { Appointment } from '../../../core/models/appointment.model';
import { Patient } from '../../../core/models/patient.model';

@Component({
  selector: 'app-patient-dashboard',
  imports: [RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css',
})
export class PatientDashboard implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly authService = inject(AuthService);
  private readonly appointmentApi = inject(AppointmentApiService);
  private readonly patientApi = inject(PatientApiService);
  private readonly router = inject(Router);

  patient?: Patient;
  appointments: Appointment[] = [];
  today = new Date();

  ngOnInit(): void {
    const user = this.authService.currentUser();

    if (!user?.patientId) {
      this.router.navigate(['/login']);
      return;
    }

    this.patientApi.getPatientById(user.patientId).subscribe({
      next: patient => {
        this.patient = patient;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error(error);
      }
    });

    this.appointmentApi.getAppointments().subscribe({
      next: appointments => {
        this.appointments = appointments;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error(error);
      }
    });
  }

  get pendingCount(): number {
    return this.appointments.filter(item => item.status === 'Pending').length;
  }

  get confirmedCount(): number {
    return this.appointments.filter(item => item.status === 'Confirmed').length;
  }

  get completedCount(): number {
    return this.appointments.filter(item => item.status === 'Completed').length;
  }

  get cancelledCount(): number {
    return this.appointments.filter(item => item.status === 'Cancelled').length;
  }

  get upcomingAppointments(): Appointment[] {
    const today = this.startOfDay(new Date());
    const limit = new Date(today);
    limit.setDate(today.getDate() + 30);

    return this.appointments
      .filter(item => {
        const date = this.startOfDay(new Date(item.scheduledDate));

        return (
          date >= today &&
          date <= limit &&
          item.status !== 'Completed' &&
          item.status !== 'Cancelled'
        );
      })
      .sort((a, b) =>
        new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime()
      );
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getDoctorSpecialisation(): string {
    return '';
  }

  private startOfDay(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }
}