import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { RouterLink } from '@angular/router';
import {
  catchError,
  forkJoin,
  of
} from 'rxjs';

import { Appointment } from '../../core/models/appointment.model';
import { AppointmentService } from '../../core/services/appointment.service';
import {
  HealthRecord,
  UpdateHealthRecordRequest
} from '../../core/models/health-record.model';
import { HealthRecordService } from '../../core/services/health-record.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const PAGE_SIZE = 6;
const PRINT_DELAY_IN_MS = 250;

@Component({
  selector: 'app-doctor-health-records',
  imports: [
    DatePipe,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './doctor-health-records.html',
  styleUrl: './doctor-health-records.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorHealthRecords {
  private readonly formBuilder = inject(FormBuilder);
  private readonly appointmentService =
    inject(AppointmentService);
  private readonly healthRecordService =
    inject(HealthRecordService);

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly selectedRecord =
    signal<HealthRecord | null>(null);

  readonly editingRecord =
    signal<HealthRecord | null>(null);

  readonly loading = signal(false);
  readonly saving = signal(false);

  readonly errorMessage = signal('');
  readonly successDialogMessage = signal('');
  readonly searchText = signal('');
  readonly currentPage = signal(1);

  readonly editForm =
    this.formBuilder.nonNullable.group({
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

  readonly completedAppointments = computed(() => {
    return this.appointments().filter(
      (appointment) =>
        this.getStatusText(
          appointment.status
        ) === 'completed'
    );
  });

  readonly doctorRecords = computed(() => {
    return this.healthRecords()
      .filter((record) =>
        this.isDoctorTreatedRecord(record)
      )
      .sort(
        (first, second) =>
          this.getRecordSortValue(second) -
          this.getRecordSortValue(first)
      );
  });

  readonly filteredRecords = computed(() => {
    const searchValue =
      this.searchText()
        .trim()
        .toLowerCase();

    return this.doctorRecords().filter(
      (record) =>
        this.matchesSearch(
          record,
          searchValue
        )
    );
  });

  readonly totalPages = computed(() => {
    const pages = Math.ceil(
      this.filteredRecords().length /
      PAGE_SIZE
    );

    return Math.max(pages, 1);
  });

  readonly pagedRecords = computed(() => {
    const startIndex =
      (this.currentPage() - 1) *
      PAGE_SIZE;

    return this.filteredRecords().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly treatedPatientCount = computed(() => {
    return new Set(
      this.doctorRecords()
        .map((record) => record.patientId)
        .filter((patientId) => patientId > 0)
    ).size;
  });

  readonly recordCount = computed(() =>
    this.doctorRecords().length
  );

  readonly latestRecord =
    computed<HealthRecord | null>(() => {
      return this.doctorRecords()[0] ??
        null;
    });

  get diagnosis() {
    return this.editForm.controls.diagnosis;
  }

  get prescription() {
    return this.editForm.controls.prescription;
  }

  get notes() {
    return this.editForm.controls.notes;
  }

  constructor() {
    this.loadDoctorHealthRecords();
  }

  loadDoctorHealthRecords(
    clearMessages = true
  ): void {
    this.loading.set(true);

    if (clearMessages) {
      this.errorMessage.set('');
    }

    this.appointmentService
      .getMyDoctorAppointments()
      .subscribe({
        next: (appointments) => {
          this.appointments.set(appointments);
          this.loadHealthRecordsForTreatedPatients();
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load doctor appointments.'
            )
          );
        }
      });
  }

  onSearchInput(event: Event): void {
    const input =
      event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  clearSearch(): void {
    this.searchText.set('');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() <= 1) {
      return;
    }

    this.currentPage.update(
      (page) => page - 1
    );
  }

  goToNextPage(): void {
    if (
      this.currentPage() >=
      this.totalPages()
    ) {
      return;
    }

    this.currentPage.update(
      (page) => page + 1
    );
  }

  openRecord(record: HealthRecord): void {
    this.errorMessage.set('');

    this.selectedRecord.set(
      this.prepareRecord(record)
    );
  }

  closeRecord(): void {
    this.selectedRecord.set(null);
  }

  openEditDialog(
    record: HealthRecord
  ): void {
    const safeRecord =
      this.prepareRecord(record);

    if (!this.canEditRecord(safeRecord)) {
      this.errorMessage.set(
        'The health record ID is unavailable. Refresh the page and try again.'
      );

      return;
    }

    this.errorMessage.set('');
    this.successDialogMessage.set('');
    this.selectedRecord.set(null);
    this.editingRecord.set(safeRecord);

    this.editForm.reset({
      diagnosis:
        safeRecord.diagnosis ?? '',
      prescription:
        safeRecord.prescription ?? '',
      notes:
        safeRecord.notes ?? ''
    });
  }

  closeEditDialog(): void {
    if (this.saving()) {
      return;
    }

    this.editingRecord.set(null);
    this.resetEditForm();
  }

  updateRecord(): void {
    this.errorMessage.set('');
    this.successDialogMessage.set('');

    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();

      this.errorMessage.set(
        'Please correct the health record details.'
      );

      return;
    }

    const record =
      this.editingRecord();

    if (!record) {
      this.errorMessage.set(
        'Invalid health record selected.'
      );

      return;
    }

    const safeRecord =
      this.prepareRecord(record);

    const recordId =
      this.getNumericPrimaryRecordId(
        safeRecord
      );

    if (recordId <= 0) {
      this.errorMessage.set(
        'Unable to identify this health record. Refresh the page and try again.'
      );

      return;
    }

    const formValue =
      this.editForm.getRawValue();

    const request:
      UpdateHealthRecordRequest = {
        diagnosis:
          formValue.diagnosis.trim(),
        prescription:
          formValue.prescription.trim(),
        notes:
          formValue.notes.trim()
      };

    this.saving.set(true);

    this.healthRecordService
      .updateHealthRecord(
        recordId,
        request
      )
      .subscribe({
        next: (updatedRecord) => {
          const safeUpdatedRecord =
            this.prepareRecord({
              ...safeRecord,
              ...updatedRecord,
              healthRecordId:
                updatedRecord.healthRecordId ??
                safeRecord.healthRecordId,
              recordId:
                updatedRecord.recordId ??
                safeRecord.recordId,
              createdAt:
                updatedRecord.createdAt ??
                safeRecord.createdAt,
              diagnosis:
                request.diagnosis,
              prescription:
                request.prescription,
              notes:
                request.notes,
              updatedDate:
                updatedRecord.updatedDate ??
                new Date().toISOString()
            });

          this.replaceHealthRecord(
            safeRecord,
            safeUpdatedRecord
          );

          this.saving.set(false);
          this.editingRecord.set(null);
          this.resetEditForm();

          this.successDialogMessage.set(
            'Health record updated successfully. The patient can now see the latest changes.'
          );
        },
        error: (error: unknown) => {
          this.saving.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not update the health record.'
            )
          );
        }
      });
  }

  closeSuccessDialog(): void {
    this.successDialogMessage.set('');
  }

  canEditRecord(
    record: HealthRecord
  ): boolean {
    return (
      this.getNumericPrimaryRecordId(
        record
      ) > 0
    );
  }

  hasUpdated(
    record: HealthRecord
  ): boolean {
    return Boolean(
      this.getValidDate(record.updatedDate)
    );
  }

  getRecordId(
    record: HealthRecord
  ): number | string {
    const recordId =
      this.getNumericPrimaryRecordId(
        record
      );

    return recordId > 0
      ? recordId
      : '-';
  }

  getPatientName(
    record: HealthRecord
  ): string {
    return this.getSafeText(
      record.patientName,
      'Patient'
    );
  }

  getDoctorDisplayName(
    name: string | null | undefined
  ): string {
    const cleanName =
      (name ?? '').trim();

    if (!cleanName) {
      return 'Doctor';
    }

    const normalizedName =
      cleanName.toLowerCase();

    if (
      normalizedName.startsWith('dr.') ||
      normalizedName.startsWith('dr ')
    ) {
      return cleanName;
    }

    return `Dr. ${cleanName}`;
  }

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    const cleanValue =
      (value ?? '').trim();

    return cleanValue || fallback;
  }

  getCreatedDateText(
    record: HealthRecord
  ): string {
    return this.formatDateTimeForDisplay(
      record.createdAt,
      'Not available'
    );
  }

  getUpdatedDateText(
    record: HealthRecord
  ): string {
    return this.formatDateTimeForDisplay(
      record.updatedDate,
      'Not updated'
    );
  }

  getDiagnosisError(): string {
    if (
      !this.diagnosis.touched ||
      !this.diagnosis.errors
    ) {
      return '';
    }

    if (
      this.diagnosis.errors['required']
    ) {
      return 'Diagnosis is required.';
    }

    if (
      this.diagnosis.errors['minlength']
    ) {
      return (
        'Diagnosis must contain at least 3 characters.'
      );
    }

    if (
      this.diagnosis.errors['maxlength']
    ) {
      return (
        'Diagnosis cannot exceed 500 characters.'
      );
    }

    return '';
  }

  getPrescriptionError(): string {
    if (
      !this.prescription.touched ||
      !this.prescription.errors
    ) {
      return '';
    }

    if (
      this.prescription.errors['required']
    ) {
      return 'Prescription is required.';
    }

    if (
      this.prescription.errors['minlength']
    ) {
      return (
        'Prescription must contain at least 3 characters.'
      );
    }

    if (
      this.prescription.errors['maxlength']
    ) {
      return (
        'Prescription cannot exceed 500 characters.'
      );
    }

    return '';
  }

  getNotesError(): string {
    if (
      !this.notes.touched ||
      !this.notes.errors
    ) {
      return '';
    }

    if (
      this.notes.errors['maxlength']
    ) {
      return (
        'Notes cannot exceed 1000 characters.'
      );
    }

    return '';
  }

  printHealthRecordPdf(
    record: HealthRecord
  ): void {
    const safeRecord =
      this.prepareRecord(record);

    const printWindow = window.open(
      '',
      '_blank',
      'width=900,height=700'
    );

    if (!printWindow) {
      this.errorMessage.set(
        'Please allow popups to download or print the health record.'
      );

      return;
    }

    const recordId =
      this.escapeHtml(
        String(
          this.getRecordId(safeRecord)
        )
      );

    const patientName =
      this.escapeHtml(
        this.getPatientName(safeRecord)
      );

    const doctorName =
      this.escapeHtml(
        this.getDoctorDisplayName(
          safeRecord.doctorName
        )
      );

    const specialisation =
      this.escapeHtml(
        this.getSafeText(
          safeRecord.specialisation,
          'Not assigned'
        )
      );

    const visitDate =
      this.escapeHtml(
        this.formatDateForDisplay(
          safeRecord.visitDate
        )
      );

    const createdAt =
      this.escapeHtml(
        this.getCreatedDateText(
          safeRecord
        )
      );

    const updatedAt =
      this.escapeHtml(
        this.getUpdatedDateText(
          safeRecord
        )
      );

    const diagnosis =
      this.escapeHtml(
        this.getSafeText(
          safeRecord.diagnosis,
          'Diagnosis not provided.'
        )
      );

    const prescription =
      this.escapeHtml(
        this.getSafeText(
          safeRecord.prescription,
          'Prescription not provided.'
        )
      );

    const notes =
      this.escapeHtml(
        this.getSafeText(
          safeRecord.notes,
          'No additional notes.'
        )
      );

    printWindow.document.open();

    printWindow.document.write(`
      <!DOCTYPE html>
      <html lang="en">
        <head>
          <meta charset="utf-8">
          <title>Health Record #${recordId}</title>

          <style>
            * {
              box-sizing: border-box;
            }

            body {
              margin: 0;
              padding: 24px;
              color: #111c36;
              background: #f7f8fc;
              font-family: Arial, sans-serif;
            }

            .document {
              width: min(820px, 100%);
              margin: 0 auto;
              padding: 30px;
              border: 1px solid #e1e5ee;
              border-radius: 18px;
              background: #ffffff;
            }

            .header {
              padding: 24px;
              border-radius: 16px;
              color: #ffffff;
              background:
                linear-gradient(
                  135deg,
                  #4024c7,
                  #5b3df5
                );
            }

            .header h1 {
              margin: 0;
              font-size: 28px;
            }

            .header p {
              margin: 8px 0 0;
              color: #eeeaff;
            }

            .grid {
              margin-top: 20px;
              display: grid;
              grid-template-columns: 1fr 1fr;
              gap: 12px;
            }

            .box {
              padding: 15px;
              border: 1px solid #e1e5ee;
              border-radius: 13px;
              background: #f8f9fd;
            }

            .box span {
              display: block;
              margin-bottom: 7px;
              color: #66758e;
              font-size: 11px;
              font-weight: bold;
              text-transform: uppercase;
            }

            .box strong {
              color: #111c36;
              font-size: 15px;
            }

            .section {
              margin-top: 15px;
              padding: 17px;
              border: 1px solid #ded8ff;
              border-radius: 14px;
              background: #faf9ff;
            }

            .section h2 {
              margin: 0 0 9px;
              font-size: 17px;
            }

            .section p {
              margin: 0;
              color: #3e4d67;
              line-height: 1.7;
              white-space: pre-wrap;
            }

            .footer {
              margin-top: 22px;
              padding-top: 14px;
              border-top: 1px solid #e1e5ee;
              color: #66758e;
              font-size: 12px;
            }

            @media print {
              body {
                padding: 0;
                background: #ffffff;
              }

              .document {
                width: auto;
                border: none;
                border-radius: 0;
              }
            }
          </style>
        </head>

        <body>
          <main class="document">
            <header class="header">
              <h1>HealthAxis Medical Record</h1>
              <p>Doctor treatment record</p>
            </header>

            <section class="grid">
              <div class="box">
                <span>Record ID</span>
                <strong>#${recordId}</strong>
              </div>

              <div class="box">
                <span>Appointment ID</span>
                <strong>#${safeRecord.appointmentId}</strong>
              </div>

              <div class="box">
                <span>Patient Name</span>
                <strong>${patientName}</strong>
              </div>

              <div class="box">
                <span>Doctor Name</span>
                <strong>${doctorName}</strong>
              </div>

              <div class="box">
                <span>Specialisation</span>
                <strong>${specialisation}</strong>
              </div>

              <div class="box">
                <span>Visit Date</span>
                <strong>${visitDate}</strong>
              </div>

              <div class="box">
                <span>Record Created</span>
                <strong>${createdAt}</strong>
              </div>

              <div class="box">
                <span>Last Updated</span>
                <strong>${updatedAt}</strong>
              </div>
            </section>

            <section class="section">
              <h2>Diagnosis</h2>
              <p>${diagnosis}</p>
            </section>

            <section class="section">
              <h2>Prescription</h2>
              <p>${prescription}</p>
            </section>

            <section class="section">
              <h2>Doctor Notes</h2>
              <p>${notes}</p>
            </section>

            <footer class="footer">
              This record was generated by the HealthAxis Doctor Portal.
            </footer>
          </main>

          <script>
            window.onload = function () {
              window.setTimeout(function () {
                window.print();
              }, ${PRINT_DELAY_IN_MS});
            };

            window.onafterprint = function () {
              window.close();
            };
          </script>
        </body>
      </html>
    `);

    printWindow.document.close();
  }

  private loadHealthRecordsForTreatedPatients(): void {
    const patientIds = [
      ...new Set(
        this.completedAppointments().map(
          (appointment) =>
            appointment.patientId
        )
      )
    ];

    if (patientIds.length === 0) {
      this.healthRecords.set([]);
      this.currentPage.set(1);
      this.loading.set(false);
      return;
    }

    let failedRequests = 0;

    const requests = patientIds.map(
      (patientId) =>
        this.healthRecordService
          .getHealthRecordsByPatientId(
            patientId
          )
          .pipe(
            catchError(() => {
              failedRequests += 1;

              return of(
                [] as HealthRecord[]
              );
            })
          )
    );

    forkJoin(requests).subscribe({
      next: (recordsByPatient) => {
        const completedAppointmentIds =
          new Set(
            this.completedAppointments().map(
              (appointment) =>
                appointment.appointmentId
            )
          );

        const records = recordsByPatient
          .flat()
          .map((record) =>
            this.prepareRecord(record)
          )
          .filter((record) =>
            completedAppointmentIds.has(
              this.getAppointmentId(
                record
              )
            )
          );

        this.healthRecords.set(
          this.getUniqueRecords(records)
        );

        this.ensureCurrentPageIsValid();
        this.loading.set(false);

        if (failedRequests > 0) {
          this.errorMessage.set(
            'Some patient health records could not be loaded. Refresh the page to try again.'
          );
        }
      },
      error: (error: unknown) => {
        this.loading.set(false);

        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not load health records.'
          )
        );
      }
    });
  }

  private prepareRecord(
    record: HealthRecord
  ): HealthRecord {
    return this.enrichRecord(
      this.normalizeRecord(record)
    );
  }

  private normalizeRecord(
    record: HealthRecord
  ): HealthRecord {
    const value =
      record as unknown as
        Record<string, unknown>;

    const healthRecordId =
      this.firstValidNumber(
        record.healthRecordId,
        record.recordId,
        value['HealthRecordId'],
        value['healthRecordID'],
        value['HealthRecordID'],
        value['RecordId'],
        value['recordID'],
        value['RecordID'],
        value['id'],
        value['Id']
      );

    const appointmentId =
      this.firstValidNumber(
        record.appointmentId,
        value['AppointmentId'],
        value['appointmentID'],
        value['AppointmentID']
      );

    const patientId =
      this.firstValidNumber(
        record.patientId,
        value['PatientId'],
        value['patientID'],
        value['PatientID']
      );

    const doctorId =
      this.firstValidNumber(
        record.doctorId,
        value['DoctorId'],
        value['doctorID'],
        value['DoctorID']
      );

    const patientName =
      this.firstString(
        record.patientName,
        value['PatientName']
      );

    const doctorName =
      this.firstString(
        record.doctorName,
        value['DoctorName']
      );

    const specialisation =
      this.firstString(
        record.specialisation,
        value['Specialisation']
      );

    const visitDate =
      this.firstString(
        record.visitDate,
        value['VisitDate']
      );

    const createdAt =
      this.firstString(
        record.createdAt,
        value['CreatedAt'],
        value['createdDate'],
        value['CreatedDate']
      );

    const updatedDate =
      this.firstString(
        record.updatedDate,
        value['UpdatedDate'],
        value['updatedAt'],
        value['UpdatedAt']
      );

    return {
      ...record,
      healthRecordId:
        healthRecordId > 0
          ? healthRecordId
          : record.healthRecordId,
      recordId:
        healthRecordId > 0
          ? healthRecordId
          : record.recordId,
      appointmentId,
      patientId,
      patientName,
      doctorId,
      doctorName,
      specialisation,
      visitDate,
      createdAt:
        createdAt || null,
      diagnosis:
        this.firstString(
          record.diagnosis,
          value['Diagnosis']
        ),
      prescription:
        this.firstString(
          record.prescription,
          value['Prescription']
        ),
      notes:
        this.firstString(
          record.notes,
          value['Notes']
        ),
      updatedDate:
        updatedDate || null
    };
  }

  private enrichRecord(
    record: HealthRecord
  ): HealthRecord {
    const appointment =
      this.completedAppointments().find(
        (item) =>
          item.appointmentId ===
          this.getAppointmentId(record)
      );

    if (!appointment) {
      return record;
    }

    return {
      ...record,
      appointmentId:
        this.getAppointmentId(record) ||
        appointment.appointmentId,
      patientId:
        record.patientId ||
        appointment.patientId,
      patientName:
        record.patientName ||
        appointment.patientName,
      doctorId:
        record.doctorId ||
        appointment.doctorId,
      doctorName:
        record.doctorName ||
        appointment.doctorName,
      specialisation:
        record.specialisation ||
        appointment.specialisation
    };
  }

  private replaceHealthRecord(
    currentRecord: HealthRecord,
    updatedRecord: HealthRecord
  ): void {
    this.healthRecords.update(
      (records) =>
        records.map((record) =>
          this.isSameRecord(
            record,
            currentRecord
          )
            ? updatedRecord
            : record
        )
    );

    this.selectedRecord.update(
      (record) => {
        if (
          !record ||
          !this.isSameRecord(
            record,
            currentRecord
          )
        ) {
          return record;
        }

        return updatedRecord;
      }
    );
  }

  private isSameRecord(
    first: HealthRecord,
    second: HealthRecord
  ): boolean {
    const firstRecordId =
      this.getNumericPrimaryRecordId(
        first
      );

    const secondRecordId =
      this.getNumericPrimaryRecordId(
        second
      );

    if (
      firstRecordId > 0 &&
      secondRecordId > 0
    ) {
      return (
        firstRecordId ===
        secondRecordId
      );
    }

    return (
      this.getAppointmentId(first) ===
      this.getAppointmentId(second)
    );
  }

  private getUniqueRecords(
    records: HealthRecord[]
  ): HealthRecord[] {
    const uniqueRecords =
      new Map<string, HealthRecord>();

    for (const record of records) {
      const recordId =
        this.getNumericPrimaryRecordId(
          record
        );

      const appointmentId =
        this.getAppointmentId(record);

      const key = recordId > 0
        ? `record-${recordId}`
        : `appointment-${appointmentId}`;

      uniqueRecords.set(key, record);
    }

    return [
      ...uniqueRecords.values()
    ];
  }

  private getNumericPrimaryRecordId(
    record: HealthRecord
  ): number {
    const value =
      record as unknown as
        Record<string, unknown>;

    return this.firstValidNumber(
      record.healthRecordId,
      record.recordId,
      value['HealthRecordId'],
      value['healthRecordID'],
      value['HealthRecordID'],
      value['RecordId'],
      value['recordID'],
      value['RecordID'],
      value['id'],
      value['Id']
    );
  }

  private getAppointmentId(
    record: HealthRecord
  ): number {
    const value =
      record as unknown as
        Record<string, unknown>;

    return this.firstValidNumber(
      record.appointmentId,
      value['AppointmentId'],
      value['appointmentID'],
      value['AppointmentID']
    );
  }

  private isDoctorTreatedRecord(
    record: HealthRecord
  ): boolean {
    const completedAppointmentIds =
      new Set(
        this.completedAppointments().map(
          (appointment) =>
            appointment.appointmentId
        )
      );

    return completedAppointmentIds.has(
      this.getAppointmentId(record)
    );
  }

  private matchesSearch(
    record: HealthRecord,
    searchValue: string
  ): boolean {
    if (!searchValue) {
      return true;
    }

    const searchableText = [
      record.patientName,
      record.doctorName,
      record.specialisation,
      record.visitDate,
      record.createdAt,
      record.updatedDate,
      record.diagnosis,
      record.prescription,
      record.notes,
      this.getAppointmentId(
        record
      ).toString(),
      String(
        this.getRecordId(record)
      )
    ]
      .join(' ')
      .toLowerCase();

    return searchableText.includes(
      searchValue
    );
  }

  private getRecordSortValue(
    record: HealthRecord
  ): number {
    const visitDate =
      this.createLocalDate(
        record.visitDate
      );

    if (
      !Number.isNaN(
        visitDate.getTime()
      )
    ) {
      return visitDate.getTime();
    }

    return (
      this.getValidDate(
        record.createdAt
      )?.getTime() ?? 0
    );
  }

  private ensureCurrentPageIsValid(): void {
    if (
      this.currentPage() >
      this.totalPages()
    ) {
      this.currentPage.set(
        this.totalPages()
      );
    }

    if (this.currentPage() < 1) {
      this.currentPage.set(1);
    }
  }

  private resetEditForm(): void {
    this.editForm.reset({
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  private firstValidNumber(
    ...values: unknown[]
  ): number {
    for (const value of values) {
      const numberValue =
        Number(value);

      if (
        Number.isFinite(numberValue) &&
        numberValue > 0
      ) {
        return numberValue;
      }
    }

    return 0;
  }

  private firstString(
    ...values: unknown[]
  ): string {
    for (const value of values) {
      if (
        typeof value === 'string' &&
        value.trim()
      ) {
        return value.trim();
      }
    }

    return '';
  }

  private getStatusText(
    status: string | number
  ): string {
    const value = String(status)
      .trim()
      .toLowerCase();

    switch (value) {
      case '1':
        return 'pending';

      case '2':
        return 'confirmed';

      case '3':
        return 'cancelled';

      case '4':
        return 'completed';

      default:
        return value;
    }
  }

  private formatDateForDisplay(
    dateValue: string
  ): string {
    const date =
      this.createLocalDate(dateValue);

    if (
      Number.isNaN(date.getTime())
    ) {
      return 'Not available';
    }

    return date.toLocaleDateString();
  }

  private formatDateTimeForDisplay(
    dateValue: string | null | undefined,
    fallback: string
  ): string {
    const date =
      this.getValidDate(dateValue);

    return date
      ? date.toLocaleString()
      : fallback;
  }

  private getValidDate(
    dateValue: string | null | undefined
  ): Date | null {
    if (!dateValue) {
      return null;
    }

    const date = new Date(dateValue);

    return Number.isNaN(date.getTime())
      ? null
      : date;
  }

  private createLocalDate(
    dateValue: string
  ): Date {
    const dateOnly =
      dateValue.split('T')[0];

    const parts = dateOnly
      .split('-')
      .map(Number);

    if (
      parts.length === 3 &&
      parts.every(
        (part) =>
          Number.isFinite(part)
      )
    ) {
      const [year, month, day] = parts;

      return new Date(
        year,
        month - 1,
        day
      );
    }

    return new Date(dateValue);
  }

  private escapeHtml(
    value: string
  ): string {
    return value
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
  }
}