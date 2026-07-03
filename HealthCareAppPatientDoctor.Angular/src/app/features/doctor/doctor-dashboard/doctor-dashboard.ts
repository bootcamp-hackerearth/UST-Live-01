import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';

import { DoctorService } from '../../../core/services/doctor-service';
import { DoctorProfile } from '../../../core/models/doctor-profile';
import { AuthService } from '../../../core/services/auth-service';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard implements OnInit {

  private doctorService = inject(DoctorService);

  private authService = inject(AuthService);

  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  doctor?: DoctorProfile;

  isLoading = false;

  ngOnInit(): void {

    this.loadProfile();

  }

  loadProfile(): void {

    this.isLoading = true;

    this.doctorService
      .getMyProfile()
      .subscribe({

        next: (response) => {

          this.doctor = response;

          this.isLoading = false;

            this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;
        

        }

      });

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

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