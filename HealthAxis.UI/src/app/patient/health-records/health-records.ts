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

@Component({
  selector: 'app-health-records',
  imports: [DatePipe, RouterLink],
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

  readonly filteredRecords = computed(() => {
    const searchValue = this.searchText().trim().toLowerCase();

    const records = searchValue
      ? this.healthRecords().filter((record) =>
          this.includesSearchValue(record, searchValue)
        )
      : this.healthRecords();

    return records
      .slice()
      .sort(
        (first, second) =>
          new Date(second.visitDate).getTime() -
          new Date(first.visitDate).getTime()
      );
  });

  readonly latestRecord = computed<HealthRecord | null>(() => {
    const record = this.healthRecords()
      .slice()
      .sort(
        (first, second) =>
          new Date(second.visitDate).getTime() -
          new Date(first.visitDate).getTime()
      )[0];

    return record ?? null;
  });

  readonly updatedRecordsCount = computed(() =>
    this.healthRecords().filter((record) => this.hasUpdated(record)).length
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
          getFriendlyErrorMessage(error, 'Could not load your health records.')
        );
      }
    });
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchText.set(input.value);
  }

  openRecordDetails(record: HealthRecord): void {
    this.selectedRecord.set(record);
  }

  closeRecordDetails(): void {
    this.selectedRecord.set(null);
  }

  getRecordId(record: HealthRecord): number {
    return record.healthRecordId ?? record.recordId ?? record.appointmentId;
  }

  hasUpdated(record: HealthRecord): boolean {
    return Boolean(record.updatedDate);
  }

  getDoctorDisplayName(name: string | null | undefined): string {
    const cleanName = (name ?? '').trim();

    if (!cleanName || cleanName.toLowerCase() === 'doctor') {
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
      record.visitDate
    ]
      .filter((value): value is string => typeof value === 'string')
      .join(' ')
      .toLowerCase();

    return searchableText.includes(searchValue);
  }
  printHealthRecord(record: HealthRecord): void {
  const printWindow = window.open('', '_blank', 'width=900,height=700');

  if (!printWindow) {
    this.errorMessage.set('Please allow popups to print or save the health record.');
    return;
  }

  const recordId = this.getRecordId(record);
  const patientName = this.escapeHtml(this.getSafeText(record.patientName, 'Patient'));
  const doctorName = this.escapeHtml(this.getDoctorDisplayName(record.doctorName));
  const specialisation = this.escapeHtml(
    this.getSafeText(record.specialisation, 'Not assigned')
  );
  const diagnosis = this.escapeHtml(
    this.getSafeText(record.diagnosis, 'Diagnosis not provided.')
  );
  const prescription = this.escapeHtml(
    this.getSafeText(record.prescription, 'No prescription added.')
  );
  const notes = this.escapeHtml(
    this.getSafeText(record.notes, 'No doctor notes added.')
  );
  const visitDate = new Date(record.visitDate).toLocaleDateString();

  printWindow.document.open();

  printWindow.document.write(`
    <!DOCTYPE html>
    <html>
      <head>
        <title>Health Record #${recordId}</title>

        <style>
          body {
            font-family: Arial, sans-serif;
            margin: 0;
            background: #f8fafc;
            color: #0f172a;
          }

          .document {
            width: 800px;
            margin: 24px auto;
            background: white;
            padding: 32px;
            border-radius: 18px;
            border: 1px solid #e2e8f0;
          }

          .header {
            background: linear-gradient(135deg, #0f172a, #047857);
            color: white;
            padding: 22px;
            border-radius: 16px;
            margin-bottom: 24px;
          }

          .header h1 {
            margin: 0;
            font-size: 26px;
          }

          .header p {
            margin: 8px 0 0;
            color: #d1fae5;
          }

          .grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
            margin-bottom: 20px;
          }

          .box {
            border: 1px solid #dbeafe;
            background: #f8fafc;
            border-radius: 12px;
            padding: 14px;
          }

          .box span {
            display: block;
            color: #64748b;
            font-size: 12px;
            font-weight: bold;
            margin-bottom: 6px;
          }

          .box strong {
            font-size: 15px;
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
              background: white;
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
        <div class="document">
          <div class="header">
            <h1>HealthAxis Medical Record</h1>
            <p>Patient Health Record Document</p>
          </div>

          <div class="grid">
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
          </div>

          <div class="section">
            <h2>Diagnosis</h2>
            <p>${diagnosis}</p>
          </div>

          <div class="section">
            <h2>Prescription</h2>
            <p>${prescription}</p>
          </div>

          <div class="section">
            <h2>Doctor Notes</h2>
            <p>${notes}</p>
          </div>

          <div class="footer">
            This record is generated from HealthAxis. Please consult your doctor before changing any medication.
          </div>
        </div>

        <script>
          window.onload = function () {
            window.print();
          };
        </script>
      </body>
    </html>
  `);

  printWindow.document.close();
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