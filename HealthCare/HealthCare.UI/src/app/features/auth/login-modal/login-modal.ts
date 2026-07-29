import { Component, EventEmitter, Output, signal } from '@angular/core';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login-modal.html',
  styleUrl: './login-modal.css'
})
export class LoginModal {

  @Output() modalClosed = new EventEmitter<void>();

  loginForm: FormGroup;

  serverError = signal('');

  constructor(
    private readonly fb: FormBuilder,
    private readonly authService: AuthService,
    private readonly router: Router,
    private readonly toastr: ToastrService
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

    this.serverError.set('');

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.authService.login(this.loginForm.value)
      .subscribe({

        next: (res: any) => {

          this.authService.saveToken(res.accessToken);

          const role = this.authService.getRole();

          this.modalClosed.emit();

          if (role === 'Patient') {

            this.router.navigate([
              '/patient/dashboard'
            ]);

          } else if (role === 'Doctor') {

            this.router.navigate([
              '/doctor/dashboard'
            ]);

          } else if (role === 'Admin') {

           globalThis.location.href = `${globalThis.location.origin}/blazor/login-redirect?token=${res.accessToken}`;
          }
        },

        error: (err: any) => {

         if (err?.error?.message) {
          this.serverError.set(err.error.message);
        } 
        else {
           this.serverError.set('Login failed.');
          }

          queueMicrotask(() => {
            this.toastr.error(
              this.serverError(),
              'Error'
            );
          });
        }
      });
  }

  closeModal(): void {
    this.modalClosed.emit();
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }
}