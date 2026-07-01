import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MockAuth } from '../../../services/mock-auth';
import {
  HealthRecordDto,
  MockPatientData,
  PatientDto
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-patient-health-records',
  imports: [RouterLink],
  templateUrl: './patient-health-records.html',
  styleUrl: './patient-health-records.css',
})
export class PatientHealthRecords implements OnInit {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);

  patient?: PatientDto;
  healthRecords: HealthRecordDto[] = [];

  ngOnInit(): void {
    if (!this.auth.isLoggedIn() || !this.auth.isPatient()) {
      this.router.navigate(['/login']);
      return;
    }

    this.patient = this.auth.getCurrentPatient();

    if (!this.patient) {
      this.router.navigate(['/login']);
      return;
    }

    this.healthRecords = this.patientData.getHealthRecordsByPatient(this.patient.patientId);
  }

  getDoctorSpecialisation(doctorId: number): string {
    return this.patientData.getDoctorSpecialisation(doctorId) ?? 'Specialisation unavailable';
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}