import {
  Component,
  EventEmitter,
  OnInit,
  Output
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { finalize, timeout } from 'rxjs';

import {
  ChangePasswordDto,
  PatientDto,
  UpdatePatientDto
} from '../../../../../shared/models/patient.models';

import { PatientApiService } from '../../../../../core/services/patient-api.service';
import { AuthService } from '../../../../../core/services/auth.service';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfile implements OnInit {
  patient?: PatientDto;

  isLoading = false;

  isEditMode = false;
  isPasswordMode = false;

  isDiscardProfileModalOpen = false;
  isDiscardPasswordModalOpen = false;
  isPasswordConfirmModalOpen = false;
  isProfileSaveConfirmModalOpen = false;

  hasSubmitted = false;
  hasPasswordSubmitted = false;

  message = '';
  passwordMessage = '';

  todayDate = '';

  genderOptions: string[] = [
    'Male',
    'Female',
    'Transgender',
    'Other'
  ];

  form: UpdatePatientDto = {
    patientName: '',
    dateOfBirth: '',
    gender: '',
    email: '',
    phoneNumber: '',
    insuranceId: ''
  };

  passwordForm: ChangePasswordDto = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  @Output() profileUpdated = new EventEmitter<void>();
  @Output() passwordChanged = new EventEmitter<void>();

  constructor(
    private patientApiService: PatientApiService,
    private authService: AuthService
  ) {
    this.todayDate = new Date().toISOString().split('T')[0];
  }

  ngOnInit(): void {
    this.loadProfile();
  }

  get isNameInvalid(): boolean {
    const name = this.form.patientName.trim();
    const namePattern = /^[A-Za-z]+(?: [A-Za-z]+)*$/;

    return name.length < 2 || name.length > 100 || !namePattern.test(name);
  }

  get isDateOfBirthInvalid(): boolean {
    if (!this.form.dateOfBirth) {
      return true;
    }

    return this.form.dateOfBirth > this.todayDate;
  }

  get isGenderInvalid(): boolean {
    return !this.form.gender;
  }

  get isEmailInvalid(): boolean {
    const email = this.form.email.trim();
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    return !emailPattern.test(email);
  }

  get isPhoneInvalid(): boolean {
    const phone = this.form.phoneNumber.replace(/\s/g, '');
    const phonePattern = /^[0-9]{10}$/;

    return !phonePattern.test(phone);
  }

  get isInsuranceInvalid(): boolean {
    const insuranceId = this.form.insuranceId ?? '';

    return (
      insuranceId.trim().length === 0 ||
      insuranceId.trim().length > 30
    );
  }

  get isFormInvalid(): boolean {
    return (
      this.isNameInvalid ||
      this.isDateOfBirthInvalid ||
      this.isGenderInvalid ||
      this.isEmailInvalid ||
      this.isPhoneInvalid ||
      this.isInsuranceInvalid
    );
  }

  get isCurrentPasswordInvalid(): boolean {
    return !this.passwordForm.currentPassword.trim();
  }

  get isNewPasswordInvalid(): boolean {
    const password = this.passwordForm.newPassword;
    const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    return !passwordPattern.test(password);
  }

  get isConfirmPasswordInvalid(): boolean {
    return !this.passwordForm.confirmPassword.trim();
  }

  get isPasswordMismatch(): boolean {
    return (
      this.passwordForm.confirmPassword.trim().length > 0 &&
      this.passwordForm.newPassword !== this.passwordForm.confirmPassword
    );
  }

  get isSamePasswordInvalid(): boolean {
    return (
      this.passwordForm.currentPassword.trim().length > 0 &&
      this.passwordForm.currentPassword === this.passwordForm.newPassword
    );
  }

  get isPasswordFormInvalid(): boolean {
    return (
      this.isCurrentPasswordInvalid ||
      this.isNewPasswordInvalid ||
      this.isConfirmPasswordInvalid ||
      this.isPasswordMismatch ||
      this.isSamePasswordInvalid
    );
  }

  loadProfile(): void {
    this.isLoading = true;
    this.message = '';

    this.patientApiService.getMyProfile().pipe(
      timeout(15000),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe({
      next: (patient: PatientDto) => {
        this.patient = patient;

        this.form = {
          patientName: patient.patientName,
          dateOfBirth: patient.dateOfBirth,
          gender: patient.gender,
          email: patient.email,
          phoneNumber: patient.phoneNumber,
          insuranceId: patient.insuranceId ?? ''
        };
      },
      error: (error: unknown) => {
        console.log('Patient profile API error:', error);
        this.message = this.getErrorMessage(error);
      }
    });
  }

  enableEdit(): void {
    this.message = '';
    this.hasSubmitted = false;
    this.isEditMode = true;
    this.isPasswordMode = false;
  }

  cancelEdit(): void {
    this.message = '';
    this.hasSubmitted = false;

    if (this.hasProfileChanges()) {
      this.isDiscardProfileModalOpen = true;
      return;
    }

    this.discardProfileChanges();
  }

  closeDiscardProfileModal(): void {
    this.isDiscardProfileModalOpen = false;
  }

  confirmDiscardProfileChanges(): void {
    this.isDiscardProfileModalOpen = false;
    this.discardProfileChanges();
  }

  saveProfile(): void {
    this.message = '';
    this.hasSubmitted = true;

    if (this.isFormInvalid) {
      this.message = 'Please correct the highlighted fields.';
      return;
    }

    this.isProfileSaveConfirmModalOpen = true;
  }

  closeProfileSaveConfirmModal(): void {
    this.isProfileSaveConfirmModalOpen = false;
  }

  confirmSaveProfile(): void {
    this.message = '';

    this.patientApiService.updateMyProfile({
      fullName: this.form.patientName.trim(),
      dateOfBirth: this.form.dateOfBirth,
      gender: this.mapGenderToNumber(this.form.gender),
      email: this.form.email.trim(),
      phoneNumber: this.form.phoneNumber.trim(),
      insuranceId: (this.form.insuranceId ?? '').trim()
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isProfileSaveConfirmModalOpen = false;
        this.isEditMode = false;
        this.hasSubmitted = false;

        this.loadProfile();
        this.profileUpdated.emit();
      },
      error: (error: unknown) => {
        console.log('Patient profile update API error:', error);
        this.isProfileSaveConfirmModalOpen = false;
        this.message = this.getErrorMessage(error);
      }
    });
  }

  enablePasswordChange(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = false;
    this.isPasswordMode = true;
    this.isEditMode = false;

    this.resetPasswordForm();
  }

  cancelPasswordChange(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = false;

    if (this.hasPasswordChanges()) {
      this.isDiscardPasswordModalOpen = true;
      return;
    }

    this.discardPasswordChanges();
  }

  closeDiscardPasswordModal(): void {
    this.isDiscardPasswordModalOpen = false;
  }

  confirmDiscardPasswordChanges(): void {
    this.isDiscardPasswordModalOpen = false;
    this.discardPasswordChanges();
  }

  changePassword(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = true;

    if (this.isPasswordFormInvalid) {
      this.passwordMessage = 'Please correct the highlighted password fields.';
      return;
    }

    this.isPasswordConfirmModalOpen = true;
  }

  closePasswordConfirmModal(): void {
    this.isPasswordConfirmModalOpen = false;
  }

  confirmPasswordChange(): void {
    this.passwordMessage = '';

    this.authService.changePassword({
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword,
      confirmNewPassword: this.passwordForm.confirmPassword
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isPasswordConfirmModalOpen = false;
        this.isPasswordMode = false;
        this.hasPasswordSubmitted = false;

        this.resetPasswordForm();
        this.passwordChanged.emit();
      },
      error: (error: unknown) => {
        console.log('Patient password change API error:', error);
        this.isPasswordConfirmModalOpen = false;
        this.passwordMessage = this.getErrorMessage(error);
      }
    });
  }

  private hasProfileChanges(): boolean {
    if (!this.patient) {
      return false;
    }

    return (
      this.form.patientName !== this.patient.patientName ||
      this.form.dateOfBirth !== this.patient.dateOfBirth ||
      this.form.gender !== this.patient.gender ||
      this.form.email !== this.patient.email ||
      this.form.phoneNumber !== this.patient.phoneNumber ||
      this.form.insuranceId !== (this.patient.insuranceId ?? '')
    );
  }

  private hasPasswordChanges(): boolean {
    return (
      this.passwordForm.currentPassword.trim().length > 0 ||
      this.passwordForm.newPassword.trim().length > 0 ||
      this.passwordForm.confirmPassword.trim().length > 0
    );
  }

  private discardProfileChanges(): void {
    this.isEditMode = false;
    this.loadProfile();
  }

  private discardPasswordChanges(): void {
    this.isPasswordMode = false;
    this.resetPasswordForm();
  }

  private resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  private mapGenderToNumber(gender: string): number {
    switch (gender) {
      case 'Male':
        return 0;

      case 'Female':
        return 1;

      case 'Transgender':
        return 2;

      case 'Other':
        return 3;

      default:
        return 3;
    }
  }

  private getErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          errors?: Record<string, string[]>;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (apiError.error?.message) {
        return apiError.error.message;
      }

      if (apiError.error?.Message) {
        return apiError.error.Message;
      }

      if (apiError.error?.errors) {
        const firstError = Object.values(apiError.error.errors)[0]?.[0];

        if (firstError) {
          return firstError;
        }
      }
    }

    return 'Something went wrong. Please try again.';
  }
}