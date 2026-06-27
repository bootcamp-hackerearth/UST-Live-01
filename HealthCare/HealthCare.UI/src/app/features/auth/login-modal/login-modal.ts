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

    // Form validation
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    // Call Login API
    this.authService.login(this.loginForm.value)
      .subscribe({

        next: (res: any) => {

          console.log('Login Response:', res);

          // Save JWT token
          this.authService.saveToken(
            res.accessToken
          );

          // Get role from JWT
          const role =
            this.authService.getRole();

          console.log('Role:', role);

          // Close popup
          this.close.emit();

          // Role based navigation
          if (role === 'Patient') {

            this.router.navigate([
              '/patient/dashboard'
            ]);

          }
          else if (role === 'Doctor') {

            this.router.navigate([
              '/doctor/dashboard'
            ]);

          }
          else if (role === 'Admin') {

            // Redirect to Blazor Admin Portal
            window.location.href =
              `https://localhost:7125/login-redirect?token=${res.accessToken}`;
          }
          else {

            alert('Role not recognized');
          }
        },

        error: (err) => {

          console.error(err);

          alert(
            'Invalid Email or Password'
          );
        }
      });
  }

  closeModal(): void {

    this.close.emit();
  }
}