import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { Doctor } from '../../../core/models/doctor.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { PatientService } from '../../../core/services/patient.service';
import { Pagination } from '../../../shared/pagination/pagination';

import {
  SPECIALISATION_OPTIONS,
  getSpecialisationName
} from '../../../core/constants/specialisation.constants';

@Component({
  selector: 'app-doctor-search',
  imports: [FormsModule, RouterLink, Pagination],
  templateUrl: './doctor-search.html',
  styleUrls: ['./doctor-search.css']
})
export class DoctorSearch implements OnInit {
  isLoading = false;
  errorMessage = '';

  searchText = '';
  selectedSpecialisation = '';

  doctors: Doctor[] = [];
  filteredDoctors: Doctor[] = [];
  pagedDoctors: Doctor[] = [];

  specialisations = SPECIALISATION_OPTIONS;

  currentPage = 1;
  pageSize = 6;
  totalItems = 0;

  constructor(private readonly patientService: PatientService) {}

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.patientService
      .getDoctors(
        1,
        100,
        '',
        ''
      )
      .subscribe({
        next: (result: Doctor[] | PagedResult<Doctor>) => {
          if (Array.isArray(result)) {
            this.doctors = result;
          } else {
            this.doctors = result.items ?? [];
          }

          this.applyFilters();
        },

        error: (error) => {
          console.log('Doctor search error:', error);

          this.doctors = [];
          this.filteredDoctors = [];
          this.pagedDoctors = [];

          this.errorMessage =
            error?.error?.message ??
            `Unable to load doctors. Status: ${error.status}`;
        },

        complete: () => {
          this.isLoading = false;
        }
      });
  }

  onSearchChanged(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  onSpecialisationChanged(): void {
    this.currentPage = 1;
    this.applyFilters();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedSpecialisation = '';
    this.currentPage = 1;
    this.applyFilters();
  }

  onPageChanged(page: number): void {
    this.currentPage = page;
    this.updatePagedDoctors();
  }

  private applyFilters(): void {
    const search =
      this.searchText.trim().toLowerCase();

    this.filteredDoctors =
      this.doctors.filter((doctor) => {
        const doctorName =
          doctor.fullName?.toLowerCase() ?? '';

        const doctorId =
          doctor.doctorId?.toString() ?? '';

        const doctorSpecialisation =
          String(doctor.specialisation ?? '');

        const specialisationName =
          this.getSpecialisationName(doctor.specialisation).toLowerCase();

        const matchesSearch =
          !search ||
          doctorName.includes(search) ||
          doctorId.includes(search) ||
          specialisationName.includes(search);

        const matchesSpecialisation =
          !this.selectedSpecialisation ||
          doctorSpecialisation === this.selectedSpecialisation;

        return matchesSearch && matchesSpecialisation;
      });

    this.totalItems = this.filteredDoctors.length;
    this.updatePagedDoctors();
  }

  private updatePagedDoctors(): void {
    const startIndex =
      (this.currentPage - 1) * this.pageSize;

    const endIndex =
      startIndex + this.pageSize;

    this.pagedDoctors =
      this.filteredDoctors.slice(startIndex, endIndex);
  }

  getSpecialisationName(value: number | string): string {
    if (value === null || value === undefined || value === '') {
      return 'Not specified';
    }

    return getSpecialisationName(value);
  }

  formatFee(value: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(value);
  }
}