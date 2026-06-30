import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { HealthRecordCreateRequest } from '../../../core/models/health-record-create-request';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-add-health-record',
  imports: [FormsModule, DatePipe],
  templateUrl: './add-health-record.html',
  styleUrls: ['./add-health-record.css']
})
export class AddHealthRecord implements OnInit {
  isLoading = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  appointmentIdFromRoute: number | null = null;
  selectedAppointmentId = '';

  appointments: Appointment[] = [];
  selectedAppointment: Appointment | null = null;

  form = {
    visitDate: new Date().toISOString().split('T')[0],
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private doctorService: DoctorService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    const appointmentId =
      this.route.snapshot.paramMap.get('appointmentId');

    this.appointmentIdFromRoute =
      appointmentId ? Number(appointmentId) : null;

    this.loadAppointments();
  }

  loadAppointments(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        if (this.appointmentIdFromRoute) {
          this.selectedAppointmentId =
            String(this.appointmentIdFromRoute);

          this.onAppointmentChanged();
        }
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to load appointments. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onAppointmentChanged(): void {
    const id = Number(this.selectedAppointmentId);

    this.selectedAppointment =
      this.appointments.find(
        appointment => appointment.appointmentId === id
      ) ?? null;
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedAppointment) {
      this.errorMessage = 'Please select an appointment.';
      return;
    }

    if (!this.form.diagnosis.trim()) {
      this.errorMessage = 'Diagnosis is required.';
      return;
    }

    if (!this.form.prescription.trim()) {
      this.errorMessage = 'Prescription is required.';
      return;
    }

    const doctorReferenceId = this.tokenService.getReferenceId();

    if (!doctorReferenceId) {
      this.errorMessage = 'Doctor details were not found. Please login again.';
      return;
    }

    const request: HealthRecordCreateRequest = {
      appointmentId: this.selectedAppointment.appointmentId,
      patientId: this.selectedAppointment.patientId ?? 0,
      doctorId: Number(doctorReferenceId),
      visitDate: this.form.visitDate,
      diagnosis: this.form.diagnosis.trim(),
      prescription: this.form.prescription.trim(),
      notes: this.form.notes.trim()
    };

    this.isSaving = true;

    this.doctorService.addHealthRecord(request).subscribe({
      next: () => {
        this.successMessage = 'Health record added successfully.';

        setTimeout(() => {
          this.router.navigate(['/doctor/schedule']);
        }, 1000);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to add health record. Status: ${error.status}`;
      },

      complete: () => {
        this.isSaving = false;
      }
    });
  }

  getPatientDisplayName(appointment: Appointment): string {
    return appointment.patientName ??
      `Patient ID ${appointment.patientId ?? '-'}`;
  }
}