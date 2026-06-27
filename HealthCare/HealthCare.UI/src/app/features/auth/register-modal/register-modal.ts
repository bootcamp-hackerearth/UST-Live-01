import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {

    this.registerForm = this.fb.group({

      fullName: ['', Validators.required],

      gender: ['', Validators.required],

      dateOfBirth: ['', Validators.required],

      email: ['', [Validators.required, Validators.email]],

      phoneNumber: ['', Validators.required],

      password: ['', Validators.required],

      confirmPassword: ['', Validators.required]

    });

  }

  onSubmit() {

    if (this.registerForm.invalid)
      return;

    if (this.registerForm.value.password !==
        this.registerForm.value.confirmPassword) {

      alert('Passwords do not match');
      return;
    }

    const dto = {
      fullName: this.registerForm.value.fullName,
      email: this.registerForm.value.email,
      phoneNumber: this.registerForm.value.phoneNumber,
      password: this.registerForm.value.password
    };

    this.authService.register(dto)
      .subscribe({

        next: () => {

          alert('Patient Registered Successfully');

          this.close.emit();
        },

        error: () => {
          alert('Registration Failed');
        }

      });

  }

  closeModal() {
    this.close.emit();
  }
}