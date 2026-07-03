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

import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

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

  today = new Date().toISOString().split('T')[0];

  patientInitials = 'HA';


  profileForm = this.fb.group({

    fullName: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100),
        Validators.pattern(/^[A-Za-z. ]+$/)
      ]
    ],

    dateOfBirth: [
      '',
      [
        Validators.required,
        this.futureDateValidator()
      ]
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
      },
      [
        Validators.required,
        Validators.email,
        Validators.pattern(
          /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[A-Za-z]{2,}$/
        )
      ]
    ],
    insuranceId: [
      '',
      [
        Validators.required,
        Validators.maxLength(30),
        Validators.pattern(/^[A-Za-z0-9]+$/)
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

          this.patientInitials =
            response.fullName
              .split(' ')
              .map(x => x[0])
              .slice(0, 2)
              .join('')
              .toUpperCase();

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

  futureDateValidator(): ValidatorFn {

    return (control: AbstractControl): ValidationErrors | null => {

      if (!control.value)
        return null;

      const selected = new Date(control.value);

      const today = new Date();

      today.setHours(0, 0, 0, 0);

      if (selected > today) {

        return {
          futureDate: true
        };

      }

      return null;

    };

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

     if (this.profileForm.dirty) {

    const leave =
      confirm(
        'You have unsaved changes. Leave this page?'
      );

    if (!leave)
      return;

  }


    this.router.navigate([
      '/patient/profile'
    ]);

  }

  allowOnlyNumbers(event: KeyboardEvent): void {

    const charCode = event.which ?? event.keyCode;

    if (charCode < 48 || charCode > 57) {

      event.preventDefault();

    }

  }

  preventInvalidPaste(event: ClipboardEvent): void {

    const text =
      event.clipboardData?.getData('text') ?? '';

    if (!/^\d+$/.test(text)) {

      event.preventDefault();

    }

  }

}