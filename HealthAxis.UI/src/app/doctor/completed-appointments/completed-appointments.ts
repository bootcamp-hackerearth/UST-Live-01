import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';
import {
  catchError,
  forkJoin,
  of
} from 'rxjs';

import { Appointment } from '../../core/models/appointment.model';
import {
  CreateHealthRecordRequest,
  HealthRecord
} from '../../core/models/health-record.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { HealthRecordService } from '../../core/services/health-record.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const PAGE_SIZE = 6;
const PRINT_DELAY_IN_MS = 250;

@Component({
  selector: 'app-completed-appointments',
  imports: [
    DatePipe,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './completed-appointments.html',
  styleUrl: './completed-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CompletedAppointments {
  private readonly formBuilder = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly appointmentService =
    inject(AppointmentService);
  private readonly healthRecordService =
    inject(HealthRecordService);

  private requestedAppointmentId: number | null =
    this.getRequestedAppointmentId();

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly loadingRecord = signal(false);

  readonly selectedAppointment =
    signal<Appointment | null>(null);

  readonly selectedHealthRecord =
    signal<HealthRecord | null>(null);

  readonly searchText = signal('');
  readonly confirmedPage = signal(1);
  readonly completedPage = signal(1);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly todayDate =
    this.formatDateForInput(new Date());

  readonly healthRecordForm =
    this.formBuilder.nonNullable.group({
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

  readonly allConfirmedAppointments = computed(() => {
    return this.appointments()
      .filter(
        (appointment) =>
          this.getStatusText(
            appointment.status
          ) === 'confirmed'
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(first).getTime() -
          this.getAppointmentStartDateTime(second).getTime()
      );
  });

  readonly readyAppointments = computed(() => {
    return this.allConfirmedAppointments()
      .filter((appointment) =>
        this.hasAppointmentStarted(appointment)
      )
      .filter((appointment) =>
        this.matchesSearch(appointment)
      );
  });

  readonly upcomingConfirmedCount = computed(() => {
    return this.allConfirmedAppointments().filter(
      (appointment) =>
        !this.hasAppointmentStarted(appointment)
    ).length;
  });

  readonly allCompletedAppointments = computed(() => {
    return this.appointments()
      .filter(
        (appointment) =>
          this.getStatusText(
            appointment.status
          ) === 'completed'
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(second).getTime() -
          this.getAppointmentStartDateTime(first).getTime()
      );
  });

  readonly completedAppointments = computed(() => {
    return this.allCompletedAppointments()
      .filter((appointment) =>
        this.matchesSearch(appointment)
      );
  });

  readonly confirmedTotalPages = computed(() =>
    this.getTotalPages(
      this.readyAppointments().length
    )
  );

  readonly completedTotalPages = computed(() =>
    this.getTotalPages(
      this.completedAppointments().length
    )
  );

  readonly pagedReadyAppointments = computed(() => {
    const startIndex =
      (this.confirmedPage() - 1) * PAGE_SIZE;

    return this.readyAppointments().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly pagedCompletedAppointments = computed(() => {
    const startIndex =
      (this.completedPage() - 1) * PAGE_SIZE;

    return this.completedAppointments().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly readyCount = computed(() =>
    this.allConfirmedAppointments().filter(
      (appointment) =>
        this.hasAppointmentStarted(appointment)
    ).length
  );

  readonly completedCount = computed(() =>
    this.allCompletedAppointments().length
  );

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

  constructor() {
    this.refreshPage();
  }

  refreshPage(): void {
    this.loadAppointments();
  }

  loadAppointments(
    clearMessages = true
  ): void {
    this.loading.set(true);

    if (clearMessages) {
      this.errorMessage.set('');
      this.successMessage.set('');
    }

    this.appointmentService
      .getMyDoctorAppointments()
      .subscribe({
        next: (appointments) => {
          this.appointments.set(appointments);
          this.ensurePagesAreValid();
          this.loadHealthRecordsForCompletedPatients();
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load appointments.'
            )
          );
        }
      });
  }

  onSearchInput(event: Event): void {
    const input =
      event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.confirmedPage.set(1);
    this.completedPage.set(1);
  }

  clearSearch(): void {
    this.searchText.set('');
    this.confirmedPage.set(1);
    this.completedPage.set(1);
  }

  goToPreviousConfirmedPage(): void {
    if (this.confirmedPage() <= 1) {
      return;
    }

    this.confirmedPage.update(
      (page) => page - 1
    );
  }

  goToNextConfirmedPage(): void {
    if (
      this.confirmedPage() >=
      this.confirmedTotalPages()
    ) {
      return;
    }

    this.confirmedPage.update(
      (page) => page + 1
    );
  }

  goToPreviousCompletedPage(): void {
    if (this.completedPage() <= 1) {
      return;
    }

    this.completedPage.update(
      (page) => page - 1
    );
  }

  goToNextCompletedPage(): void {
    if (
      this.completedPage() >=
      this.completedTotalPages()
    ) {
      return;
    }

    this.completedPage.update(
      (page) => page + 1
    );
  }

  openHealthRecordDialog(
    appointment: Appointment
  ): void {
    if (!this.canAddHealthRecord(appointment)) {
      this.errorMessage.set(
        this.getHealthRecordRestrictionMessage(
          appointment
        )
      );

      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.selectedAppointment.set(appointment);

    this.healthRecordForm.reset({
      visitDate: this.getDateInputValue(
        appointment.scheduledDate
      ),
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
    this.resetHealthRecordForm();
  }

  saveHealthRecord(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.healthRecordForm.invalid) {
      this.healthRecordForm.markAllAsTouched();

      this.errorMessage.set(
        'Please correct the health record details.'
      );

      return;
    }

    const appointment =
      this.selectedAppointment();

    if (!appointment) {
      this.errorMessage.set(
        'Please select an appointment.'
      );

      return;
    }

    if (!this.canAddHealthRecord(appointment)) {
      this.errorMessage.set(
        this.getHealthRecordRestrictionMessage(
          appointment
        )
      );

      return;
    }

    const formValue =
      this.healthRecordForm.getRawValue();

    const request: CreateHealthRecordRequest = {
      appointmentId:
        appointment.appointmentId,
      patientId:
        appointment.patientId,
      doctorId:
        appointment.doctorId,
      visitDate:
        formValue.visitDate,
      diagnosis:
        formValue.diagnosis.trim(),
      prescription:
        formValue.prescription.trim(),
      notes:
        formValue.notes.trim()
    };

    this.saving.set(true);

    this.healthRecordService
      .createHealthRecord(request)
      .subscribe({
        next: (createdRecord) => {
          const safeRecord = this.enrichRecord(
            createdRecord?.appointmentId
              ? createdRecord
              : this.buildLocalHealthRecord(
                  appointment,
                  request
                )
          );

          this.healthRecords.update(
            (records) => [
              safeRecord,
              ...records.filter(
                (record) =>
                  Number(record.appointmentId) !==
                  appointment.appointmentId
              )
            ]
          );

          this.saving.set(false);
          this.selectedAppointment.set(null);
          this.resetHealthRecordForm();

          this.successMessage.set(
            'Health record added successfully. The appointment is now completed.'
          );

          this.loadAppointments(false);
        },
        error: (error: unknown) => {
          this.saving.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not save the health record.'
            )
          );
        }
      });
  }

  openCompletedHealthRecord(
    appointment: Appointment
  ): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    const existingRecord =
      this.getHealthRecordForAppointment(
        appointment
      );

    if (existingRecord) {
      this.selectedHealthRecord.set(
        existingRecord
      );

      return;
    }

    this.loadingRecord.set(true);

    this.healthRecordService
      .getHealthRecordsByPatientId(
        appointment.patientId
      )
      .subscribe({
        next: (records) => {
          const enrichedRecords = records.map(
            (record) =>
              this.enrichRecord(record)
          );

          this.mergeHealthRecords(
            enrichedRecords
          );

          const record =
            this.getHealthRecordForAppointment(
              appointment
            );

          this.loadingRecord.set(false);

          if (!record) {
            this.errorMessage.set(
              'The health record is not available for this appointment.'
            );

            return;
          }

          this.selectedHealthRecord.set(record);
        },
        error: (error: unknown) => {
          this.loadingRecord.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load the health record.'
            )
          );
        }
      });
  }

  closeCompletedHealthRecord(): void {
    this.selectedHealthRecord.set(null);
  }

  printHealthRecordForAppointment(
    appointment: Appointment
  ): void {
    const record =
      this.getHealthRecordForAppointment(
        appointment
      );

    if (!record) {
      this.errorMessage.set(
        'The health record is not available for this appointment.'
      );

      return;
    }

    this.printHealthRecordPdf(record);
  }

  printHealthRecordPdf(
    record: HealthRecord
  ): void {
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
      this.getRecordId(record);

    const patientName = this.escapeHtml(
      this.getSafeText(
        record.patientName,
        'Patient'
      )
    );

    const doctorName = this.escapeHtml(
      this.getDoctorDisplayName(
        record.doctorName
      )
    );

    const specialisation =
      this.escapeHtml(
        this.getSafeText(
          record.specialisation,
          'Not assigned'
        )
      );

    const visitDate = this.escapeHtml(
      this.formatDateForDisplay(
        record.visitDate
      )
    );

    const createdAt = this.escapeHtml(
      this.formatDateTimeForDisplay(
        record.createdAt,
        'Not available'
      )
    );

    const updatedAt = this.escapeHtml(
      this.formatDateTimeForDisplay(
        record.updatedDate,
        'Not updated'
      )
    );

    const diagnosis = this.escapeHtml(
      this.getSafeText(
        record.diagnosis,
        'Diagnosis not provided.'
      )
    );

    const prescription = this.escapeHtml(
      this.getSafeText(
        record.prescription,
        'Prescription not provided.'
      )
    );

    const notes = this.escapeHtml(
      this.getSafeText(
        record.notes,
        'No additional notes.'
      )
    );

    const printableDocument = `
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
              background: #f7f8fc;
              color: #111c36;
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
              <p>Completed consultation health record</p>
            </header>

            <section class="grid">
              <div class="box">
                <span>Record ID</span>
                <strong>#${recordId}</strong>
              </div>

              <div class="box">
                <span>Appointment ID</span>
                <strong>#${record.appointmentId}</strong>
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
    `;

    printWindow.document.documentElement.innerHTML =
      printableDocument;
  }

  canAddHealthRecord(
    appointment: Appointment
  ): boolean {
    return (
      this.getStatusText(
        appointment.status
      ) === 'confirmed' &&
      this.hasAppointmentStarted(
        appointment
      )
    );
  }

  getHealthRecordRestrictionMessage(
    appointment: Appointment
  ): string {
    if (
      this.getStatusText(
        appointment.status
      ) !== 'confirmed'
    ) {
      return (
        'A health record can be added only to a confirmed appointment.'
      );
    }

    if (
      !this.hasAppointmentStarted(appointment)
    ) {
      return (
        'The health record can be added after the scheduled appointment time.'
      );
    }

    return '';
  }

  hasHealthRecord(
    appointment: Appointment
  ): boolean {
    return Boolean(
      this.getHealthRecordForAppointment(
        appointment
      )
    );
  }

  getHealthRecordForAppointment(
    appointment: Appointment
  ): HealthRecord | undefined {
    return this.healthRecords().find(
      (record) =>
        Number(record.appointmentId) ===
        appointment.appointmentId
    );
  }

  getRecordId(
    record: HealthRecord
  ): number | string {
    return (
      record.healthRecordId ??
      record.recordId ??
      '-'
    );
  }

  getStatusLabel(
    status: string | number
  ): string {
    switch (this.getStatusText(status)) {
      case 'pending':
        return 'Pending';

      case 'confirmed':
        return 'Confirmed';

      case 'completed':
        return 'Completed';

      case 'cancelled':
        return 'Cancelled';

      default:
        return String(status);
    }
  }

  getPatientName(
    appointment: Appointment
  ): string {
    return appointment.patientName?.trim() ||
      'Patient';
  }

  getDoctorDisplayName(
    value: string | null | undefined
  ): string {
    const name = (value ?? '').trim();

    if (!name) {
      return 'Doctor';
    }

    const normalizedName =
      name.toLowerCase();

    if (
      normalizedName.startsWith('dr.') ||
      normalizedName.startsWith('dr ')
    ) {
      return name;
    }

    return `Dr. ${name}`;
  }

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    const cleanValue = (value ?? '').trim();

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

    if (this.diagnosis.errors['required']) {
      return 'Diagnosis is required.';
    }

    if (this.diagnosis.errors['minlength']) {
      return (
        'Diagnosis must contain at least 3 characters.'
      );
    }

    if (this.diagnosis.errors['maxlength']) {
      return (
        'Diagnosis cannot exceed 300 characters.'
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

    if (this.prescription.errors['required']) {
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

    if (this.notes.errors['maxlength']) {
      return (
        'Notes cannot exceed 500 characters.'
      );
    }

    return '';
  }

  private loadHealthRecordsForCompletedPatients(): void {
    const completedAppointments =
      this.allCompletedAppointments();

    const patientIds = [
      ...new Set(
        completedAppointments.map(
          (appointment) =>
            appointment.patientId
        )
      )
    ];

    if (patientIds.length === 0) {
      this.healthRecords.set([]);
      this.loading.set(false);
      this.openRequestedAppointment();
      return;
    }

    const requests = patientIds.map(
      (patientId) =>
        this.healthRecordService
          .getHealthRecordsByPatientId(
            patientId
          )
          .pipe(
            catchError(() =>
              of([] as HealthRecord[])
            )
          )
    );

    forkJoin(requests).subscribe({
      next: (recordsByPatient) => {
        const completedIds = new Set(
          completedAppointments.map(
            (appointment) =>
              appointment.appointmentId
          )
        );

        const records = recordsByPatient
          .flat()
          .filter(
            (record) =>
              completedIds.has(
                Number(record.appointmentId)
              )
          )
          .map((record) =>
            this.enrichRecord(record)
          );

        this.healthRecords.set(
          this.getUniqueRecords(records)
        );

        this.loading.set(false);
        this.openRequestedAppointment();
      },
      error: () => {
        this.healthRecords.set([]);
        this.loading.set(false);
        this.openRequestedAppointment();

        this.errorMessage.set(
          'Appointments loaded, but some health records could not be loaded.'
        );
      }
    });
  }

  private openRequestedAppointment(): void {
    const appointmentId =
      this.requestedAppointmentId;

    if (!appointmentId) {
      return;
    }

    this.requestedAppointmentId = null;

    const appointment =
      this.appointments().find(
        (item) =>
          item.appointmentId ===
          appointmentId
      );

    if (!appointment) {
      this.errorMessage.set(
        'The selected appointment could not be found.'
      );

      return;
    }

    this.openHealthRecordDialog(appointment);
  }

  private getRequestedAppointmentId():
    number | null {
    const value =
      this.route.snapshot.queryParamMap.get(
        'appointmentId'
      );

    if (!value) {
      return null;
    }

    const appointmentId = Number(value);

    return Number.isInteger(appointmentId) &&
      appointmentId > 0
      ? appointmentId
      : null;
  }

  private mergeHealthRecords(
    newRecords: HealthRecord[]
  ): void {
    this.healthRecords.update(
      (currentRecords) =>
        this.getUniqueRecords([
          ...newRecords,
          ...currentRecords
        ])
    );
  }

  private getUniqueRecords(
    records: HealthRecord[]
  ): HealthRecord[] {
    const recordsByAppointment =
      new Map<number, HealthRecord>();

    for (const record of records) {
      recordsByAppointment.set(
        Number(record.appointmentId),
        record
      );
    }

    return [
      ...recordsByAppointment.values()
    ];
  }

  private enrichRecord(
    record: HealthRecord
  ): HealthRecord {
    const appointment =
      this.appointments().find(
        (item) =>
          item.appointmentId ===
          Number(record.appointmentId)
      );

    if (!appointment) {
      return record;
    }

    return {
      ...record,
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

  private buildLocalHealthRecord(
    appointment: Appointment,
    request: CreateHealthRecordRequest
  ): HealthRecord {
    return {
      healthRecordId:
        appointment.appointmentId,
      recordId:
        appointment.appointmentId,
      appointmentId:
        appointment.appointmentId,
      patientId:
        appointment.patientId,
      patientName:
        appointment.patientName,
      doctorId:
        appointment.doctorId,
      doctorName:
        appointment.doctorName,
      specialisation:
        appointment.specialisation,
      visitDate:
        request.visitDate,
      createdAt:
        new Date().toISOString(),
      diagnosis:
        request.diagnosis,
      prescription:
        request.prescription,
      notes:
        request.notes,
      updatedDate:
        null
    };
  }

  private resetHealthRecordForm(): void {
    this.healthRecordForm.reset({
      visitDate: this.todayDate,
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  private ensurePagesAreValid(): void {
    if (
      this.confirmedPage() >
      this.confirmedTotalPages()
    ) {
      this.confirmedPage.set(
        this.confirmedTotalPages()
      );
    }

    if (
      this.completedPage() >
      this.completedTotalPages()
    ) {
      this.completedPage.set(
        this.completedTotalPages()
      );
    }
  }

  private getTotalPages(
    totalItems: number
  ): number {
    const pages = Math.ceil(
      totalItems / PAGE_SIZE
    );

    return Math.max(pages, 1);
  }

  private matchesSearch(
    appointment: Appointment
  ): boolean {
    const searchValue =
      this.searchText()
        .trim()
        .toLowerCase();

    if (!searchValue) {
      return true;
    }

    const searchableText = [
      appointment.patientName,
      appointment.doctorName,
      appointment.specialisation,
      appointment.scheduledDate,
      appointment.timeSlot,
      appointment.status,
      appointment.appointmentId.toString()
    ]
      .join(' ')
      .toLowerCase();

    return searchableText.includes(
      searchValue
    );
  }

  private hasAppointmentStarted(
    appointment: Appointment
  ): boolean {
    return (
      this.getAppointmentStartDateTime(
        appointment
      ).getTime() <= Date.now()
    );
  }

  private getAppointmentStartDateTime(
    appointment: Appointment
  ): Date {
    const appointmentDate =
      this.createLocalDate(
        appointment.scheduledDate
      );

    const startTimeText =
      appointment.timeSlot
        .split('-')[0]
        ?.trim();

    if (!startTimeText) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    const timeMatch =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)$/i
        .exec(startTimeText);

    if (!timeMatch) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    let hour = Number(timeMatch[1]);
    const minute = Number(timeMatch[2]);
    const period =
      timeMatch[3].toUpperCase();

    if (
      period === 'PM' &&
      hour !== 12
    ) {
      hour += 12;
    }

    if (
      period === 'AM' &&
      hour === 12
    ) {
      hour = 0;
    }

    appointmentDate.setHours(
      hour,
      minute,
      0,
      0
    );

    return appointmentDate;
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
        (part) => Number.isFinite(part)
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

  private getDateInputValue(
    dateValue: string
  ): string {
    return dateValue.split('T')[0];
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

  private formatDateForInput(
    date: Date
  ): string {
    const year = date.getFullYear();

    const month =
      `${date.getMonth() + 1}`
        .padStart(2, '0');

    const day =
      `${date.getDate()}`
        .padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private formatDateForDisplay(
    dateValue: string
  ): string {
    const date =
      this.createLocalDate(dateValue);

    return date.toLocaleDateString();
  }

  private formatDateTimeForDisplay(
    dateValue: string | null | undefined,
    fallback: string
  ): string {
    if (!dateValue) {
      return fallback;
    }

    const date = new Date(dateValue);

    if (Number.isNaN(date.getTime())) {
      return fallback;
    }

    return date.toLocaleString();
  }

  private escapeHtml(
    value: string
  ): string {
    return value
      .replaceAll('&', '&amp;')
      .replaceAll('<', '&lt;')
      .replaceAll('>', '&gt;')
      .replaceAll('"', '&quot;')
      .replaceAll("'", '&#039;');
  }

  private static noFutureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate =
      CompletedAppointments.parseDateInput(
        String(control.value)
      );

    const today =
      CompletedAppointments.startOfDay(
        new Date()
      );

    if (selectedDate > today) {
      return {
        futureDate: true
      };
    }

    return null;
  }

  private static parseDateInput(
    value: string
  ): Date {
    const [year, month, day] =
      value.split('-').map(Number);

    return new Date(
      year,
      month - 1,
      day
    );
  }

  private static startOfDay(
    date: Date
  ): Date {
    const cleanDate = new Date(date);

    cleanDate.setHours(0, 0, 0, 0);

    return cleanDate;
  }
}