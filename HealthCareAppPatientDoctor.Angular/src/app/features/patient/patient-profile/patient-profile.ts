import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

import { PatientService } from '../../../core/services/patient-service';
import { PatientProfile } from '../../../core/models/patient-profile';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfileComponent implements OnInit {

  private patientService = inject(PatientService);

  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);

  patient?: PatientProfile;

  isLoading = false;

  ngOnInit(): void {

    this.loadProfile();

  }

  loadProfile(): void {

    this.isLoading = true;

    this.patientService
      .getMyPatientProfile()
      .subscribe({

        next: response => {

          this.patient = response;

          this.isLoading = false;

          this.cdr.detectChanges();

        },

        error: err => {

          console.log(err);

          this.isLoading = false;

          this.cdr.detectChanges();

        }

      });

  }

  editProfile(): void {

    this.router.navigate([
        '/patient/profile/edit'
    ]);

}

  getGenderName(value: number): string {

  switch (value) {

    case 0:
      return 'Male';

    case 1:
      return 'Female';

    case 2:
      return 'Other';

    default:
      return 'Unknown';

  }

}

}