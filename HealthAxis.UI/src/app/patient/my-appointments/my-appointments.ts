import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';

import {
  Appointment,
  AppointmentStatusCode
} from '../../core/models/appointment.model';
import { HealthRecord } from '../../core/models/health-record.model';
import { AppointmentService } from '../../core/services/appointment.service';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const STATUS_FILTERS = [
  'All',
  'Pending',
  'Confirmed',
  'Completed',
  'Cancelled'
] as const;

type StatusFilter = typeof STATUS_FILTERS[number];

@Component({
  selector: 'app-my-appointments',
  imports: [DatePipe, RouterLink],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyAppointments {
  private readonly appointmentService = inject(AppointmentService);
  private readonly patientService = inject(PatientService);
  private readonly pageSize = 8;

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly loading = signal(false);
  readonly cancelling = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly searchText = signal('');
  readonly selectedStatus = signal<StatusFilter>('All');
  readonly currentPage = signal(1);

  readonly cancelTarget = signal<Appointment | null>(null);
  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly selectedHealthRecord = signal<HealthRecord | null>(null);
  readonly cancellationReason = signal('');

  readonly statusFilters = STATUS_FILTERS;

  readonly pendingCount = computed(() => this.countByStatus('Pending'));
  readonly confirmedCount = computed(() => this.countByStatus('Confirmed'));
  readonly completedCount = computed(() => this.countByStatus('Completed'));
  readonly cancelledCount = computed(() => this.countByStatus('Cancelled'));

  readonly filteredAppointments = computed(() => {
    const searchValue = this.searchText().trim().toLowerCase();
    const status = this.selectedStatus();

    return this.appointments()
      .filter((appointment) =>
        this.matchesFilter(appointment, status, searchValue)
      )
      .sort(
        (first, second) =>
          new Date(second.scheduledDate).getTime() -
          new Date(first.scheduledDate).getTime()
      );
  });

  readonly totalPages = computed(() => {
    const totalItems = this.filteredAppointments().length;
    const pages = Math.ceil(totalItems / this.pageSize);

    return pages > 0 ? pages : 1;
  });

  readonly pagedAppointments = computed(() => {
    const startIndex = (this.currentPage() - 1) * this.pageSize;

    return this.filteredAppointments().slice(
      startIndex,
      startIndex + this.pageSize
    );
  });

  readonly nextAppointment = computed<Appointment | null>(() => {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const appointment = this.appointments()
      .filter((item) => this.isUpcomingAppointment(item, today))
      .sort(
        (first, second) =>
          new Date(first.scheduledDate).getTime() -
          new Date(second.scheduledDate).getTime()
      )[0];

    return appointment ?? null;
  });

  constructor() {
    this.loadAppointments();
    this.loadHealthRecords();
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.getMyAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.currentPage.set(1);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load your appointments.')
        );
      }
    });
  }

  loadHealthRecords(): void {
    this.patientService.getMyHealthRecords().subscribe({
      next: (records) => {
        this.healthRecords.set(records);
      },
      error: () => {
        this.healthRecords.set([]);
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  onStatusChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    this.selectedStatus.set(select.value as StatusFilter);
    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedStatus.set('All');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update((page) => page - 1);
    }
  }

  goToNextPage(): void {
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update((page) => page + 1);
    }
  }

  onCancellationReasonInput(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;
    this.cancellationReason.set(textarea.value);
  }

  openAppointmentDetails(appointment: Appointment): void {
    this.selectedAppointment.set(appointment);
  }

  closeAppointmentDetails(): void {
    this.selectedAppointment.set(null);
  }

  openCancelDialog(appointment: Appointment): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.cancelTarget.set(appointment);
    this.cancellationReason.set('');
  }

  closeCancelDialog(): void {
    if (this.cancelling()) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
  }

  confirmCancelAppointment(): void {
    const appointment = this.cancelTarget();

    if (!appointment) {
      this.errorMessage.set('Please select an appointment to cancel.');
      return;
    }

    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        'Only future pending or confirmed appointments can be cancelled.'
      );
      return;
    }

    this.cancelling.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateAppointmentStatus(
      appointment.appointmentId,
      {
        status: AppointmentStatusCode.Cancelled,
        cancellationReason: this.getOptionalCancellationReason()
      }
    ).subscribe({
      next: () => {
        this.cancelling.set(false);
        this.cancelTarget.set(null);
        this.cancellationReason.set('');
        this.successMessage.set('Appointment cancelled successfully.');
        this.loadAppointments();
      },
      error: (error: unknown) => {
        this.cancelling.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not cancel appointment. Please try again.'
          )
        );
      }
    });
  }

  canCancel(appointment: Appointment): boolean {
    const status = appointment.status.toLowerCase();
    const isCancellableStatus = status === 'pending' || status === 'confirmed';

    return isCancellableStatus && !this.isPastAppointment(appointment);
  }

  getAppointmentStatusText(
    appointment: Appointment
  ): string {
    return appointment.status.trim();
  }

  getAppointmentStatusClass(
    appointment: Appointment
  ): string {
    return this.getStatusClass(
      appointment.status
    );
  }

  isPastPending(
    appointment: Appointment
  ): boolean {
    return (
      appointment.status
        .trim()
        .toLowerCase() === 'pending' &&
      this.isPastAppointment(appointment)
    );
  }

  canDownloadAppointmentPdf(appointment: Appointment): boolean {
    const status = appointment.status.toLowerCase();

    return status === 'pending' ||
      status === 'confirmed' ||
      status === 'completed';
  }

  canViewHealthRecord(appointment: Appointment): boolean {
    return appointment.status.toLowerCase() === 'completed';
  }

  getHealthRecordForAppointment(
    appointment: Appointment
  ): HealthRecord | undefined {
    return this.healthRecords().find(
      (record) => record.appointmentId === appointment.appointmentId
    );
  }

  hasHealthRecord(appointment: Appointment): boolean {
    return Boolean(this.getHealthRecordForAppointment(appointment));
  }

  openHealthRecordDetails(appointment: Appointment): void {
    const record = this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'Health record is not available yet for this completed appointment.'
      );
      return;
    }

    this.selectedAppointment.set(null);
    this.selectedHealthRecord.set(record);
  }

  closeHealthRecordDetails(): void {
    this.selectedHealthRecord.set(null);
  }

  getRecordId(record: HealthRecord): number {
    return record.healthRecordId ?? record.recordId ?? record.appointmentId;
  }

  getStatusClass(status: string): string {
    return `${status.trim().toLowerCase()}-badge`;
  }

  getDoctorDisplayName(name: string | null | undefined): string {
    const cleanName = (name ?? '').trim();

    if (!cleanName) {
      return 'Doctor not assigned';
    }

    if (
      cleanName.toLowerCase().startsWith('dr.') ||
      cleanName.toLowerCase().startsWith('dr ')
    ) {
      return cleanName;
    }

    return `Dr. ${cleanName}`;
  }

  getSafeText(value: string | null | undefined, fallback: string): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  printAppointmentPdf(appointment: Appointment): void {
    if (!this.canDownloadAppointmentPdf(appointment)) {
      this.errorMessage.set(
        'PDF is not available for cancelled appointments.'
      );
      return;
    }

    const appointmentId = appointment.appointmentId;

    const doctorName = this.escapeHtml(
      this.getDoctorDisplayName(appointment.doctorName)
    );

    const specialisation = this.escapeHtml(
      this.getSafeText(
        appointment.specialisation,
        'Not assigned'
      )
    );

    const appointmentDate = this.escapeHtml(
      new Date(
        appointment.scheduledDate
      ).toLocaleDateString()
    );

    const timeSlot = this.escapeHtml(
      this.getSafeText(
        appointment.timeSlot,
        'Not assigned'
      )
    );

    const status = this.escapeHtml(
      appointment.status || 'Pending'
    );

    const generatedOn = this.escapeHtml(
      new Date().toLocaleString()
    );

    const printContent = `
      <!DOCTYPE html>
      <html lang="en">
        <head>
          <meta charset="UTF-8" />

          <meta
            name="viewport"
            content="width=device-width, initial-scale=1.0"
          />

          <title>Appointment #${appointmentId}</title>

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
              background: linear-gradient(
                135deg,
                #0f172a,
                #0284c7
              );
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
              color: #dbeafe;
            }

            .status {
              display: inline-block;
              margin-top: 14px;
              padding: 8px 13px;
              border-radius: 999px;
              background: #dcfce7;
              color: #15803d;
              font-weight: bold;
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

            .note {
              margin-top: 22px;
              padding: 16px;
              border-radius: 14px;
              background: #eff6ff;
              color: #334155;
              line-height: 1.6;
              font-size: 14px;
            }

            .footer {
              margin-top: 24px;
              padding-top: 14px;
              border-top: 1px solid #e2e8f0;
              color: #64748b;
              font-size: 12px;
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
              <h1>HealthAxis Appointment Details</h1>
              <p>Patient booked appointment document</p>
              <span class="status">${status}</span>
            </section>

            <section class="grid">
              <div class="box">
                <span>Appointment ID</span>
                <strong>#${appointmentId}</strong>
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
                <span>Appointment Date</span>
                <strong>${appointmentDate}</strong>
              </div>

              <div class="box">
                <span>Time Slot</span>
                <strong>${timeSlot}</strong>
              </div>

              <div class="box">
                <span>Status</span>
                <strong>${status}</strong>
              </div>

              <div class="box">
                <span>Generated On</span>
                <strong>${generatedOn}</strong>
              </div>
            </section>

            <section class="note">
              Please check your appointment status before
              visiting the hospital. Completed appointments
              may have health records available in the
              Health Records section.
            </section>

            <section class="footer">
              This document is generated by HealthAxis
              Patient Portal.
            </section>
          </main>
        </body>
      </html>
    `;

    this.openPrintableDocument(
      printContent,
      'Please allow popups to download or print the appointment.'
    );
  }

  printHealthRecordForAppointment(appointment: Appointment): void {
    const record = this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'Health record is not available yet for this completed appointment.'
      );
      return;
    }

    this.printHealthRecordPdf(record);
  }

  printHealthRecordPdf(record: HealthRecord): void {
    const recordId = this.getRecordId(record);

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

    const specialisation = this.escapeHtml(
      this.getSafeText(
        record.specialisation,
        'Not assigned'
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
        'No prescription added.'
      )
    );

    const notes = this.escapeHtml(
      this.getSafeText(
        record.notes,
        'No doctor notes added.'
      )
    );

    const visitDate = this.escapeHtml(
      new Date(
        record.visitDate
      ).toLocaleDateString()
    );

    const printContent = `
      <!DOCTYPE html>
      <html lang="en">
        <head>
          <meta charset="UTF-8" />

          <meta
            name="viewport"
            content="width=device-width, initial-scale=1.0"
          />

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
              background: linear-gradient(
                135deg,
                #0f172a,
                #047857
              );
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
              <p>Patient health record document</p>
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
              This record is generated by HealthAxis.
              Please consult your doctor before changing
              any medication.
            </section>
          </main>
        </body>
      </html>
    `;

    this.openPrintableDocument(
      printContent,
      'Please allow popups to download or print the health record.'
    );
  }

  private openPrintableDocument(
    printContent: string,
    popupErrorMessage: string
  ): void {
    const printWindow = globalThis.open(
      '',
      '_blank',
      'width=900,height=700'
    );

    if (!printWindow) {
      this.errorMessage.set(
        popupErrorMessage
      );
      return;
    }

    const printDocumentBlob = new Blob(
      [printContent],
      {
        type: 'text/html;charset=utf-8'
      }
    );

    const printDocumentUrl =
      globalThis.URL.createObjectURL(
        printDocumentBlob
      );

    let isUrlRevoked = false;

    const revokePrintDocumentUrl = (): void => {
      if (isUrlRevoked) {
        return;
      }

      globalThis.URL.revokeObjectURL(
        printDocumentUrl
      );

      isUrlRevoked = true;
    };

    printWindow.addEventListener(
      'load',
      () => {
        printWindow.focus();

        globalThis.setTimeout(() => {
          printWindow.print();
        }, 300);

        globalThis.setTimeout(
          revokePrintDocumentUrl,
          60000
        );
      },
      {
        once: true
      }
    );

    printWindow.addEventListener(
      'afterprint',
      () => {
        revokePrintDocumentUrl();
        printWindow.close();
      },
      {
        once: true
      }
    );

    printWindow.location.href =
      printDocumentUrl;
  }

  private getOptionalCancellationReason(): string | null {
    const reason = this.cancellationReason().trim();

    return reason || null;
  }

  private countByStatus(status: string): number {
    return this.appointments().filter(
      (appointment) => appointment.status.toLowerCase() === status.toLowerCase()
    ).length;
  }

  private matchesFilter(
    appointment: Appointment,
    status: StatusFilter,
    searchValue: string
  ): boolean {
    const matchesStatus =
      status === 'All' ||
      appointment.status.toLowerCase() === status.toLowerCase();

    const searchableText = [
      appointment.doctorName,
      appointment.specialisation,
      appointment.status,
      appointment.timeSlot,
      appointment.scheduledDate,
      appointment.appointmentId.toString(),
      appointment.cancellationReason
    ]
      .join(' ')
      .toLowerCase();

    return matchesStatus && (!searchValue || searchableText.includes(searchValue));
  }
onAppointmentBackdropClick(event: MouseEvent): void {
  if (event.target === event.currentTarget) {
    this.closeAppointmentDetails();
  }
}

onHealthRecordBackdropClick(event: MouseEvent): void {
  if (event.target === event.currentTarget) {
    this.closeHealthRecordDetails();
  }
}
  private isPastAppointment(appointment: Appointment): boolean {
    const appointmentStart = this.getAppointmentStartDateTime(appointment);

    return appointmentStart.getTime() < Date.now();
  }

  private getAppointmentStartDateTime(appointment: Appointment): Date {
    const appointmentDate = new Date(appointment.scheduledDate);
    const timeParts = /^(\d{1,2}):(\d{2})\s*(AM|PM)$/i.exec(
      appointment.timeSlot.trim()
    );

    if (!timeParts) {
      appointmentDate.setHours(23, 59, 59, 999);
      return appointmentDate;
    }

    let hours = Number(timeParts[1]);
    const minutes = Number(timeParts[2]);
    const period = timeParts[3].toUpperCase();

    if (period === 'PM' && hours !== 12) {
      hours += 12;
    }

    if (period === 'AM' && hours === 12) {
      hours = 0;
    }

    appointmentDate.setHours(hours, minutes, 0, 0);
    return appointmentDate;
  }

  private isUpcomingAppointment(
    appointment: Appointment,
    today: Date
  ): boolean {
    const appointmentDate = new Date(appointment.scheduledDate);
    appointmentDate.setHours(0, 0, 0, 0);

    const status = appointment.status.toLowerCase();

    return appointmentDate >= today &&
      status !== 'cancelled' &&
      status !== 'completed';
  }

  private escapeHtml(value: string): string {
    return value
      .replaceAll('&', '&amp;')
      .replaceAll('<', '&lt;')
      .replaceAll('>', '&gt;')
      .replaceAll('"', '&quot;')
      .replaceAll("'", '&#039;');
  }
}