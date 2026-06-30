import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

interface PatientHealthRecord {
  recordId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes: string;
}

@Component({
  selector: 'app-doctor-patient-profile',
  imports: [
    RouterLink
  ],
  templateUrl: './doctor-patient-profile.html',
  styleUrl: './doctor-patient-profile.css'
})
export class DoctorPatientProfile {
  patientId = 0;

  patient = {
    patientId: 101,
    patientName: 'Arun Menon',
    email: 'arun.patient@healthaxis.com',
    phoneNumber: '9876543210',
    dateOfBirth: '1998-04-15',
    gender: 'Male',
    insuranceID: 'INS-PAT-001'
  };

  records: PatientHealthRecord[] = [
    {
      recordId: 1,
      visitDate: '2026-06-20',
      diagnosis: 'Skin allergy',
      prescription: 'Antihistamine medication',
      notes: 'Avoid irritants and follow up if symptoms continue.'
    },
    {
      recordId: 2,
      visitDate: '2026-05-12',
      diagnosis: 'Routine consultation',
      prescription: 'Lifestyle management',
      notes: 'Continue regular exercise and balanced diet.'
    }
  ];

  constructor(private route: ActivatedRoute) {
    this.patientId = Number(this.route.snapshot.paramMap.get('patientId')) || 101;
  }
}
