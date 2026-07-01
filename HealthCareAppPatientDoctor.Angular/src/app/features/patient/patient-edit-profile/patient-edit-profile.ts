import {
  ChangeDetectorRef,
  Component,
  inject,
  OnInit
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { PatientService } from '../../../core/services/patient-service';

import { UpdatePatient } from '../../../core/models/update-patient';

@Component({
  selector: 'app-patient-edit-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './patient-edit-profile.html',
  styleUrl: './patient-edit-profile.css'
})
export class PatientEditProfile implements OnInit {

  private fb = inject(FormBuilder);

  private patientService = inject(PatientService);

  private toastr = inject(ToastrService);

  private router = inject(Router);

  private cdr = inject(ChangeDetectorRef);

  isLoading = false;

  profileForm = this.fb.group({

    fullName: [
      '',
      [
        Validators.required,
        Validators.maxLength(100)
      ]
    ],

    dateOfBirth: [
      '',
      Validators.required
    ],

    gender: [
      0,
      Validators.required
    ],

    phoneNumber: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[0-9]{10}$/)
      ]
    ],

    email: [
      {
        value: '',
        disabled: true
      }
    ],

    insuranceId: [
      '',
      [
        Validators.required,
        Validators.maxLength(30)
      ]
    ]

  });

  ngOnInit(): void {

    this.loadProfile();

  }

  loadProfile(): void {

    this.isLoading = true;

    this.patientService
      .getMyPatientProfile()
      .subscribe({

        next: (response) => {

          this.profileForm.patchValue({

            fullName: response.fullName,

            dateOfBirth: response.dateOfBirth,

            gender: response.gender,

            phoneNumber: response.phoneNumber,

            email: response.email,

            insuranceId: response.insuranceId

          });

          this.isLoading = false;

          this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;

          this.toastr.error(
            'Unable to load profile.'
          );

        }

      });

  }

  save(): void {

    if (this.profileForm.invalid) {

      this.profileForm.markAllAsTouched();

      return;

    }

    this.isLoading = true;

    const request: UpdatePatient = {

      fullName:
        this.profileForm.get('fullName')?.value ?? '',

      dateOfBirth:
        this.profileForm.get('dateOfBirth')?.value ?? '',

      gender:
        Number(this.profileForm.get('gender')?.value),

      phoneNumber:
        this.profileForm.get('phoneNumber')?.value ?? '',
        email:
    this.profileForm.getRawValue().email ?? '',

      insuranceId:
        this.profileForm.get('insuranceId')?.value ?? ''

    };

    this.patientService
      .updateMyProfile(request)
      .subscribe({

        next: () => {

          this.isLoading = false;

          this.toastr.success(
            'Profile updated successfully.'
          );

          this.router.navigate([
            '/patient/profile'
          ]);

            this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;

          this.toastr.error(
            err?.error?.message ??
            'Unable to update profile.'
          );

            this.cdr.detectChanges();

        }

      });

  }

  cancel(): void {

    this.router.navigate([
      '/patient/profile'
    ]);

  }

}