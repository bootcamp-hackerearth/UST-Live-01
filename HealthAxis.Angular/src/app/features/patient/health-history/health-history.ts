import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { HealthRecord } from '../../../core/models/health-record.model';
import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-health-history',
  imports: [RouterLink],
  templateUrl: './health-history.html',
  styleUrls: ['./health-history.css']
})
export class HealthHistory implements OnInit {
  isLoading = false;
  errorMessage = '';

  healthRecords: HealthRecord[] = [];

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {
    this.loadHealthHistory();
  }

  loadHealthHistory(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.patientService.getMyHealthRecords().subscribe({
      next: (records: HealthRecord[]) => {
        console.log('Health history response:', records);

        this.healthRecords = records.sort(
          (a, b) =>
            new Date(b.visitDate).getTime() -
            new Date(a.visitDate).getTime()
        );
      },

      error: (error: HttpErrorResponse) => {
        console.log('Health history error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load health history. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  formatDate(value: string): string {
    if (!value) {
      return '-';
    }

    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  getDoctorName(record: HealthRecord): string {
    return record.doctorName ??
      `Doctor ID ${record.doctorId}`;
  }

  getSpecialisationName(value: number | string): string {
    if (typeof value === 'string' && isNaN(Number(value))) {
      return value;
    }

    const specialisationMap: Record<number, string> = {
      0: 'Cardiology',
      1: 'General Medicine',
      2: 'Dermatology',
      3: 'Pediatrics',
      4: 'Orthopedics',
      5: 'Neurology'
    };

    return specialisationMap[Number(value)] ??
      `Specialisation ${value}`;
  }
}