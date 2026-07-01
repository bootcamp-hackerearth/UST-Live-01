import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MockAuth } from '../../../services/mock-auth';
import {
  AppointmentDto,
  MockPatientData,
  PatientDto
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-patient-dashboard',
  imports: [RouterLink],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css',
})
export class PatientDashboard {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);

  patient?: PatientDto = this.auth.getCurrentPatient();

  today = new Date();

  get pendingCount(): number {
    return this.getPatientId()
      ? this.patientData.getStatusCount(this.getPatientId(), 'Pending')
      : 0;
  }

  get confirmedCount(): number {
    return this.getPatientId()
      ? this.patientData.getStatusCount(this.getPatientId(), 'Confirmed')
      : 0;
  }

  get completedCount(): number {
    return this.getPatientId()
      ? this.patientData.getStatusCount(this.getPatientId(), 'Completed')
      : 0;
  }

  get cancelledCount(): number {
    return this.getPatientId()
      ? this.patientData.getStatusCount(this.getPatientId(), 'Cancelled')
      : 0;
  }

  get upcomingAppointments(): AppointmentDto[] {
    if (!this.getPatientId()) {
      return [];
    }

    return this.patientData.getUpcomingAppointmentsWithinDays(this.getPatientId(), 30);
  }
  

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getDoctorSpecialisation(doctorId: number): string {
  return this.patientData.getDoctorSpecialisation(doctorId) ?? 'Specialisation unavailable';
}

  private getPatientId(): number {
    if (!this.patient) {
      this.router.navigate(['/login']);
      return 0;
    }

    return this.patient.patientId;
  }
}