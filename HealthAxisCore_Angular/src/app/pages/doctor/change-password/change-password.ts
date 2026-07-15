import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-change-password',
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {
  passwordForm: FormGroup;

  submitted = signal(false);

  isSaving = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  showCurrentPassword = signal(false);

  showNewPassword = signal(false);

  showConfirmPassword = signal(false);

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {
    this.passwordForm = this.formBuilder.group({
      currentPassword: [
        '',
        [
          Validators.required
        ]
      ],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8)
        ]
      ],
      confirmPassword: [
        '',
        [
          Validators.required
        ]
      ]
    });
  }

  get currentPassword() {
    return this.passwordForm.get('currentPassword');
  }

  get newPassword() {
    return this.passwordForm.get('newPassword');
  }

  get confirmPassword() {
    return this.passwordForm.get('confirmPassword');
  }

  passwordsDoNotMatch(): boolean {
    return this.passwordForm.value.newPassword &&
      this.passwordForm.value.confirmPassword &&
      this.passwordForm.value.newPassword !== this.passwordForm.value.confirmPassword;
  }

  toggleCurrentPassword(): void {
    this.showCurrentPassword.update(value => !value);
  }

  toggleNewPassword(): void {
    this.showNewPassword.update(value => !value);
  }

  toggleConfirmPassword(): void {
    this.showConfirmPassword.update(value => !value);
  }

  submitPasswordChange(): void {
    this.submitted.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.passwordForm.invalid || this.passwordsDoNotMatch()) {
      this.passwordForm.markAllAsTouched();
      return;
    }

    this.isSaving.set(true);

    this.authService.changeFirstLoginPassword({
      currentPassword: this.passwordForm.value.currentPassword,
      newPassword: this.passwordForm.value.newPassword,
      confirmPassword: this.passwordForm.value.confirmPassword
    }).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.successMessage.set('Password changed successfully.');
        this.router.navigate(['/doctor/dashboard']);
      },
      error: error => {
        this.isSaving.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }
}
