import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { Appointment } from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { CreateHealthRecordRequest } from '../../core/models/health-record.model';
import { HealthRecordService } from '../../core/services/health-record.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-completed-appointments',
  imports: [DatePipe, ReactiveFormsModule, RouterLink],
  templateUrl: './completed-appointments.html',
  styleUrl: './completed-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CompletedAppointments {
  private readonly formBuilder = inject(FormBuilder);
  private readonly appointmentService = inject(AppointmentService);
  private readonly healthRecordService = inject(HealthRecordService);

  readonly appointments = signal<Appointment[]>([]);
  readonly loading = signal(false);
  readonly saving = signal(false);

  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly searchText = signal('');

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly todayDate = this.formatDateForInput(new Date());

  readonly healthRecordForm = this.formBuilder.nonNullable.group({
    visitDate: [
      this.todayDate,
      [
        Validators.required,
        CompletedAppointments.noFutureDateValidator
      ]
    ],
    diagnosis: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(300)
      ]
    ],
    prescription: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(500)
      ]
    ],
    notes: [
      '',
      [
        Validators.maxLength(500)
      ]
    ]
  });

  readonly confirmedAppointments = computed(() =>
    this.appointments()
      .filter((appointment) => this.getStatusText(appointment.status) === 'confirmed')
      .filter((appointment) => this.matchesSearch(appointment))
      .sort((first, second) =>
        new Date(first.scheduledDate).getTime() -
        new Date(second.scheduledDate).getTime()
      )
  );

  readonly completedAppointments = computed(() =>
    this.appointments()
      .filter((appointment) => this.getStatusText(appointment.status) === 'completed')
      .filter((appointment) => this.matchesSearch(appointment))
      .sort((first, second) =>
        new Date(second.scheduledDate).getTime() -
        new Date(first.scheduledDate).getTime()
      )
  );

  readonly readyCount = computed(() => this.confirmedAppointments().length);
  readonly completedCount = computed(() => this.completedAppointments().length);

  constructor() {
    this.loadAppointments();
  }

  get visitDate() {
    return this.healthRecordForm.controls.visitDate;
  }

  get diagnosis() {
    return this.healthRecordForm.controls.diagnosis;
  }

  get prescription() {
    return this.healthRecordForm.controls.prescription;
  }

  get notes() {
    return this.healthRecordForm.controls.notes;
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load appointments.')
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
  }

  openHealthRecordDialog(appointment: Appointment): void {
    if (this.getStatusText(appointment.status) !== 'confirmed') {
      this.errorMessage.set(
        'Health record can be added only for confirmed appointments.'
      );
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.selectedAppointment.set(appointment);

    this.healthRecordForm.reset({
      visitDate: this.todayDate,
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  closeHealthRecordDialog(): void {
    if (this.saving()) {
      return;
    }

    this.selectedAppointment.set(null);
    this.healthRecordForm.reset({
      visitDate: this.todayDate,
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  saveHealthRecord(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.healthRecordForm.invalid) {
      this.healthRecordForm.markAllAsTouched();
      this.errorMessage.set('Please complete the health record details.');
      return;
    }

    const appointment = this.selectedAppointment();

    if (!appointment) {
      this.errorMessage.set('Please select an appointment.');
      return;
    }

    if (this.getStatusText(appointment.status) !== 'confirmed') {
      this.errorMessage.set(
        'Only confirmed appointments can be completed with health record.'
      );
      return;
    }

    const formValue = this.healthRecordForm.getRawValue();

    const request: CreateHealthRecordRequest = {
      appointmentId: appointment.appointmentId,
      patientId: appointment.patientId,
      doctorId: appointment.doctorId,
      visitDate: formValue.visitDate,
      diagnosis: formValue.diagnosis.trim(),
      prescription: formValue.prescription.trim(),
      notes: formValue.notes.trim()
    };

    this.saving.set(true);

    this.healthRecordService.createHealthRecord(request).subscribe({
      next: () => {
        this.saving.set(false);
        this.selectedAppointment.set(null);
        this.successMessage.set(
          'Health record added successfully. Appointment marked as completed.'
        );
        this.loadAppointments();
      },
      error: (error: unknown) => {
        this.saving.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not save health record.')
        );
      }
    });
  }

  getStatusLabel(status: string | number): string {
    const normalizedStatus = this.getStatusText(status);

    if (normalizedStatus === 'pending') {
      return 'Pending';
    }

    if (normalizedStatus === 'confirmed') {
      return 'Confirmed';
    }

    if (normalizedStatus === 'completed') {
      return 'Completed';
    }

    if (normalizedStatus === 'cancelled') {
      return 'Cancelled';
    }

    return String(status);
  }

  private matchesSearch(appointment: Appointment): boolean {
    const searchValue = this.searchText().trim().toLowerCase();

    if (!searchValue) {
      return true;
    }

    const searchableText = [
      appointment.patientName,
      appointment.scheduledDate,
      appointment.timeSlot,
      appointment.status,
      appointment.appointmentId.toString()
    ]
      .join(' ')
      .toLowerCase();

    return searchableText.includes(searchValue);
  }

  private getStatusText(status: string | number): string {
    const value = String(status).trim().toLowerCase();

    if (value === '1') {
      return 'pending';
    }

    if (value === '2') {
      return 'confirmed';
    }

    if (value === '3') {
      return 'cancelled';
    }

    if (value === '4') {
      return 'completed';
    }

    return value;
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private static noFutureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate = new Date(control.value);
    const today = new Date();

    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    if (selectedDate > today) {
      return {
        futureDate: true
      };
    }

    return null;
  }
}