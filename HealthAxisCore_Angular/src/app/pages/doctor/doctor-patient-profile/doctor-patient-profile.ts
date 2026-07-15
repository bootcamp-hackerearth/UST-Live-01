import { Component, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { PatientService } from '../../../core/services/patient.service';
import { HealthRecordService } from '../../../core/services/health-record.service';
import { PatientDto } from '../../../core/models/patient.model';
import { HealthRecordDto } from '../../../core/models/health-record.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-doctor-patient-profile',
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './doctor-patient-profile.html',
  styleUrl: './doctor-patient-profile.css'
})
export class DoctorPatientProfile {
  patientId = 0;

  patient = signal<PatientDto | null>(null);

  records = signal<HealthRecordDto[]>([]);

  isLoading = signal(false);

  errorMessage = signal('');

  constructor(
    private readonly route: ActivatedRoute,
    private readonly patientService: PatientService,
    private readonly healthRecordService: HealthRecordService,
    private readonly authService: AuthService
  ) {
    this.patientId = Number(this.route.snapshot.paramMap.get('patientId'));

    this.loadPatient();
    this.loadRecords();
  }

  loadPatient(): void {
    this.isLoading.set(true);

    this.patientService.getById(this.patientId).subscribe({
      next: patient => {
        this.patient.set(patient);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  loadRecords(): void {
    this.healthRecordService.getByPatientId(this.patientId).subscribe({
      next: records => {
        this.records.set(records);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }
}
