import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

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

const PAGE_SIZE = 8;
const PRINT_DELAY_IN_MS = 250;

type StatusFilter = typeof STATUS_FILTERS[number];

interface PrintDetail {
  label: string;
  value: string;
}

interface PrintSection {
  title: string;
  value: string;
}

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
  private readonly appointmentService = inject(AppointmentService);
  private readonly patientService = inject(PatientService);
  private readonly route = inject(ActivatedRoute);

  readonly appointments = signal<Appointment[]>([]);
  readonly healthRecords = signal<HealthRecord[]>([]);

  readonly loading = signal(false);
  readonly cancelling = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly searchText = signal('');
  readonly selectedStatus = signal<StatusFilter>('All');
  readonly currentPage = signal(1);

  readonly selectedAppointment = signal<Appointment | null>(null);
  readonly selectedHealthRecord = signal<HealthRecord | null>(null);
  readonly cancelTarget = signal<Appointment | null>(null);
  readonly cancellationReason = signal('');

  readonly statusFilters = STATUS_FILTERS;

  readonly pendingCount = computed(() =>
    this.countByStatus('Pending')
  );

  readonly confirmedCount = computed(() =>
    this.countByStatus('Confirmed')
  );

  readonly completedCount = computed(() =>
    this.countByStatus('Completed')
  );

  readonly cancelledCount = computed(() =>
    this.countByStatus('Cancelled')
  );

  readonly filteredAppointments = computed(() => {
    const status = this.selectedStatus();
    const searchValue = this.searchText()
      .trim()
      .toLowerCase();

    return this.appointments()
      .filter((appointment) =>
        this.matchesFilter(
          appointment,
          status,
          searchValue
        )
      )
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(second).getTime() -
          this.getAppointmentStartDateTime(first).getTime()
      );
  });

  readonly totalPages = computed(() => {
    const totalItems = this.filteredAppointments().length;
    const pages = Math.ceil(totalItems / PAGE_SIZE);

    return Math.max(pages, 1);
  });

  readonly pagedAppointments = computed(() => {
    const startIndex =
      (this.currentPage() - 1) * PAGE_SIZE;

    return this.filteredAppointments().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly nextAppointment = computed<Appointment | null>(() => {
    const appointment = this.appointments()
      .filter((item) => this.isUpcomingAppointment(item))
      .sort(
        (first, second) =>
          this.getAppointmentStartDateTime(first).getTime() -
          this.getAppointmentStartDateTime(second).getTime()
      )[0];

    return appointment ?? null;
  });

  constructor() {
    this.applyStatusFromQuery();
    this.loadAppointments();
    this.loadHealthRecords();
  }

  loadAppointments(clearMessages = true): void {
    this.loading.set(true);

    if (clearMessages) {
      this.errorMessage.set('');
      this.successMessage.set('');
    }

    this.appointmentService.getMyAppointments().subscribe({
      next: (appointments) => {
        this.appointments.set(appointments);
        this.ensureCurrentPageIsValid();
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
    this.patientService.getMyHealthRecords().subscribe({
      next: (records) => {
        this.healthRecords.set(records);
      },
      error: () => {
        /*
         * The appointments page should continue working even when
         * health records cannot be loaded.
         */
        this.healthRecords.set([]);
      }
    });
  }

  selectStatus(status: StatusFilter): void {
    this.selectedStatus.set(status);
    this.currentPage.set(1);
  }

  isStatusSelected(status: StatusFilter): boolean {
    return this.selectedStatus() === status;
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  onStatusChange(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.selectedStatus.set(
      select.value as StatusFilter
    );

    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedStatus.set('All');
    this.currentPage.set(1);
  }

  goToPreviousPage(): void {
    if (this.currentPage() <= 1) {
      return;
    }

    this.currentPage.update((page) => page - 1);
  }

  goToNextPage(): void {
    if (this.currentPage() >= this.totalPages()) {
      return;
    }

    this.currentPage.update((page) => page + 1);
  }

  openAppointmentDetails(
    appointment: Appointment
  ): void {
    this.selectedAppointment.set(appointment);
  }

  closeAppointmentDetails(): void {
    this.selectedAppointment.set(null);
  }

  openCancelDialog(appointment: Appointment): void {
    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        'Only future pending or confirmed appointments can be cancelled.'
      );

      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.cancellationReason.set('');
    this.cancelTarget.set(appointment);
  }

  closeCancelDialog(): void {
    if (this.cancelling()) {
      return;
    }

    this.cancelTarget.set(null);
    this.cancellationReason.set('');
  }

  onCancellationReasonInput(event: Event): void {
    const textarea = event.target as HTMLTextAreaElement;

    this.cancellationReason.set(textarea.value);
  }

  confirmCancelAppointment(): void {
    const appointment = this.cancelTarget();

    if (!appointment) {
      this.errorMessage.set(
        'Please select an appointment to cancel.'
      );

      return;
    }

    if (!this.canCancel(appointment)) {
      this.errorMessage.set(
        'This appointment can no longer be cancelled.'
      );

      this.cancelTarget.set(null);
      return;
    }

    this.cancelling.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.updateAppointmentStatus(
      appointment.appointmentId,
      {
        status: AppointmentStatusCode.Cancelled,
        cancellationReason:
          this.getOptionalCancellationReason()
      }
    ).subscribe({
      next: () => {
        this.cancelling.set(false);
        this.cancelTarget.set(null);
        this.cancellationReason.set('');

        this.successMessage.set(
          'Appointment cancelled successfully.'
        );

        this.loadAppointments(false);
      },
      error: (error: unknown) => {
        this.cancelling.set(false);

        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not cancel the appointment. Please try again.'
          )
        );
      }
    });
  }

  canCancel(appointment: Appointment): boolean {
    const status = this.normalizeStatus(
      appointment.status
    );

    const isCancellableStatus =
      status === 'pending' ||
      status === 'confirmed';

    return isCancellableStatus &&
      !this.isPastAppointment(appointment);
  }

  isCancelled(appointment: Appointment): boolean {
    return this.normalizeStatus(
      appointment.status
    ) === 'cancelled';
  }

  getAppointmentStatusText(
    appointment: Appointment
  ): string {
    const status = appointment.status.trim();

    if (
      this.normalizeStatus(status) === 'pending' &&
      this.isPastAppointment(appointment)
    ) {
      return 'Past Pending';
    }

    return status || 'Pending';
  }

  getAppointmentStatusClass(
    appointment: Appointment
  ): string {
    const status = this.normalizeStatus(
      appointment.status
    );

    if (
      status === 'pending' &&
      this.isPastAppointment(appointment)
    ) {
      return 'past-pending-badge';
    }

    return this.getStatusClass(status);
  }

  getStatusClass(status: string): string {
    switch (this.normalizeStatus(status)) {
      case 'pending':
        return 'pending-badge';

      case 'confirmed':
        return 'confirmed-badge';

      case 'completed':
        return 'completed-badge';

      case 'cancelled':
        return 'cancelled-badge';

      default:
        return 'default-badge';
    }
  }

  canDownloadAppointmentPdf(
    appointment: Appointment
  ): boolean {
    return !this.isCancelled(appointment);
  }

  canViewHealthRecord(
    appointment: Appointment
  ): boolean {
    return this.normalizeStatus(
      appointment.status
    ) === 'completed';
  }

  hasHealthRecord(
    appointment: Appointment
  ): boolean {
    return Boolean(
      this.getHealthRecordForAppointment(appointment)
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

  openHealthRecordDetails(
    appointment: Appointment
  ): void {
    const record =
      this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'The health record is not available yet for this appointment.'
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
    return record.healthRecordId ??
      record.recordId ??
      record.appointmentId;
  }

  getDoctorDisplayName(
    name: string | null | undefined
  ): string {
    const cleanName = (name ?? '').trim();

    if (!cleanName) {
      return 'Doctor not assigned';
    }

    const lowerCaseName = cleanName.toLowerCase();

    if (
      lowerCaseName.startsWith('dr.') ||
      lowerCaseName.startsWith('dr ')
    ) {
      return cleanName;
    }

    return `Dr. ${cleanName}`;
  }

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  printAppointmentPdf(
    appointment: Appointment
  ): void {
    if (!this.canDownloadAppointmentPdf(appointment)) {
      this.errorMessage.set(
        'PDF is not available for cancelled appointments.'
      );

      return;
    }

    const details: PrintDetail[] = [
      {
        label: 'Appointment ID',
        value: `#${appointment.appointmentId}`
      },
      {
        label: 'Doctor Name',
        value: this.getDoctorDisplayName(
          appointment.doctorName
        )
      },
      {
        label: 'Specialisation',
        value: this.getSafeText(
          appointment.specialisation,
          'Not assigned'
        )
      },
      {
        label: 'Appointment Date',
        value: new Date(
          appointment.scheduledDate
        ).toLocaleDateString()
      },
      {
        label: 'Time Slot',
        value: this.getSafeText(
          appointment.timeSlot,
          'Not assigned'
        )
      },
      {
        label: 'Status',
        value: this.getAppointmentStatusText(
          appointment
        )
      },
      {
        label: 'Generated On',
        value: new Date().toLocaleString()
      }
    ];

    const sections: PrintSection[] = [
      {
        title: 'Important Information',
        value:
          'Please verify the appointment status before visiting the hospital. Arrive 10 to 15 minutes before the scheduled time.'
      }
    ];

    this.openPrintDocument(
      `Appointment #${appointment.appointmentId}`,
      'HealthAxis Appointment Details',
      'Patient appointment document',
      this.getAppointmentStatusText(appointment),
      details,
      sections
    );
  }

  printHealthRecordForAppointment(
    appointment: Appointment
  ): void {
    const record =
      this.getHealthRecordForAppointment(appointment);

    if (!record) {
      this.errorMessage.set(
        'The health record is not available yet for this appointment.'
      );

      return;
    }

    this.printHealthRecordPdf(record);
  }

  printHealthRecordPdf(record: HealthRecord): void {
    const recordId = this.getRecordId(record);

    const details: PrintDetail[] = [
      {
        label: 'Record ID',
        value: `#${recordId}`
      },
      {
        label: 'Appointment ID',
        value: `#${record.appointmentId}`
      },
      {
        label: 'Patient Name',
        value: this.getSafeText(
          record.patientName,
          'Patient'
        )
      },
      {
        label: 'Doctor Name',
        value: this.getDoctorDisplayName(
          record.doctorName
        )
      },
      {
        label: 'Specialisation',
        value: this.getSafeText(
          record.specialisation,
          'Not assigned'
        )
      },
      {
        label: 'Visit Date',
        value: new Date(
          record.visitDate
        ).toLocaleDateString()
      }
    ];

    const sections: PrintSection[] = [
      {
        title: 'Diagnosis',
        value: this.getSafeText(
          record.diagnosis,
          'Diagnosis not provided.'
        )
      },
      {
        title: 'Prescription',
        value: this.getSafeText(
          record.prescription,
          'No prescription was added.'
        )
      },
      {
        title: 'Doctor Notes',
        value: this.getSafeText(
          record.notes,
          'No doctor notes were added.'
        )
      }
    ];

    this.openPrintDocument(
      `Health Record #${recordId}`,
      'HealthAxis Medical Record',
      'Patient health record document',
      '',
      details,
      sections
    );
  }

  private applyStatusFromQuery(): void {
    const requestedStatus =
      this.route.snapshot.queryParamMap.get('status');

    if (!requestedStatus) {
      return;
    }

    const matchingStatus = STATUS_FILTERS.find(
      (status) =>
        status.toLowerCase() ===
        requestedStatus.toLowerCase()
    );

    if (matchingStatus) {
      this.selectedStatus.set(matchingStatus);
    }
  }

  private countByStatus(status: string): number {
    const expectedStatus =
      this.normalizeStatus(status);

    return this.appointments().filter(
      (appointment) =>
        this.normalizeStatus(
          appointment.status
        ) === expectedStatus
    ).length;
  }

  private matchesFilter(
    appointment: Appointment,
    status: StatusFilter,
    searchValue: string
  ): boolean {
    const appointmentStatus =
      this.normalizeStatus(appointment.status);

    const matchesStatus =
      status === 'All' ||
      appointmentStatus ===
        this.normalizeStatus(status);

    const searchableText = [
      appointment.appointmentId.toString(),
      appointment.doctorName,
      appointment.specialisation,
      appointment.scheduledDate,
      appointment.timeSlot,
      appointment.status,
      appointment.cancellationReason ?? ''
    ]
      .join(' ')
      .toLowerCase();

    const matchesSearch =
      !searchValue ||
      searchableText.includes(searchValue);

    return matchesStatus && matchesSearch;
  }

  private getOptionalCancellationReason():
    string | null {
    const reason =
      this.cancellationReason().trim();

    return reason || null;
  }

  private ensureCurrentPageIsValid(): void {
    if (this.currentPage() > this.totalPages()) {
      this.currentPage.set(this.totalPages());
    }

    if (this.currentPage() < 1) {
      this.currentPage.set(1);
    }
  }

  private isPastAppointment(
    appointment: Appointment
  ): boolean {
    return this.getAppointmentStartDateTime(
      appointment
    ).getTime() < Date.now();
  }

  private isUpcomingAppointment(
    appointment: Appointment
  ): boolean {
    const status = this.normalizeStatus(
      appointment.status
    );

    const isUpcomingStatus =
      status === 'pending' ||
      status === 'confirmed';

    return isUpcomingStatus &&
      !this.isPastAppointment(appointment);
  }

  private getAppointmentStartDateTime(
    appointment: Appointment
  ): Date {
    const appointmentDate =
      this.createLocalDate(
        appointment.scheduledDate
      );

    const timeParts = appointment.timeSlot
      .trim()
      .match(
        /^(\d{1,2}):(\d{2})\s*(AM|PM)/i
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

    appointmentDate.setHours(
      hours,
      minutes,
      0,
      0
    );

    return appointmentDate;
  }

  private createLocalDate(dateValue: string): Date {
    const dateOnly = dateValue.split('T')[0];
    const dateParts = dateOnly
      .split('-')
      .map(Number);

    if (
      dateParts.length === 3 &&
      dateParts.every((part) => Number.isFinite(part))
    ) {
      const [year, month, day] = dateParts;

      return new Date(
        year,
        month - 1,
        day
      );
    }

    return new Date(dateValue);
  }

  private normalizeStatus(status: string): string {
    return status.trim().toLowerCase();
  }

  private openPrintDocument(
    windowTitle: string,
    documentTitle: string,
    subtitle: string,
    badge: string,
    details: readonly PrintDetail[],
    sections: readonly PrintSection[]
  ): void {
    const printWindow = window.open(
      '',
      '_blank',
      'width=900,height=700'
    );

    if (!printWindow) {
      this.errorMessage.set(
        'Please allow popups to download or print the document.'
      );

      return;
    }

    const detailsHtml = details
      .map(
        (detail) => `
          <div class="detail-box">
            <span>${this.escapeHtml(detail.label)}</span>
            <strong>${this.escapeHtml(detail.value)}</strong>
          </div>
        `
      )
      .join('');

    const sectionsHtml = sections
      .map(
        (section) => `
          <section class="document-section">
            <h2>${this.escapeHtml(section.title)}</h2>
            <p>${this.escapeHtml(section.value)}</p>
          </section>
        `
      )
      .join('');

    const badgeHtml = badge
      ? `<span class="status-badge">${this.escapeHtml(badge)}</span>`
      : '';

    printWindow.document.open();

    printWindow.document.write(`
      <!DOCTYPE html>
      <html lang="en">
        <head>
          <meta charset="utf-8">
          <title>${this.escapeHtml(windowTitle)}</title>

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

            .document-header {
              padding: 24px;
              border-radius: 16px;
              color: #ffffff;
              background: linear-gradient(
                135deg,
                #4024c7,
                #5b3df5
              );
            }

            .document-header h1 {
              margin: 0;
              font-size: 28px;
            }

            .document-header p {
              margin: 8px 0 0;
              color: #eeeaff;
            }

            .status-badge {
              display: inline-block;
              margin-top: 14px;
              padding: 7px 12px;
              border-radius: 999px;
              color: #4024c7;
              background: #ffffff;
              font-size: 13px;
              font-weight: bold;
            }

            .details-grid {
              margin-top: 20px;
              display: grid;
              grid-template-columns: 1fr 1fr;
              gap: 12px;
            }

            .detail-box {
              padding: 15px;
              border: 1px solid #e1e5ee;
              border-radius: 13px;
              background: #f8f9fd;
            }

            .detail-box span {
              display: block;
              margin-bottom: 7px;
              color: #66758e;
              font-size: 11px;
              font-weight: bold;
              text-transform: uppercase;
            }

            .detail-box strong {
              color: #111c36;
              font-size: 15px;
            }

            .document-section {
              margin-top: 15px;
              padding: 17px;
              border: 1px solid #ded8ff;
              border-radius: 14px;
              background: #faf9ff;
            }

            .document-section h2 {
              margin: 0 0 9px;
              font-size: 17px;
            }

            .document-section p {
              margin: 0;
              color: #3e4d67;
              line-height: 1.7;
              white-space: pre-wrap;
            }

            .document-footer {
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
            <header class="document-header">
              <h1>${this.escapeHtml(documentTitle)}</h1>
              <p>${this.escapeHtml(subtitle)}</p>
              ${badgeHtml}
            </header>

            <section class="details-grid">
              ${detailsHtml}
            </section>

            ${sectionsHtml}

            <footer class="document-footer">
              This document was generated by the HealthAxis Patient Portal.
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

  private escapeHtml(value: string): string {
    return value
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;');
  }
}