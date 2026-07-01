import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { AuthService } from '../../../core/services/auth-service';
import { ChangePassword as ChangePasswordModel } from '../../../core/models/change-password';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css'
})
export class ChangePassword {

  private fb = inject(FormBuilder);

  private authService = inject(AuthService);

  private router = inject(Router);

  private toastr = inject(ToastrService);

  isLoading = false;

  changePasswordForm = this.fb.group({

    currentPassword: [
      '',
      Validators.required
    ],

    newPassword: [
      '',
      [
        Validators.required,
        Validators.minLength(8)
      ]
    ],

    confirmNewPassword: [
      '',
      Validators.required
    ]

  });

  changePassword(): void {

    if (this.changePasswordForm.invalid) {

      this.changePasswordForm.markAllAsTouched();

      return;

    }

    const request: ChangePasswordModel = {

      currentPassword:
        this.changePasswordForm.value.currentPassword!,

      newPassword:
        this.changePasswordForm.value.newPassword!,

      confirmNewPassword:
        this.changePasswordForm.value.confirmNewPassword!

    };

    this.isLoading = true;

    this.authService
      .changePassword(request)
      .subscribe({

        next: () => {

          this.isLoading = false;

          localStorage.setItem(
            'mustChangePassword',
            'false'
          );

          this.toastr.success(
            'Password changed successfully.');

          this.router.navigate(['/doctor']);

        },

        error: (err) => {

          this.isLoading = false;

          this.toastr.error(
            err.error.message);

        }

      });

  }

}