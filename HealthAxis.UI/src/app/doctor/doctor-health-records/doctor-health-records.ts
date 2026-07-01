import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { forkJoin } from 'rxjs';

import { Appointment } from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import {
  HealthRecord,
  UpdateHealthRecordRequest
} from '../../core/models/health-record.model';
import { HealthRecordService } from '../../core/services/health-record.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-doctor-health-records',
  imports: [DatePipe, ReactiveFormsModule, RouterLink],
  templateUrl: './doctor-health-records.html',
  styleUrl: './doctor-health-records.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorHealthRecords {
  private readonly formBuilder = inject(FormBuilder);
  private readonly appointmentService = inject(AppointmentService);
  private readonly healthRecordService = inject(HealthRecordService);

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);
  readonly selectedRecord = signal<HealthRecord | null>(null);
  readonly editingRecord = signal<HealthRecord | null>(null);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly errorMessage = signal('');
  readonly successDialogMessage = signal('');
  readonly searchText = signal('');

  readonly editForm = this.formBuilder.nonNullable.group({
    diagnosis: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(500)
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
    notes: ['', [Validators.maxLength(1000)]]
  });

  readonly completedAppointments = computed(() =>
    this.appointments().filter(
      (appointment) => this.getStatusText(appointment.status) === 'completed'
    )
  );

  readonly treatedPatientCount = computed(() =>
    new Set(
      this.completedAppointments().map((appointment) => appointment.patientId)
    ).size
  );

  readonly recordCount = computed(() => this.filteredRecords().length);

  readonly latestRecord = computed<HealthRecord | null>(() => {
    const record = this.filteredRecords()
      .slice()
      .sort((first, second) =>
        new Date(second.visitDate).getTime() -
        new Date(first.visitDate).getTime()
      )[0];

    return record ?? null;
  });

  readonly filteredRecords = computed(() => {
    const searchValue = this.searchText().trim().toLowerCase();

    return this.healthRecords()
      .filter((record) => this.isDoctorTreatedRecord(record))
      .filter((record) => this.matchesSearch(record, searchValue))
      .sort((first, second) =>
        new Date(second.visitDate).getTime() -
        new Date(first.visitDate).getTime()
      );
  });

  constructor() {
    this.loadDoctorHealthRecords();
  }

  get diagnosis() {
    return this.editForm.controls.diagnosis;
  }

  get prescription() {
    return this.editForm.controls.prescription;
  }

  get notes() {
    return this.editForm.controls.notes;
  }

  loadDoctorHealthRecords(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.loadHealthRecordsForTreatedPatients();
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load doctor appointments.')
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
  }

  openRecord(record: HealthRecord): void {
    this.selectedRecord.set(record);
  }

  closeRecord(): void {
    this.selectedRecord.set(null);
  }

  openEditDialog(record: HealthRecord): void {
    this.errorMessage.set('');
    this.successDialogMessage.set('');
    this.editingRecord.set(record);

    this.editForm.reset({
      diagnosis: record.diagnosis,
      prescription: record.prescription,
      notes: record.notes ?? ''
    });
  }

  closeEditDialog(): void {
    if (this.saving()) {
      return;
    }

    this.editingRecord.set(null);
    this.editForm.reset({
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  updateRecord(): void {
    this.errorMessage.set('');
    this.successDialogMessage.set('');

    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      this.errorMessage.set('Please correct the health record details.');
      return;
    }

    const record = this.editingRecord();
    const recordId = this.getNumericRecordId(record);

    if (!record || recordId <= 0) {
      this.errorMessage.set('Invalid health record selected.');
      return;
    }

    const formValue = this.editForm.getRawValue();

    const request: UpdateHealthRecordRequest = {
      diagnosis: formValue.diagnosis.trim(),
      prescription: formValue.prescription.trim(),
      notes: formValue.notes.trim()
    };

    this.saving.set(true);

    this.healthRecordService.updateHealthRecord(recordId, request).subscribe({
      next: () => {
        this.saving.set(false);
        this.editingRecord.set(null);
        this.loadDoctorHealthRecords();

        this.successDialogMessage.set(
          'Health record updated successfully. Patient can now see the latest changes.'
        );
      },
      error: (error: unknown) => {
        this.saving.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not update health record.')
        );
      }
    });
  }

  closeSuccessDialog(): void {
    this.successDialogMessage.set('');
  }

  hasUpdated(record: HealthRecord): boolean {
    return Boolean(record.updatedDate);
  }

  getRecordId(record: HealthRecord): number | string {
    return record.healthRecordId ?? record.recordId ?? '-';
  }

  private getNumericRecordId(record: HealthRecord | null): number {
    if (!record) {
      return 0;
    }

    return record.healthRecordId ?? record.recordId ?? 0;
  }

  private loadHealthRecordsForTreatedPatients(): void {
    const patientIds = [
      ...new Set(
        this.completedAppointments().map((appointment) => appointment.patientId)
      )
    ];

    if (patientIds.length === 0) {
      this.healthRecords.set([]);
      this.loading.set(false);
      return;
    }

    const requests = patientIds.map((patientId) =>
      this.healthRecordService.getHealthRecordsByPatientId(patientId)
    );

    forkJoin(requests).subscribe({
      next: (recordsByPatient) => {
        this.healthRecords.set(recordsByPatient.flat());
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load health records.')
        );
      }
    });
  }

  private isDoctorTreatedRecord(record: HealthRecord): boolean {
    const completedAppointmentIds = new Set(
      this.completedAppointments().map((appointment) => appointment.appointmentId)
    );

    return completedAppointmentIds.has(record.appointmentId);
  }

  private matchesSearch(record: HealthRecord, searchValue: string): boolean {
    if (!searchValue) {
      return true;
    }

    const searchableText = [
      record.patientName,
      record.doctorName,
      record.specialisation,
      record.visitDate,
      record.diagnosis,
      record.prescription,
      record.notes,
      record.appointmentId.toString()
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
}