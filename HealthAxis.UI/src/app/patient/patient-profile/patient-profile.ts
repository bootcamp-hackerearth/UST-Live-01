import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';

import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { AuthService } from '../../core/services/auth.service';
import { ChangePasswordRequest } from '../../core/models/auth.model';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-patient-profile',
  imports: [ReactiveFormsModule, DatePipe],
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
  readonly passwordSaving = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly successDialogMessage = signal('');
  readonly isEditDialogOpen = signal(false);

  readonly maxDate = this.formatDateForInput(new Date());

  readonly editForm = this.formBuilder.nonNullable.group({
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
    email: [
      '',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(100)
      ]
    ]
  });

  readonly passwordForm = this.formBuilder.nonNullable.group(
    {
      currentPassword: ['', [Validators.required]],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(50)
        ]
      ],
      confirmPassword: ['', [Validators.required]]
    },
    {
      validators: PatientProfile.passwordMatchValidator
    }
  );

  constructor() {
    this.loadProfile();
  }

  get fullName() {
    return this.editForm.controls.fullName;
  }

  get dateOfBirth() {
    return this.editForm.controls.dateOfBirth;
  }

  get gender() {
    return this.editForm.controls.gender;
  }

  get phoneNumber() {
    return this.editForm.controls.phoneNumber;
  }

  get email() {
    return this.editForm.controls.email;
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
      next: (patient: Patient) => {
        this.patient.set(patient);
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

  openEditDialog(): void {
    const patient = this.patient();

    if (!patient) {
      this.errorMessage.set('Patient details not found.');
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');
    this.successDialogMessage.set('');

    this.editForm.reset({
      fullName: patient.fullName,
      dateOfBirth: this.formatDateForInput(new Date(patient.dateOfBirth)),
      gender: patient.gender,
      phoneNumber: patient.phoneNumber,
      email: patient.email
    });

    this.isEditDialogOpen.set(true);
  }

  closeEditDialog(): void {
    if (this.saving()) {
      return;
    }

    this.isEditDialogOpen.set(false);
  }

  updatePatientDetails(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.successDialogMessage.set('');

    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      this.errorMessage.set('Please correct the patient details.');
      return;
    }

    const currentPatient = this.patient();

    if (!currentPatient) {
      this.errorMessage.set('Patient details not found.');
      return;
    }

    const formValue = this.editForm.getRawValue();

    const request: Patient = {
      ...currentPatient,
      fullName: formValue.fullName.trim(),
      dateOfBirth: formValue.dateOfBirth,
      gender: formValue.gender,
      phoneNumber: formValue.phoneNumber.trim(),
      email: formValue.email.trim()
    };

    this.saving.set(true);

    this.patientService
      .updatePatientById(currentPatient.patientId, request)
      .subscribe({
        next: (updatedPatient: Patient) => {
          this.patient.set(updatedPatient);
          this.saving.set(false);
          this.isEditDialogOpen.set(false);

          this.successDialogMessage.set(
            'Patient details updated successfully.'
          );
        },
        error: (error: unknown) => {
          this.saving.set(false);
          this.errorMessage.set(
            getFriendlyErrorMessage(error, 'Could not update patient details.')
          );
        }
      });
  }

  closeSuccessDialog(): void {
    this.successDialogMessage.set('');
  }

  changePassword(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.successDialogMessage.set('');

    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();
      this.errorMessage.set('Please correct the password details.');
      return;
    }

    const formValue = this.passwordForm.getRawValue();

    const request: ChangePasswordRequest = {
      currentPassword: formValue.currentPassword,
      newPassword: formValue.newPassword,
      confirmNewPassword: formValue.confirmPassword
    };

    this.passwordSaving.set(true);

    this.authService.changePassword(request).subscribe({
      next: () => {
        this.passwordSaving.set(false);
        this.passwordForm.reset({
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        });

        this.successDialogMessage.set('Password changed successfully.');
      },
      error: (error: unknown) => {
        this.passwordSaving.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not change password.')
        );
      }
    });
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
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

  private static passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (!newPassword || !confirmPassword) {
      return null;
    }

    if (newPassword !== confirmPassword) {
      return {
        passwordMismatch: true
      };
    }

    return null;
  }
}