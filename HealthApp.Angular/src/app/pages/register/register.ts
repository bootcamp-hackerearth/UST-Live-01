import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { GenderType } from '../../services/mock-patient-data';
import { MockAuth } from '../../services/mock-auth';

@Component({
  selector: 'app-register',
  imports: [RouterLink, FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private auth = inject(MockAuth);
  private router = inject(Router);

  fullName = '';
  email = '';
  password = '';
  confirmPassword = '';
  phoneNumber = '';
  gender: GenderType | '' = '';
  dateOfBirth = '';
  insuranceId = '';

  genders: GenderType[] = ['Male', 'Female', 'Transgender', 'Other'];

  isSubmitting = false;
  message = '';
  isError = false;

  registerPatient(): void {
  this.message = '';
  this.isError = false;
  this.isSubmitting = false;

  const fullNamePattern = /^[A-Z][a-zA-Z\s]{2,}$/;
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const phonePattern = /^[6-9]\d{9}$/;

  if (
    !this.fullName.trim() ||
    !this.email.trim() ||
    !this.password.trim() ||
    !this.confirmPassword.trim() ||
    !this.phoneNumber.trim() ||
    !this.gender ||
    !this.dateOfBirth
  ) {
    this.showError('Please fill all required fields.');
    return;
  }

  if (!fullNamePattern.test(this.fullName.trim())) {
    this.showError('Full name must start with an uppercase letter and be at least 3 characters long.');
    return;
  }

  if (!emailPattern.test(this.email.trim())) {
    this.showError('Valid email address is required.');
    return;
  }

  if (!phonePattern.test(this.phoneNumber.trim())) {
    this.showError('Invalid phone number. Phone number must start with 6-9 and contain 10 digits.');
    return;
  }

  if (this.password.length < 6) {
    this.showError('Password must be at least 6 characters long.');
    return;
  }

  if (this.password !== this.confirmPassword) {
    this.showError('Password and confirm password do not match.');
    return;
  }

  if (new Date(this.dateOfBirth) > new Date()) {
    this.showError('Date of birth cannot be in the future.');
    return;
  }

  this.isSubmitting = true;

  try {
    const result = this.auth.registerPatient({
      fullName: this.fullName,
      email: this.email,
      password: this.password,
      dateOfBirth: this.dateOfBirth,
      gender: this.gender as GenderType,
      phoneNumber: this.phoneNumber,
      insuranceId: this.insuranceId
    });

    this.isSubmitting = false;

    if (!result.success) {
      this.showError(result.message);
      return;
    }

    this.message = 'Patient registered successfully. You can login now.';
    this.isError = false;

    setTimeout(() => {
      this.router.navigate(['/login']);
    }, 900);
  } catch {
    this.isSubmitting = false;
    this.showError('Registration failed. Please try again.');
  }
}

  private showError(message: string): void {
  this.isSubmitting = false;
  this.message = message;
  this.isError = true;
}
}