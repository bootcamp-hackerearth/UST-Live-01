import {
  ChangeDetectorRef,
  Component,
  OnInit
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { DoctorService } from '../../../core/services/doctor-service';
import { Doctor } from '../../../core/models/doctor';

@Component({
  selector: 'app-view-doctors',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule
  ],
  templateUrl: './view-doctors.html',
  styleUrl: './view-doctors.css'
})
export class ViewDoctors implements OnInit {

  doctors: Doctor[] = [];

  filteredDoctors: Doctor[] = [];

  search = '';

  loading = true;

  constructor(
    private doctorService: DoctorService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {

    this.loadDoctors();

  }

  loadDoctors(): void {

    this.loading = true;

    this.doctorService.getAllDoctors().subscribe({

      next: (data) => {

        console.log('Doctors received:', data);

        this.doctors = data.filter(x => x.isActive);

        // Create a new array reference
        this.filteredDoctors = [...this.doctors];

        this.loading = false;

        // Force Angular to refresh the UI
        this.cdr.detectChanges();

      },

      error: (err) => {

        console.error(err);

        this.loading = false;

        this.cdr.detectChanges();

      }

    });

  }

  filterDoctors(): void {

    const keyword = this.search.trim().toLowerCase();

    if (!keyword) {

      this.filteredDoctors = [...this.doctors];

      return;

    }

    this.filteredDoctors = this.doctors.filter(x =>

      x.fullName.toLowerCase().includes(keyword) ||

      this.getSpecialisationName(x.specialisation)
        .toLowerCase()
        .includes(keyword)

    );

  }

  getSpecialisationName(value: number): string {

    switch (value) {

      case 0:
        return 'Endocrinologist';

      case 1:
        return 'Oncologist';

      case 2:
        return 'Gynecologist';

      case 3:
        return 'Orthopedic Surgeon';

      case 4:
        return 'Psychiatrist';

      case 5:
        return 'Pediatrician';

      case 6:
        return 'Neurologist';

      case 7:
        return 'Dermatologist';

      case 8:
        return 'Cardiologist';

      case 9:
        return 'General Practitioner';

      default:
        return 'Specialist';

    }

  }

}