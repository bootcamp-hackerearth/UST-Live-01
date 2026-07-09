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

  currentPage = 1;
  pageSize = 5;
  totalRecords = 0;
  totalPages = 0;

  specialisations = [
    { value: '', label: 'All specialisations' },
    { value: 0, label: 'General Medicine' },
    { value: 1, label: 'Pediatrician' },
    { value: 2, label: 'Cardiology' },
    { value: 3, label: 'Dermatology' },
    { value: 4, label: 'Orthopaedics' }
  ];

  constructor(
    private readonly router: Router,
    private readonly doctorService: DoctorService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadDoctors();
    }
  }

  loadDoctors() {
    this.doctorService.getDoctors(this.currentPage, this.pageSize).subscribe({
      next: (res: any) => {
        console.log('Doctors paged response ✅:', res);

        this.doctors = res.items || res.data || [];
        this.currentPage = res.pageNumber || this.currentPage;
        this.pageSize = res.pageSize || this.pageSize;
        this.totalRecords = res.totalRecords || 0;
        this.totalPages = res.totalPages || 0;

        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Doctor load failed ❌:', err);
      }
    });
  }

  getSpecialisationName(value: number): string {
    switch (Number(value)) {
      case 0: return 'General Medicine';
      case 1: return 'Pediatrician';
      case 2: return 'Cardiology';
      case 3: return 'Dermatology';
      case 4: return 'Orthopaedics';
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

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.loadDoctors();
  }

  nextPage() {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
    this.loadDoctors();
  }

  previousPage() {
    if (this.currentPage <= 1) {
      return;
    }

    this.currentPage--;
    this.loadDoctors();
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }
}