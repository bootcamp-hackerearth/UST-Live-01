import { Component, EventEmitter, Output } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';

import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login-modal.html',
  styleUrl: './login-modal.css'
})
export class LoginModal {

  @Output() close = new EventEmitter<void>();

  loginForm: FormGroup;

  // Error message shown inside popup
  serverError = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {

    this.loginForm = this.fb.group({
      email: [
        '',
        [Validators.required, Validators.email]
      ],

      password: [
        '',
        Validators.required
      ]
    });
  }

  onSubmit(): void {

    this.serverError = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.authService.login(this.loginForm.value)
      .subscribe({

        next: (res: any) => {

          this.authService.saveToken(res.accessToken);

          const role = this.authService.getRole();

          this.close.emit();

          if (role === 'Patient') {

            this.router.navigate([
              '/patient/dashboard'
            ]);

          } else if (role === 'Doctor') {

            this.router.navigate([
              '/doctor/dashboard'
            ]);

          } else if (role === 'Admin') {

            window.location.href =
              `https://localhost:7125/login-redirect?token=${res.accessToken}`;
          }
        },

        error: (err) => {

       if (err.status === 0) {
        this.serverError =
      'Unable to connect to server. Please try again later.';
      }
        else if (err.status === 401 || err.status === 400) {
         this.serverError =
          'Invalid email or password.';
         }
        else if (err.status === 500) {
        this.serverError =
       'Server error. Please try again later.';
       }
        else {
        this.serverError =
         'Login failed. Please try again.';
      }
}
      });
  }

  closeModal(): void {
    this.close.emit();
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}