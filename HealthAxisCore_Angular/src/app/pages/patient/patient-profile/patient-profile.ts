import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { AuthService } from '../../../core/services/auth.service';
import { PatientService } from '../../../core/services/patient.service';
import { UpdatePatientRequest } from '../../../core/models/patient.model';

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

  passwordForm: FormGroup;

  isLoading = signal(false);

  isSaving = signal(false);

  isChangingPassword = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  passwordErrorMessage = signal('');

  passwordSuccessMessage = signal('');

  passwordSubmitted = signal(false);

  showCurrentPassword = signal(false);

  showNewPassword = signal(false);

  showConfirmPassword = signal(false);

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly patientService: PatientService
  ) {
    this.profileForm = this.formBuilder.group({
      patientName: [
        '',
        [
          Validators.required,
          Validators.minLength(2)
        ]
      ],
      email: [
        {
          value: '',
          disabled: true
        }
      ],
      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{10}$/)
        ]
      ],
      dateOfBirth: [
        '',
        [
          Validators.required
        ]
      ],
      gender: [
        '',
        [
          Validators.required
        ]
      ],
      insuranceID: ['']
    });

    this.passwordForm = this.formBuilder.group({
      currentPassword: [
        '',
        [
          Validators.required
        ]
      ],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8)
        ]
      ],
      confirmPassword: [
        '',
        [
          Validators.required
        ]
      ]
    });

    this.loadProfile();
  }

  get currentPassword() {
    return this.passwordForm.get('currentPassword');
  }

  get newPassword() {
    return this.passwordForm.get('newPassword');
  }

  get confirmPassword() {
    return this.passwordForm.get('confirmPassword');
  }

  loadProfile(): void {
    const patientId = this.authService.patientId();

    if (!patientId) {
      this.errorMessage.set('Patient ID missing. Please login again.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.patientService.getById(patientId).subscribe({
      next: patient => {
        this.profileForm.patchValue({
          patientName: patient.patientName,
          email: patient.email,
          phoneNumber: patient.phoneNumber,
          dateOfBirth: patient.dateOfBirth?.split('T')[0],
          gender: patient.gender,
          insuranceID: patient.insuranceID ?? ''
        });

        this.isLoading.set(false);
      },
      error: error => {
        this.isLoading.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  saveProfile(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    const patientId = this.authService.patientId();

    if (!patientId) {
      this.errorMessage.set('Patient ID missing. Please login again.');
      return;
    }

    const request: UpdatePatientRequest = {
      patientName: this.profileForm.value.patientName,
      dateOfBirth: this.profileForm.value.dateOfBirth,
      gender: this.profileForm.value.gender,
      phoneNumber: this.profileForm.value.phoneNumber,
      insuranceID: this.profileForm.value.insuranceID
    };

    this.isSaving.set(true);

    this.patientService.update(patientId, request).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.successMessage.set('Profile updated successfully.');
      },
      error: error => {
        this.isSaving.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  changePassword(): void {
    this.passwordSubmitted.set(true);
    this.passwordErrorMessage.set('');
    this.passwordSuccessMessage.set('');

    if (this.passwordForm.invalid || this.passwordsDoNotMatch()) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    this.isChangingPassword.set(true);

    this.authService.changePassword({
      currentPassword: this.passwordForm.value.currentPassword,
      newPassword: this.passwordForm.value.newPassword,
      confirmPassword: this.passwordForm.value.confirmPassword
    }).subscribe({
      next: () => {
        this.isChangingPassword.set(false);
        this.passwordSuccessMessage.set('Password changed successfully.');
        this.passwordForm.reset();
        this.passwordSubmitted.set(false);
      },
      error: error => {
        this.isChangingPassword.set(false);
        this.passwordErrorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  passwordsDoNotMatch(): boolean {
    const newPassword = this.passwordForm.value.newPassword;

    const confirmPassword = this.passwordForm.value.confirmPassword;

    return !!newPassword &&
      !!confirmPassword &&
      newPassword !== confirmPassword;
  }

  toggleCurrentPassword(): void {
    this.showCurrentPassword.update(value => !value);
  }

  toggleNewPassword(): void {
    this.showNewPassword.update(value => !value);
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword.update(value => !value);
  }
}
