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

type HealthRecordFilter =
  | 'All'
  | 'Final'
  | 'Updated';

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

  readonly filteredRecords = computed(() => {
    const searchValue =
      this.searchText()
        .trim()
        .toLowerCase();

    const selectedFilter =
      this.selectedRecordFilter();

    return this.healthRecords()
      .filter((record) => {
        const isUpdated =
          this.hasUpdated(record);

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

        const matchesSearch =
          !searchValue ||
          this.includesSearchValue(
            record,
            searchValue
          );

        return (
          matchesRecordFilter &&
          matchesSearch
        );
      })
      .slice()
      .sort(
        (first, second) =>
          this.getRecordDateValue(second) -
          this.getRecordDateValue(first)
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
    this.healthRecords().filter(
      (record) => this.hasUpdated(record)
    ).length
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
  }

  onRecordFilterChange(
    event: Event
  ): void {
    const select =
      event.target as HTMLSelectElement;

    this.selectedRecordFilter.set(
      select.value as HealthRecordFilter
    );
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedRecordFilter.set('All');
  }

  openRecordDetails(record: HealthRecord): void {
    this.selectedRecord.set(record);
  }

  closeRecordDetails(): void {
    this.selectedRecord.set(null);
  }

  closeHealthRecordDetails(): void {
    this.closeRecordDetails();
  }

  onHealthRecordBackdropClick(event: Event): void {
    if (event.target === event.currentTarget) {
      this.closeRecordDetails();
    }
  }

  getRecordId(record: HealthRecord): number {
    return record.healthRecordId ??
      record.recordId ??
      record.appointmentId;
  }

  hasUpdated(record: HealthRecord): boolean {
    return Boolean(record.updatedDate);
  }

  getDoctorDisplayName(
    name: string | null | undefined
  ): string {
    const cleanName = (name ?? '').trim();

    if (
      !cleanName ||
      cleanName.toLowerCase() === 'doctor'
    ) {
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

  getSafeText(
    value: string | null | undefined,
    fallback: string
  ): string {
    const cleanValue = (value ?? '').trim();

    return cleanValue || fallback;
  }

  getCreatedDateText(record: HealthRecord): string {
    if (!record.createdAt) {
      return 'Not available';
    }

    const createdDate = new Date(record.createdAt);

    if (Number.isNaN(createdDate.getTime())) {
      return 'Not available';
    }

    return createdDate.toLocaleString();
  }

 printHealthRecord(record: HealthRecord): void {
  const printWindow = globalThis.open(
    '',
    '_blank',
    'width=900,height=700'
  );

  if (!printWindow) {
    this.errorMessage.set(
      'Please allow popups to print or save the health record.'
    );
    return;
  }

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

  const visitDate =
    new Date(record.visitDate)
      .toLocaleDateString();

  const createdAt = this.escapeHtml(
    this.getCreatedDateText(record)
  );

  const updatedAt = this.escapeHtml(
    record.updatedDate
      ? new Date(record.updatedDate)
          .toLocaleString()
      : 'Not updated'
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

        <title>
          Health Record #${recordId}
        </title>

        <style>
          * {
            box-sizing: border-box;
          }

          body {
            margin: 0;
            padding: 24px;
            font-family: Arial, sans-serif;
            color: #111c36;
            background: #f7f8fc;
          }

          .document {
            width: 800px;
            max-width: 100%;
            margin: 0 auto;
            padding: 32px;
            border: 1px solid #e6e8f0;
            border-radius: 20px;
            background: #ffffff;
          }

          .header {
            padding: 24px;
            border-radius: 17px;
            color: #ffffff;
            background: linear-gradient(
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
          }

          .grid {
            margin-top: 22px;
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 13px;
          }

          .box {
            padding: 14px;
            border: 1px solid #e6e8f0;
            border-radius: 13px;
            background: #faf9ff;
          }

          .box span {
            display: block;
            margin-bottom: 6px;
            color: #66738a;
            font-size: 11px;
            font-weight: bold;
            text-transform: uppercase;
          }

          .box strong {
            color: #111c36;
            font-size: 14px;
          }

          .section {
            margin-top: 16px;
            padding: 17px;
            border: 1px solid #e6e8f0;
            border-radius: 14px;
          }

          .section h2 {
            margin: 0 0 10px;
            color: #111c36;
            font-size: 17px;
          }

          .section p {
            margin: 0;
            color: #334155;
            line-height: 1.7;
            white-space: pre-wrap;
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
            margin-top: 24px;
            padding-top: 14px;
            border-top: 1px solid #e6e8f0;
            color: #66738a;
            font-size: 12px;
            line-height: 1.6;
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

            <p>
              Patient Health Record Document
            </p>
          </header>

          <section class="grid">
            <div class="box">
              <span>Record ID</span>
              <strong>#${recordId}</strong>
            </div>

            <div class="box">
              <span>Appointment ID</span>
              <strong>
                #${record.appointmentId}
              </strong>
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

  printWindow.addEventListener(
    'load',
    () => {
      printWindow.focus();
      printWindow.print();

      globalThis.URL.revokeObjectURL(
        printDocumentUrl
      );
    },
    {
      once: true
    }
  );

  printWindow.location.href =
    printDocumentUrl;
}

  private getRecordDateValue(
    record: HealthRecord
  ): number {
    const dateValue =
      record.createdAt ?? record.visitDate;

    return new Date(dateValue).getTime();
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

  private escapeHtml(value: string): string {
    return value
      .replaceAll('&', '&amp;')
      .replaceAll('<', '&lt;')
      .replaceAll('>', '&gt;')
      .replaceAll('"', '&quot;')
      .replaceAll("'", '&#039;');
  }
}