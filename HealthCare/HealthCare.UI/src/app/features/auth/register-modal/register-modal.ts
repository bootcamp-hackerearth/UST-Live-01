import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {ReactiveFormsModule, FormBuilder, FormGroup, Validators} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './register-modal.html',
  styleUrl: './register-modal.css'
})
export class RegisterModal {

  @Output() close = new EventEmitter<void>();

  registerForm: FormGroup;

  serverError = '';
  successMessage = '';
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastr: ToastrService
  ) {

    this.registerForm = this.fb.group({

      fullName: ['', Validators.required],

      gender: ['', Validators.required],

      dateOfBirth: ['', Validators.required],

      email: ['', [
        Validators.required,
        Validators.email
      ]],

      phoneNumber: ['', [
        Validators.required,
        Validators.pattern(/^[6-9]\d{9}$/)
      ]],

      insuranceId: [''],

      password: ['', [
        Validators.required,
        Validators.minLength(6)
      ]],

      confirmPassword: ['', Validators.required]

    });

    // Clear emailTaken error when user types again
    this.registerForm.get('email')?.valueChanges.subscribe(() => {

      const control = this.registerForm.get('email');

      if (control?.hasError('emailTaken')) {
        control.setErrors(null);
      }
    });
  }

  onSubmit() {

  this.serverError = '';
  this.successMessage = '';

  if (this.registerForm.invalid) {
    this.registerForm.markAllAsTouched();
    return;
  }

  if (
    this.registerForm.value.password !==
    this.registerForm.value.confirmPassword
  ) {
    this.toastr.error('Passwords do not match', 'Error ❌');
    return;
  }

  const dto = this.registerForm.value;

  //  START LOADING
  this.isLoading = true;

  this.authService.register(dto)
    .subscribe({

      next: () => {

        this.isLoading = false; //  stop loading
        this.toastr.success('Patient registered successfully','Success ');
        this.close.emit();

      
      },

      error: (err) => {

        this.isLoading = false; //  stop loading

        console.log(err);

        if (err.status === 0) {
          this.toastr.error('Unable to connect to server.', 'Error ❌');
          return;
        }

        if (err.error?.message) {

          const message = err.error.message;

          if (message === 'Email already exists') {

            const emailControl = this.registerForm.get('email');

            emailControl?.setErrors({ emailTaken: true });
            emailControl?.markAsTouched();

            return;
          }

          this.toastr.error(message, 'Error ❌')
          return;
        }

        this.toastr.error('Registration failed. Please try again.', 'Error ❌');
      }
    });
}

  closeModal() {
    this.close.emit();
  }

  get f() {
    return this.registerForm.controls;
  }
}
