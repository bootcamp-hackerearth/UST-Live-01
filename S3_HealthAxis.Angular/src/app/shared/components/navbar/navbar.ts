import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive
} from '@angular/router';
import { filter, Subscription } from 'rxjs';

import { AuthService } from '../../../core/services/auth.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { PatientPortalService } from '../../../core/services/patient-portal.service';
import { TokenService } from '../../../core/services/token.service';

const CREDENTIAL_MESSAGES = {
  currentRequired: 'Current credential is required.',
  newRequired: 'New credential is required.',
  confirmRequired: 'Please confirm the new credential.',
  sameAsCurrent: 'New credential cannot be the same as current credential.',
  mismatch: 'New credential and confirmation do not match.',
  weak: 'Credential must be at least 8 characters and include uppercase, lowercase, number, and special character.',
  changed: 'Credential changed successfully.',
  updateFailed: 'Could not update credential. Please try again.',
  sessionExpired: 'Session expired or unauthorized. Please login again.',
  apiUnavailable: 'Could not connect to the API. Please make sure the API is running.'
};

@Component({
  selector: 'app-navbar',
  imports: [
    FormsModule,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements OnInit, OnDestroy {
  currentPath = '';
  isMenuOpen = false;

  showChangePasswordModal = false;
  showLogoutConfirmModal = false;

  changingPassword = false;

  passwordSuccessMessage = String();
  passwordErrorMessage = String();

  showCurrentPassword = false;
  showNewPassword = false;
  showConfirmPassword = false;

  fullName = '';

  changePasswordForm = {
    currentPassword: String(),
    newPassword: String(),
    confirmNewPassword: String()
  };

  private routerSubscription?: Subscription;

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private authService: AuthService,
    private patientPortalService: PatientPortalService,
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {
    this.setCurrentPath(this.router.url);
    this.loadLoggedInUserName();

    this.routerSubscription = this.router.events
      .pipe(
        filter((event): event is NavigationEnd => event instanceof NavigationEnd)
      )
      .subscribe((event) => {
        this.setCurrentPath(event.urlAfterRedirects);
        this.isMenuOpen = false;
        this.loadLoggedInUserName();
      });
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
  }

  private setCurrentPath(url: string): void {
    const cleanUrl = url.split('?')[0].split('#')[0];
    this.currentPath = cleanUrl === '' ? '/' : cleanUrl;
  }

  private loadLoggedInUserName(): void {
    if (!this.isLoggedIn) {
      this.fullName = '';
      return;
    }

    const referenceId = this.tokenService.getReferenceId();

    if (!referenceId) {
      this.fullName = '';
      return;
    }

    if (this.isPatient) {
      this.patientPortalService.getPatientProfile(referenceId).subscribe({
        next: (patient) => {
          this.fullName = patient.fullName || '';
        },
        error: () => {
          this.fullName = '';
        }
      });

      return;
    }

    if (this.isDoctor) {
      this.doctorService.getDoctorById(referenceId).subscribe({
        next: (doctor) => {
          this.fullName = doctor.fullName || '';
        },
        error: () => {
          this.fullName = '';
        }
      });

      return;
    }

    this.fullName = '';
  }

  get isLoggedIn(): boolean {
    return this.tokenService.isLoggedIn();
  }

  get role(): string {
    return this.tokenService.getRole() ?? '';
  }

  get isPatient(): boolean {
    return this.role.toLowerCase() === 'patient';
  }

  get isDoctor(): boolean {
    return this.role.toLowerCase() === 'doctor';
  }

  get userEmail(): string {
    return this.tokenService.getEmail() ?? '';
  }

  get displayName(): string {
    if (this.fullName.trim()) {
      return this.fullName.trim();
    }

    const email = this.userEmail;

    if (!email) {
      return 'User';
    }

    return email.split('@')[0];
  }

  get isHomePage(): boolean {
    return this.currentPath === '/';
  }

  get isLoginPage(): boolean {
    return this.currentPath === '/login';
  }

  get isRegisterPage(): boolean {
    return this.currentPath === '/register';
  }

  get showHomeLink(): boolean {
    return !this.isHomePage;
  }

  get showPatientJourneyLink(): boolean {
    return this.isHomePage && !this.isLoggedIn;
  }

  get showLoginLink(): boolean {
    return !this.isLoginPage && !this.isLoggedIn;
  }

  get showRegisterLink(): boolean {
    return !this.isRegisterPage && !this.isLoggedIn;
  }

  get hasMinLength(): boolean {
    return this.changePasswordForm.newPassword.length >= 8;
  }

  get hasUppercase(): boolean {
    return /[A-Z]/.test(this.changePasswordForm.newPassword);
  }

  get hasLowercase(): boolean {
    return /[a-z]/.test(this.changePasswordForm.newPassword);
  }

  get hasNumber(): boolean {
    return /[0-9]/.test(this.changePasswordForm.newPassword);
  }

  get hasSpecialCharacter(): boolean {
    return /[^A-Za-z0-9]/.test(this.changePasswordForm.newPassword);
  }

  get isNewPasswordStrong(): boolean {
    return (
      this.hasMinLength &&
      this.hasUppercase &&
      this.hasLowercase &&
      this.hasNumber &&
      this.hasSpecialCharacter
    );
  }

  get isNewPasswordSameAsCurrent(): boolean {
    return (
      !!this.changePasswordForm.currentPassword &&
      !!this.changePasswordForm.newPassword &&
      this.changePasswordForm.currentPassword === this.changePasswordForm.newPassword
    );
  }

  get passwordStrengthScore(): number {
    let score = 0;

    if (this.hasMinLength) {
      score++;
    }

    if (this.hasUppercase) {
      score++;
    }

    if (this.hasLowercase) {
      score++;
    }

    if (this.hasNumber) {
      score++;
    }

    if (this.hasSpecialCharacter) {
      score++;
    }

    return score;
  }

  get passwordStrengthLabel(): string {
    if (!this.changePasswordForm.newPassword) {
      return '';
    }

    if (this.passwordStrengthScore <= 2) {
      return 'Weak';
    }

    if (this.passwordStrengthScore <= 4) {
      return 'Medium';
    }

    return 'Strong';
  }

  get passwordStrengthClass(): string {
    if (!this.changePasswordForm.newPassword) {
      return '';
    }

    if (this.passwordStrengthScore <= 2) {
      return 'weak';
    }

    if (this.passwordStrengthScore <= 4) {
      return 'medium';
    }

    return 'strong';
  }

  get passwordsMatch(): boolean {
    return (
      this.changePasswordForm.newPassword ===
      this.changePasswordForm.confirmNewPassword
    );
  }

  get canSubmitChangePassword(): boolean {
    return (
      !!this.changePasswordForm.currentPassword &&
      !!this.changePasswordForm.newPassword &&
      !!this.changePasswordForm.confirmNewPassword &&
      this.isNewPasswordStrong &&
      !this.isNewPasswordSameAsCurrent &&
      this.passwordsMatch
    );
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  openChangePasswordModal(): void {
    this.isMenuOpen = false;
    this.showLogoutConfirmModal = false;
    this.showChangePasswordModal = true;
    this.resetPasswordMessages();
  }

  closeChangePasswordModal(): void {
    if (this.changingPassword) {
      return;
    }

    this.showChangePasswordModal = false;
    this.resetPasswordForm();
  }

  toggleCurrentPasswordVisibility(): void {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  submitChangePassword(): void {
    this.resetPasswordMessages();

    if (!this.changePasswordForm.currentPassword) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.currentRequired;
      return;
    }

    if (!this.changePasswordForm.newPassword) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.newRequired;
      return;
    }

    if (!this.changePasswordForm.confirmNewPassword) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.confirmRequired;
      return;
    }

    if (this.isNewPasswordSameAsCurrent) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.sameAsCurrent;
      return;
    }

    if (!this.passwordsMatch) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.mismatch;
      return;
    }

    if (!this.isNewPasswordStrong) {
      this.passwordErrorMessage = CREDENTIAL_MESSAGES.weak;
      return;
    }

    this.changingPassword = true;

    this.authService.changePassword({
      currentPassword: this.changePasswordForm.currentPassword,
      newPassword: this.changePasswordForm.newPassword,
      confirmNewPassword: this.changePasswordForm.confirmNewPassword
    }).subscribe({
      next: (response) => {
        this.changingPassword = false;
        this.passwordSuccessMessage =
          response?.message ?? CREDENTIAL_MESSAGES.changed;

        setTimeout(() => {
          this.showChangePasswordModal = false;
          this.resetPasswordForm();
        }, 1200);
      },
      error: (error) => {
        this.changingPassword = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.passwordErrorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.passwordErrorMessage = CREDENTIAL_MESSAGES.sessionExpired;
          return;
        }

        if (error.status === 0) {
          this.passwordErrorMessage = CREDENTIAL_MESSAGES.apiUnavailable;
          return;
        }

        this.passwordErrorMessage = CREDENTIAL_MESSAGES.updateFailed;
      }
    });
  }

  openLogoutConfirmModal(): void {
    this.isMenuOpen = false;
    this.showChangePasswordModal = false;
    this.showLogoutConfirmModal = true;
  }

  closeLogoutConfirmModal(): void {
    this.showLogoutConfirmModal = false;
  }

  confirmLogout(): void {
    this.tokenService.clearAuthData();
    this.fullName = '';
    this.showLogoutConfirmModal = false;
    this.isMenuOpen = false;
    this.router.navigate(['/login']);
  }

  private resetPasswordMessages(): void {
    this.passwordSuccessMessage = String();
    this.passwordErrorMessage = String();
  }

  private resetPasswordForm(): void {
    this.changePasswordForm = {
      currentPassword: String(),
      newPassword: String(),
      confirmNewPassword: String()
    };

    this.showCurrentPassword = false;
    this.showNewPassword = false;
    this.showConfirmPassword = false;
    this.resetPasswordMessages();
  }
}

