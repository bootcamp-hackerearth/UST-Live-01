import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth } from '../../../services/mock-auth';
import {
  AppointmentDto,
  MockPatientData,
  PatientDto
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-patient-appointments',
  imports: [RouterLink, FormsModule],
  templateUrl: './patient-appointments.html',
  styleUrl: './patient-appointments.css',
})
export class PatientAppointments implements OnInit {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);

  patient?: PatientDto;
  appointments: AppointmentDto[] = [];

  selectedAppointment?: AppointmentDto;
  cancellationReason = '';

  message = '';
  isError = false;

  ngOnInit(): void {
    if (!this.auth.isLoggedIn() || !this.auth.isPatient()) {
      this.router.navigate(['/login']);
      return;
    }

    this.patient = this.auth.getCurrentPatient();
    this.loadAppointments();
  }

  loadAppointments(): void {
    if (!this.patient) {
      this.appointments = [];
      return;
    }

    this.appointments = this.patientData.getAppointmentsByPatient(this.patient.patientId);
  }

  canCancel(appointment: AppointmentDto): boolean {
    return appointment.status === 'Pending' || appointment.status === 'Confirmed';
  }

  openCancelModal(appointment: AppointmentDto): void {
    this.message = '';
    this.isError = false;
    this.selectedAppointment = appointment;
    this.cancellationReason = '';
  }

  closeCancelModal(): void {
    this.selectedAppointment = undefined;
    this.cancellationReason = '';
  }

  confirmCancelAppointment(): void {
    if (!this.selectedAppointment) {
      return;
    }

    const result = this.patientData.cancelAppointment({
      appointmentId: this.selectedAppointment.appointmentId,
      reason: this.cancellationReason
    });

    if (!result.success) {
      this.message = result.message;
      this.isError = true;
      return;
    }

    this.message = result.message;
    this.isError = false;

    this.closeCancelModal();
    this.loadAppointments();
  }

  getDoctorSpecialisation(doctorId: number): string {
    return this.patientData.getDoctorSpecialisation(doctorId) ?? 'Specialisation unavailable';
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}