import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth, MockUser } from '../../services/mock-auth';
import {DoctorDto, GenderType,MockPatientData,PatientDto} from '../../services/mock-patient-data';

@Component({
  selector: 'app-portal-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './portal-layout.html',
  styleUrl: './portal-layout.css',
})
export class PortalLayout implements OnInit {
  private auth = inject(MockAuth);
  private router = inject(Router);
  private patientData = inject(MockPatientData);

  profileMenuOpen = false;
  showProfileModal = false;
  showPasswordModal = false;

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
  showDoctorProfileModal = false;
  doctorProfile?: DoctorDto;

  currentUser?: MockUser;
  patient?: PatientDto;

  isSidebarCollapsed = true;
  showSidebarClickAway = false;
  showLogoutConfirmModal = false;

  ngOnInit(): void {
  if (!this.auth.isLoggedIn()) {
    this.router.navigate(['/login']);
    return;
  }

  const user = this.auth.getCurrentUser();

  if (!user || (user.role !== 'Patient' && user.role !== 'Doctor')) {
    this.router.navigate(['/login']);
    return;
  }

    this.currentUser = this.auth.getCurrentUser();
    this.patient = this.auth.getCurrentPatient();
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
    return this.currentUser?.role === 'Doctor';
  }

  get isPatientPortal(): boolean {
    return this.currentUser?.role === 'Patient';
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
    return this.isDoctorPortal ? '/doctor/health-records' : '/patient/health-records';
  }

  get userRoleLabel(): string {
    return this.isDoctorPortal ? 'Doctor' : 'Patient';
  }

  toggleSidebar(): void {
    if (this.isSidebarCollapsed) {
      this.isSidebarCollapsed = false;
      this.showSidebarClickAway = true;
    } else {
      this.isSidebarCollapsed = true;
      this.showSidebarClickAway = false;
    }
  }

  collapseSidebarFromOutside(): void {
    this.isSidebarCollapsed = true;
    this.showSidebarClickAway = false;
  }

  openLogoutConfirmation(): void {
    this.showLogoutConfirmModal = true;
  }

  closeLogoutConfirmation(): void {
    this.showLogoutConfirmModal = false;
  }

  confirmLogout(): void {
    this.showLogoutConfirmModal = false;
    this.auth.logout();
    this.router.navigate(['/login']);
  }

  getInitials(name?: string): string {
    if (!name?.trim()) {
      return 'P';
    }

    return name
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map(part => part[0])
      .join('')
      .toUpperCase();
  }
  toggleProfileMenu(): void {
  this.profileMenuOpen = !this.profileMenuOpen;
}

closeProfileMenu(): void {
  this.profileMenuOpen = false;
}

openProfileModal(): void {
  this.closeProfileMenu();
  this.profileMessage = '';
  this.profileIsError = false;

  if (!this.patient) {
    return;
  }

  this.editProfile = {
    fullName: this.patient.fullName,
    email: this.patient.email ?? '',
    phoneNumber: this.patient.phoneNumber,
    gender: this.patient.gender,
    dateOfBirth: this.patient.dateOfBirth,
    insuranceId: this.patient.insuranceId ?? ''
  };

  this.showProfileModal = true;
}

closeProfileModal(): void {
  this.showProfileModal = false;
}

saveProfile(): void {
  if (!this.patient) {
    this.profileMessage = 'Patient details are missing. Please login again.';
    this.profileIsError = true;
    return;
  }

  const result = this.patientData.updatePatientProfile(this.patient.patientId, {
    fullName: this.editProfile.fullName,
    email: this.editProfile.email,
    phoneNumber: this.editProfile.phoneNumber,
    gender: this.editProfile.gender as GenderType,
    dateOfBirth: this.editProfile.dateOfBirth,
    insuranceId: this.editProfile.insuranceId
  });

  if (!result.success || !result.data) {
    this.profileMessage = result.message;
    this.profileIsError = true;
    return;
  }

  this.patient = result.data;
  this.auth.syncCurrentUserFromPatient(result.data);
  this.currentUser = this.auth.getCurrentUser();

  this.profileMessage = result.message;
  this.profileIsError = false;
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
  const result = this.auth.changeCurrentUserPassword(
    this.passwordForm.currentPassword,
    this.passwordForm.newPassword,
    this.passwordForm.confirmPassword
  );

  if (!result.success) {
    this.passwordMessage = result.message;
    this.passwordIsError = true;
    return;
  }

  this.passwordMessage = result.message;
  this.passwordIsError = false;

  this.passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };
}
openDoctorProfileModal(): void {
  this.closeProfileMenu();

  if (!this.currentUser?.doctorId) {
    return;
  }

  this.doctorProfile = this.patientData.getDoctorById(this.currentUser.doctorId);

  if (!this.doctorProfile) {
    return;
  }

  this.showDoctorProfileModal = true;
}

closeDoctorProfileModal(): void {
  this.showDoctorProfileModal = false;
}
}