import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
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
import {
  CreateHealthRecordRequest,
  HealthRecord
} from '../../core/models/health-record.model';
import { AppointmentService } from '../../core/services/appointment.service';
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

  private readonly pageSize = 6;

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly loadingRecord = signal(false);

  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly selectedHealthRecord = signal<HealthRecord | null>(null);

  readonly searchText = signal('');
  readonly confirmedPage = signal(1);
  readonly completedPage = signal(1);

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

  readonly confirmedTotalPages = computed(() =>
    this.getTotalPages(this.confirmedAppointments().length)
  );

  readonly completedTotalPages = computed(() =>
    this.getTotalPages(this.completedAppointments().length)
  );

  readonly pagedConfirmedAppointments = computed(() => {
    const startIndex = (this.confirmedPage() - 1) * this.pageSize;

    return this.confirmedAppointments().slice(
      startIndex,
      startIndex + this.pageSize
    );
  });

  readonly pagedCompletedAppointments = computed(() => {
    const startIndex = (this.completedPage() - 1) * this.pageSize;

    return this.completedAppointments().slice(
      startIndex,
      startIndex + this.pageSize
    );
  });

  readonly readyCount = computed(() => this.confirmedAppointments().length);
  readonly completedCount = computed(() => this.completedAppointments().length);

  constructor() {
    this.refreshPage();
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

  refreshPage(): void {
    this.loadAppointments();
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.confirmedPage.set(1);
        this.completedPage.set(1);
        this.loadHealthRecordsForCompletedPatients();
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load appointments.')
        );
      }
    });
  }

  private loadHealthRecordsForCompletedPatients(): void {
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
          this.completedAppointments().map((appointment) => appointment.appointmentId)
        );

        const records = recordsByPatient
          .flat()
          .filter((record) => completedAppointmentIds.has(Number(record.appointmentId)))
          .map((record) => this.enrichRecord(record));

        this.healthRecords.set(records);
        this.loading.set(false);
      },
      error: () => {
        this.healthRecords.set([]);
        this.loading.set(false);
        this.errorMessage.set('Could not load health records for completed appointments.');
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
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
    if (this.confirmedPage() > 1) {
      this.confirmedPage.update((page) => page - 1);
    }
  }

  goToNextConfirmedPage(): void {
    if (this.confirmedPage() < this.confirmedTotalPages()) {
      this.confirmedPage.update((page) => page + 1);
    }
  }

  goToPreviousCompletedPage(): void {
    if (this.completedPage() > 1) {
      this.completedPage.update((page) => page - 1);
    }
  }

  goToNextCompletedPage(): void {
    if (this.completedPage() < this.completedTotalPages()) {
      this.completedPage.update((page) => page + 1);
    }
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
      next: (createdRecord) => {
        const safeRecord = this.enrichRecord(
          createdRecord && createdRecord.appointmentId
            ? createdRecord
            : this.buildLocalHealthRecord(appointment, request)
        );

        this.healthRecords.update((records) => [
          safeRecord,
          ...records.filter(
            (record) => Number(record.appointmentId) !== appointment.appointmentId
          )
        ]);

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

  openCompletedHealthRecord(appointment: Appointment): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    const record = this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'Health record is not available for this appointment yet.'
      );
      return;
    }

    this.selectedHealthRecord.set(record);
  }

  closeCompletedHealthRecord(): void {
    this.selectedHealthRecord.set(null);
  }

  printHealthRecordForAppointment(appointment: Appointment): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    const record = this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'Health record is not available for this appointment yet.'
      );
      return;
    }

    this.printHealthRecordPdf(record);
  }

  printHealthRecordPdf(record: HealthRecord): void {
    const printWindow = window.open('', '_blank', 'width=900,height=700');

    if (!printWindow) {
      this.errorMessage.set(
        'Please allow popups to download or print the health record.'
      );
      return;
    }

    const recordId = this.getRecordId(record);
    const patientName = this.escapeHtml(
      this.getSafeText(record.patientName, 'Patient')
    );
    const doctorName = this.escapeHtml(
      this.getSafeText(record.doctorName, 'Doctor')
    );
    const specialisation = this.escapeHtml(
      this.getSafeText(record.specialisation, 'Not assigned')
    );
    const visitDate = this.escapeHtml(
      new Date(record.visitDate).toLocaleDateString()
    );
    const diagnosis = this.escapeHtml(
      this.getSafeText(record.diagnosis, 'Diagnosis not provided.')
    );
    const prescription = this.escapeHtml(
      this.getSafeText(record.prescription, 'Prescription not provided.')
    );
    const notes = this.escapeHtml(
      this.getSafeText(record.notes, 'No additional notes.')
    );

    printWindow.document.open();

    printWindow.document.write(`
      <!DOCTYPE html>
      <html>
        <head>
          <title>Health Record #${recordId}</title>

          <style>
            body {
              margin: 0;
              background: #f8fafc;
              color: #0f172a;
              font-family: Arial, sans-serif;
            }

            .document {
              width: 800px;
              margin: 24px auto;
              background: #ffffff;
              border: 1px solid #e2e8f0;
              border-radius: 18px;
              padding: 32px;
            }

            .header {
              background: linear-gradient(135deg, #0f172a, #047857);
              color: #ffffff;
              padding: 24px;
              border-radius: 16px;
              margin-bottom: 24px;
            }

            .header h1 {
              margin: 0;
              font-size: 28px;
            }

            .header p {
              margin: 8px 0 0;
              color: #d1fae5;
            }

            .grid {
              display: grid;
              grid-template-columns: 1fr 1fr;
              gap: 14px;
            }

            .box {
              border: 1px solid #dbeafe;
              background: #f8fafc;
              border-radius: 14px;
              padding: 16px;
            }

            .box span {
              display: block;
              color: #64748b;
              font-size: 12px;
              font-weight: bold;
              margin-bottom: 7px;
            }

            .box strong {
              color: #0f172a;
              font-size: 16px;
            }

            .section {
              border: 1px solid #dbeafe;
              border-radius: 14px;
              padding: 16px;
              margin-top: 16px;
            }

            .section h2 {
              margin: 0 0 10px;
              font-size: 18px;
            }

            .section p {
              margin: 0;
              line-height: 1.7;
              white-space: pre-wrap;
            }

            .footer {
              margin-top: 24px;
              padding-top: 14px;
              border-top: 1px solid #e2e8f0;
              font-size: 12px;
              color: #64748b;
            }

            @media print {
              body {
                background: #ffffff;
              }

              .document {
                width: auto;
                margin: 0;
                border: none;
                border-radius: 0;
              }
            }
          </style>
        </head>

        <body>
          <main class="document">
            <section class="header">
              <h1>HealthAxis Medical Record</h1>
              <p>Doctor completed visit health record</p>
            </section>

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

            <section class="footer">
              This record is generated by HealthAxis Doctor Portal.
            </section>
          </main>

          <script>
            window.onload = function () {
              setTimeout(function () {
                window.print();
              }, 300);
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

  hasHealthRecord(appointment: Appointment): boolean {
    return Boolean(this.getHealthRecordForAppointment(appointment));
  }

  getHealthRecordForAppointment(
    appointment: Appointment
  ): HealthRecord | undefined {
    return this.healthRecords().find(
      (record) => Number(record.appointmentId) === appointment.appointmentId
    );
  }

  getRecordId(record: HealthRecord): number | string {
    const value = record as HealthRecord & {
      id?: number;
      healthRecordID?: number;
      recordID?: number;
    };

    return value.healthRecordId ??
      value.recordId ??
      value.id ??
      value.healthRecordID ??
      value.recordID ??
      '-';
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

  getSafeText(value: string | null | undefined, fallback: string): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  private enrichRecord(record: HealthRecord): HealthRecord {
    const appointment = this.appointments().find(
      (item) => item.appointmentId === Number(record.appointmentId)
    );

    if (!appointment) {
      return record;
    }

    return {
      ...record,
      patientId: record.patientId || appointment.patientId,
      patientName: record.patientName || appointment.patientName,
      doctorId: record.doctorId || appointment.doctorId,
      doctorName: record.doctorName || appointment.doctorName,
      specialisation: record.specialisation || appointment.specialisation
    };
  }

  private buildLocalHealthRecord(
    appointment: Appointment,
    request: CreateHealthRecordRequest
  ): HealthRecord {
    return {
      healthRecordId: appointment.appointmentId,
      recordId: appointment.appointmentId,
      appointmentId: appointment.appointmentId,
      patientId: appointment.patientId,
      patientName: appointment.patientName,
      doctorId: appointment.doctorId,
      doctorName: appointment.doctorName,
      specialisation: appointment.specialisation,
      visitDate: request.visitDate,
      diagnosis: request.diagnosis,
      prescription: request.prescription,
      notes: request.notes
    };
  }

  private getTotalPages(totalItems: number): number {
    const pages = Math.ceil(totalItems / this.pageSize);

    return pages > 0 ? pages : 1;
  }

  private matchesSearch(appointment: Appointment): boolean {
    const searchValue = this.searchText().trim().toLowerCase();

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

  private escapeHtml(value: string): string {
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