import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Appointment } from '../../../core/models/appointment.model';
import { HealthRecord } from '../../../core/models/health-record.model';
import { Patient } from '../../../core/models/patient.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { DoctorService } from '../../../core/services/doctor.service';

@Component({
  selector: 'app-patient-profile',
  imports: [FormsModule],
  templateUrl: './patient-profile.html',
  styleUrls: ['./patient-profile.css']
})
export class PatientProfile implements OnInit {
  isLoading = false;
  errorMessage = '';

  selectedPatientId = '';
  patient: Patient | null = null;
  healthRecords: HealthRecord[] = [];

  appointments: Appointment[] = [];
  patientOptions: { patientId: number; patientName: string }[] = [];

  constructor(
    private route: ActivatedRoute,
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {
    const patientIdFromRoute =
      this.route.snapshot.paramMap.get('patientId');

    this.loadAppointmentPatients(patientIdFromRoute);
  }

  loadAppointmentPatients(patientIdFromRoute: string | null): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getMyAppointments().subscribe({
      next: (result: Appointment[] | PagedResult<Appointment>) => {
        this.appointments = Array.isArray(result)
          ? result
          : result.items ?? [];

        this.patientOptions = this.buildPatientOptions(this.appointments);

        if (patientIdFromRoute) {
          this.selectedPatientId = patientIdFromRoute;
          this.loadPatientProfile(Number(patientIdFromRoute));
        }
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to load patients. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onPatientChanged(): void {
    if (!this.selectedPatientId) {
      this.patient = null;
      this.healthRecords = [];
      return;
    }

    this.loadPatientProfile(Number(this.selectedPatientId));
  }

  loadPatientProfile(patientId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getPatientProfile(patientId).subscribe({
      next: (patient: Patient) => {
        this.patient = {
          ...patient,
          dateOfBirth: patient.dateOfBirth
            ? patient.dateOfBirth.split('T')[0]
            : ''
        };

        this.loadHealthRecords(patientId);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to load patient profile. Status: ${error.status}`;
        this.isLoading = false;
      }
    });
  }

  loadHealthRecords(patientId: number): void {
    this.doctorService.getPatientHealthRecords(patientId).subscribe({
      next: (records: HealthRecord[]) => {
        this.healthRecords = records.sort(
          (a, b) =>
            new Date(b.visitDate).getTime() -
            new Date(a.visitDate).getTime()
        );
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.error?.message ??
          `Unable to load health records. Status: ${error.status}`;
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

  getGenderText(value: number | string): string {
    const genderMap: Record<number, string> = {
      0: 'Male',
      1: 'Female',
      2: 'Other'
    };

    return typeof value === 'string' && Number.isNaN(Number(value))
      ? value
      : genderMap[Number(value)] ?? '-';
  }

  private buildPatientOptions(
    appointments: Appointment[]
  ): { patientId: number; patientName: string }[] {
    const map = new Map<number, string>();

    appointments.forEach((appointment) => {
      if (!appointment.patientId) {
        return;
      }

      map.set(
        appointment.patientId,
        appointment.patientName ?? `Patient ID ${appointment.patientId}`
      );
    });

    return Array.from(map.entries()).map(([patientId, patientName]) => ({
      patientId,
      patientName
    }));
  }
}