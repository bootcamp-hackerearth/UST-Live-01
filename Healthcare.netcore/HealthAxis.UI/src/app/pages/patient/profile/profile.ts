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

  private patientService = inject(PatientService);
  private zone = inject(NgZone);
  private cdr = inject(ChangeDetectorRef);

  isEditMode = false;
  isPasswordMode = false;

  submitted = false;
  passwordSubmitted = false;

  isSaving = false;
  isChangingPassword = false;

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
    if (typeof window !== 'undefined') {
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

          // ✅ IMPORTANT FIX: force UI refresh immediately
          this.cdr.detectChanges();
        });
      },
      error: (err: any) => {
        console.error('Patient profile load failed ❌:', err);
      }
    });
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
    if (!this.submitted) return '';

    if (!this.patient.fullName.trim()) {
      return 'Full name is required.';
    }

    if (this.patient.fullName.trim().length < 3) {
      return 'Full name must be at least 3 characters.';
    }

    return '';
  }

  get emailError(): string {
    if (!this.submitted) return '';

    if (!this.patient.email.trim()) {
      return 'Email is required.';
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(this.patient.email.trim())) {
      return 'Enter a valid email address.';
    }

    return '';
  }

  get phoneError(): string {
    if (!this.submitted) return '';

    if (!this.patient.phoneNumber.trim()) {
      return 'Phone number is required.';
    }

    const phonePattern = /^[0-9]{10}$/;

    if (!phonePattern.test(this.patient.phoneNumber.trim())) {
      return 'Phone number must be 10 digits.';
    }

    return '';
  }

  get dobError(): string {
    if (!this.submitted) return '';

    if (!this.patient.dateOfBirth) {
      return 'Date of birth is required.';
    }

    const selectedDate = new Date(this.patient.dateOfBirth);
    const today = new Date();

    if (selectedDate > today) {
      return 'Future date is not allowed.';
    }

    return '';
  }

  hasProfileErrors(): boolean {
    return !!(
      this.fullNameError ||
      this.emailError ||
      this.phoneError ||
      this.dobError
    );
  }

  enableEdit() {
    this.backupPatient = { ...this.patient };
    this.submitted = false;
    this.isEditMode = true;
    this.isPasswordMode = false;
  }

  cancelEdit() {
    this.patient = { ...this.backupPatient };
    this.submitted = false;
    this.isEditMode = false;

    // ✅ also refresh after cancel
    this.cdr.detectChanges();
  }

  saveProfile() {
    this.submitted = true;

    if (this.hasProfileErrors()) {
      return;
    }

    const payload = {
      fullName: this.patient.fullName,
      dateOfBirth: this.patient.dateOfBirth,
      gender: Number(this.patient.gender),
      phoneNumber: this.patient.phoneNumber,
      email: this.patient.email
    };

    console.log('Profile update payload ✅:', payload);

    this.isSaving = true;

    this.patientService.updatePatient(this.patient.patientId, payload).subscribe({
      next: (res: any) => {
        console.log('Profile updated ✅:', res);

        alert('Profile updated successfully ✅');

        this.isSaving = false;
        this.isEditMode = false;
        this.submitted = false;

        this.loadCurrentPatient();
      },
      error: (err: any) => {
        console.error('Profile update failed ❌:', err);

        this.isSaving = false;

        const message =
          err?.error?.message ||
          err?.error ||
          'Profile update failed.';

        alert(message);
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

    if (!this.passwordModel.newPassword.trim()) {
      return 'New password is required.';
    }

    if (this.passwordModel.newPassword.length < 6) {
      return 'New password must be at least 6 characters.';
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

    this.passwordModel = {
      oldPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  changePassword() {
  this.passwordSubmitted = true;

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

        this.cdr.detectChanges();
      });

      setTimeout(() => {
        alert('Password changed successfully ✅');
      }, 0);
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

      setTimeout(() => {
        alert(message);
      }, 0);
    }
  });
}
  }
