import { Component, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../core/models/login-request';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  loginForm: FormGroup;

  showPassword = signal(false);

  submitted = signal(false);

  isLoading = signal(false);

  errorMessage = signal('');

  constructor(
    private readonly formBuilder: FormBuilder,
    private readonly authService: AuthService
  ) {
    this.loginForm = this.formBuilder.group({
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],
      password: [
        '',
        [
          Validators.required
        ]
      ],
      rememberMe: [true]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  togglePasswordVisibility(): void {
    this.showPassword.update(value => !value);
  }

  submitLogin(): void {
    this.submitted.set(true);
    this.errorMessage.set('');

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    // CHANGED:
    // Remove accidental leading/trailing spaces and normalize the email.
    const email = String(
      this.loginForm.value.email ?? ''
    )
      .trim()
      .toLowerCase();

    // Do not trim passwords because spaces may be legitimate password
    // characters.
    const password = String(
      this.loginForm.value.password ?? ''
    );

    const request: LoginRequest = {
      email,
      password
    };

    this.isLoading.set(true);

    this.authService.login(request).subscribe({
      next: () => {
        this.isLoading.set(false);
      },
      error: error => {
        this.isLoading.set(false);

        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
      }
    });
  }
}
