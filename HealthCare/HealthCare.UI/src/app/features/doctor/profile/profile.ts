import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class  DoctorProfileComponent implements OnInit {

  profileForm!: FormGroup;

  patientId = 0;

  loading = false;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService
  ) { }

  ngOnInit(): void {

    this.profileForm = this.fb.group({

      firstName: ['', Validators.required],

      email: [{ value: '', disabled: true }],

      phoneNumber: ['', Validators.required],

      gender: ['', Validators.required],

      dateofbirth: ['', Validators.required]

    });

    this.loadProfile();
  }

  loadProfile() {

    this.loading = true;

    this.patientService.getProfile()
      .subscribe({

        next: (res: any) => {

          this.patientId = res.patientId;

          this.profileForm.patchValue(res);

          this.loading = false;
        },

        error: () => {

          this.loading = false;
        }

      });
  }

  updateProfile() {

    if (this.profileForm.invalid)
      return;

    this.patientService.updateProfile(
      this.profileForm.getRawValue()
    )
    .subscribe({

      next: () => {

        alert('Profile updated successfully');
      },

      error: () => {

        alert('Unable to update profile');
      }

    });
  }

}