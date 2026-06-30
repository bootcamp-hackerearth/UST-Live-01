import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { ChangePasswordRequest } from '../../core/models/auth.model';
import { Gender } from '../../core/models/gender.enum';
import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-patient-profile',
  imports: [ReactiveFormsModule, RouterLink, DatePipe],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientProfile {
  private readonly formBuilder = inject(FormBuilder);
  private readonly patientService = inject(PatientService);
  private readonly authService = inject(AuthService);

  readonly patient = signal<Patient | null>(null);
  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly changingPassword = signal(false);
  readonly isEditing = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly passwordErrorMessage = signal('');
  readonly passwordSuccessMessage = signal('');

  readonly genderOptions = [
    {
      label: 'Male',
      value: Gender.Male
    },
    {
      label: 'Female',
      value: Gender.Female
    },
    {
      label: 'Other',
      value: Gender.Other
    }
  ] as const;

  readonly profileForm = this.formBuilder.nonNullable.group({
    fullName: [
      '',
      [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(80),
        Validators.pattern(/^[A-Za-z ]+$/)
      ]
    ],
    dateOfBirth: [
      '',
      [
        Validators.required,
        PatientProfile.noFutureDateValidator
      ]
    ],
    gender: ['', [Validators.required]],
    phoneNumber: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[6-9][0-9]{9}$/)
      ]
    ],
    email: ['', [Validators.required, Validators.email]]
  });

  readonly passwordForm = this.formBuilder.nonNullable.group(
    {
      currentPassword: ['', [Validators.required]],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/)
        ]
      ],
      confirmPassword: ['', [Validators.required]]
    },
    {
      validators: PatientProfile.passwordsMatchValidator
    }
  );

  constructor() {
    this.loadProfile();
    this.profileForm.disable();
  }

  get fullName() {
    return this.profileForm.controls.fullName;
  }

  get dateOfBirth() {
    return this.profileForm.controls.dateOfBirth;
  }

  get gender() {
    return this.profileForm.controls.gender;
  }

  get phoneNumber() {
    return this.profileForm.controls.phoneNumber;
  }

  get email() {
    return this.profileForm.controls.email;
  }

  get currentPassword() {
    return this.passwordForm.controls.currentPassword;
  }

  get newPassword() {
    return this.passwordForm.controls.newPassword;
  }

  get confirmPassword() {
    return this.passwordForm.controls.confirmPassword;
  }

  loadProfile(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.patientService.getMyProfile().subscribe({
      next: (patient) => {
        this.patient.set(patient);
        this.patchProfileForm(patient);
        this.profileForm.disable();
        this.isEditing.set(false);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load patient profile.')
        );
      }
    });
  }

  enableEdit(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.profileForm.enable();
    this.isEditing.set(true);
  }

  cancelEdit(): void {
    const currentPatient = this.patient();

    if (currentPatient) {
      this.patchProfileForm(currentPatient);
    }

    this.profileForm.disable();
    this.isEditing.set(false);
    this.errorMessage.set('');
    this.successMessage.set('');
  }

  updateProfile(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      this.errorMessage.set('Please correct the highlighted profile details.');
      return;
    }

    const currentPatient = this.patient();

    if (!currentPatient) {
      this.errorMessage.set('Profile data not loaded. Please refresh the page.');
      return;
    }

    const formValue = this.profileForm.getRawValue();

    const updatedPatient: Patient = {
      ...currentPatient,
      fullName: formValue.fullName.trim(),
      dateOfBirth: formValue.dateOfBirth,
      gender: formValue.gender,
      phoneNumber: formValue.phoneNumber.trim(),
      email: formValue.email.trim()
    };

    this.saving.set(true);

    this.patientService.updateMyProfile(updatedPatient).subscribe({
      next: (patient) => {
        this.patient.set(patient);
        this.patchProfileForm(patient);
        this.profileForm.disable();
        this.isEditing.set(false);
        this.saving.set(false);
        this.successMessage.set('Profile updated successfully.');
      },
      error: (error: unknown) => {
        this.saving.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not update patient profile.')
        );
      }
    });
  }

  changePassword(): void {
    this.passwordErrorMessage.set('');
    this.passwordSuccessMessage.set('');

    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      this.passwordErrorMessage.set('Please correct the password details.');
      return;
    }

    const formValue = this.passwordForm.getRawValue();

    if (formValue.currentPassword === formValue.newPassword) {
      this.passwordErrorMessage.set(
        'New password must be different from current password.'
      );
      return;
    }

    const request: ChangePasswordRequest = {
  currentPassword: formValue.currentPassword,
  newPassword: formValue.newPassword,
  confirmNewPassword: formValue.confirmPassword
};

    this.changingPassword.set(true);

    this.authService.changePassword(request).subscribe({
      next: () => {
        this.changingPassword.set(false);
        this.passwordForm.reset();
        this.passwordSuccessMessage.set('Password changed successfully.');
      },
      error: (error: unknown) => {
        this.changingPassword.set(false);
        this.passwordErrorMessage.set(
          getFriendlyErrorMessage(error, 'Could not change password.')
        );
      }
    });
  }

  private patchProfileForm(patient: Patient): void {
    this.profileForm.patchValue({
      fullName: patient.fullName,
      dateOfBirth: patient.dateOfBirth.split('T')[0],
      gender: patient.gender,
      phoneNumber: patient.phoneNumber,
      email: patient.email
    });
  }

  private static passwordsMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return {
        passwordMismatch: true
      };
    }

    return null;
  }

  private static noFutureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate = new Date(control.value);
    const today = new Date();

    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    if (selectedDate > today) {
      return {
        futureDate: true
      };
    }

    return null;
  }
}