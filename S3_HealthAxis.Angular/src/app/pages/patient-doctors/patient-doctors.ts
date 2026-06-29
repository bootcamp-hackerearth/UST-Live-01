import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

import { DoctorService } from '../../core/services/doctor.service';

import {
  Doctor,
  DoctorSpecialisation,
  DoctorSpecialisationOption
} from '../../shared/models/doctor.models';

@Component({
  selector: 'app-patient-doctors',
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './patient-doctors.html',
  styleUrl: './patient-doctors.css'
})
export class PatientDoctors implements OnInit {
  loading = true;
  errorMessage = '';

  doctors: Doctor[] = [];
  searchText = '';
  selectedSpecialisation: number | null = null;

  specialisations: DoctorSpecialisationOption[] = [
    {
      value: DoctorSpecialisation.GeneralPractitioner,
      label: 'General Practitioner'
    },
    {
      value: DoctorSpecialisation.Cardiologist,
      label: 'Cardiologist'
    },
    {
      value: DoctorSpecialisation.Dermatologist,
      label: 'Dermatologist'
    },
    {
      value: DoctorSpecialisation.Neurologist,
      label: 'Neurologist'
    },
    {
      value: DoctorSpecialisation.Pediatrician,
      label: 'Pediatrician'
    },
    {
      value: DoctorSpecialisation.Psychiatrist,
      label: 'Psychiatrist'
    },
    {
      value: DoctorSpecialisation.OrthopedicSurgeon,
      label: 'Orthopedic Surgeon'
    },
    {
      value: DoctorSpecialisation.Gynecologist,
      label: 'Gynecologist'
    },
    {
      value: DoctorSpecialisation.Oncologist,
      label: 'Oncologist'
    },
    {
      value: DoctorSpecialisation.Endocrinologist,
      label: 'Endocrinologist'
    }
  ];

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors(): void {
    this.loading = true;
    this.errorMessage = '';

    this.doctorService
      .getDoctors('name', this.selectedSpecialisation)
      .subscribe({
        next: (doctors) => {
          this.doctors = doctors ?? [];
          this.loading = false;
        },
        error: (error) => {
          this.loading = false;

          if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You are not authorized to view doctors. Please login again.';
            return;
          }

          if (error.status === 0) {
            this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
            return;
          }

          this.errorMessage = 'Could not load doctors. Please try again.';
        }
      });
  }

  applySpecialisationFilter(): void {
    this.loadDoctors();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedSpecialisation = null;
    this.loadDoctors();
  }

  get filteredDoctors(): Doctor[] {
    const search = this.searchText.trim().toLowerCase();

    return this.doctors.filter((doctor) => {
      const matchesSearch =
        !search ||
        doctor.fullName.toLowerCase().includes(search) ||
        this.specialisationText(doctor.specialisation).toLowerCase().includes(search);

      return matchesSearch;
    });
  }

  specialisationText(value: number): string {
    const item = this.specialisations.find(option => option.value === value);

    return item?.label ?? 'Specialist';
  }

  activeDoctorsCount(): number {
    return this.doctors.filter(doctor => doctor.isActive).length;
  }

  trackDoctor(_: number, doctor: Doctor): number {
    return doctor.doctorId;
  }
}


