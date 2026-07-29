import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {

  registerForm: FormGroup;

  errorMessage = '';
  successMessage = '';
  isLoading = false;
  showSuccessDialog = false;

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router) {

    this.registerForm = this.fb.group(
      {
        fullName: ['', Validators.required],
        dateOfBirth: ['', Validators.required],
        gender: ['', Validators.required],
        phoneNumber: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        insuranceId: ['']
      },
      {
        validators: this.passwordMatchValidator
      }
    );
  }

  register(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    const { confirmPassword, ...request } = this.registerForm.value;

    this.authService.registerPatient(request)
      .subscribe({
        next: () => {

          this.isLoading = false;

          this.registerForm.reset();

          this.showSuccessDialog = true;
          this.successMessage = 'Patient registered successfully.';
        },

        error: (error: HttpErrorResponse) => {

          this.isLoading = false;

          if (typeof error.error === 'string') {
            this.errorMessage = error.error;
          } else {
            this.errorMessage =
              'Registration failed. Please try again.';
          }
        }
      });
  }

  closeDialog(): void {
    this.showSuccessDialog = false;
  }

  goToLogin(): void {
    this.showSuccessDialog = false;
    this.router.navigate(['/login']);
  }

  isInvalid(controlName: string): boolean {

    const control = this.registerForm.get(controlName);

    return !!control &&
      control.invalid &&
      (control.dirty || control.touched);
  }

  hasPasswordMismatch(): boolean {

    const control = this.registerForm.get('confirmPassword');

    return !!control &&
      control.hasError('passwordMismatch') &&
      (control.dirty || control.touched);
  }

  private passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {

    const password = control.get('password')?.value;
    const confirmPasswordControl = control.get('confirmPassword');

    if (!confirmPasswordControl) {
      return null;
    }

    const confirmPassword = confirmPasswordControl.value;

    if (!password || !confirmPassword) {
      return null;
    }

    if (password !== confirmPassword) {

      confirmPasswordControl.setErrors({
        passwordMismatch: true
      });

      return {
        passwordMismatch: true
      };
    }

    return null;
  }
}
