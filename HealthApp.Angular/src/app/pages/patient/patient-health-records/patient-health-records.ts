import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { HealthRecord } from '../../../core/models/health-record.model';
import { Patient } from '../../../core/models/patient.model';
import { PatientApiService } from '../../../core/services/patient-api.service';

@Component({
  selector: 'app-patient-health-records',
  imports: [RouterLink],
  templateUrl: './patient-health-records.html',
  styleUrl: './patient-health-records.css',
})
export class PatientHealthRecords implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly authService = inject(AuthService);
  private readonly patientApi = inject(PatientApiService);
  private readonly router = inject(Router);

  patient?: Patient;
  healthRecords: HealthRecord[] = [];

  ngOnInit(): void {
    const user = this.authService.currentUser();

    if (!user?.patientId) {
      this.router.navigate(['/login']);
      return;
    }

    this.patientApi.getPatientById(user.patientId).subscribe({
      next: patient => {
        this.patient = patient;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error(error);
      }
    });

    this.patientApi.getPatientHealthRecords(user.patientId).subscribe({
      next: records => {
        this.healthRecords = records;
        this.cdr.detectChanges();
      },
      error: error => {
        console.error(error);
      }
    });
  }

  getDoctorSpecialisation(): string {
    return '';
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }
}