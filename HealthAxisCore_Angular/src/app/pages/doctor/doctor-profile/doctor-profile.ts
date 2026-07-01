import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { DoctorDto } from '../../../core/models/doctor.model';

@Component({
  selector: 'app-doctor-profile',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile {
  doctor = signal<DoctorDto | null>(null);

  statusForm: FormGroup;

  passwordForm: FormGroup;

  isLoading = signal(false);

  isSavingStatus = signal(false);

  isChangingPassword = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  passwordErrorMessage = signal('');

  passwordSuccessMessage = signal('');

  submittedPassword = signal(false);

  showCurrentPassword = signal(false);

  showNewPassword = signal(false);

  showConfirmPassword = signal(false);

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private doctorService: DoctorService
  ) {
    this.statusForm = this.formBuilder.group({
      isActive: [true]
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
    const doctorId = this.authService.doctorId();

    if (!doctorId) {
      this.errorMessage.set('Doctor ID missing. Please login again.');
      return;
    }

    this.isLoading.set(true);
    this.errorMessage.set('');

    this.doctorService.getById(doctorId).subscribe({
      next: doctor => {
        this.doctor.set(doctor);

        this.statusForm.patchValue({
          isActive: doctor.isActive
        });

        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  saveStatus(): void {
    const doctor = this.doctor();

    if (!doctor) {
      this.errorMessage.set('Doctor profile not found.');
      return;
    }

    this.isSavingStatus.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    this.doctorService.updateOwnStatus(
      doctor.doctorId,
      this.statusForm.value.isActive
    ).subscribe({
      next: updatedDoctor => {
        this.doctor.set(updatedDoctor);
        this.isSavingStatus.set(false);
        this.successMessage.set('Availability status updated successfully.');
      },
      error: error => {
        this.isSavingStatus.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  passwordsDoNotMatch(): boolean {
    return this.passwordForm.value.newPassword &&
      this.passwordForm.value.confirmPassword &&
      this.passwordForm.value.newPassword !== this.passwordForm.value.confirmPassword;
  }

  changePassword(): void {
    this.submittedPassword.set(true);
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
        this.submittedPassword.set(false);
      },
      error: error => {
        this.isChangingPassword.set(false);
        this.passwordErrorMessage.set(this.authService.getErrorMessage(error));
      }
    });
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
