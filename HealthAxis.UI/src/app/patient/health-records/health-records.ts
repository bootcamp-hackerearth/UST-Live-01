import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { HealthRecord } from '../../core/models/health-record.model';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

type HealthRecordFilter = 'All' | 'Final' | 'Updated';
type DateValue = string | Date | null | undefined;

const PRINT_WINDOW_FEATURES = 'width=900,height=700';
const PRINT_DELAY_IN_MS = 300;
const PAGE_SIZE = 8;
const NOT_AVAILABLE = 'Not available';
const DATE_ONLY_PATTERN = /^\d{4}-\d{2}-\d{2}$/;
const TIME_ZONE_PATTERN = /(?:z|[+-]\d{2}:\d{2})$/i;

@Component({
  selector: 'app-health-records',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './health-records.html',
  styleUrl: './health-records.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HealthRecords {
  private readonly patientService = inject(PatientService);

  readonly healthRecords = signal<HealthRecord[]>([]);
  readonly selectedRecord = signal<HealthRecord | null>(null);
  readonly loading = signal(false);
  readonly errorMessage = signal('');
  readonly searchText = signal('');
  readonly selectedRecordFilter =
    signal<HealthRecordFilter>('All');

  readonly currentPage = signal(1);

  readonly filteredRecords = computed(() => {
    const searchValue = this.searchText()
      .trim()
      .toLowerCase();

    const selectedFilter = this.selectedRecordFilter();

    return this.healthRecords()
      .filter((record) =>
        this.matchesFilter(
          record,
          selectedFilter,
          searchValue
        )
      )
      .slice()
      .sort(
        (first, second) =>
          this.getRecordDateValue(second) -
          this.getRecordDateValue(first)
      );
  });

  readonly totalPages = computed(() => {
    const pageCount = Math.ceil(
      this.filteredRecords().length / PAGE_SIZE
    );

    return Math.max(pageCount, 1);
  });

  readonly pagedRecords = computed(() => {
    const page = Math.min(
      this.currentPage(),
      this.totalPages()
    );

    const startIndex =
      (page - 1) * PAGE_SIZE;

    return this.filteredRecords().slice(
      startIndex,
      startIndex + PAGE_SIZE
    );
  });

  readonly latestRecord = computed<HealthRecord | null>(() => {
    const record = this.healthRecords()
      .slice()
      .sort(
        (first, second) =>
          this.getRecordDateValue(second) -
          this.getRecordDateValue(first)
      )[0];

    return record ?? null;
  });

  readonly updatedRecordsCount = computed(() =>
    this.healthRecords()
      .filter((record) => this.hasUpdated(record))
      .length
  );

  constructor() {
    this.loadHealthRecords();
  }

  loadHealthRecords(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.patientService.getMyHealthRecords().subscribe({
      next: (records) => {
        this.healthRecords.set(records);
        this.currentPage.set(1);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);

        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not load your health records.'
          )
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.searchText.set(input.value);
    this.currentPage.set(1);
  }

  onRecordFilterChange(event: Event): void {
    const select = event.target as HTMLSelectElement;

    this.selectedRecordFilter.set(
      select.value as HealthRecordFilter
    );

    this.currentPage.set(1);
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedRecordFilter.set('All');
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
    if (this.currentPage() < this.totalPages()) {
      this.currentPage.update(
        (page) => page + 1
      );
    }
  }

  openRecordDetails(record: HealthRecord): void {
    this.selectedRecord.set(record);
  }

  closeRecordDetails(): void {
    this.selectedRecord.set(null);
  }

  getRecordId(record: HealthRecord): number {
    return record.healthRecordId ??
      record.recordId ??
      record.appointmentId;
  }

  hasUpdated(record: HealthRecord): boolean {
    return Boolean(record.updatedDate?.trim());
  }

  getDoctorDisplayName(
    name: string | null | undefined
  ): string {
    const cleanName = (name ?? '').trim();
    const lowerName = cleanName.toLowerCase();

    if (!cleanName || lowerName === 'doctor') {
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

  getDisplayDate(
    value: string | null | undefined
  ): Date | null {
    return this.parseDate(value);
  }

  getDisplayDateTime(
    value: string | null | undefined
  ): Date | null {
    return this.parseDate(value);
  }

  printHealthRecord(record: HealthRecord): void {
    const printWindow = globalThis.open(
      '',
      '_blank',
      PRINT_WINDOW_FEATURES
    );

    if (!printWindow) {
      this.errorMessage.set(
        'Please allow popups to print or save the health record.'
      );

      return;
    }

    const printableDocument =
      this.createHealthRecordDocument(record);

    const parsedDocument =
      new DOMParser().parseFromString(
        printableDocument,
        'text/html'
      );

    const importedHead =
      printWindow.document.importNode(
        parsedDocument.head,
        true
      );

    const importedBody =
      printWindow.document.importNode(
        parsedDocument.body,
        true
      );

    printWindow.document.head.replaceWith(
      importedHead
    );

    printWindow.document.body.replaceWith(
      importedBody
    );

    printWindow.document.documentElement.lang =
      parsedDocument.documentElement.lang;

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

  private createHealthRecordDocument(
    record: HealthRecord
  ): string {
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
      this.formatDate(record.visitDate)
    );

    const createdAt = this.escapeHtml(
      this.formatDateTime(record.createdAt)
    );

    const updatedAt = this.escapeHtml(
      record.updatedDate
        ? this.formatDateTime(record.updatedDate)
        : 'Not updated'
    );

    return `
      <!doctype html>

      <html lang="en">
        <head>
          <meta charset="utf-8">

          <meta
            name="viewport"
            content="width=device-width, initial-scale=1"
          >

          <title>Health Record #${recordId}</title>

          <style>
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
              color: #111c36;
              background: #f7f8fc;
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
              border: 1px solid #e6e8f0;
              border-radius: 18px;
              background: #ffffff;
            }

            .header {
              padding: 23px;
              border-radius: 15px;
              color: #ffffff;
              background:
                linear-gradient(
                  135deg,
                  #704dff,
                  #4d27e9
                );
            }

            .header h1 {
              margin: 0;
              font-size: 26px;
            }

            .header p {
              margin: 8px 0 0;
              color: #eeeaff;
              font-size: 14px;
            }

            .grid {
              margin-top: 20px;
              display: grid;
              grid-template-columns:
                repeat(2, minmax(0, 1fr));
              gap: 12px;
            }

            .box {
              min-height: 76px;
              padding: 14px;
              border: 1px solid #e6e8f0;
              border-radius: 12px;
              background: #faf9ff;
            }

            .box span {
              display: block;
              margin-bottom: 7px;
              color: #66738a;
              font-size: 10px;
              font-weight: 700;
              text-transform: uppercase;
              letter-spacing: 0.03em;
            }

            .box strong {
              color: #111c36;
              font-size: 14px;
              line-height: 1.4;
              overflow-wrap: anywhere;
            }

            .section {
              margin-top: 15px;
              padding: 16px;
              border: 1px solid #e6e8f0;
              border-radius: 13px;
              break-inside: avoid;
            }

            .section h2 {
              margin: 0 0 9px;
              color: #111c36;
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

            .diagnosis {
              background: #fffaf0;
            }

            .prescription {
              background: #f1f7ff;
            }

            .notes {
              background: #effbf7;
            }

            .footer {
              margin-top: 22px;
              padding-top: 13px;
              border-top: 1px solid #e6e8f0;
              color: #66738a;
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
              .box,
              .section {
                print-color-adjust: exact;
                -webkit-print-color-adjust: exact;
              }
            }
          </style>
        </head>

        <body>
          <main class="document">
            <header class="header">
              <h1>HealthAxis Medical Record</h1>
              <p>Patient Health Record Document</p>
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

            <section class="section diagnosis">
              <h2>Diagnosis</h2>
              <p>${diagnosis}</p>
            </section>

            <section class="section prescription">
              <h2>Prescription</h2>
              <p>${prescription}</p>
            </section>

            <section class="section notes">
              <h2>Doctor Notes</h2>
              <p>${notes}</p>
            </section>

            <footer class="footer">
              This record was generated from HealthAxis.
              Please consult your doctor before changing
              any medication.
            </footer>
          </main>

        </body>
      </html>
    `;
  }

  private matchesFilter(
    record: HealthRecord,
    selectedFilter: HealthRecordFilter,
    searchValue: string
  ): boolean {
    const isUpdated = this.hasUpdated(record);

    const matchesRecordFilter =
      selectedFilter === 'All' ||
      (
        selectedFilter === 'Updated' &&
        isUpdated
      ) ||
      (
        selectedFilter === 'Final' &&
        !isUpdated
      );

    return (
      matchesRecordFilter &&
      (
        !searchValue ||
        this.includesSearchValue(
          record,
          searchValue
        )
      )
    );
  }

  private getRecordDateValue(
    record: HealthRecord
  ): number {
    const value =
      record.createdAt ??
      record.visitDate;

    return this.parseDate(value)?.getTime() ?? 0;
  }

  private includesSearchValue(
    record: HealthRecord,
    searchValue: string
  ): boolean {
    const searchableText = [
      record.doctorName,
      record.patientName,
      record.specialisation,
      record.diagnosis,
      record.prescription,
      record.notes,
      record.visitDate,
      record.createdAt,
      record.updatedDate
    ]
      .filter(
        (value): value is string =>
          typeof value === 'string'
      )
      .join(' ')
      .toLowerCase();

    return searchableText.includes(searchValue);
  }

  private formatDate(
    value: DateValue
  ): string {
    const date = this.parseDate(value);

    if (!date) {
      return NOT_AVAILABLE;
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
    value: DateValue
  ): string {
    const date = this.parseDate(value);

    if (!date) {
      return NOT_AVAILABLE;
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
    value: DateValue
  ): Date | null {
    if (!value) {
      return null;
    }

    if (value instanceof Date) {
      return Number.isNaN(value.getTime())
        ? null
        : value;
    }

    const trimmedValue = value.trim();

    if (!trimmedValue) {
      return null;
    }

    if (DATE_ONLY_PATTERN.test(trimmedValue)) {
      const [year, month, day] = trimmedValue
        .split('-')
        .map(Number);

      const localDate = new Date(
        year,
        month - 1,
        day
      );

      const isValidDate =
        localDate.getFullYear() === year &&
        localDate.getMonth() === month - 1 &&
        localDate.getDate() === day;

      return isValidDate
        ? localDate
        : null;
    }

    const normalizedValue =
      TIME_ZONE_PATTERN.test(trimmedValue)
        ? trimmedValue
        : `${trimmedValue}Z`;

    const date = new Date(normalizedValue);

    return Number.isNaN(date.getTime())
      ? null
      : date;
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