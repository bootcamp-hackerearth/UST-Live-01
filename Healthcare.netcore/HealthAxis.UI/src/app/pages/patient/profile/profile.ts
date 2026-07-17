import { Component, OnInit, inject, NgZone, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PatientService } from '../../../services/patient';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class Profile implements OnInit {

  private readonly patientService = inject(PatientService);
  private readonly zone = inject(NgZone);
  private readonly cdr = inject(ChangeDetectorRef);

  isEditMode = false;
  isPasswordMode = false;

  submitted = false;
  passwordSubmitted = false;

  isSaving = false;
  isChangingPassword = false;

  successMessage = '';
  errorMessage = '';

  patient = {
    patientId: 0,
    fullName: '',
    email: '',
    phoneNumber: '',
    dateOfBirth: '',
    gender: 0,
    status: 'Active'
  };

  backupPatient = { ...this.patient };

  passwordModel = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  ngOnInit() {
    if (globalThis.window !== undefined) {
      this.loadCurrentPatient();
    }
  }

  loadCurrentPatient() {
    this.patientService.getCurrentPatient().subscribe({
      next: (res: any) => {
        console.log('Current patient ✅:', res);

        const mappedPatient = {
          patientId: res.patientId,
          fullName: res.fullName || '',
          email: res.email || '',
          phoneNumber: res.phoneNumber || '',
          dateOfBirth: res.dateOfBirth ? res.dateOfBirth.split('T')[0] : '',
          gender: Number(res.gender),
          status: 'Active'
        };

        localStorage.setItem('patientId', String(res.patientId));

        this.zone.run(() => {
          this.patient = mappedPatient;
          this.backupPatient = { ...mappedPatient };
          this.cdr.detectChanges();
        });
      },
      error: (err: any) => {
        console.error('Patient profile load failed ❌:', err);
        this.showError('Failed to load patient profile.');
      }
    });
  }

  get todayDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  get minDateOfBirth(): string {
    return '1900-01-01';
  }

  get avatarInitial(): string {
    return this.patient.fullName
      ? this.patient.fullName.charAt(0).toUpperCase()
      : 'P';
  }

  getGenderName(value: number): string {
    switch (Number(value)) {
      case 0: return 'Male';
      case 1: return 'Female';
      case 2: return 'Other';
      default: return 'Unknown';
    }
  }

  get fullNameError(): string {
  if (!this.submitted && !this.isEditMode) return '';

  const fullName = this.patient.fullName.trim();

  if (!fullName) {
    return 'Full name is required.';
  }

  if (fullName.length < 3) {
    return 'Full name must be at least 3 characters.';
  }

  const namePattern = /^[A-Za-z ]+$/;

  if (!namePattern.test(fullName)) {
    return 'Full name should contain only letters and spaces.';
  }

  return '';
}
  get emailError(): string {
  if (!this.submitted && !this.isEditMode) return '';

  const email = this.patient.email.trim();

  if (!email) {
    return 'Email is required.';
  }

  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

  if (!emailPattern.test(email)) {
    return 'Enter a valid email address.';
  }

  return '';
}

  get phoneError(): string {
    if (!this.submitted) return '';

    const phoneNumber = this.patient.phoneNumber.trim();

    if (!phoneNumber) {
      return 'Phone number is required.';
    }

    const phonePattern = /^\d{10}$/;

    if (!phonePattern.test(phoneNumber)) {
      return 'Phone number must be exactly 10 digits.';
    }

    return '';
  }

 get dobError(): string {
  if (!this.submitted && !this.isEditMode) return '';

  if (!this.patient.dateOfBirth) {
    return 'Date of birth is required.';
  }

  const selectedDate = new Date(this.patient.dateOfBirth);
  const today = new Date();
  const minDate = new Date('1900-01-01');

  if (selectedDate < minDate) {
    return 'Date of birth year must be 1900 or later.';
  }

  if (selectedDate > today) {
    return 'Future date is not allowed.';
  }

  return '';
}

 get genderError(): string {
  if (!this.submitted) return '';

  const genderValue = Number(this.patient.gender);

  if (![0, 1, 2].includes(genderValue)) {
    return 'Gender is required.';
  }

  return '';
}

  hasProfileErrors(): boolean {
    return !!(
      this.fullNameError ||
      this.emailError ||
      this.phoneError ||
      this.dobError ||
      this.genderError
    );
  }

  enableEdit() {
    this.backupPatient = { ...this.patient };
    this.submitted = false;
    this.successMessage = '';
    this.errorMessage = '';
    this.isEditMode = true;
    this.isPasswordMode = false;
  }

  cancelEdit() {
    this.patient = { ...this.backupPatient };
    this.submitted = false;
    this.isEditMode = false;
    this.successMessage = '';
    this.errorMessage = '';
    this.cdr.detectChanges();
  }

  saveProfile() {
    this.submitted = true;
    this.successMessage = '';
    this.errorMessage = '';

    if (this.hasProfileErrors()) {
      return;
    }

    const payload = {
      fullName: this.patient.fullName.trim(),
      dateOfBirth: this.patient.dateOfBirth,
      gender: Number(this.patient.gender),
      phoneNumber: this.patient.phoneNumber.trim(),
      email: this.patient.email.trim()
    };

    console.log('Profile update payload ✅:', payload);

    this.isSaving = true;

    this.patientService.updatePatient(this.patient.patientId, payload).subscribe({
      next: (res: any) => {
        console.log('Profile updated ✅:', res);

        this.isSaving = false;
        this.isEditMode = false;
        this.submitted = false;

        this.showSuccess('Profile updated successfully.');

        this.loadCurrentPatient();
      },
      error: (err: any) => {
        console.error('Profile update failed ❌:', err);

        this.isSaving = false;

        const message =
          err?.error?.message ||
          err?.error ||
          'Profile update failed.';

        this.showError(message);
      }
    });
  }

  get oldPasswordError(): string {
    if (!this.passwordSubmitted) return '';

    if (!this.passwordModel.oldPassword.trim()) {
      return 'Old password is required.';
    }

    return '';
  }

  get newPasswordError(): string {
    if (!this.passwordSubmitted) return '';

    const newPassword = this.passwordModel.newPassword;

    if (!newPassword.trim()) {
      return 'New password is required.';
    }

    if (newPassword.length < 8) {
      return 'New password must be at least 8 characters.';
    }

    const hasUppercase = /[A-Z]/.test(newPassword);
    const hasDigit = /\d/.test(newPassword);
    const hasSpecial = /[^A-Za-z0-9]/.test(newPassword);

    if (!hasUppercase || !hasDigit || !hasSpecial) {
      return 'Password must contain uppercase letter, number, and special character.';
    }

    return '';
  }

  get confirmPasswordError(): string {
    if (!this.passwordSubmitted) return '';

    if (!this.passwordModel.confirmPassword.trim()) {
      return 'Confirm password is required.';
    }

    if (this.passwordModel.newPassword !== this.passwordModel.confirmPassword) {
      return 'New password and confirm password do not match.';
    }

    return '';
  }

  hasPasswordErrors(): boolean {
    return !!(
      this.oldPasswordError ||
      this.newPasswordError ||
      this.confirmPasswordError
    );
  }

  enablePasswordChange() {
    this.passwordSubmitted = false;
    this.successMessage = '';
    this.errorMessage = '';
    this.isPasswordMode = true;
    this.isEditMode = false;

    this.passwordModel = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  cancelPasswordChange() {
    this.passwordSubmitted = false;
    this.isPasswordMode = false;
    this.successMessage = '';
    this.errorMessage = '';

    this.passwordModel = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  changePassword() {
    this.passwordSubmitted = true;
    this.successMessage = '';
    this.errorMessage = '';

    if (this.hasPasswordErrors()) {
      return;
    }

    const payload = {
      email: this.patient.email,
      oldPassword: this.passwordModel.oldPassword,
      newPassword: this.passwordModel.newPassword,
      confirmPassword: this.passwordModel.confirmPassword
    };

    console.log('Change password payload ✅:', payload);

    this.isChangingPassword = true;
    this.cdr.detectChanges();

    this.patientService.changePassword(payload).subscribe({
      next: (res: any) => {
        console.log('Password changed ✅:', res);

        this.zone.run(() => {
          this.isChangingPassword = false;
          this.isPasswordMode = false;
          this.passwordSubmitted = false;

          this.passwordModel = {
            oldPassword: '',
            newPassword: '',
            confirmPassword: ''
          };

          this.showSuccess('Password changed successfully.');
          this.cdr.detectChanges();
        });
      },
      error: (err: any) => {
        console.error('Password change failed ❌:', err);

        this.zone.run(() => {
          this.isChangingPassword = false;
          this.cdr.detectChanges();
        });

        const message =
          err?.error?.message ||
          err?.error ||
          'Password change failed.';

        this.showError(message);
      }
    });
  }

  private showSuccess(message: string): void {
    this.successMessage = message;
    this.errorMessage = '';

    this.cdr.detectChanges();

    setTimeout(() => {
      this.successMessage = '';
      this.cdr.detectChanges();
    }, 3000);
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.successMessage = '';

    this.cdr.detectChanges();

    setTimeout(() => {
      this.errorMessage = '';
      this.cdr.detectChanges();
    }, 3000);
  }
}