import { Component, inject, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { finalize } from 'rxjs';

import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { RegisterPatientDto } from '../../../dtos/auth.dto';

@Component({
  selector: 'app-patient-register',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './patient-register.html',
  styleUrl: './patient-register.css'
})
export class PatientRegister {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly notificationService = inject(NotificationService);

  readonly isSubmitting = signal(false);

  form: RegisterPatientDto = {
    fullName: '',
    dateOfBirth: '',
    gender: '',
    phoneNumber: '',
    email: '',
    insuranceId: '',
    password: '',
    confirmPassword: ''
  };

  registerPatient(registerForm: NgForm): void {
    if (registerForm.invalid) {
      registerForm.control.markAllAsTouched();
      this.notificationService.warning('Please fill all required fields correctly.');
      return;
    }

    if (this.form.password !== this.form.confirmPassword) {
      registerForm.control.markAllAsTouched();
      this.notificationService.warning('Passwords do not match.');
      return;
    }

    const request: RegisterPatientDto = {
      ...this.form,
      fullName: this.form.fullName.trim(),
      phoneNumber: this.form.phoneNumber.trim(),
      email: this.form.email.trim(),
      insuranceId: this.form.insuranceId?.trim() || null
    };

    this.isSubmitting.set(true);

    this.authService
      .registerPatient(request)
      .pipe(
        finalize(() => {
          this.isSubmitting.set(false);
        })
      )
      .subscribe({
        next: response => {
          this.notificationService.success(
            response.message || 'Patient registered successfully.'
          );

          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 900);
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  goBackHome(): void {
    this.router.navigate(['/']);
  }
}