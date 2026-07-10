import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { ChangePasswordRequest } from '../../core/models/auth.model';
import { Doctor } from '../../core/models/doctor.model';
import { DoctorService } from '../../core/services/doctor.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-doctor-profile',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DoctorProfile {
  readonly doctorStatusState = inject(DoctorStatusStateService);

  private readonly formBuilder = inject(FormBuilder);
  private readonly doctorService = inject(DoctorService);
  private readonly authService = inject(AuthService);

  readonly doctor = signal<Doctor | null>(null);
  readonly loading = signal(false);
  readonly changingPassword = signal(false);
  readonly updatingStatus = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly statusErrorMessage = signal('');
  readonly passwordErrorMessage = signal('');
  readonly passwordSuccessMessage = signal('');

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
      validators: DoctorProfile.passwordsMatchValidator
    }
  );

  constructor() {
    this.loadProfile();
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

  getDoctorStatusText(): string {
    const status = this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading Status';
    }

    return status ? 'Currently Active' : 'Currently Inactive';
  }

  getStatusButtonText(): string {
    const status = this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading...';
    }

    return status ? 'Set Inactive' : 'Set Active';
  }

  loadProfile(): void {
    this.loading.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');
    this.statusErrorMessage.set('');

    this.doctorService.getMyDoctorProfile().subscribe({
      next: (doctor) => {
        this.doctor.set(doctor);
        this.doctorStatusState.setStatus(Boolean(doctor.isActive));
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load doctor profile.')
        );
      }
    });
  }

  toggleDoctorStatus(): void {
    const currentDoctor = this.doctor();
    const currentStatus = this.doctorStatusState.isActive();

    this.statusErrorMessage.set('');
    this.successMessage.set('');

    if (!currentDoctor) {
      this.statusErrorMessage.set('Doctor profile is not loaded.');
      return;
    }

    if (currentStatus === null) {
      this.statusErrorMessage.set('Doctor status is still loading.');
      return;
    }

    const nextStatus = !currentStatus;

    this.updatingStatus.set(true);

    this.doctorService.updateMyStatus(nextStatus).subscribe({
      next: (response) => {
        this.doctor.set({
          ...currentDoctor,
          isActive: response.isActive
        });

        this.doctorStatusState.setStatus(response.isActive);
        this.updatingStatus.set(false);
        this.successMessage.set(response.message);
      },
      error: (error: unknown) => {
        this.updatingStatus.set(false);
        this.statusErrorMessage.set(
          getFriendlyErrorMessage(error, 'Could not update availability status.')
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
}