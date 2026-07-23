import { Component, OnInit, inject, NgZone, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DoctorProfileService } from '../../../services/doctor-profile';

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css']
})
export class DoctorProfile implements OnInit {

  private readonly doctorProfileService = inject(DoctorProfileService);
  private readonly http = inject(HttpClient);
  private readonly zone = inject(NgZone);
  private readonly cdr = inject(ChangeDetectorRef);

  doctor = {
    doctorId: 0,
    fullName: 'Doctor',
    email: 'doctor@healthaxis.com',
    specialisation: 'Not available',
    experience: '0 years',
    consultationFee: '₹0',
    status: 'Active',
    isActive: true
  };

  showPasswordForm = false;
  passwordSubmitted = false;
  isChangingPassword = false;
  isUpdatingStatus = false;

  passwordModel = {
    oldPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  private readonly authUrl = '/api/auth/change-password';
  private readonly doctorStatusUrl = '/api/doctors/me/status';

  ngOnInit() {
    if (globalThis.window !== undefined) {
      this.loadDoctorProfile();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadDoctorProfile() {
    this.doctorProfileService.getCurrentDoctor().subscribe({
      next: (res: any) => {
        console.log('Doctor profile ✅:', res);

        const email = this.getEmailFromToken();

        this.zone.run(() => {
          this.doctor = {
            doctorId: res.doctorId,
            fullName: res.fullName || 'Doctor',
            email: email || 'doctor@healthaxis.com',
            specialisation: this.getSpecialisationName(res.specialisation),
            experience: `${res.yearsOfExperience || 0} years`,
            consultationFee: `₹${res.consultationFee || 0}`,
            status: res.isActive ? 'Active' : 'Inactive',
            isActive: res.isActive
          };

          localStorage.setItem('doctorId', String(res.doctorId));
          this.cdr.detectChanges();
        });
      },
      error: (err: any) => {
        console.error('Doctor profile load failed ❌:', err);
      }
    });
  }

  getEmailFromToken(): string {
    const token = localStorage.getItem('token');

    if (!token) {
      return '';
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));

      return (
        payload.email ||
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/emailaddress'] ||
        ''
      );
    } catch {
      return '';
    }
  }

  getSpecialisationName(value: number): string {
    switch (Number(value)) {
      case 0: return 'General Medicine';
      case 1: return 'Pediatrician';
      case 2: return 'Cardiology';
      case 3: return 'Dermatology';
      case 4: return 'Orthopaedics';
      default: return 'Other';
    }
  }

  toggleActivationStatus() {
    console.log('Activation button clicked ✅');

    if (this.isUpdatingStatus) {
      return;
    }

    const newStatus = !this.doctor.isActive;

    const payload = {
      isActive: newStatus
    };

    console.log('Doctor status payload ✅:', payload);

    this.isUpdatingStatus = true;
    this.cdr.detectChanges();

    this.http.put<any>(
      this.doctorStatusUrl,
      payload,
      {
        headers: this.getHeaders()
      }
    ).subscribe({
      next: (res: any) => {
        console.log('Doctor status updated ✅:', res);

        this.zone.run(() => {
          this.doctor.isActive = res.isActive;
          this.doctor.status = res.isActive ? 'Active' : 'Inactive';
          this.isUpdatingStatus = false;

          this.cdr.detectChanges();
        });

        alert(
          res.isActive
            ? 'Profile activated successfully ✅'
            : 'Profile deactivated successfully ✅'
        );
      },
      error: (err: any) => {
        console.error('Doctor status update failed ❌:', err);

        this.zone.run(() => {
          this.isUpdatingStatus = false;
          this.cdr.detectChanges();
        });

        const message =
          err?.error?.message ||
          err?.error ||
          'Failed to update profile status.';

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

  togglePasswordForm() {
    this.showPasswordForm = !this.showPasswordForm;
    this.passwordSubmitted = false;

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
      email: this.doctor.email,
      oldPassword: this.passwordModel.oldPassword,
      newPassword: this.passwordModel.newPassword,
      confirmPassword: this.passwordModel.confirmPassword
    };

    console.log('Doctor change password payload ✅:', payload);

    this.isChangingPassword = true;
    this.cdr.detectChanges();

    this.http.post<any>(this.authUrl, payload).subscribe({
      next: (res: any) => {
        console.log('Doctor password changed ✅:', res);

        this.zone.run(() => {
          this.isChangingPassword = false;
          this.showPasswordForm = false;
          this.passwordSubmitted = false;

          this.passwordModel = {
            oldPassword: '',
            newPassword: '',
            confirmPassword: ''
          };

          this.cdr.detectChanges();
        });

        alert('Password changed successfully ✅');
      },
      error: (err: any) => {
        console.error('Doctor password change failed ❌:', err);

        this.zone.run(() => {
          this.isChangingPassword = false;
          this.cdr.detectChanges();
        });

        const message =
          err?.error?.message ||
          err?.error ||
          'Password change failed.';

        alert(message);
      }
    });
  }
}