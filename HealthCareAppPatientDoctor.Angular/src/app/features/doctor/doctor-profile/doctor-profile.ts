import {
  ChangeDetectorRef,
  Component,
  inject,
  OnInit
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { Router, RouterModule } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { DoctorService } from '../../../core/services/doctor-service';

import { DoctorProfile as DoctorProfileModel } from '../../../core/models/doctor-profile';

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile implements OnInit {

  private doctorService = inject(DoctorService);

  private toastr = inject(ToastrService);

  private router = inject(Router);

  private cdr = inject(ChangeDetectorRef);

  doctor?: DoctorProfileModel;

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

          this.toastr.error(
            'Unable to load doctor profile.'
          );

          this.cdr.detectChanges();

        }

      });

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

  back(): void {

    this.router.navigate([
      '/doctor'
    ]);

  }

}