import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';

import { AuthService } from '../../services/auth.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  loginForm: any;
  showPassword = false;
  submitted = false;

  loginError = '';

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get email() {
    return this.loginForm.get('email');
  }

  get password() {
    return this.loginForm.get('password');
  }

  goHome() {
    this.router.navigate(['/']);
  }

  login() {
    
    this.submitted = true;
    this.loginError = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.auth.login(this.loginForm.value).subscribe({
      next: (res: any) => {
        this.auth.saveTokens(res);

        const role = (res.role || this.auth.getRole() || '').trim();
        const isFirstLogin = res.isFirstLogin === true;

        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');

        if (role === 'Patient') {
          if (returnUrl && returnUrl.startsWith('/patient')) {
            this.router.navigateByUrl(returnUrl);
          } else {
            this.router.navigate(['/patient']);
          }
        }

        else if (role === 'Doctor') {
          if (isFirstLogin) {
            this.router.navigate(['/change-password']);
          } else if (returnUrl && returnUrl.startsWith('/doctor')) {
            this.router.navigateByUrl(returnUrl);
          } else {
            this.router.navigate(['/doctor']);
          }
        }

        else if (role === 'Admin') {
          window.location.replace(this.auth.getAdminPortalBridgeUrl());
        }

        else {
          this.loginError = 'Invalid user role. Please contact administrator.';
          this.auth.logout();
        }
      },

      error: (err) => {
        console.error('Login error:', err);

        if (typeof err.error === 'string') {
          this.loginError = err.error;
        } else if (err.error?.message) {
          this.loginError = err.error.message;
        } else if (err.error?.errors) {
          this.loginError = JSON.stringify(err.error.errors);
        } else {
          this.loginError = 'Invalid email or password';
        }

        alert(this.loginError);
      }
    });
  }
}
