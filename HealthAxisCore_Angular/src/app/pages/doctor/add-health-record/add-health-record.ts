import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { AppointmentService } from '../../../core/services/appointment.service';
import { HealthRecordService } from '../../../core/services/health-record.service';
import { AppointmentDto } from '../../../core/models/appointment.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-add-health-record',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './add-health-record.html',
  styleUrl: './add-health-record.css'
})
export class AddHealthRecord {
  appointmentId = 0;

  appointment = signal<AppointmentDto | null>(null);

  recordAlreadyExists = signal(false);

  recordForm: FormGroup;

  isLoading = signal(false);

  isSaving = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly formBuilder: FormBuilder,
    private readonly appointmentService: AppointmentService,
    private readonly healthRecordService: HealthRecordService,
    private readonly authService: AuthService
  ) {
    this.appointmentId = Number(this.route.snapshot.paramMap.get('appointmentId'));

    this.recordForm = this.formBuilder.group({
      diagnosis: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],
      prescription: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],
      notes: ['']
    });

    this.loadAppointment();
  }

  get diagnosis() {
    return this.recordForm.get('diagnosis');
  }

  get prescription() {
    return this.recordForm.get('prescription');
  }

  get notes() {
    return this.recordForm.get('notes');
  }

  loadAppointment(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    this.recordAlreadyExists.set(false);

    this.appointmentService.getAppointments().subscribe({
      next: appointments => {
        const found = appointments.find(
          appointment => appointment.appointmentId === this.appointmentId
        );

        if (!found) {
          this.errorMessage.set('Appointment not found.');
          this.isLoading.set(false);
          return;
        }

        this.appointment.set(found);

        if (found.status !== 'Completed') {
          this.errorMessage.set('Health record can be added only after appointment completion.');
          this.isLoading.set(false);
          return;
        }

        this.checkExistingHealthRecord();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  checkExistingHealthRecord(): void {
    this.healthRecordService.existsForAppointment(this.appointmentId).subscribe({
      next: exists => {
        this.recordAlreadyExists.set(exists);

        if (exists) {
          this.errorMessage.set('');
        }

        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  submitRecord(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.recordAlreadyExists()) {
      this.errorMessage.set('Health record already exists for this appointment.');
      return;
    }

    if (this.recordForm.invalid) {
      this.recordForm.markAllAsTouched();
      return;
    }

    const appointment = this.appointment();

    if (!appointment) {
      this.errorMessage.set('Appointment not found.');
      return;
    }

    if (appointment.status !== 'Completed') {
      this.errorMessage.set('Health record can be added only after appointment completion.');
      return;
    }

    this.isSaving.set(true);

    this.healthRecordService.create({
      patientId: appointment.patientId,
      appointmentId: appointment.appointmentId,
      diagnosis: this.recordForm.value.diagnosis,
      prescription: this.recordForm.value.prescription,
      notes: this.recordForm.value.notes
    }).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.recordAlreadyExists.set(true);
        this.successMessage.set('Health record added successfully.');
        this.router.navigate(['/doctor/schedule']);
      },
      error: error => {
        this.isSaving.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  canShowForm(): boolean {
    const appointment = this.appointment();

    return !!appointment &&
      appointment.status === 'Completed' &&
      !this.recordAlreadyExists();
  }

  canSave(): boolean {
    return !this.isSaving() &&
      this.canShowForm();
  }
}
