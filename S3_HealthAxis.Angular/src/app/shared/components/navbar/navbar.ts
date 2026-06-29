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
import { TokenService } from '../../../core/services/token.service';

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
  changingPassword = false;

  passwordSuccessMessage = '';
  passwordErrorMessage = '';

  showNewPassword = false;
  showConfirmPassword = false;

  changePasswordForm = {
    newPassword: '',
    confirmNewPassword: ''
  };

  private routerSubscription?: Subscription;

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.setCurrentPath(this.router.url);

    this.routerSubscription = this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe((event) => {
        this.setCurrentPath(event.urlAfterRedirects);
        this.isMenuOpen = false;
      });
  }

  ngOnDestroy(): void {
    this.routerSubscription?.unsubscribe();
  }

  private setCurrentPath(url: string): void {
    const cleanUrl = url.split('?')[0].split('#')[0];
    this.currentPath = cleanUrl === '' ? '/' : cleanUrl;
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

  get passwordStrengthScore(): number {
    const password = this.changePasswordForm.newPassword;
    let score = 0;

    if (password.length >= 8) score++;
    if (/[A-Z]/.test(password)) score++;
    if (/[a-z]/.test(password)) score++;
    if (/[0-9]/.test(password)) score++;
    if (/[^A-Za-z0-9]/.test(password)) score++;

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
      this.passwordStrengthScore >= 3 &&
      !!this.changePasswordForm.confirmNewPassword &&
      this.passwordsMatch
    );
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  openChangePasswordModal(): void {
    this.isMenuOpen = false;
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

  toggleNewPasswordVisibility(): void {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  submitChangePassword(): void {
    this.resetPasswordMessages();

    if (!this.changePasswordForm.newPassword) {
      this.passwordErrorMessage = 'New password is required.';
      return;
    }

    if (!this.changePasswordForm.confirmNewPassword) {
      this.passwordErrorMessage = 'Confirm password is required.';
      return;
    }

    if (!this.passwordsMatch) {
      this.passwordErrorMessage = 'Passwords do not match.';
      return;
    }

    if (this.passwordStrengthScore < 3) {
      this.passwordErrorMessage =
        'Password must be stronger. Use at least 8 characters with uppercase, lowercase, number, or symbol.';
      return;
    }

    this.changingPassword = true;

    this.authService.changePassword({
      newPassword: this.changePasswordForm.newPassword,
      confirmNewPassword: this.changePasswordForm.confirmNewPassword
    }).subscribe({
      next: (response) => {
        this.changingPassword = false;
        this.passwordSuccessMessage =
          response?.message ?? 'Password changed successfully.';

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
          this.passwordErrorMessage =
            'Session expired or unauthorized. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.passwordErrorMessage =
            'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.passwordErrorMessage =
          'Could not change password. Please try again.';
      }
    });
  }

  logout(): void {
    this.tokenService.clearAuthData();
    this.isMenuOpen = false;
    this.router.navigate(['/login']);
  }

  private resetPasswordMessages(): void {
    this.passwordSuccessMessage = '';
    this.passwordErrorMessage = '';
  }

  private resetPasswordForm(): void {
    this.changePasswordForm = {
      newPassword: '',
      confirmNewPassword: ''
    };

    this.showNewPassword = false;
    this.showConfirmPassword = false;
    this.resetPasswordMessages();
  }
}

