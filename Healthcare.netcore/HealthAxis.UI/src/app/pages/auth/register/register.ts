import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {

  private http = inject(HttpClient);
  private router = inject(Router);

  fullName = '';
  dateOfBirth = '';
  gender = '';
  phoneNumber = '';
  email = '';
  password = '';
  confirmPassword = '';

  submitted = false;
  isRegistering = false;

  todayDate = new Date().toISOString().split('T')[0];

  private registerUrl = 'https://localhost:7130/api/auth/register';

  get fullNameError(): string {
    if (!this.submitted) return '';

    if (!this.fullName.trim()) {
      return 'Full name is required.';
    }

    if (this.fullName.trim().length < 3) {
      return 'Full name must be at least 3 characters.';
    }

    return '';
  }

  get dateOfBirthError(): string {
    if (!this.submitted) return '';

    if (!this.dateOfBirth) {
      return 'Date of birth is required.';
    }

    const selectedDate = new Date(this.dateOfBirth);
    const today = new Date();

    if (selectedDate > today) {
      return 'Future date is not allowed.';
    }

    return '';
  }

  get genderError(): string {
    if (!this.submitted) return '';

    if (this.gender === '') {
      return 'Gender is required.';
    }

    return '';
  }

  get phoneNumberError(): string {
    if (!this.submitted) return '';

    if (!this.phoneNumber.trim()) {
      return 'Phone number is required.';
    }

    const phonePattern = /^[0-9]{10}$/;

    if (!phonePattern.test(this.phoneNumber.trim())) {
      return 'Phone number must be 10 digits.';
    }

    return '';
  }

  get emailError(): string {
    if (!this.submitted) return '';

    if (!this.email.trim()) {
      return 'Email is required.';
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(this.email.trim())) {
      return 'Enter a valid email address.';
    }

    return '';
  }

  get passwordError(): string {
    if (!this.submitted) return '';

    if (!this.password.trim()) {
      return 'Password is required.';
    }

    if (this.password.length < 6) {
      return 'Password must be at least 6 characters.';
    }

    return '';
  }

  get confirmPasswordError(): string {
    if (!this.submitted) return '';

    if (!this.confirmPassword.trim()) {
      return 'Confirm password is required.';
    }

    if (this.password !== this.confirmPassword) {
      return 'Password and confirm password do not match.';
    }

    return '';
  }

  hasErrors(): boolean {
    return !!(
      this.fullNameError ||
      this.dateOfBirthError ||
      this.genderError ||
      this.phoneNumberError ||
      this.emailError ||
      this.passwordError ||
      this.confirmPasswordError
    );
  }

  register() {
    this.submitted = true;

    if (this.hasErrors()) {
      return;
    }

    const payload = {
      fullName: this.fullName,
      dateOfBirth: this.dateOfBirth,
      gender: Number(this.gender),
      phoneNumber: this.phoneNumber,
      email: this.email,
      password: this.password,
      confirmPassword: this.confirmPassword,
      insuranceId: 'NA'
    };

    console.log('Patient register payload ✅:', payload);

    this.isRegistering = true;

    this.http.post<any>(this.registerUrl, payload).subscribe({
      next: (res: any) => {
        console.log('Patient registered ✅:', res);

        this.isRegistering = false;

        alert('Registration successful ✅ Please login.');

        this.router.navigate(['/login']);
      },
      error: (err: any) => {
        console.error('Patient registration failed ❌:', err);

        this.isRegistering = false;

        const message =
          err?.error?.message ||
          err?.error?.title ||
          err?.error ||
          'Registration failed.';

        alert(message);
      }
    });
  }
}