import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {

  registerForm: any;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.registerForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
        patientName: ['', [Validators.required, Validators.minLength(2)]],
        dateOfBirth: ['', Validators.required],
        gender: [0, Validators.required],
        phoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern(/^[0-9]{10}$/)
          ]
        ]
      },
      {
        validators: this.passwordMatchValidator
      }
    );
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (!password || !confirmPassword) {
      return null;
    }

    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  get email() {
    return this.registerForm.get('email');
  }

  get password() {
    return this.registerForm.get('password');
  }

  get confirmPassword() {
    return this.registerForm.get('confirmPassword');
  }

  get patientName() {
    return this.registerForm.get('patientName');
  }

  get dateOfBirth() {
    return this.registerForm.get('dateOfBirth');
  }

  get gender() {
    return this.registerForm.get('gender');
  }

  get phoneNumber() {
    return this.registerForm.get('phoneNumber');
  }

  goHome() {
    this.router.navigate(['/']);
  }

  register() {
    this.submitted = true;

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const form = this.registerForm.value;

    const payload = {
      email: form.email,
      password: form.password,
      confirmPassword: form.confirmPassword,
      role: 'Patient',
      patientName: form.patientName,
      dateOfBirth: form.dateOfBirth,
      gender: Number(form.gender),
      phoneNumber: form.phoneNumber
    };

    console.log('Sending payload:', payload);


  

    this.auth.register(payload).subscribe({
      next: (res: any) => {
        alert('✅ Registered successfully');

        if (res.token) {
          this.auth.saveTokens(res);
        }

        this.router.navigate(['/patient']);
      },
      error: (err) => {
        console.error('Register error:', err);
        console.log('Backend message:', err.error);
        alert(err.error?.message || 'Registration failed');
      }
    });
  }
}