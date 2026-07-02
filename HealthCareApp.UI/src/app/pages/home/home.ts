import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { timeout } from 'rxjs';

import { AuthService } from '../../core/services/auth.service';
import {
  LoginRequest,
  PatientRegisterRequest,
  UserRole
} from '../../shared/models/auth.models';

interface LandingFeature {
  title: string;
  description: string;
  icon: string;
}

interface CareHighlight {
  title: string;
  description: string;
}

interface LoginFormModel {
  usernameOrEmail: string;
  password: string;
}

interface CountryCodeOption {
  label: string;
  value: string;
}

interface PatientRegisterFormModel {
  fullName: string;
  dateOfBirth: string;
  gender: string;
  email: string;
  countryCode: string;
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
export class Home implements OnInit {
  brandName = 'HealthAxis';

  heroTitle = 'Healthcare access made simple, secure, and connected.';

  heroDescription =
    'Book appointments, stay connected with doctors, and keep your care journey organized in one modern digital healthcare platform.';

  platformStatus = 'For Patients and Doctors';

  currentHighlightIndex = 0;

  isLoginModalOpen = false;
  isSignupModalOpen = false;

  isSubmittingLogin = false;
  isSubmittingSignup = false;

  loginMessage = '';
  signupMessage = '';

  todayDate = '';

  namePattern = '^[A-Za-z][A-Za-z\\s]{1,99}$';
  phonePattern = '^[0-9]{10}$';
  passwordPattern = '^(?=.*[A-Z])(?=.*\\d)(?=.*[^A-Za-z0-9]).{8,}$';

  genderOptions: string[] = [
    'Male',
    'Female',
    'Transgender',
    'Other'
  ];

  countryCodeOptions: CountryCodeOption[] = [
    {
      label: 'India +91',
      value: '+91'
    },
    {
      label: 'USA +1',
      value: '+1'
    },
    {
      label: 'UK +44',
      value: '+44'
    },
    {
      label: 'UAE +971',
      value: '+971'
    },
    {
      label: 'Australia +61',
      value: '+61'
    }
  ];

  features: LandingFeature[] = [];
  highlights: CareHighlight[] = [];

  loginForm: LoginFormModel = {
    usernameOrEmail: '',
    password: ''
  };

  patientRegisterForm: PatientRegisterFormModel = {
    fullName: '',
    dateOfBirth: '',
    gender: '',
    email: '',
    countryCode: '+91',
    phoneNumber: '',
    insuranceId: '',
    password: '',
    confirmPassword: ''
  };

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.todayDate = new Date().toISOString().split('T')[0];
    this.loadLandingContent();
  }

  get currentHighlight(): CareHighlight {
    if (this.highlights.length === 0) {
      return {
        title: '',
        description: ''
      };
    }

    return this.highlights[this.currentHighlightIndex];
  }

  get isPasswordMismatch(): boolean {
    const password = this.patientRegisterForm.password.trim();
    const confirmPassword = this.patientRegisterForm.confirmPassword.trim();

    return confirmPassword.length > 0 && password !== confirmPassword;
  }

  openLoginModal(): void {
    this.loginMessage = '';
    this.isSignupModalOpen = false;
    this.isLoginModalOpen = true;
  }

  openSignupModal(): void {
    this.signupMessage = '';
    this.isLoginModalOpen = false;
    this.isSignupModalOpen = true;
  }

  closeModals(): void {
    this.isLoginModalOpen = false;
    this.isSignupModalOpen = false;

    this.resetLoginForm();
    this.resetSignupForm();
  }

  submitLogin(form: NgForm): void {
    this.loginMessage = '';

    if (form.invalid) {
      form.control.markAllAsTouched();
      this.loginMessage = 'Please enter email and password.';
      return;
    }

    this.isSubmittingLogin = true;
    this.loginMessage = 'Checking your credentials...';

    const request: LoginRequest = {
      email: this.loginForm.usernameOrEmail.trim(),
      password: this.loginForm.password
    };

    this.authService.login(request).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        const loggedInRole = this.authService.getRole();

        if (!loggedInRole) {
          this.authService.logout();
          this.isSubmittingLogin = false;
          this.loginMessage = 'Unable to identify user role. Please contact support.';
          return;
        }

        this.redirectAfterLogin(loggedInRole);
      },
      error: (error: unknown) => {
        this.isSubmittingLogin = false;
        this.loginMessage = this.getLoginErrorMessage(error);
      }
    });
  }

  submitSignup(form: NgForm): void {
    this.signupMessage = '';

    if (form.invalid) {
      form.control.markAllAsTouched();
      this.signupMessage = 'Please correct the highlighted fields.';
      return;
    }

    if (this.isPasswordMismatch) {
      this.signupMessage = 'Password and Confirm Password do not match.';
      return;
    }

    if (this.patientRegisterForm.dateOfBirth > this.todayDate) {
      this.signupMessage = 'Date of birth cannot be a future date.';
      return;
    }

    this.isSubmittingSignup = true;
    this.signupMessage = 'Creating your patient account...';

    const request: PatientRegisterRequest = {
      fullName: this.patientRegisterForm.fullName.trim(),
      dateOfBirth: this.patientRegisterForm.dateOfBirth,
      gender: this.mapGenderToNumber(this.patientRegisterForm.gender),
      email: this.patientRegisterForm.email.trim(),
      phoneNumber: this.patientRegisterForm.phoneNumber.trim(),
      insuranceId: this.patientRegisterForm.insuranceId.trim(),
      password: this.patientRegisterForm.password,
      confirmPassword: this.patientRegisterForm.confirmPassword
    };

    this.authService.registerPatient(request).pipe(
      timeout(15000)
    ).subscribe({
      next: (response) => {
        this.isSubmittingSignup = false;
        this.signupMessage =
          response.message || 'Patient account created successfully. Please login.';

        form.resetForm({
          fullName: '',
          dateOfBirth: '',
          gender: '',
          email: '',
          countryCode: '+91',
          phoneNumber: '',
          insuranceId: '',
          password: '',
          confirmPassword: ''
        });

        setTimeout(() => {
          this.isSignupModalOpen = false;
          this.openLoginModal();
          this.loginMessage = 'Patient account created successfully. Please login.';
        }, 1200);
      },
      error: (error: unknown) => {
        this.isSubmittingSignup = false;
        this.signupMessage = this.getSignupErrorMessage(error);
      }
    });
  }

  scrollToAbout(): void {
    const aboutSection = document.getElementById('about-section');

    if (!aboutSection) {
      return;
    }

    aboutSection.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    });
  }

  showPreviousHighlight(): void {
    if (this.currentHighlightIndex > 0) {
      this.currentHighlightIndex--;
    }
  }

  showNextHighlight(): void {
    if (this.currentHighlightIndex < this.highlights.length - 1) {
      this.currentHighlightIndex++;
    }
  }

  private redirectAfterLogin(role: UserRole): void {
    this.isSubmittingLogin = false;
    this.isLoginModalOpen = false;
    this.isSignupModalOpen = false;

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

      window.location.href =
        `https://localhost:7075/admin-login-bridge?token=${encodeURIComponent(token)}`;

      return;
    }

    this.authService.logout();
    this.loginMessage = 'Unsupported user role. Please contact support.';
  }

  private resetLoginForm(): void {
    this.loginForm = {
      usernameOrEmail: '',
      password: ''
    };

    this.loginMessage = '';
    this.isSubmittingLogin = false;
  }

  private resetSignupForm(): void {
    this.patientRegisterForm = {
      fullName: '',
      dateOfBirth: '',
      gender: '',
      email: '',
      countryCode: '+91',
      phoneNumber: '',
      insuranceId: '',
      password: '',
      confirmPassword: ''
    };

    this.signupMessage = '';
    this.isSubmittingSignup = false;
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

  private getLoginErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      return (
        apiError.error?.message ??
        apiError.error?.Message ??
        'Invalid email or password.'
      );
    }

    return 'Invalid email or password.';
  }

  private getSignupErrorMessage(error: unknown): string {
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

    return 'Something went wrong while creating the patient account.';
  }

  private loadLandingContent(): void {
    this.features = [
      {
        title: 'For Patients',
        description:
          'Book appointments, view upcoming visits, manage profile details, and access personal health records anytime.',
        icon: 'PT'
      },
      {
        title: 'For Doctors',
        description:
          'Review appointment schedules, manage appointment actions, and maintain structured health records after consultation.',
        icon: 'DR'
      },
      {
        title: 'Care Records',
        description:
          'Keep diagnosis, prescriptions, visit notes, and care history organized for better continuity of treatment.',
        icon: 'HR'
      }
    ];

    this.highlights = [
      {
        title: 'Easy Appointment Booking',
        description:
          'Patients can choose care, book a slot, and track appointment status without manual follow-up.'
      },
      {
        title: 'Doctor-Led Care Flow',
        description:
          'Doctors can manage visits clearly with confirm, cancel, complete, and health record actions.'
      },
      {
        title: 'Personal Health Timeline',
        description:
          'Patients can view their appointment journey and health records from one organized portal.'
      },
      {
        title: 'Focused Care Experience',
        description:
          'Each user sees only the features needed for their healthcare journey, keeping the portal simple and clean.'
      }
    ];
  }
}