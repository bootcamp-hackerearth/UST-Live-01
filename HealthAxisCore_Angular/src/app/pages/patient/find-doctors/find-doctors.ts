import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { DoctorService } from '../../../core/services/doctor.service';
import { DoctorDto } from '../../../core/models/doctor.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-find-doctors',
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './find-doctors.html',
  styleUrl: './find-doctors.css'
})
export class FindDoctors {
  doctors = signal<DoctorDto[]>([]);

  searchText = signal('');

  selectedSpecialisation = signal('');

  isLoading = signal(false);

  errorMessage = signal('');

  specialisations = [
    'Cardiologist',
    'Dermatologist',
    'Neurologist',
    'Pediatrician',
    'Psychiatrist',
    'GeneralPractitioner',
    'Endocrinologist',
    'Oncologist',
    'Gynecologist',
    'OrthopedicSurgeon'
  ];

  constructor(
    private readonly doctorService: DoctorService,
    private readonly authService: AuthService
  ) {
    this.loadDoctors();
  }

  filteredDoctors(): DoctorDto[] {
    return this.doctors().filter(doctor => {
      const search = this.searchText().toLowerCase();

      const matchesSearch =
        !search ||
        doctor.doctorName.toLowerCase().includes(search) ||
        doctor.specialisation.toLowerCase().includes(search);

      return matchesSearch;
    });
  }

  loadDoctors(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.doctorService.getDoctors(this.selectedSpecialisation()).subscribe({
      next: doctors => {
        this.doctors.set(doctors);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  onSpecialisationChange(): void {
    this.loadDoctors();
  }

  clearFilters(): void {
    this.searchText.set('');
    this.selectedSpecialisation.set('');
    this.loadDoctors();
  }
}
