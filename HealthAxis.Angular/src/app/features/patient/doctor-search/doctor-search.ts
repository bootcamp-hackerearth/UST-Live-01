import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Doctor } from '../../../core/models/doctor.model';
import { PagedResult } from '../../../core/models/paged-result.model';
import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-doctor-search',
  imports: [FormsModule, RouterLink],
  templateUrl: './doctor-search.html',
  styleUrls: ['./doctor-search.css']
})
export class DoctorSearch implements OnInit {
  private readonly pageSize = 50;

  isLoading = false;
  errorMessage = '';

  searchText = '';
  selectedSpecialisation = '';

  doctors: Doctor[] = [];
  filteredDoctors: Doctor[] = [];

  specialisations: { value: string; label: string }[] = [
    {
      value: '',
      label: 'All Specialisations'
    }
  ];

  constructor(private patientService: PatientService) {}

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.patientService
      .getDoctors(
        1,
        this.pageSize,
        '',
        ''
      )
      .subscribe({
        next: (result: Doctor[] | PagedResult<Doctor>) => {
          console.log('Doctors API response:', result);

          if (Array.isArray(result)) {
            this.doctors = result;
          } else {
            this.doctors = result.items ?? [];
          }

          this.buildSpecialisationOptions();
          this.applyFilters();
        },

        error: (error) => {
          console.log('Doctor search error:', error);

          this.doctors = [];
          this.filteredDoctors = [];

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
    this.applyFilters();
  }

  onSpecialisationChanged(): void {
    this.applyFilters();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedSpecialisation = '';
    this.applyFilters();
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

        const matchesSearch =
          !search ||
          doctorName.includes(search) ||
          doctorId.includes(search);

        const matchesSpecialisation =
          !this.selectedSpecialisation ||
          doctorSpecialisation === this.selectedSpecialisation;

        return matchesSearch && matchesSpecialisation;
      });
  }

  private buildSpecialisationOptions(): void {
    const options =
      new Map<string, string>();

    this.doctors.forEach((doctor) => {
      const value =
        String(doctor.specialisation ?? '');

      if (!value) {
        return;
      }

      options.set(
        value,
        this.getSpecialisationName(doctor.specialisation)
      );
    });

    this.specialisations = [
      {
        value: '',
        label: 'All Specialisations'
      },
      ...Array.from(options.entries()).map(([value, label]) => ({
        value,
        label
      }))
    ];
  }

  getSpecialisationName(value: number | string): string {
    if (value === null || value === undefined) {
      return 'Not specified';
    }

    if (typeof value === 'string' && isNaN(Number(value))) {
      return this.formatSpecialisationText(value);
    }

    const specialisationMap: Record<number, string> = {
      0: 'Cardiology',
      1: 'General Medicine',
      2: 'Dermatology',
      3: 'Pediatrics',
      4: 'Orthopedics',
      5: 'Neurology'
    };

    const numberValue =
      Number(value);

    return specialisationMap[numberValue] ??
      `Specialisation ${value}`;
  }

  private formatSpecialisationText(value: string): string {
    return value
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .trim();
  }

  formatFee(value: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(value);
  }
}