import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {
  Router,
  RouterLink
} from '@angular/router';

import {
  RegisterPatientRequest
} from '../../core/models/auth.model';
import {
  Gender
} from '../../core/models/gender.enum';
import {
  AuthService
} from '../../core/services/auth.service';

const REDIRECT_DELAY_IN_MS = 3000;

@Component({
  selector: 'app-patient-register',
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './patient-register.html',
  styleUrl: './patient-register.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientRegister {
  private readonly formBuilder =
    inject(FormBuilder);

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  readonly loading = signal(false);
  readonly registrationCompleted = signal(false);

  readonly errorMessage = signal('');
  readonly successMessage = signal('');

  readonly showPassword = signal(false);
  readonly showConfirmPassword = signal(false);

  readonly genderOptions = [
    {
      label: 'Male',
      value: Gender.Male
    },
    {
      label: 'Female',
      value: Gender.Female
    },
    {
      label: 'Other',
      value: Gender.Other
    }
  ] as const;

  readonly registerForm =
    this.formBuilder.nonNullable.group(
      {
        fullName: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(80),
            Validators.pattern(/^[A-Za-z ]+$/)
          ]
        ],
        dateOfBirth: [
          '',
          [
            Validators.required,
            PatientRegister.noFutureDateValidator
          ]
        ],
        gender: [
          '',
          [
            Validators.required
          ]
        ],
        phoneNumber: [
          '',
          [
            Validators.required,
            Validators.pattern(/^[1-9]\d{9}$/)
          ]
        ],
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
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(
              /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/
            )
          ]
        ],
        confirmPassword: [
          '',
          [
            Validators.required
          ]
        ]
      },
      {
        validators:
          PatientRegister.passwordsMatchValidator
      }
    );

  get fullName() {
    return this.registerForm.controls.fullName;
  }

  get dateOfBirth() {
    return this.registerForm.controls.dateOfBirth;
  }

  get gender() {
    return this.registerForm.controls.gender;
  }

  get phoneNumber() {
    return this.registerForm.controls.phoneNumber;
  }

  get email() {
    return this.registerForm.controls.email;
  }

  get password() {
    return this.registerForm.controls.password;
  }

  get confirmPassword() {
    return this.registerForm.controls.confirmPassword;
  }

  registerPatient(): void {
    if (
      this.loading() ||
      this.registrationCompleted()
    ) {
      return;
    }

    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    const formValue =
      this.registerForm.getRawValue();

    const request: RegisterPatientRequest = {
      fullName: formValue.fullName.trim(),
      dateOfBirth: formValue.dateOfBirth,
      gender: formValue.gender as Gender,
      phoneNumber: formValue.phoneNumber.trim(),
      email: formValue.email.trim(),
      password: formValue.password,
      confirmPassword: formValue.confirmPassword
    };

    this.loading.set(true);

    this.authService
      .registerPatient(request)
      .subscribe({
        next: () => {
          this.loading.set(false);
          this.registrationCompleted.set(true);

          this.successMessage.set(
            'Patient account created successfully. Redirecting you to login...'
          );

          this.registerForm.disable({
            emitEvent: false
          });

          globalThis.setTimeout(() => {
            void this.router.navigate(['/login']);
          }, REDIRECT_DELAY_IN_MS);
        },
        error: (error: unknown) => {
          this.loading.set(false);

          this.errorMessage.set(
            this.getFriendlyMessage(
              error,
              'Registration failed. Please check your details and try again.'
            )
          );
        }
      });
  }

  togglePasswordVisibility(): void {
    this.showPassword.update(
      (isVisible) => !isVisible
    );
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword.update(
      (isVisible) => !isVisible
    );
  }

  private getFriendlyMessage(
    error: unknown,
    fallback: string
  ): string {
    const possibleError = error as {
      friendlyMessage?: unknown;
    };

    if (
      typeof possibleError.friendlyMessage ===
      'string'
    ) {
      return possibleError.friendlyMessage;
    }

    return fallback;
  }

  private static passwordsMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const password =
      control.get('password')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (
      password &&
      confirmPassword &&
      password !== confirmPassword
    ) {
      return {
        passwordMismatch: true
      };
    }

    return null;
  }

  private static noFutureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate =
      new Date(control.value);

    const today = new Date();

    selectedDate.setHours(
      0,
      0,
      0,
      0
    );

    today.setHours(
      0,
      0,
      0,
      0
    );

    if (selectedDate > today) {
      return {
        futureDate: true
      };
    }

    return null;
  }
}