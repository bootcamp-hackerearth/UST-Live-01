import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  ChangePasswordDto,
  PatientDto,
  UpdatePatientDto
} from '../../../../../shared/models/patient.models';
import { PatientFakeDataService } from '../../../../../core/services/patient-fake-data.service';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfile {

  patient?: PatientDto;

  isEditMode = false;
  isPasswordMode = false;

  hasSubmitted = false;
  hasPasswordSubmitted = false;

  message = '';
  passwordMessage = '';

  todayDate = '';

  genderOptions: string[] = ['Male', 'Female', 'Other'];

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

  constructor(private service: PatientFakeDataService) {
    this.todayDate = new Date().toISOString().split('T')[0];
    this.loadProfile();
  }

  // ===============================
  // PROFILE VALIDATIONS
  // ===============================

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
    const phonePattern = /^(?:\+91)?[6-9]\d{9}$/;

    return !phonePattern.test(phone);
  }

  get isInsuranceInvalid(): boolean {
    return !!this.form.insuranceId && this.form.insuranceId.length > 50;
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

  // ===============================
  // PASSWORD VALIDATIONS
  // ===============================

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

  // ===============================
  // PROFILE ACTIONS
  // ===============================

  loadProfile(): void {
    this.patient = this.service.getPatientProfile();

    this.form = {
      patientName: this.patient.patientName,
      dateOfBirth: this.patient.dateOfBirth,
      gender: this.patient.gender,
      email: this.patient.email,
      phoneNumber: this.patient.phoneNumber,
      insuranceId: this.patient.insuranceId ?? ''
    };
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
    this.isEditMode = false;
    this.loadProfile();
  }

  saveProfile(): void {
    this.message = '';
    this.hasSubmitted = true;

    if (this.isFormInvalid) {
      this.message = 'Please correct the highlighted fields.';
      return;
    }

    const request: UpdatePatientDto = {
      patientName: this.form.patientName.trim(),
      dateOfBirth: this.form.dateOfBirth,
      gender: this.form.gender,
      email: this.form.email.trim(),
      phoneNumber: this.form.phoneNumber.trim(),
      insuranceId: this.form.insuranceId?.trim() || undefined
    };

    this.service.updatePatientProfile(request);

    this.loadProfile();
    this.isEditMode = false;
    this.hasSubmitted = false;

    this.profileUpdated.emit();
  }

  // ===============================
  // PASSWORD ACTIONS
  // ===============================

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
    this.isPasswordMode = false;

    this.resetPasswordForm();
  }

  changePassword(): void {
    this.passwordMessage = '';
    this.hasPasswordSubmitted = true;

    if (this.isPasswordFormInvalid) {
      this.passwordMessage = 'Please correct the highlighted password fields.';
      return;
    }

    this.service.changePatientPassword(this.passwordForm);

    this.isPasswordMode = false;
    this.hasPasswordSubmitted = false;

    this.resetPasswordForm();

    this.passwordChanged.emit();
  }

  private resetPasswordForm(): void {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }
}