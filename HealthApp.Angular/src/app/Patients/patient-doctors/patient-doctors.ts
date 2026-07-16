import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/sidebar/sidebar';
import { Router } from '@angular/router';


import { DoctorService } from '../../Patient.service/patient-doctorservice';
import { Doctor } from '../../models/doctor/doctor.model';

@Component({
  selector: 'app-patient-doctors',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './patient-doctors.html',
  styleUrls: ['./patient-doctors.css']
})
export class PatientDoctors implements OnInit {

  allDoctors = signal<Doctor[]>([]);
  filteredDoctors = signal<Doctor[]>([]);
  paginatedDoctors = signal<Doctor[]>([]);

  nameSearch: string = '';
  specialisationSearch: string = '';

  pageNumber = signal(1);
  pageSize = 6;
  totalPages = signal(0);

  constructor(
    private doctorService: DoctorService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadDoctors();
  }

  loadDoctors() {
    this.doctorService.getActiveDoctors(1, 1000)
      .subscribe({
        next: (res: any) => {
          this.allDoctors.set(res.data || res);
          this.filterDoctors();
        },
        error: (err) => {
          console.error('Failed to load doctors', err);
        }
      });
  }

  filterDoctors() {
    this.filteredDoctors.set(this.allDoctors().filter(d =>
      (!this.specialisationSearch || d.specialisation === this.specialisationSearch) &&
      (!this.nameSearch || d.fullName?.toLowerCase().includes(this.nameSearch.toLowerCase()))
    ));

    this.pageNumber.set(1);
    this.calculateTotalPages();
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedDoctors.set(this.filteredDoctors().slice(start, end));
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(value => value + 1);
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(value => value - 1);
      this.paginate();
    }
  }

  calculateTotalPages() {
    this.totalPages.set(Math.ceil(this.filteredDoctors().length / this.pageSize));
  }

  getExperience(startDate?: Date): number {
    if (!startDate) return 0;

    const start = new Date(startDate);
    const today = new Date();

    let years = today.getFullYear() - start.getFullYear();

    const monthDiff = today.getMonth() - start.getMonth();

    if (
      monthDiff < 0 ||
      (monthDiff === 0 && today.getDate() < start.getDate())
    ) {
      years--;
    }

    return years < 0 ? 0 : years;
  }

  book(doctor: any) {
    console.log("Selected doctor:", doctor);

    localStorage.setItem('selectedDoctor', JSON.stringify(doctor));

    this.router.navigate(['/appointments']);
  }

}
