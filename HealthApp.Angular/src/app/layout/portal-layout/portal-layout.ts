import { Component, OnInit, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { CurrentUser } from '../../core/models/current-user';
import { Doctor } from '../../core/models/doctor.model';
import {
  GenderType,
  Patient,
  UpdatePatientRequest
} from '../../core/models/patient.model';
import { AuthService } from '../../core/services/auth.service';
import { DoctorApiService } from '../../core/services/doctor-api.service';
import { PatientApiService } from '../../core/services/patient-api.service';

type PortalRole = 'Patient' | 'Doctor';

interface PortalUserView {
  userId: string;
  email: string;
  fullName: string;
  role: PortalRole;
  patientId?: number;
  doctorId?: number;
}

@Component({
  selector: 'app-portal-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './portal-layout.html',
  styleUrl: './portal-layout.css',
})
export class PortalLayout implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly patientApi = inject(PatientApiService);
  private readonly doctorApi = inject(DoctorApiService);
  private readonly router = inject(Router);

  realUser: CurrentUser | null = null;

  currentUser?: PortalUserView;
  patient?: Patient;
  realPatient?: Patient;
  realDoctorProfile?: Doctor;

  doctorProfileLoadError = '';

  isSidebarCollapsed = true;
  showSidebarClickAway = false;

  profileMenuOpen = false;

  showProfileModal = false;
  showPasswordModal = false;
  showDoctorProfileModal = false;
  showLogoutConfirmModal = false;

  profileMessage = '';
  profileIsError = false;

  passwordMessage = '';
  passwordIsError = false;

  genders: GenderType[] = ['Male', 'Female', 'Transgender', 'Other'];

  editProfile = {
    fullName: '',
    email: '',
    phoneNumber: '',
    gender: '' as GenderType | '',
    dateOfBirth: '',
    insuranceId: ''
  };

  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    const user = this.authService.currentUser();

    if (!user || (user.role !== 'Patient' && user.role !== 'Doctor')) {
      this.router.navigate(['/login']);
      return;
    }

    this.realUser = user;

    this.currentUser = {
      userId: user.userId,
      email: user.email,
      role: user.role,
      fullName: user.email,
      patientId: user.patientId,
      doctorId: user.doctorId
    };

    if (user.role === 'Patient') {
      this.loadPatientProfile();
    }

    if (user.role === 'Doctor') {
      this.loadDoctorProfile();
    }
  }

  get shellClass(): string {
    return this.isSidebarCollapsed
      ? 'portal-shell sidebar-collapsed'
      : 'portal-shell';
  }

  get sidebarClass(): string {
    return this.isSidebarCollapsed
      ? 'portal-sidebar collapsed'
      : 'portal-sidebar';
  }

  get isDoctorPortal(): boolean {
    return this.authService.currentRole() === 'Doctor';
  }

  get isPatientPortal(): boolean {
    return this.authService.currentRole() === 'Patient';
  }

  get portalTitle(): string {
    return this.isDoctorPortal ? 'Doctor Portal' : 'Patient Portal';
  }

  get portalSubtitle(): string {
    return this.isDoctorPortal ? 'HealthApp Clinical' : 'HealthApp Care';
  }

  get dashboardLink(): string {
    return this.isDoctorPortal ? '/doctor/dashboard' : '/patient/dashboard';
  }

  get appointmentsLink(): string {
    return this.isDoctorPortal ? '/doctor/appointments' : '/patient/appointments';
  }

  get healthRecordsLink(): string {
    return '/patient/health-records';
  }

  get userRoleLabel(): string {
    return this.isDoctorPortal ? 'Doctor' : 'Patient';
  }

  toggleSidebar(): void {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
    this.showSidebarClickAway = !this.isSidebarCollapsed;
  }

  collapseSidebarFromOutside(): void {
    this.isSidebarCollapsed = true;
    this.showSidebarClickAway = false;
  }

  toggleProfileMenu(): void {
    this.profileMenuOpen = !this.profileMenuOpen;
  }

  closeProfileMenu(): void {
    this.profileMenuOpen = false;
  }

  openLogoutConfirmation(): void {
    this.showLogoutConfirmModal = true;
  }

  closeLogoutConfirmation(): void {
    this.showLogoutConfirmModal = false;
  }

  confirmLogout(): void {
    this.showLogoutConfirmModal = false;
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  getInitials(name?: string): string {
    if (!name?.trim()) {
      return this.isDoctorPortal ? 'D' : 'P';
    }

    return name
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0])
      .join('')
      .toUpperCase();
  }

  openProfileModal(): void {
    this.closeProfileMenu();
    this.profileMessage = '';
    this.profileIsError = false;

    if (this.realPatient) {
      this.setEditProfileForm(this.realPatient);
      this.showProfileModal = true;
      return;
    }

    this.loadPatientProfile(true);
  }

  closeProfileModal(): void {
    this.showProfileModal = false;
  }

  saveProfile(): void {
    const user = this.authService.currentUser();

    if (!user?.patientId) {
      this.profileMessage = 'Patient details are missing. Please login again.';
      this.profileIsError = true;
      return;
    }

    if (!this.editProfile.gender) {
      this.profileMessage = 'Gender is required.';
      this.profileIsError = true;
      return;
    }

    const request: UpdatePatientRequest = {
      fullName: this.editProfile.fullName,
      email: this.editProfile.email,
      phoneNumber: this.editProfile.phoneNumber,
      gender: this.editProfile.gender,
      dateOfBirth: this.editProfile.dateOfBirth,
      insuranceId: this.editProfile.insuranceId ?? ''
    };

    this.patientApi.updatePatient(user.patientId, request).subscribe({
      next: patient => {
        this.applyPatientProfile(patient);
        this.profileMessage = 'Profile updated successfully.';
        this.profileIsError = false;
      },
      error: error => {
        this.profileMessage = this.authService.getErrorMessage(error);
        this.profileIsError = true;
      }
    });
  }

  openPasswordModal(): void {
    this.closeProfileMenu();
    this.passwordMessage = '';
    this.passwordIsError = false;

    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };

    this.showPasswordModal = true;
  }

  closePasswordModal(): void {
    this.showPasswordModal = false;
  }

  changePassword(): void {
    const user = this.authService.currentUser();

    if (!user?.email) {
      this.passwordMessage = 'User email was not found. Please login again.';
      this.passwordIsError = true;
      return;
    }

    this.authService.changePassword({
      email: user.email,
      currentPassword: this.passwordForm.currentPassword,
      newPassword: this.passwordForm.newPassword,
      confirmNewPassword: this.passwordForm.confirmPassword
    }).subscribe({
      next: response => {
        this.passwordMessage = response.message;
        this.passwordIsError = false;

        this.passwordForm = {
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        };
      },
      error: error => {
        this.passwordMessage = this.authService.getErrorMessage(error);
        this.passwordIsError = true;
      }
    });
  }

  openDoctorProfileModal(): void {
    this.closeProfileMenu();
    this.doctorProfileLoadError = '';

    if (this.realDoctorProfile) {
      this.showDoctorProfileModal = true;
      return;
    }

    this.showDoctorProfileModal = true;
    this.loadDoctorProfile();
  }

  closeDoctorProfileModal(): void {
    this.showDoctorProfileModal = false;
  }

  private loadPatientProfile(openModalAfterLoad = false): void {
    const user = this.authService.currentUser();

    if (!user?.patientId) {
      this.profileMessage = 'Patient details are missing. Please login again.';
      this.profileIsError = true;
      return;
    }

    this.patientApi.getPatientById(user.patientId).subscribe({
      next: patient => {
        this.applyPatientProfile(patient);

        if (openModalAfterLoad) {
          this.setEditProfileForm(patient);
          this.showProfileModal = true;
        }
      },
      error: error => {
        this.profileMessage = this.authService.getErrorMessage(error);
        this.profileIsError = true;
      }
    });
  }

  private loadDoctorProfile(): void {
    this.doctorApi.getLoggedInDoctorProfile().subscribe({
      next: doctor => {
        this.realDoctorProfile = doctor;
        this.doctorProfileLoadError = '';

        if (this.currentUser) {
          this.currentUser = {
            ...this.currentUser,
            fullName: doctor.fullName,
            doctorId: doctor.doctorId
          };
        }
      },
      error: error => {
        this.doctorProfileLoadError = this.authService.getErrorMessage(error);
      }
    });
  }

  private applyPatientProfile(patient: Patient): void {
    this.realPatient = patient;
    this.patient = patient;

    if (this.currentUser) {
      this.currentUser = {
        ...this.currentUser,
        fullName: patient.fullName,
        email: patient.email ?? this.currentUser.email,
        patientId: patient.patientId
      };
    }
  }

  private setEditProfileForm(patient: Patient): void {
    this.editProfile = {
      fullName: patient.fullName,
      email: patient.email ?? '',
      phoneNumber: patient.phoneNumber,
      gender: patient.gender,
      dateOfBirth: patient.dateOfBirth.split('T')[0],
      insuranceId: patient.insuranceId ?? ''
    };
  }
}