import { Component, OnInit } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';

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

  genderOptions: string[] = ['Male', 'Female', 'Other'];

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

  constructor(private router: Router) {
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
      this.loginMessage = 'Please enter username/email and password.';
      return;
    }

    this.isSubmittingLogin = true;
    this.loginMessage = 'Checking your credentials...';

    setTimeout(() => {
      this.isSubmittingLogin = false;

      this.isLoginModalOpen = false;
      this.isSignupModalOpen = false;

      this.router.navigate(['/patient/dashboard']);
    }, 700);
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

    this.isSubmittingSignup = true;
    this.signupMessage = 'Creating your patient account...';

    setTimeout(() => {
      this.isSubmittingSignup = false;
      this.signupMessage =
        'Patient registration validation is ready. Backend connection will be added next.';
    }, 900);
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