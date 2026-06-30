import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
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
  readonly loading = signal(false);
  readonly errorMessage = signal('');
  readonly searchText = signal('');

  readonly filteredRecords = computed(() => {
    const searchValue = this.searchText().trim().toLowerCase();

    if (!searchValue) {
      return this.healthRecords();
    }

    return this.healthRecords().filter((record) =>
      this.includesSearchValue(record, searchValue)
    );
  });

  readonly latestRecord = computed<HealthRecord | null>(() => {
  const record = this.healthRecords()
    .slice()
    .sort((first, second) =>
      new Date(second.visitDate).getTime() -
      new Date(first.visitDate).getTime()
    )[0];

  return record ?? null;
});

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

  getRecordId(record: HealthRecord): number {
    return record.healthRecordId ?? record.recordId ?? record.appointmentId;
  }

  private includesSearchValue(record: HealthRecord, searchValue: string): boolean {
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
}