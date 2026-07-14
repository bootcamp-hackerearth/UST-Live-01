import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { timeout } from 'rxjs';

import { AuthService } from '../../core/services/auth.service';
import {
  LoginRequest,
  PatientRegisterRequest,
  UserRole
} from '../../shared/models/auth.models';

type HomeModal =
  | ''
  | 'login'
  | 'register'
  | 'visit';

type HomeMessageType =
  | 'success'
  | 'info'
  | 'warning'
  | 'error';

interface LandingGoal {
  number: string;
  title: string;
  text: string;
}

interface LandingFeature {
  icon: string;
  title: string;
  text: string;
}

interface LoginForm {
  email: string;
  password: string;
}

interface RegisterForm {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  phoneNumber: string;
  insuranceId: string;
  password: string;
  confirmPassword: string;
}

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  activeModal: HomeModal = '';

  isLoggingIn = false;
  isRegistering = false;

  modalMessage = '';
  modalMessageType: HomeMessageType = 'info';

  currentGoalIndex = 0;

  loginForm: LoginForm = {
    email: '',
    password: ''
  };

  registerForm: RegisterForm = {
    fullName: '',
    dateOfBirth: '',
    gender: '',
    email: '',
    phoneNumber: '',
    insuranceId: '',
    password: '',
    confirmPassword: ''
  };

  genderOptions: string[] = [
    'Male',
    'Female',
    'Transgender',
    'Other'
  ];

  goals: LandingGoal[] = [
    {
      number: '01',
      title: 'One trusted care gateway',
      text: 'Bring patients, doctors, bookings, profiles, and health records into one secure healthcare workspace.'
    },
    {
      number: '02',
      title: 'Faster appointment movement',
      text: 'Patients can request visits, doctors can confirm consultations, and completed visits can become records.'
    },
    {
      number: '03',
      title: 'Role-based digital access',
      text: 'Patients and doctors enter dedicated dashboards with guarded access and token-based protection.'
    }
  ];

  features: LandingFeature[] = [
    {
      icon: 'PX',
      title: 'Patient Experience',
      text: 'Profile management, appointment booking, visit tracking, and health record access in one location.'
    },
    {
      icon: 'DX',
      title: 'Doctor Workspace',
      text: 'Appointment confirmation, consultation flow, health record creation, and doctor profile security.'
    },
    {
      icon: 'SC',
      title: 'Secure Care Layer',
      text: 'JWT authentication, guarded routes, role checks, and protected dashboard access.'
    }
  ];

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
  }

  get activeGoal(): LandingGoal {
    return this.goals[this.currentGoalIndex];
  }

  get todayDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  scrollToAbout(): void {
    this.scrollToSection('about-section');
  }

  scrollToGoals(): void {
    this.scrollToSection('goals-section');
  }

  nextGoal(): void {
    this.currentGoalIndex =
      this.currentGoalIndex === this.goals.length - 1
        ? 0
        : this.currentGoalIndex + 1;
  }

  previousGoal(): void {
    this.currentGoalIndex =
      this.currentGoalIndex === 0
        ? this.goals.length - 1
        : this.currentGoalIndex - 1;
  }

  openLoginModal(): void {
    this.resetModalMessage();
    this.activeModal = 'login';
  }

  openCreatePatientModal(): void {
    this.resetModalMessage();
    this.activeModal = 'register';
  }

  openSignUpModal(): void {
    this.openCreatePatientModal();
  }

  openBookVisitModal(): void {
    if (this.authService.isPatient()) {
      this.router.navigate(['/patient/dashboard']);
      return;
    }

    this.modalMessage = 'Login as a patient or register a patient account to book an appointment.';
    this.modalMessageType = 'info';
    this.activeModal = 'visit';
  }

  closeModals(): void {
    if (this.isLoggingIn || this.isRegistering) {
      return;
    }

    this.activeModal = '';
    this.resetModalMessage();
  }

  submitLogin(): void {
    this.resetModalMessage();

    const email = this.loginForm.email.trim();
    const password = this.loginForm.password;

    if (!email || !password) {
      this.setModalMessage('Please enter email and password.', 'warning');
      return;
    }

    const request: LoginRequest = {
      email,
      password
    };

    this.isLoggingIn = true;

    this.authService.login(request).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isLoggingIn = false;
        this.routeAfterLogin();
      },
      error: (error: unknown) => {
        this.isLoggingIn = false;
        this.setModalMessage(this.getErrorMessage(error, 'Login failed. Please try again.'), 'error');
      }
    });
  }

  submitRegister(): void {
    this.resetModalMessage();

    if (!this.isRegisterFormValid()) {
      return;
    }

    const request: PatientRegisterRequest = {
      fullName: this.registerForm.fullName.trim(),
      dateOfBirth: this.registerForm.dateOfBirth,
      gender: this.mapGenderToNumber(this.registerForm.gender),
      email: this.registerForm.email.trim(),
      phoneNumber: this.registerForm.phoneNumber.trim(),
      insuranceId: this.registerForm.insuranceId.trim(),
      password: this.registerForm.password,
      confirmPassword: this.registerForm.confirmPassword
    };

    this.isRegistering = true;

    this.authService.registerPatient(request).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isRegistering = false;
        this.resetRegisterForm();

        this.activeModal = 'login';
        this.setModalMessage(
          'Patient account created successfully. Please login to continue.',
          'success'
        );
      },
      error: (error: unknown) => {
        this.isRegistering = false;
        this.setModalMessage(
          this.getErrorMessage(error, 'Registration failed. Please try again.'),
          'error'
        );
      }
    });
  }

  continueVisitFlow(): void {
    this.activeModal = 'login';
    this.setModalMessage('Login with your patient account to continue booking.', 'info');
  }

  private routeAfterLogin(): void {
    const role = this.authService.getRole();

    this.activeModal = '';
    this.resetModalMessage();

    if (role === 'Patient') {
      this.router.navigate(['/patient/dashboard']);
      return;
    }

    if (role === 'Doctor') {
      this.router.navigate(['/doctor/dashboard']);
      return;
    }

    if (role === 'Admin') {

      const token = this.authService.getToken();

    globalThis.location.href =
  `https://localhost:7075/admin-login-bridge?token=${encodeURIComponent(token)}`;
      return;

    }

    this.router.navigate(['/route-unavailable']);
  }

  private isRegisterFormValid(): boolean {
    const fullName = this.registerForm.fullName.trim();
    const email = this.registerForm.email.trim();
    const phoneNumber = this.registerForm.phoneNumber.trim();
    const insuranceId = this.registerForm.insuranceId.trim();

    if (fullName.length < 2) {
      this.setModalMessage('Full name must contain at least 2 characters.', 'warning');
      return false;
    }

    if (!this.registerForm.dateOfBirth) {
      this.setModalMessage('Date of birth is required.', 'warning');
      return false;
    }

    if (this.registerForm.dateOfBirth > this.todayDate) {
      this.setModalMessage('Date of birth cannot be a future date.', 'warning');
      return false;
    }

    if (!this.registerForm.gender) {
      this.setModalMessage('Please select gender.', 'warning');
      return false;
    }

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      this.setModalMessage('Please enter a valid email address.', 'warning');
      return false;
    }

    if (!/^\d{10}$/.test(phoneNumber)) {
      this.setModalMessage('Phone number must contain exactly 10 digits.', 'warning');
      return false;
    }

    if (!insuranceId || insuranceId.length > 30) {
      this.setModalMessage('Insurance ID is required and cannot exceed 30 characters.', 'warning');
      return false;
    }

    if (!/^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/.test(this.registerForm.password)) {
      this.setModalMessage(
        'Password must be at least 8 characters and include uppercase letter, number, and special character.',
        'warning'
      );
      return false;
    }

    if (this.registerForm.password !== this.registerForm.confirmPassword) {
      this.setModalMessage('Password and confirm password do not match.', 'warning');
      return false;
    }

    return true;
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

  private resetRegisterForm(): void {
    this.registerForm = {
      fullName: '',
      dateOfBirth: '',
      gender: '',
      email: '',
      phoneNumber: '',
      insuranceId: '',
      password: '',
      confirmPassword: ''
    };
  }

  private resetModalMessage(): void {
    this.modalMessage = '';
    this.modalMessageType = 'info';
  }

  private setModalMessage(message: string, type: HomeMessageType): void {
    this.modalMessage = message;
    this.modalMessageType = type;
  }

  private scrollToSection(sectionId: string): void {
    const section = document.getElementById(sectionId);

    if (!section) {
      return;
    }

    section.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    });
  }

  private getErrorMessage(error: unknown, fallbackMessage: string): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          title?: string;
          errors?: Record<string, string[]>;
        } | string;
        name?: string;
        message?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (typeof apiError.error === 'string' && apiError.error.trim()) {
        return apiError.error;
      }

      if (typeof apiError.error === 'object' && apiError.error !== null) {
        if (apiError.error.message) {
          return apiError.error.message;
        }

        if (apiError.error.Message) {
          return apiError.error.Message;
        }

        if (apiError.error.title) {
          return apiError.error.title;
        }

        if (apiError.error.errors) {
          const firstError = Object.values(apiError.error.errors)[0]?.[0];

          if (firstError) {
            return firstError;
          }
        }
      }

      if (apiError.message) {
        return apiError.message;
      }
    }

    return fallbackMessage;
  }
}