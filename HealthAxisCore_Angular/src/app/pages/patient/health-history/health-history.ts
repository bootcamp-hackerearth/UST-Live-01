import { Component } from '@angular/core';

interface HealthRecord {
  recordId: number;
  visitDate: string;
  doctorName: string;
  specialisation: string;
  diagnosis: string;
  prescription: string;
  notes: string;
}

@Component({
  selector: 'app-health-history',
  imports: [],
  templateUrl: './health-history.html',
  styleUrl: './health-history.css'
})
export class HealthHistory {
  records: HealthRecord[] = [
    {
      recordId: 1,
      visitDate: '2026-06-20',
      doctorName: 'Dr. Isha Nair',
      specialisation: 'Dermatologist',
      diagnosis: 'Skin allergy',
      prescription: 'Antihistamine medication',
      notes: 'Avoid irritants and follow up if symptoms continue.'
    },
    {
      recordId: 2,
      visitDate: '2026-05-16',
      doctorName: 'Dr. Aravind Menon',
      specialisation: 'Cardiologist',
      diagnosis: 'Routine checkup',
      prescription: 'Lifestyle management',
      notes: 'Continue regular exercise and balanced diet.'
    }
  ];
}
