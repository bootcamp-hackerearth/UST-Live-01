import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { DoctorDashboardService } from '../../core/services/doctor-dashboard.service';
import { TokenService } from '../../core/services/token.service';

import {
  DoctorHealthRecord,
  DoctorPatient
} from '../../shared/models/doctor-dashboard.models';

import { getDoctorSpecialisationText } from '../../shared/models/doctor.models';

@Component({
  selector: 'app-doctor-patients',
  imports: [
    CommonModule,
    DatePipe,
    FormsModule
  ],
  templateUrl: './doctor-patients.html',
  styleUrl: './doctor-patients.css'
})
export class DoctorPatients implements OnInit {
  doctorId: number | null = null;

  loading = true;
  loadingRecords = false;

  errorMessage = '';
  searchText = '';

  patients: DoctorPatient[] = [];
  selectedPatient?: DoctorPatient;
  selectedPatientRecords: DoctorHealthRecord[] = [];

  currentPage = 1;
  pageSize = 5;
  pageSizeOptions = [5, 10, 20];

  constructor(
    private tokenService: TokenService,
    private doctorDashboardService: DoctorDashboardService
  ) {}

  ngOnInit(): void {
    this.doctorId = this.tokenService.getReferenceId();

    if (!this.doctorId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    this.loadPatients();
  }

  loadPatients(): void {
    if (!this.doctorId) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    this.doctorDashboardService.getDoctorPatients(this.doctorId).subscribe({
      next: (patients: DoctorPatient[]) => {
        this.patients = patients ?? [];
        this.currentPage = 1;
        this.loading = false;
      },
      error: (error: any) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view doctor patients.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load patients. Please try again.';
      }
    });
  }

  selectPatient(patient: DoctorPatient): void {
    this.selectedPatient = patient;
    this.selectedPatientRecords = [];
    this.loadingRecords = true;
    this.errorMessage = '';

    this.doctorDashboardService.getPatientHealthRecords(patient.patientId).subscribe({
      next: (records: DoctorHealthRecord[]) => {
        this.selectedPatientRecords = records ?? [];
        this.loadingRecords = false;
      },
      error: (error: any) => {
        this.loadingRecords = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view patient health records.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load patient health records.';
      }
    });
  }

  clearSelectedPatient(): void {
    this.selectedPatient = undefined;
    this.selectedPatientRecords = [];
  }

  onSearchChange(value: string): void {
    this.searchText = value;
    this.currentPage = 1;
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.currentPage = page;
  }

  get filteredPatients(): DoctorPatient[] {
    const search = this.searchText.trim().toLowerCase();

    if (!search) {
      return this.patients;
    }

    return this.patients.filter(patient =>
      patient.fullName.toLowerCase().includes(search) ||
      patient.patientId.toString().includes(search) ||
      (patient.email ?? '').toLowerCase().includes(search) ||
      patient.phoneNumber.toLowerCase().includes(search)
    );
  }

  get paginatedPatients(): DoctorPatient[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    return this.filteredPatients.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages(): number {
    const total = Math.ceil(this.filteredPatients.length / this.pageSize);
    return total > 0 ? total : 1;
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  get paginationStart(): number {
    if (this.filteredPatients.length === 0) {
      return 0;
    }

    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get paginationEnd(): number {
    const end = this.currentPage * this.pageSize;
    return end > this.filteredPatients.length ? this.filteredPatients.length : end;
  }

  get totalPatients(): number {
    return this.patients.length;
  }

  genderText(value: number): string {
    switch (Number(value)) {
      case 1:
        return 'Male';

      case 2:
        return 'Female';

      case 3:
        return 'Non-binary';

      case 4:
        return 'Prefer not to say';

      default:
        return 'Not specified';
    }
  }

  specialisationText(value: number): string {
    return getDoctorSpecialisationText(value);
  }

  trackPatient(_: number, patient: DoctorPatient): number {
    return patient.patientId;
  }

  trackRecord(_: number, record: DoctorHealthRecord): number {
    return record.healthRecordId;
  }
}

