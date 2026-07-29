import {ChangeDetectorRef,Component,OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import {FormBuilder,FormGroup,ReactiveFormsModule,Validators} from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  loginForm!: FormGroup;

  errors: { top?: string } = {};

  constructor(
    private readonly auth: AuthService,
    private readonly router: Router,
    private readonly fb: FormBuilder,
    private readonly cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],
      password: [
        '',
        Validators.required
      ]
    });
  }

  login(): void {
    this.errors.top = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.auth.login(this.loginForm.getRawValue()).subscribe({
      next: (res: any) => {
        if (!res?.accessToken || !res?.role) {
          this.errors.top = 'Invalid email or password';
          return;
        }

        localStorage.setItem('token', res.accessToken);
        localStorage.setItem('role', res.role);

        if (res.role === 'Admin') {
          globalThis.location.href =
            `/blazor/dashboard?token=${encodeURIComponent(res.accessToken)}`;

          return;
        }

        if (res.role === 'Patient') {
          this.router.navigate(['/patient-dashboard']);
          return;
        }

        if (res.role === 'Doctor') {
          this.router.navigate(['/doctor-dashboard']);
          return;
        }

        this.errors.top = 'Invalid email or password';
      },

      error: (err: any) => {
        console.error('Login error:', err);

        if (err.status === 400 || err.status === 401) {
          this.errors.top = 'Invalid email or password';
        } else if (err.status === 0) {
          this.errors.top =
            'Unable to connect to the server. Please try again.';
        } else {
          this.errors.top =
            'Something went wrong. Try again.';
        }

        this.cdr.detectChanges();
      }
    });
  }

  goPatientRegister(): void {
    this.router.navigate(['/register-patient']);
  }
}
