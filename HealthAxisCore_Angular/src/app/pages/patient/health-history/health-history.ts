import { Component, signal } from '@angular/core';

import { AuthService } from '../../../core/services/auth.service';
import { PatientService } from '../../../core/services/patient.service';
import { HealthRecordDto } from '../../../core/models/health-record.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-health-history',
  imports: [CommonModule],
  templateUrl: './health-history.html',
  styleUrl: './health-history.css'
})
export class HealthHistory {
  records = signal<HealthRecordDto[]>([]);

  isLoading = signal(false);

  errorMessage = signal('');

  constructor(
    private authService: AuthService,
    private patientService: PatientService
  ) {
    this.loadRecords();
  }

  loadRecords(): void {
    const patientId = this.authService.patientId();

    if (!patientId) {
      this.errorMessage.set('Patient ID missing. Please login again.');
      return;
    }

    this.isLoading.set(true);

    this.patientService.getHealthRecords(patientId).subscribe({
      next: records => {
        this.records.set(records);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }
}
