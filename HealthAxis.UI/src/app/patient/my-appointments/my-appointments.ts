import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  computed,
  inject,
  signal
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

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

const DATE_FILTERS = [
  'All Dates',
  'This Week',
  'Custom'
] as const;

const PAGE_SIZE = 8;
const PRINT_DELAY_IN_MS = 350;
const PRINT_WINDOW_FEATURES = 'width=900,height=700';
const CONFIRMED_CANCELLATION_CUTOFF_IN_MS =
  2 * 60 * 60 * 1000;

const CANCELLED_BY_PATIENT =
  'Cancelled by patient';

const CANCELLATION_REASON_MARKER =
  'Reason:';

const PENDING_STATUS = 'pending';
const CONFIRMED_STATUS = 'confirmed';
const COMPLETED_STATUS = 'completed';
const CANCELLED_STATUS = 'cancelled';

type StatusFilter = typeof STATUS_FILTERS[number];
type DateFilter = typeof DATE_FILTERS[number];

@Component({
  selector: 'app-my-appointments',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MyAppointments {
  private readonly appointmentService =
    inject(AppointmentService);

  private readonly patientService =
    inject(PatientService);

  private readonly activatedRoute =
    inject(ActivatedRoute);

  private readonly destroyRef =
    inject(DestroyRef);

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly loading = signal(false);
  readonly cancelling = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly cancelErrorMessage = signal('');

  readonly searchText = signal('');

  readonly selectedStatus =
    signal<StatusFilter>('All');

  readonly selectedDateFilter =
    signal<DateFilter>('All Dates');

  readonly customStartDate = signal('');
  readonly customEndDate = signal('');

  readonly currentPage = signal(1);

  readonly cancelTarget =
    signal<Appointment | null>(null);

  readonly selectedAppointment =
    signal<Appointment | null>(null);

  readonly selectedHealthRecord =
    signal<HealthRecord | null>(null);

  readonly cancellationReason = signal('');

  readonly statusFilters = STATUS_FILTERS;
  readonly dateFilters = DATE_FILTERS;

  readonly customDateRangeError = computed(() => {
    const startDate = this.parseLocalDate(
      this.customStartDate()
    );

    const endDate = this.parseLocalDate(
      this.customEndDate()
    );

    if (
      startDate &&
      endDate &&
      startDate.getTime() > endDate.getTime()
    ) {
      return 'Start date cannot be after end date.';
    }

    return '';
  });

  readonly pendingCount = computed(() =>
    this.countByStatus(PENDING_STATUS)
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus(CONFIRMED_STATUS)
  );

  readonly completedCount = computed(() =>
    this.countByStatus(COMPLETED_STATUS)
  );

  readonly cancelledCount = computed(() =>
    this.countByStatus(CANCELLED_STATUS)
  );

  readonly filteredAppointments = computed(() => {
    const searchValue = this.searchText()
      .trim()
      .toLowerCase();

    const selectedStatus = this.selectedStatus();

    return this.appointments()
      .filter((appointment) =>
        this.matchesFilter(
          appointment,
          selectedStatus,
          searchValue
        ) &&
        this.matchesDateFilter(appointment)
      )
      .sort(
        (first, second) =>
          this.getAppointmentDateValue(second) -
          this.getAppointmentDateValue(first)
      );
  });

  readonly totalPages = computed(() => {
    const totalItems =
      this.filteredAppointments().length;

    const pageCount =
      Math.ceil(totalItems / PAGE_SIZE);

    return Math.max(pageCount, 1);
  });

  readonly pagedAppointments = computed(() => {
    const startIndex =
      (this.currentPage() - 1) * PAGE_SIZE;

    return this.filteredAppointments().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly nextAppointment =
    computed<Appointment | null>(() => {
      const currentTime = Date.now();

      const appointment = this.appointments()
        .filter((item) =>
          this.isUpcomingAppointment(
            item,
            currentTime
          )
        )
        .sort(
          (first, second) =>
            this.getAppointmentDateValue(first) -
            this.getAppointmentDateValue(second)
        )[0];

      return appointment ?? null;
    });

  constructor() {
    this.applyStatusFilterFromRoute();
    this.loadAppointments();
    this.loadHealthRecords();
  }

  private applyStatusFilterFromRoute(): void {
    this.activatedRoute.queryParamMap
      .pipe(
        takeUntilDestroyed(
          this.destroyRef
        )
      )
      .subscribe((parameters) => {
        const statusParameter =
          parameters.get('status')
            ?.trim()
            .toLowerCase();

        const matchingStatus =
          STATUS_FILTERS.find(
            (status) =>
              status.toLowerCase() ===
              statusParameter
          ) ?? 'All';

        this.selectedStatus.set(
          matchingStatus
        );

        this.currentPage.set(1);
      });
  }

  loadAppointments(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService
      .getMyAppointments()
      .subscribe({
        next: (appointments) => {
          this.appointments.set(appointments);
          this.currentPage.set(1);
          this.loading.set(false);
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not load your appointments.'
            )
          );
        }
      });
  }

  loadHealthRecords(): void {
    this.patientService
      .getMyHealthRecords()
      .subscribe({
        next: (records) => {
          this.healthRecords.set(records);
        },
        error: () => {
          this.healthRecords.set([]);
        }
      });
  }

  onSearchInput(event: Event): void {
    const input =
      event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  onStatusChange(event: Event): void {
    const select =
      event.target as HTMLSelectElement;

    this.selectedStatus.set(
      select.value as StatusFilter
    );

    this.currentPage.set(1);
  }

  onDateFilterChange(event: Event): void {
    const select =
      event.target as HTMLSelectElement;

    this.selectedDateFilter.set(
      select.value as DateFilter
    );

    if (select.value !== 'Custom') {
      this.customStartDate.set('');
      this.customEndDate.set('');
    }

    this.currentPage.set(1);
  }

  onCustomStartDateChange(event: Event): void {
    const input =
      event.target as HTMLInputElement;

    this.customStartDate.set(input.value);
    this.currentPage.set(1);
  }

  onCustomEndDateChange(event: Event): void {
    const input =
      event.target as HTMLInputElement;

    this.customEndDate.set(input.value);
    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedStatus.set('All');
    this.selectedDateFilter.set('All Dates');
    this.customStartDate.set('');
    this.customEndDate.set('');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() > 1) {
      this.currentPage.update(
        (page) => page - 1
      );
    }
  }

  goToNextPage(): void {
    if (
      this.currentPage() <
      this.totalPages()
    ) {
      this.currentPage.update(
        (page) => page + 1
      );
    }
  }

  onCancellationReasonInput(
    event: Event
  ): void {
    const textarea =
      event.target as HTMLTextAreaElement;

    this.cancellationReason.set(
      textarea.value
    );
  }

  openAppointmentDetails(
    appointment: Appointment
  ): void {
    this.selectedAppointment.set(
      appointment
    );
  }

  closeAppointmentDetails(): void {
    this.selectedAppointment.set(null);
  }

  onAppointmentBackdropClick(
    event: MouseEvent
  ): void {
    if (
      event.target ===
      event.currentTarget
    ) {
      this.closeAppointmentDetails();
    }
  }

  openCancelDialog(
    appointment: Appointment
  ): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.cancelErrorMessage.set('');

    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        'This appointment can no longer be cancelled.'
      );

      return;
    }

    this.cancelTarget.set(appointment);
    this.cancellationReason.set('');
  }

  closeCancelDialog(): void {
    if (this.cancelling()) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
    this.cancelErrorMessage.set('');
  }

  confirmCancelAppointment(): void {
    const appointment =
      this.cancelTarget();

    if (!appointment) {
      this.cancelErrorMessage.set(
        'Please select an appointment to cancel.'
      );

      return;
    }

    if (!this.canCancel(appointment)) {
      this.cancelErrorMessage.set(
        'This appointment can no longer be cancelled.'
      );

      return;
    }

    const cancellationReason =
      this.getCancellationReason();

    this.cancelling.set(true);
    this.cancelErrorMessage.set('');
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService
      .updateAppointmentStatus(
        appointment.appointmentId,
        {
          status:
            AppointmentStatusCode.Cancelled,

          cancellationReason
        }
      )
      .subscribe({
        next: (updatedAppointment) => {
          this.cancelling.set(false);
          this.cancelTarget.set(null);
          this.cancellationReason.set('');
          this.cancelErrorMessage.set('');

          this.replaceAppointment(
            updatedAppointment
          );

          this.successMessage.set(
            'Appointment cancelled successfully.'
          );

          this.adjustCurrentPage();
        },
        error: (error: unknown) => {
          this.cancelling.set(false);

          this.cancelErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not cancel appointment. Please try again.'
            )
          );
        }
      });
  }

  canCancel(
    appointment: Appointment
  ): boolean {
    const status =
      this.normalizeStatus(
        appointment.status
      );

    const appointmentStart =
      this.getAppointmentStartDateTime(
        appointment
      );

    if (!appointmentStart) {
      return false;
    }

    const currentTime = Date.now();
    const appointmentStartTime =
      appointmentStart.getTime();

    if (status === PENDING_STATUS) {
      return appointmentStartTime >
        currentTime;
    }

    if (status === CONFIRMED_STATUS) {
      return appointmentStartTime >
        currentTime +
          CONFIRMED_CANCELLATION_CUTOFF_IN_MS;
    }

    return false;
  }

  getAppointmentStatusText(
    appointment: Appointment
  ): string {
    return this.getSafeText(
      appointment.status,
      'Pending'
    );
  }

  getAppointmentStatusClass(
    appointment: Appointment
  ): string {
    return this.getStatusClass(
      appointment.status
    );
  }

  getStatusClass(status: string): string {
    const normalizedStatus =
      this.normalizeStatus(status);

    return `${
      normalizedStatus || PENDING_STATUS
    }-badge`;
  }

  isPastPending(
    appointment: Appointment
  ): boolean {
    return (
      this.normalizeStatus(
        appointment.status
      ) === PENDING_STATUS &&
      this.isPastAppointment(appointment)
    );
  }

  canDownloadAppointmentPdf(
    appointment: Appointment
  ): boolean {
    const status =
      this.normalizeStatus(
        appointment.status
      );

    return (
      status === PENDING_STATUS ||
      status === CONFIRMED_STATUS ||
      status === COMPLETED_STATUS
    );
  }

  canViewHealthRecord(
    appointment: Appointment
  ): boolean {
    return (
      this.normalizeStatus(
        appointment.status
      ) === COMPLETED_STATUS
    );
  }

  getHealthRecordForAppointment(
    appointment: Appointment
  ): HealthRecord | undefined {
    return this.healthRecords().find(
      (record) =>
        record.appointmentId ===
        appointment.appointmentId
    );
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

  openHealthRecordDetails(
    appointment: Appointment
  ): void {
    const record =
      this.getHealthRecordForAppointment(
        appointment
      );

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

  onHealthRecordBackdropClick(
    event: MouseEvent
  ): void {
    if (
      event.target ===
      event.currentTarget
    ) {
      this.closeHealthRecordDetails();
    }
  }

  getRecordId(
    record: HealthRecord
  ): number {
    return (
      record.healthRecordId ??
      record.recordId ??
      record.appointmentId
    );
  }

  getDoctorDisplayName(
    name: string | null | undefined
  ): string {
    const cleanName =
      (name ?? '').trim();

    const lowerName =
      cleanName.toLowerCase();

    if (!cleanName) {
      return 'Doctor not assigned';
    }

    if (
      lowerName.startsWith('dr.') ||
      lowerName.startsWith('dr ')
    ) {
      return cleanName;
    }

    return `Dr. ${cleanName}`;
  }

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    return value?.trim() || fallback;
  }

  printAppointmentPdf(
    appointment: Appointment
  ): void {
    if (
      !this.canDownloadAppointmentPdf(
        appointment
      )
    ) {
      this.errorMessage.set(
        'PDF is not available for cancelled appointments.'
      );
      return;
    }

    const appointmentId =
      appointment.appointmentId;

    const doctorName = this.escapeHtml(
      this.getDoctorDisplayName(
        appointment.doctorName
      )
    );

    const specialisation =
      this.escapeHtml(
        this.getSafeText(
          appointment.specialisation,
          'Not assigned'
        )
      );

    const appointmentDate =
      this.escapeHtml(
        this.formatDate(
          appointment.scheduledDate
        )
      );

    const timeSlot = this.escapeHtml(
      this.getSafeText(
        appointment.timeSlot,
        'Not assigned'
      )
    );

    const status = this.escapeHtml(
      this.getSafeText(
        appointment.status,
        'Pending'
      )
    );

    const generatedOn =
      this.escapeHtml(
        this.formatDateTime(
          new Date()
        )
      );

    const documentContent = `
      <header class="header appointment-header">
        <h1>
          HealthAxis Appointment Details
        </h1>

        <p>
          Patient booked appointment document
        </p>

        <span class="status">
          ${status}
        </span>
      </header>

      <section class="grid">
        ${this.createDetailBox(
          'Appointment ID',
          `#${appointmentId}`
        )}

        ${this.createDetailBox(
          'Doctor Name',
          doctorName,
          false
        )}

        ${this.createDetailBox(
          'Specialisation',
          specialisation,
          false
        )}

        ${this.createDetailBox(
          'Appointment Date',
          appointmentDate,
          false
        )}

        ${this.createDetailBox(
          'Time Slot',
          timeSlot,
          false
        )}

        ${this.createDetailBox(
          'Status',
          status,
          false
        )}

        ${this.createDetailBox(
          'Generated On',
          generatedOn,
          false
        )}
      </section>

      <section class="note">
        Please check your appointment status
        before visiting the hospital.
        Completed appointments may have health
        records available in the Health Records
        section.
      </section>

      <footer class="footer">
        This document is generated by
        HealthAxis Patient Portal.
      </footer>
    `;

    const printableDocument =
      this.createPrintableDocument(
        `Appointment #${appointmentId}`,
        documentContent
      );

    this.openPrintableDocument(
      printableDocument,
      'Please allow popups to download or print the appointment.'
    );
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
        'Health record is not available yet for this completed appointment.'
      );
      return;
    }

    this.printHealthRecordPdf(record);
  }

  printHealthRecordPdf(
    record: HealthRecord
  ): void {
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

    const diagnosis = this.escapeHtml(
      this.getSafeText(
        record.diagnosis,
        'Diagnosis not provided.'
      )
    );

    const prescription =
      this.escapeHtml(
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
      this.formatDate(
        record.visitDate
      )
    );

    const documentContent = `
      <header class="header record-header">
        <h1>
          HealthAxis Medical Record
        </h1>

        <p>
          Patient health record document
        </p>
      </header>

      <section class="grid">
        ${this.createDetailBox(
          'Record ID',
          `#${recordId}`
        )}

        ${this.createDetailBox(
          'Appointment ID',
          `#${record.appointmentId}`
        )}

        ${this.createDetailBox(
          'Patient Name',
          patientName,
          false
        )}

        ${this.createDetailBox(
          'Doctor Name',
          doctorName,
          false
        )}

        ${this.createDetailBox(
          'Specialisation',
          specialisation,
          false
        )}

        ${this.createDetailBox(
          'Visit Date',
          visitDate,
          false
        )}
      </section>

      ${this.createRecordSection(
        'Diagnosis',
        diagnosis,
        'diagnosis-section'
      )}

      ${this.createRecordSection(
        'Prescription',
        prescription,
        'prescription-section'
      )}

      ${this.createRecordSection(
        'Doctor Notes',
        notes,
        'notes-section'
      )}

      <footer class="footer">
        This record is generated by HealthAxis.
        Please consult your doctor before
        changing any medication.
      </footer>
    `;

    const printableDocument =
      this.createPrintableDocument(
        `Health Record #${recordId}`,
        documentContent
      );

    this.openPrintableDocument(
      printableDocument,
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
      PRINT_WINDOW_FEATURES
    );

    if (!printWindow) {
      this.errorMessage.set(
        popupErrorMessage
      );

      return;
    }

    printWindow.document.open();
    printWindow.document.write(
      printContent
    );
    printWindow.document.close();

    printWindow.onafterprint = (): void => {
      printWindow.close();
    };

    printWindow.focus();

    globalThis.setTimeout(() => {
      if (!printWindow.closed) {
        printWindow.print();
      }
    }, PRINT_DELAY_IN_MS);
  }

  private createPrintableDocument(
    title: string,
    content: string
  ): string {
    return `
      <!doctype html>

      <html lang="en">
        <head>
          <meta charset="utf-8">

          <meta
            name="viewport"
            content="width=device-width, initial-scale=1"
          >

          <title>
            ${this.escapeHtml(title)}
          </title>

          <style>
            ${this.getPrintableStyles()}
          </style>
        </head>

        <body>
          <main class="document">
            ${content}
          </main>
        </body>
      </html>
    `;
  }

  private createDetailBox(
    label: string,
    value: string,
    escapeValue = true
  ): string {
    const displayValue = escapeValue
      ? this.escapeHtml(value)
      : value;

    return `
      <div class="box">
        <span>
          ${this.escapeHtml(label)}
        </span>

        <strong>
          ${displayValue}
        </strong>
      </div>
    `;
  }

  private createRecordSection(
    title: string,
    content: string,
    className: string
  ): string {
    return `
      <section class="section ${className}">
        <h2>
          ${this.escapeHtml(title)}
        </h2>

        <p>${content}</p>
      </section>
    `;
  }

  private getPrintableStyles(): string {
    return `
      * {
        box-sizing: border-box;
      }

      html,
      body {
        margin: 0;
        padding: 0;
      }

      body {
        padding: 24px;
        color: #0f172a;
        background: #f8fafc;
        font-family:
          Arial,
          Helvetica,
          sans-serif;
      }

      .document {
        width: 800px;
        max-width: 100%;
        margin: 0 auto;
        padding: 30px;
        border: 1px solid #e2e8f0;
        border-radius: 18px;
        background: #ffffff;
      }

      .header {
        margin-bottom: 22px;
        padding: 23px;
        border-radius: 15px;
        color: #ffffff;
      }

      .appointment-header {
        background:
          linear-gradient(
            135deg,
            #0f172a,
            #0284c7
          );
      }

      .record-header {
        background:
          linear-gradient(
            135deg,
            #0f172a,
            #047857
          );
      }

      .header h1 {
        margin: 0;
        font-size: 27px;
        line-height: 1.2;
      }

      .header p {
        margin: 8px 0 0;
        color: #dbeafe;
        font-size: 14px;
      }

      .status {
        display: inline-block;
        margin-top: 13px;
        padding: 7px 12px;
        border-radius: 999px;
        color: #15803d;
        background: #dcfce7;
        font-size: 13px;
        font-weight: 700;
      }

      .grid {
        display: grid;
        grid-template-columns:
          repeat(2, minmax(0, 1fr));
        gap: 12px;
      }

      .box {
        min-height: 76px;
        padding: 14px;
        border: 1px solid #dbeafe;
        border-radius: 12px;
        background: #f8fafc;
        break-inside: avoid;
      }

      .box span {
        display: block;
        margin-bottom: 7px;
        color: #64748b;
        font-size: 11px;
        font-weight: 700;
        text-transform: uppercase;
      }

      .box strong {
        color: #0f172a;
        font-size: 14px;
        line-height: 1.4;
        overflow-wrap: anywhere;
      }

      .section {
        margin-top: 15px;
        padding: 16px;
        border: 1px solid #dbeafe;
        border-radius: 13px;
        break-inside: avoid;
      }

      .section h2 {
        margin: 0 0 9px;
        font-size: 17px;
      }

      .section p {
        margin: 0;
        color: #334155;
        font-size: 14px;
        line-height: 1.7;
        white-space: pre-wrap;
        overflow-wrap: anywhere;
      }

      .diagnosis-section {
        background: #fffaf0;
      }

      .prescription-section {
        background: #eff6ff;
      }

      .notes-section {
        background: #ecfdf5;
      }

      .note {
        margin-top: 20px;
        padding: 15px;
        border-radius: 12px;
        color: #334155;
        background: #eff6ff;
        font-size: 14px;
        line-height: 1.6;
      }

      .footer {
        margin-top: 22px;
        padding-top: 13px;
        border-top: 1px solid #e2e8f0;
        color: #64748b;
        font-size: 12px;
        line-height: 1.6;
      }

      @page {
        size: A4 portrait;
        margin: 12mm;
      }

      @media print {
        html,
        body {
          background: #ffffff;
        }

        body {
          padding: 0;
        }

        .document {
          width: 100%;
          max-width: none;
          margin: 0;
          padding: 0;
          border: none;
          border-radius: 0;
        }

        .header,
        .status,
        .box,
        .section,
        .note {
          print-color-adjust: exact;
          -webkit-print-color-adjust: exact;
        }
      }
    `;
  }

  private getCancellationReason(): string {
    const reason =
      this.cancellationReason().trim();

    if (!reason) {
      return CANCELLED_BY_PATIENT;
    }

    return `${CANCELLED_BY_PATIENT}. ` +
      `${CANCELLATION_REASON_MARKER} ${reason}`;
  }

  private replaceAppointment(
    updatedAppointment: Appointment
  ): void {
    this.appointments.update(
      (currentAppointments) =>
        currentAppointments.map(
          (appointment) =>
            appointment.appointmentId ===
              updatedAppointment.appointmentId
              ? updatedAppointment
              : appointment
        )
    );

    this.selectedAppointment.update(
      (appointment) =>
        appointment?.appointmentId ===
          updatedAppointment.appointmentId
          ? updatedAppointment
          : appointment
    );
  }

  private adjustCurrentPage(): void {
    this.currentPage.set(
      Math.min(
        this.currentPage(),
        this.totalPages()
      )
    );
  }

  private countByStatus(
    status: string
  ): number {
    return this.appointments()
      .filter((appointment) => {
        const normalizedStatus =
          this.normalizeStatus(
            appointment.status
          );

        if (normalizedStatus !== status) {
          return false;
        }

        if (status === PENDING_STATUS) {
          return !this.isPastAppointment(
            appointment
          );
        }

        return true;
      })
      .length;
  }

  private matchesFilter(
    appointment: Appointment,
    status: StatusFilter,
    searchValue: string
  ): boolean {
    const normalizedStatus =
      this.normalizeStatus(
        appointment.status
      );

    const statusMatches =
      status === 'All' ||
      normalizedStatus ===
        status.toLowerCase();

    const excludesPastPending =
      status === 'Pending' &&
      this.isPastAppointment(
        appointment
      );

    const matchesStatus =
      statusMatches &&
      !excludesPastPending;

    const searchableText = [
      appointment.doctorName,
      appointment.specialisation,
      appointment.status,
      appointment.timeSlot,
      appointment.scheduledDate,
      appointment.appointmentId.toString(),
      appointment.cancellationReason
    ]
      .filter(
        (value): value is string =>
          typeof value === 'string'
      )
      .join(' ')
      .toLowerCase();

    return (
      matchesStatus &&
      (
        !searchValue ||
        searchableText.includes(
          searchValue
        )
      )
    );
  }

  private matchesDateFilter(
    appointment: Appointment
  ): boolean {
    const dateFilter =
      this.selectedDateFilter();

    if (dateFilter === 'All Dates') {
      return true;
    }

    const appointmentDate =
      this.parseLocalDate(
        appointment.scheduledDate
      );

    if (!appointmentDate) {
      return false;
    }

    appointmentDate.setHours(0, 0, 0, 0);

    if (dateFilter === 'This Week') {
      const weekStart =
        this.getCurrentWeekStart();

      const weekEnd =
        new Date(weekStart);

      weekEnd.setDate(
        weekEnd.getDate() + 6
      );

      weekEnd.setHours(23, 59, 59, 999);

      return (
        appointmentDate >= weekStart &&
        appointmentDate <= weekEnd
      );
    }

    const startDate =
      this.parseLocalDate(
        this.customStartDate()
      );

    const endDate =
      this.parseLocalDate(
        this.customEndDate()
      );

    if (
      startDate &&
      endDate &&
      startDate > endDate
    ) {
      return true;
    }

    if (
      startDate &&
      appointmentDate < startDate
    ) {
      return false;
    }

    if (endDate) {
      endDate.setHours(23, 59, 59, 999);

      if (appointmentDate > endDate) {
        return false;
      }
    }

    return true;
  }

  private getCurrentWeekStart(): Date {
    const today = new Date();
    const dayOfWeek = today.getDay();
    const daysSinceMonday =
      dayOfWeek === 0
        ? 6
        : dayOfWeek - 1;

    today.setDate(
      today.getDate() - daysSinceMonday
    );

    today.setHours(0, 0, 0, 0);

    return today;
  }

  private isPastAppointment(
    appointment: Appointment
  ): boolean {
    const appointmentStart =
      this.getAppointmentStartDateTime(
        appointment
      );

    return !appointmentStart ||
      appointmentStart.getTime() <=
        Date.now();
  }

  private getAppointmentStartDateTime(
    appointment: Appointment
  ): Date | null {
    const appointmentDate =
      this.parseLocalDate(
        appointment.scheduledDate
      );

    if (!appointmentDate) {
      return null;
    }

    const timeParts =
      /^(\d{1,2}):(\d{2})\s*(AM|PM)/i.exec(
        appointment.timeSlot.trim()
      );

    if (!timeParts) {
      appointmentDate.setHours(
        23,
        59,
        59,
        999
      );

      return appointmentDate;
    }

    let hours = Number(timeParts[1]);
    const minutes = Number(timeParts[2]);
    const period =
      timeParts[3].toUpperCase();

    if (
      hours < 1 ||
      hours > 12 ||
      minutes < 0 ||
      minutes > 59
    ) {
      return null;
    }

    if (
      period === 'PM' &&
      hours !== 12
    ) {
      hours += 12;
    }

    if (
      period === 'AM' &&
      hours === 12
    ) {
      hours = 0;
    }

    appointmentDate.setHours(
      hours,
      minutes,
      0,
      0
    );

    return appointmentDate;
  }

  private isUpcomingAppointment(
    appointment: Appointment,
    currentTime: number
  ): boolean {
    const status =
      this.normalizeStatus(
        appointment.status
      );

    const isActiveStatus =
      status === PENDING_STATUS ||
      status === CONFIRMED_STATUS;

    if (!isActiveStatus) {
      return false;
    }

    const appointmentStart =
      this.getAppointmentStartDateTime(
        appointment
      );

    return Boolean(
      appointmentStart &&
      appointmentStart.getTime() >=
        currentTime
    );
  }

  private getAppointmentDateValue(
    appointment: Appointment
  ): number {
    return this
      .getAppointmentStartDateTime(
        appointment
      )
      ?.getTime() ?? 0;
  }

  private parseLocalDate(
    value: string | null | undefined
  ): Date | null {
    if (!value) {
      return null;
    }

    const dateParts =
      /^(\d{4})-(\d{2})-(\d{2})/.exec(
        value.trim()
      );

    if (!dateParts) {
      const parsedDate =
        new Date(value);

      return Number.isNaN(
        parsedDate.getTime()
      )
        ? null
        : parsedDate;
    }

    const year = Number(dateParts[1]);
    const month = Number(dateParts[2]);
    const day = Number(dateParts[3]);

    const parsedDate =
      new Date(year, month - 1, day);

    if (
      parsedDate.getFullYear() !== year ||
      parsedDate.getMonth() !==
        month - 1 ||
      parsedDate.getDate() !== day
    ) {
      return null;
    }

    return parsedDate;
  }

  private normalizeStatus(
    status: string | null | undefined
  ): string {
    return status
      ?.trim()
      .toLowerCase() ?? '';
  }

  private formatDate(
    value: string | Date | null | undefined
  ): string {
    const date = this.parseDate(value);

    if (!date) {
      return 'Not available';
    }

    return new Intl.DateTimeFormat(
      'en-IN',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
      }
    ).format(date);
  }

  private formatDateTime(
    value: string | Date | null | undefined
  ): string {
    const date = this.parseDate(value);

    if (!date) {
      return 'Not available';
    }

    return new Intl.DateTimeFormat(
      'en-IN',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: true
      }
    ).format(date);
  }

  private parseDate(
    value: string | Date | null | undefined
  ): Date | null {
    if (!value) {
      return null;
    }

    const date =
      value instanceof Date
        ? value
        : this.parseLocalDate(value);

    if (!date) {
      return null;
    }

    return Number.isNaN(
      date.getTime()
    )
      ? null
      : date;
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
}