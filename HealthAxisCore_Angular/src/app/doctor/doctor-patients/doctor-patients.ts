import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface DoctorPatient {
  patientId: number;
  patientName: string;
  email: string;
  phoneNumber: string;
  lastVisit: string;
  totalVisits: number;
}

@Component({
  selector: 'app-doctor-patients',
  imports: [
    RouterLink,
    FormsModule
  ],
  templateUrl: './doctor-patients.html',
  styleUrl: './doctor-patients.css'
})
export class DoctorPatients {
  searchText = '';

  patients: DoctorPatient[] = [
    {
      patientId: 101,
      patientName: 'Arun Menon',
      email: 'arun.patient@healthaxis.com',
      phoneNumber: '9876543210',
      lastVisit: '2026-06-20',
      totalVisits: 4
    },
    {
      patientId: 102,
      patientName: 'Meera Thomas',
      email: 'meera.patient@healthaxis.com',
      phoneNumber: '9876543211',
      lastVisit: '2026-06-18',
      totalVisits: 2
    },
    {
      patientId: 103,
      patientName: 'Nikhil Rao',
      email: 'nikhil.patient@healthaxis.com',
      phoneNumber: '9876543212',
      lastVisit: '2026-06-12',
      totalVisits: 6
    }
  ];

  get filteredPatients(): DoctorPatient[] {
    return this.patients.filter(patient =>
      !this.searchText ||
      patient.patientName.toLowerCase().includes(this.searchText.toLowerCase()) ||
      patient.email.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }
}
