import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { AuthService } from '../../../core/services/auth.service';
import { DoctorPortalStateService } from '../../../core/services/doctor-portal-state.service';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [RouterLink],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})
export class DoctorDashboard implements OnInit {
  readonly doctorState = inject(DoctorPortalStateService);

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  today = new Date();

  ngOnInit(): void {
    this.doctorState.loadDoctorPortal();
  }

  get doctorDisplayName(): string {
    return (
      this.doctorState.doctorProfile()?.fullName ??
      this.authService.currentUser()?.email ??
      'Doctor'
    );
  }

  viewPatient(appointment: Appointment): void {
    this.router.navigate(['/doctor/appointments'], {
      queryParams: {
        patientId: appointment.patientId
      }
    });
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}