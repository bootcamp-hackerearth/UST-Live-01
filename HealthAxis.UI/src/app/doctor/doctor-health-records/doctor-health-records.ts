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
    notes: [
      '',
      [
        Validators.maxLength(1000)
      ]
    ]
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
      .sort(
        (first, second) =>
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
      .sort(
        (first, second) =>
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
    this.errorMessage.set('');
    this.selectedRecord.set(this.enrichRecord(this.normalizeRecord(record)));
  }

  closeRecord(): void {
    this.selectedRecord.set(null);
  }

  openEditDialog(record: HealthRecord): void {
    const safeRecord = this.enrichRecord(this.normalizeRecord(record));

    this.errorMessage.set('');
    this.successDialogMessage.set('');
    this.editingRecord.set(safeRecord);

    this.editForm.reset({
      diagnosis: safeRecord.diagnosis ?? '',
      prescription: safeRecord.prescription ?? '',
      notes: safeRecord.notes ?? ''
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

    if (!record) {
      this.errorMessage.set('Invalid health record selected.');
      return;
    }

    const safeRecord = this.enrichRecord(this.normalizeRecord(record));
    const recordId = this.getNumericRecordId(safeRecord);

    if (recordId <= 0) {
      this.errorMessage.set(
        'Unable to identify this health record. Please refresh and try again.'
      );
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
      next: (updatedRecord) => {
        const safeUpdatedRecord = this.enrichRecord(
          this.normalizeRecord({
            ...safeRecord,
            ...updatedRecord,
            diagnosis: request.diagnosis,
            prescription: request.prescription,
            notes: request.notes,
            updatedDate: new Date().toISOString()
          } as HealthRecord)
        );

        this.healthRecords.update((records) =>
          records.map((item) =>
            this.getAppointmentId(item) === this.getAppointmentId(safeRecord)
              ? safeUpdatedRecord
              : item
          )
        );

        this.selectedRecord.update((currentRecord) => {
          if (!currentRecord) {
            return currentRecord;
          }

          return this.getAppointmentId(currentRecord) === this.getAppointmentId(safeRecord)
            ? safeUpdatedRecord
            : currentRecord;
        });

        this.saving.set(false);
        this.editingRecord.set(null);

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
    const recordId = this.getNumericPrimaryRecordId(record);

    if (recordId > 0) {
      return recordId;
    }

    const appointmentId = this.getAppointmentId(record);

    return appointmentId > 0 ? appointmentId : '-';
  }

  private getNumericRecordId(record: HealthRecord | null): number {
    if (!record) {
      return 0;
    }

    const primaryId = this.getNumericPrimaryRecordId(record);

    if (primaryId > 0) {
      return primaryId;
    }

    return this.getAppointmentId(record);
  }

  private getNumericPrimaryRecordId(record: HealthRecord): number {
    const value = record as unknown as Record<string, unknown>;

    return this.firstValidNumber(
      value['healthRecordId'],
      value['HealthRecordId'],
      value['healthRecordID'],
      value['HealthRecordID'],
      value['recordId'],
      value['RecordId'],
      value['recordID'],
      value['RecordID'],
      value['id'],
      value['Id']
    );
  }

  private getAppointmentId(record: HealthRecord): number {
    const value = record as unknown as Record<string, unknown>;

    return this.firstValidNumber(
      record.appointmentId,
      value['AppointmentId'],
      value['appointmentID'],
      value['AppointmentID']
    );
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
        const completedAppointmentIds = new Set(
          this.completedAppointments().map(
            (appointment) => appointment.appointmentId
          )
        );

        const records = recordsByPatient
          .flat()
          .map((record) => this.normalizeRecord(record))
          .filter((record) =>
            completedAppointmentIds.has(this.getAppointmentId(record))
          )
          .map((record) => this.enrichRecord(record));

        this.healthRecords.set(records);
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

  private normalizeRecord(record: HealthRecord): HealthRecord {
    const value = record as unknown as Record<string, unknown>;

    const healthRecordId = this.firstValidNumber(
      record.healthRecordId,
      value['HealthRecordId'],
      value['healthRecordID'],
      value['HealthRecordID'],
      record.recordId,
      value['RecordId'],
      value['recordID'],
      value['RecordID'],
      value['id'],
      value['Id']
    );

    const appointmentId = this.firstValidNumber(
      record.appointmentId,
      value['AppointmentId'],
      value['appointmentID'],
      value['AppointmentID']
    );

    const patientId = this.firstValidNumber(
      record.patientId,
      value['PatientId'],
      value['patientID'],
      value['PatientID']
    );

    const doctorId = this.firstValidNumber(
      record.doctorId,
      value['DoctorId'],
      value['doctorID'],
      value['DoctorID']
    );

    return {
      ...record,
      healthRecordId: healthRecordId > 0 ? healthRecordId : record.healthRecordId,
      recordId: record.recordId,
      appointmentId,
      patientId,
      patientName: record.patientName ?? String(value['PatientName'] ?? ''),
      doctorId,
      doctorName: record.doctorName ?? String(value['DoctorName'] ?? ''),
      specialisation: record.specialisation ?? String(value['Specialisation'] ?? ''),
      visitDate: record.visitDate ?? String(value['VisitDate'] ?? ''),
      diagnosis: record.diagnosis ?? String(value['Diagnosis'] ?? ''),
      prescription: record.prescription ?? String(value['Prescription'] ?? ''),
      notes: record.notes ?? String(value['Notes'] ?? ''),
      updatedDate: record.updatedDate ?? String(value['UpdatedDate'] ?? '')
    };
  }

  private enrichRecord(record: HealthRecord): HealthRecord {
    const appointment = this.completedAppointments().find(
      (item) => item.appointmentId === this.getAppointmentId(record)
    );

    if (!appointment) {
      return record;
    }

    return {
      ...record,
      appointmentId: this.getAppointmentId(record) || appointment.appointmentId,
      patientId: record.patientId || appointment.patientId,
      patientName: record.patientName || appointment.patientName,
      doctorId: record.doctorId || appointment.doctorId,
      doctorName: record.doctorName || appointment.doctorName,
      specialisation: record.specialisation || appointment.specialisation
    };
  }

  private isDoctorTreatedRecord(record: HealthRecord): boolean {
    const completedAppointmentIds = new Set(
      this.completedAppointments().map(
        (appointment) => appointment.appointmentId
      )
    );

    return completedAppointmentIds.has(this.getAppointmentId(record));
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
      this.getAppointmentId(record).toString()
    ]
      .join(' ')
      .toLowerCase();

    return searchableText.includes(searchValue);
  }

  private firstValidNumber(...values: unknown[]): number {
    for (const value of values) {
      const numberValue = Number(value);

      if (Number.isFinite(numberValue) && numberValue > 0) {
        return numberValue;
      }
    }

    return 0;
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