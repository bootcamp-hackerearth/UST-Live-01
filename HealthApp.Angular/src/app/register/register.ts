import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm, NgModel } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../service/auth.service';
import { RegisterForm } from '../models/RegisterForm/RegisterForm';
import { AppPopupComponent } from '../shared/app-popup/app-popup';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, AppPopupComponent],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {

  form: RegisterForm = {
    email: '',
    password: '',
    confirmPassword: '',
    fullName: '',
    dateOfBirth: '',
    gender: '',
    phoneNumber: '',
    insuranceId: ''
  };

  maxDate = this.getPreviousDate();

  emailAlreadyExists = false;

  popupVisible = false;
  popupTitle = '';
  popupMessage = '';
  popupType: 'success' | 'error' | 'warning' = 'success';

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  submit(registerForm: NgForm): void {
    this.emailAlreadyExists = false;

    if (registerForm.invalid) {
      registerForm.control.markAllAsTouched();

      this.showPopup(
        'Invalid Registration Data',
        'Please correct the highlighted fields and try again.',
        'error'
      );

      return;
    }

    if (this.form.dateOfBirth && this.form.dateOfBirth > this.maxDate) {
      this.showPopup(
        'Invalid Date of Birth',
        'Date of birth must be a previous date.',
        'error'
      );

      return;
    }

    if (this.form.password !== this.form.confirmPassword) {
      this.showPopup(
        'Password Mismatch',
        'Password and confirm password must match.',
        'error'
      );

      return;
    }

    this.authService.register(this.form).subscribe({
      next: (res) => {
        this.showPopup(
          'Registration Successful',
          res?.message || 'Account created successfully.',
          'success'
        );

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1200);
      },

      error: (err) => {
        const message = this.getErrorMessage(err);

        if (this.isEmailAlreadyExistsError(message)) {
          this.emailAlreadyExists = true;

          this.showPopup(
            'Email Already Registered',
            'This email is already registered. Please sign in or use another email.',
            'error'
          );

          return;
        }

        this.showPopup(
          'Registration Failed',
          message,
          'error'
        );
      }
    });
  }

  showError(control: NgModel, form: NgForm): boolean {
    return !!control.invalid && (control.touched || form.submitted);
  }

  clearEmailExistsError(): void {
    this.emailAlreadyExists = false;
  }

  private isEmailAlreadyExistsError(message: string): boolean {
    const value = message.toLowerCase();

    return (
      value.includes('email already exists') ||
      value.includes('email already registered') ||
      value.includes('already taken') ||
      value.includes('duplicate email')
    );
  }

  private getErrorMessage(err: any): string {
    const error = err?.error;

    if (!error) {
      return 'Unable to create your account.';
    }

    if (typeof error === 'string') {
      return error;
    }

    if (error.message) {
      return String(error.message);
    }

    const errors = error.errors;

    if (errors) {
      if (Array.isArray(errors)) {
        const firstError = errors[0];

        return String(
          firstError?.description ||
          firstError?.message ||
          firstError ||
          'Unable to create your account.'
        );
      }

      const firstKey = Object.keys(errors)[0];

      if (firstKey && errors[firstKey]?.length) {
        return String(errors[firstKey][0]);
      }
    }

    return 'Unable to create your account.';
  }

  private getPreviousDate(): string {
    const date = new Date();
    date.setDate(date.getDate() - 1);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private showPopup(
    title: string,
    message: string,
    type: 'success' | 'error' | 'warning' = 'success'
  ): void {
    this.popupTitle = title;
    this.popupMessage = message;
    this.popupType = type;
    this.popupVisible = true;
  }

  closePopup(): void {
    this.popupVisible = false;
    this.popupTitle = '';
    this.popupMessage = '';
    this.popupType = 'success';
  }
}