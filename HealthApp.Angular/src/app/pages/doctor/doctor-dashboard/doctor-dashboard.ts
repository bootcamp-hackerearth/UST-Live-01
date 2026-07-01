import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MockAuth, MockUser } from '../../../services/mock-auth';
import {
  AppointmentDto,
  MockPatientData
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [RouterLink],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})
export class DoctorDashboard {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);

  currentUser?: MockUser = this.auth.getCurrentUser();
  today = new Date();

  get doctorId(): number {
    return this.currentUser?.doctorId ?? 0;
  }

  get todayAppointments(): AppointmentDto[] {
    if (!this.doctorId) {
      return [];
    }

    return this.patientData.getTodayAppointmentsByDoctor(this.doctorId);
  }

  get todayCount(): number {
    if (!this.doctorId) {
      return 0;
    }

    return this.patientData.getDoctorTodayCount(this.doctorId);
  }

  get pendingCount(): number {
    if (!this.doctorId) {
      return 0;
    }

    return this.patientData.getDoctorStatusCount(this.doctorId, 'Pending');
  }

  get confirmedCount(): number {
    if (!this.doctorId) {
      return 0;
    }

    return this.patientData.getDoctorStatusCount(this.doctorId, 'Confirmed');
  }

  get completedCount(): number {
    if (!this.doctorId) {
      return 0;
    }

    return this.patientData.getDoctorStatusCount(this.doctorId, 'Completed');
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  viewPatient(appointment: AppointmentDto): void {
    // Patient profile modal will be added in the Doctor Appointments phase.
    alert(`Patient profile modal will be added for ${appointment.patientName}.`);
  }
}