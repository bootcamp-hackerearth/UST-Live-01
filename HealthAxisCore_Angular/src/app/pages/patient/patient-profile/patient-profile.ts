import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { PatientSession } from '../../../services/patient-session';

@Component({
  selector: 'app-patient-profile',
  imports: [
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfile {
  profileForm: FormGroup;

  submitted = false;

  successMessage = '';

  constructor(
    private formBuilder: FormBuilder,
    private patientSession: PatientSession
  ) {
    const patient = this.patientSession.getCurrentPatient();

    this.profileForm = this.formBuilder.group({
      patientId: [patient.patientId],
      fullName: [
        patient.fullName,
        [
          Validators.required,
          Validators.minLength(2)
        ]
      ],
      email: [
        patient.email,
        [
          Validators.required,
          Validators.email
        ]
      ],
      phoneNumber: [
        patient.phoneNumber,
        [
          Validators.required,
          Validators.pattern(/^[0-9]{10}$/)
        ]
      ],
      dateOfBirth: [
        patient.dateOfBirth,
        [
          Validators.required
        ]
      ],
      gender: [
        patient.gender,
        [
          Validators.required
        ]
      ],
      insuranceID: [
        patient.insuranceID,
        [
          Validators.required
        ]
      ]
    });
  }

  saveProfile(): void {
    this.submitted = true;
    this.successMessage = '';

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.patientSession.updateCurrentPatient(this.profileForm.value);

    this.successMessage = 'Profile updated successfully. API connection will be added later.';
  }
}
