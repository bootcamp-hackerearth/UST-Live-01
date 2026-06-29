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
export class ProfileComponent implements OnInit {

  profileForm!: FormGroup;

  loading = false;
  saving = false;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.profileForm = this.fb.group({

      fullName: ['', Validators.required],

      email: [{ value: '', disabled: true }],

      gender: ['', Validators.required],

      dateOfBirth: ['', Validators.required],

    });

    this.loadProfile();
  }

  loadProfile() {

    this.loading = true;

    this.patientService.getProfile()
      .subscribe({

        next: (res: any) => {

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

    this.saving = true;

    this.patientService.updateProfile(
      this.profileForm.getRawValue()
    )
    .subscribe({

      next: () => {

        this.saving = false;

        alert('Profile updated successfully');
      },

      error: () => {

        this.saving = false;

        alert('Unable to update profile');
      }
    });
  }

}