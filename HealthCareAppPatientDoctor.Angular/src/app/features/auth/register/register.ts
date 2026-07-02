import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { Router,RouterLink } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { AuthService } from '../../../core/services/auth-service';
import { PatientRegister } from '../../../core/models/patient-register';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  isLoading = false;

  registerForm = this.fb.group({

    fullName: ['', [
      Validators.required,
      Validators.maxLength(100)
    ]],

    dateOfBirth: ['', Validators.required],

    gender: ['', Validators.required],

    phoneNumber: ['', [
      Validators.required,
      Validators.pattern('^[0-9]{10}$')
    ]],

    email: ['', [
      Validators.required,
      Validators.email
    ]],

    insuranceId: ['', Validators.required],

    password: ['', [
      Validators.required,
      Validators.minLength(8)
    ]],

    confirmPassword: ['', Validators.required]

  },
  {
    validators: this.passwordMatchValidator
  });

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {

    const password = control.get('password')?.value;

    const confirmPassword = control.get('confirmPassword')?.value;

    if (password === confirmPassword) {

      return null;

    }

    return {
      passwordMismatch: true
    };

  }

  register() {

    if (this.registerForm.invalid) {

      this.registerForm.markAllAsTouched();

      return;

    }

    this.isLoading = true;

   const formValue = this.registerForm.getRawValue();

const patient: PatientRegister = {

  fullName: formValue.fullName ?? '',

  dateOfBirth: formValue.dateOfBirth ?? '',

  gender: Number(formValue.gender),

  phoneNumber: formValue.phoneNumber ?? '',

  email: formValue.email ?? '',

  insuranceId: formValue.insuranceId ?? '',

  password: formValue.password ?? '',

  confirmPassword: formValue.confirmPassword ?? ''

};

    this.authService.register(patient).subscribe({

      next: () => {

        this.toastr.success(
          'Registration Successful'
        );

        setTimeout(() => {

          this.router.navigate(['/login']);

        },2000);

      },

      error:(err)=>{

        this.toastr.error(
          err.error.message
        );

        this.isLoading=false;

      }

    });

  }

}