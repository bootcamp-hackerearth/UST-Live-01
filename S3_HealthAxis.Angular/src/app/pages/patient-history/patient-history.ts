import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { HealthHistoryService } from '../../core/services/health-history.service';
import { TokenService } from '../../core/services/token.service';

import { HealthHistoryRecord } from '../../shared/models/health-history.models';

@Component({
  selector: 'app-patient-history',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './patient-history.html',
  styleUrl: './patient-history.css'
})
export class PatientHistory implements OnInit {
  loading = true;
  errorMessage = '';

  records: HealthHistoryRecord[] = [];

  constructor(
    private healthHistoryService: HealthHistoryService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.loadHealthHistory();
  }

  loadHealthHistory(): void {
    this.loading = true;
    this.errorMessage = '';

    const patientId = this.tokenService.getReferenceId();

    if (!patientId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      return;
    }

    this.healthHistoryService.getPatientHealthRecords(patientId).subscribe({
      next: (records) => {
        this.records = records ?? [];
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view health history. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load health history. Please try again.';
      }
    });
  }

  get totalRecords(): number {
    return this.records.length;
  }

  specialisationText(value: number): string {
    switch (value) {
      case 1:
        return 'General Practitioner';

      case 2:
        return 'Cardiologist';

      case 3:
        return 'Dermatologist';

      case 4:
        return 'Neurologist';

      case 5:
        return 'Pediatrician';

      case 6:
        return 'Psychiatrist';

      case 7:
        return 'Orthopedic Surgeon';

      case 8:
        return 'Gynecologist';

      case 9:
        return 'Oncologist';

      case 10:
        return 'Endocrinologist';

      default:
        return 'Specialist';
    }
  }

  trackRecord(_: number, record: HealthHistoryRecord): number {
    return record.healthRecordId;
  }
}

