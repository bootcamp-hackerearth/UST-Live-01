import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { HealthRecordCreateRequest } from '../../../core/models/health-record-create-request';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-add-health-record',
  imports: [FormsModule, RouterLink],
  templateUrl: './add-health-record.html',
  styleUrls: ['./add-health-record.css']
})
export class AddHealthRecord implements OnInit {
  isLoading = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  appointmentId = 0;
  selectedAppointment: Appointment | null = null;

  form = {
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
    const appointmentIdFromRoute =
      this.route.snapshot.paramMap.get('appointmentId');

    this.appointmentId =
      Number(appointmentIdFromRoute);

    if (!this.appointmentId) {
      this.errorMessage =
        'Please open Add Health Record from a completed appointment in Schedule.';
      return;
    }

    this.loadAppointment();
  }

  loadAppointment(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        const appointments =
          Array.isArray(result)
            ? result
            : result.items ?? [];

        this.selectedAppointment =
          appointments.find(
            appointment =>
              appointment.appointmentId === this.appointmentId
          ) ?? null;

        if (!this.selectedAppointment) {
          this.errorMessage =
            'Appointment was not found for the logged-in doctor.';
          return;
        }

        if (!this.canAddHealthRecord(this.selectedAppointment.status)) {
          this.errorMessage =
            'Health record can be added only after the appointment is completed.';
        }
      },

      error: (error: HttpErrorResponse) => {
        console.log('Load appointment error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load appointment. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  submit(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedAppointment) {
      this.errorMessage =
        'Appointment details are not available.';
      return;
    }

    if (!this.canAddHealthRecord(this.selectedAppointment.status)) {
      this.errorMessage =
        'Health record can be added only after the appointment is completed.';
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

    const doctorReferenceId =
      this.tokenService.getReferenceId();

    if (!doctorReferenceId) {
      this.errorMessage =
        'Doctor details were not found. Please login again.';
      return;
    }

    if (!this.selectedAppointment.patientId) {
      this.errorMessage =
        'Patient details were not found for this appointment.';
      return;
    }

    const request: HealthRecordCreateRequest = {
      appointmentId: this.selectedAppointment.appointmentId,
      patientId: this.selectedAppointment.patientId,
      doctorId: Number(doctorReferenceId),
      visitDate: this.getDateOnly(this.selectedAppointment.scheduledDate),
      diagnosis: this.form.diagnosis.trim(),
      prescription: this.form.prescription.trim(),
      notes: this.form.notes.trim()
    };

    this.isSaving = true;

    this.doctorService.addHealthRecord(request).subscribe({
      next: () => {
        this.successMessage =
          'Health record added successfully.';

        setTimeout(() => {
          this.router.navigate(['/doctor/schedule']);
        }, 1000);
      },

      error: (error: HttpErrorResponse) => {
        console.log('Add health record error:', error);

        this.errorMessage =
          error.error?.message ??
          this.getValidationErrors(error) ??
          `Unable to add health record. Status: ${error.status}`;
      },

      complete: () => {
        this.isSaving = false;
      }
    });
  }

  canAddHealthRecord(status: number | string): boolean {
    return this.getStatusText(status).toLowerCase() === 'completed';
  }

  getStatusText(status: number | string): string {
    if (typeof status === 'string') {
      return status;
    }

    const statusMap: Record<number, string> = {
      1: 'Scheduled',
      2: 'Confirmed',
      3: 'Cancelled',
      4: 'Completed'
    };

    return statusMap[status] ?? `Status ${status}`;
  }

  getStatusClass(status: number | string): string {
    const normalizedStatus =
      this.getStatusText(status).toLowerCase();

    if (normalizedStatus === 'completed') {
      return 'status-completed';
    }

    if (normalizedStatus === 'confirmed') {
      return 'status-confirmed';
    }

    if (normalizedStatus === 'cancelled') {
      return 'status-cancelled';
    }

    return 'status-scheduled';
  }

  getPatientDisplayName(appointment: Appointment): string {
    return appointment.patientName ??
      `Patient ID ${appointment.patientId ?? '-'}`;
  }

  formatDate(value: string): string {
    if (!value) {
      return '-';
    }

    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private getDateOnly(value: string): string {
    return new Date(value).toISOString().split('T')[0];
  }

  private getValidationErrors(error: HttpErrorResponse): string | null {
    const validationErrors =
      error.error?.errors;

    if (!validationErrors) {
      return null;
    }

    const messages: string[] = [];

    Object.keys(validationErrors).forEach((key) => {
      const fieldErrors =
        validationErrors[key];

      if (Array.isArray(fieldErrors)) {
        messages.push(...fieldErrors);
      }
    });

    return messages.length > 0
      ? messages.join(' ')
      : null;
  }
}