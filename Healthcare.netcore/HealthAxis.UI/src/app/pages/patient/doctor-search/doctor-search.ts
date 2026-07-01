import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DoctorService } from '../../../services/doctor';

@Component({
  selector: 'app-doctor-search',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './doctor-search.html',
  styleUrls: ['./doctor-search.css']
})
export class DoctorSearch implements OnInit {

  doctors: any[] = [];

  searchText = '';
  selectedSpecialisation = '';
  onlyAvailable = true;
  sortBy: 'experience' | 'fee' = 'experience';

  specialisations = [
    { value: '', label: 'All specialisations' },
    { value: 0, label: 'General Medicine' },
    { value: 1, label: 'Pediatrician' },
    { value: 2, label: 'Cardiologist' },
    { value: 3, label: 'Dermatologist' },
    { value: 4, label: 'Neurologist' }
  ];

  constructor(
    private router: Router,
    private doctorService: DoctorService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadDoctors();
    }
  }

  loadDoctors() {
    this.doctorService.getDoctors().subscribe({
      next: (res: any) => {
        console.log('API RESPONSE ✅:', res);
        this.doctors = res.items || res.data || [];
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('API ERROR ❌:', err);
      }
    });
  }

  getSpecialisationName(value: number): string {
    switch (value) {
      case 0: return 'General Medicine';
      case 1: return 'Pediatrician';
      case 2: return 'Cardiologist';
      case 3: return 'Dermatologist';
      case 4: return 'Neurologist';
      default: return 'Other';
    }
  }

  filteredDoctors() {
    let result = this.doctors.filter((d: any) => {

      const matchesSearch =
        !this.searchText ||
        d.fullName.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesSpec =
        this.selectedSpecialisation === '' ||
        d.specialisation == this.selectedSpecialisation;

      const matchesAvailability =
        !this.onlyAvailable || d.isActive === true;

      return matchesSearch && matchesSpec && matchesAvailability;
    });

    if (this.sortBy === 'experience') {
      result = result.sort((a: any, b: any) =>
        b.yearsOfExperience - a.yearsOfExperience
      );
    }

    if (this.sortBy === 'fee') {
      result = result.sort((a: any, b: any) =>
        a.consultationFee - b.consultationFee
      );
    }

    return result;
  }

  clearFilters() {
    this.searchText = '';
    this.selectedSpecialisation = '';
    this.onlyAvailable = true;
    this.sortBy = 'experience';
  }

  selectDoctor(d: any) {
    this.router.navigate(['/patient/book'], {
      state: { doctor: d }
    });
  }
}