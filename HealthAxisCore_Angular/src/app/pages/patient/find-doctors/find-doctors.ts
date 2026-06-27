import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface DoctorCard {
  doctorId: number;
  doctorName: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

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
  searchText = '';

  selectedSpecialisation = '';

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

  doctors: DoctorCard[] = [
    {
      doctorId: 1,
      doctorName: 'Dr. Isha Nair',
      specialisation: 'Dermatologist',
      yearsOfExperience: 10,
      consultationFee: 1000,
      isActive: true
    },
    {
      doctorId: 2,
      doctorName: 'Dr. Aravind Menon',
      specialisation: 'Cardiologist',
      yearsOfExperience: 14,
      consultationFee: 1200,
      isActive: true
    },
    {
      doctorId: 3,
      doctorName: 'Dr. Meera Thomas',
      specialisation: 'Pediatrician',
      yearsOfExperience: 8,
      consultationFee: 800,
      isActive: true
    },
    {
      doctorId: 4,
      doctorName: 'Dr. Nikhil Rao',
      specialisation: 'Neurologist',
      yearsOfExperience: 12,
      consultationFee: 1500,
      isActive: false
    }
  ];

  get filteredDoctors(): DoctorCard[] {
    return this.doctors.filter(doctor => {
      const matchesSearch =
        !this.searchText ||
        doctor.doctorName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        doctor.specialisation.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesSpecialisation =
        !this.selectedSpecialisation ||
        doctor.specialisation === this.selectedSpecialisation;

      return matchesSearch && matchesSpecialisation;
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedSpecialisation = '';
  }
}
