import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';

import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { AuthService } from '../../core/services/auth.service';
import { ChangePasswordRequest } from '../../core/models/auth.model';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const PASSWORD_PATTERN =
  /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$/;

const DATE_ONLY_PATTERN =
  /^(\d{4})-(\d{2})-(\d{2})$/;

type PasswordField =
  | 'current'
  | 'new'
  | 'confirm';

interface PasswordVisibilityState {
  readonly current: boolean;
  readonly new: boolean;
  readonly confirm: boolean;
}

const HIDDEN_PASSWORDS: PasswordVisibilityState = {
  current: false,
  new: false,
  confirm: false
};

@Component({
  selector: 'app-patient-profile',
  imports: [
    ReactiveFormsModule,
    DatePipe
  ],
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PatientProfile {
  private readonly formBuilder = inject(FormBuilder);
  private readonly patientService = inject(PatientService);
  private readonly authService = inject(AuthService);

  readonly patient = signal<Patient | null>(null);
  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly passwordSaving = signal(false);
  readonly errorMessage = signal('');
  readonly successDialogMessage = signal('');
  readonly passwordSuccessMessage = signal('');
  readonly passwordErrorMessage = signal('');
  readonly editErrorMessage = signal('');
  readonly isEditDialogOpen = signal(false);

  readonly passwordVisibility =
    signal<PasswordVisibilityState>({
      ...HIDDEN_PASSWORDS
    });

  readonly maxDate = this.formatDateForInput(new Date());

  readonly patientInitial = computed(() => {
    const name = this.patient()?.fullName?.trim();

    return name
      ? name.charAt(0).toUpperCase()
      : 'P';
  });

  readonly editForm = this.formBuilder.nonNullable.group({
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
        PatientProfile.noFutureDateValidator
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
        Validators.pattern(/^[6-9]\d{9}$/)
      ]
    ],
    email: this.formBuilder.nonNullable.control(
      {
        value: '',
        disabled: true
      },
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(100)
      ]
    )
  });

  readonly passwordForm = this.formBuilder.nonNullable.group(
    {
      currentPassword: [
        '',
        [
          Validators.required
        ]
      ],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(50),
          Validators.pattern(PASSWORD_PATTERN)
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
      validators: PatientProfile.passwordMatchValidator
    }
  );

  constructor() {
    this.loadProfile();
  }

  get fullName() {
    return this.editForm.controls.fullName;
  }

  get dateOfBirth() {
    return this.editForm.controls.dateOfBirth;
  }

  get gender() {
    return this.editForm.controls.gender;
  }

  get phoneNumber() {
    return this.editForm.controls.phoneNumber;
  }

  get currentPassword() {
    return this.passwordForm.controls.currentPassword;
  }

  get newPassword() {
    return this.passwordForm.controls.newPassword;
  }

  get confirmPassword() {
    return this.passwordForm.controls.confirmPassword;
  }

  isPasswordVisible(
    field: PasswordField
  ): boolean {
    return this.passwordVisibility()[field];
  }

  togglePasswordVisibility(
    field: PasswordField
  ): void {
    this.passwordVisibility.update(
      (currentState) => ({
        ...currentState,
        [field]: !currentState[field]
      })
    );
  }

  loadProfile(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.patientService.getMyProfile().subscribe({
      next: (patient: Patient) => {
        this.patient.set(patient);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);

        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not load patient profile.'
          )
        );
      }
    });
  }

  openEditDialog(): void {
    const currentPatient = this.patient();

    if (!currentPatient) {
      this.errorMessage.set('Patient details not found.');
      return;
    }

    this.errorMessage.set('');
    this.successDialogMessage.set('');
    this.editErrorMessage.set('');

    this.editForm.reset({
      fullName: currentPatient.fullName,
      dateOfBirth: this.getDateInputValue(
        currentPatient.dateOfBirth
      ),
      gender: currentPatient.gender,
      phoneNumber: currentPatient.phoneNumber,
      email: currentPatient.email
    });

    this.editForm.controls.email.disable({
      emitEvent: false
    });

    this.isEditDialogOpen.set(true);
  }

  closeEditDialog(): void {
    if (this.saving()) {
      return;
    }

    this.isEditDialogOpen.set(false);
    this.editErrorMessage.set('');
  }

  updatePatientDetails(): void {
    this.errorMessage.set('');
    this.successDialogMessage.set('');
    this.editErrorMessage.set('');

    if (this.editForm.invalid) {
      this.editForm.markAllAsTouched();
      this.editErrorMessage.set(
        'Please correct the patient details.'
      );
      return;
    }

    const currentPatient = this.patient();

    if (!currentPatient) {
      this.errorMessage.set('Patient details not found.');
      return;
    }

    const formValue = this.editForm.getRawValue();

    const request: Patient = {
      ...currentPatient,
      fullName: formValue.fullName.trim(),
      dateOfBirth: formValue.dateOfBirth,
      gender: formValue.gender,
      phoneNumber: formValue.phoneNumber.trim(),
      email: currentPatient.email
    };

    this.saving.set(true);

    this.patientService
      .updateMyProfile(request)
      .subscribe({
        next: (updatedPatient: Patient) => {
          this.patient.set(updatedPatient);
          this.saving.set(false);
          this.isEditDialogOpen.set(false);
          this.editErrorMessage.set('');

          this.successDialogMessage.set(
            'Patient details updated successfully.'
          );
        },
        error: (error: unknown) => {
          this.saving.set(false);

          this.editErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Could not update patient details.'
            )
          );
        }
      });
  }

  changePassword(): void {
    this.passwordSuccessMessage.set('');
    this.passwordErrorMessage.set('');

    if (this.passwordForm.invalid) {
      this.passwordForm.markAllAsTouched();

      this.passwordErrorMessage.set(
        'Please correct the password details.'
      );

      return;
    }

    const formValue = this.passwordForm.getRawValue();

    const request: ChangePasswordRequest = {
      currentPassword: formValue.currentPassword,
      newPassword: formValue.newPassword,
      confirmNewPassword: formValue.confirmPassword
    };

    this.passwordSaving.set(true);

    this.authService.changePassword(request).subscribe({
      next: () => {
        this.passwordSaving.set(false);

        this.passwordForm.reset({
          currentPassword: '',
          newPassword: '',
          confirmPassword: ''
        });

        this.passwordVisibility.set({
          ...HIDDEN_PASSWORDS
        });

        this.passwordSuccessMessage.set(
          'Password changed successfully.'
        );
      },
      error: (error: unknown) => {
        this.passwordSaving.set(false);

        this.passwordErrorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Could not change password.'
          )
        );
      }
    });
  }

  closeSuccessDialog(): void {
    this.successDialogMessage.set('');
  }

  getFullNameErrorMessage(): string {
    if (this.fullName.hasError('required')) {
      return 'Full name is required.';
    }

    if (this.fullName.hasError('minlength')) {
      return 'Full name must contain at least 3 characters.';
    }

    if (this.fullName.hasError('maxlength')) {
      return 'Full name cannot exceed 80 characters.';
    }

    if (this.fullName.hasError('pattern')) {
      return 'Use only letters and spaces.';
    }

    return '';
  }

  getPasswordErrorMessage(): string {
    if (this.newPassword.hasError('required')) {
      return 'New password is required.';
    }

    if (this.newPassword.hasError('minlength')) {
      return 'Password must contain at least 8 characters.';
    }

    if (this.newPassword.hasError('maxlength')) {
      return 'Password cannot exceed 50 characters.';
    }

    if (this.newPassword.hasError('pattern')) {
      return 'Use uppercase, lowercase, number, and special character.';
    }

    return '';
  }

  private getDateInputValue(
    dateValue: string
  ): string {
    const localDate =
      PatientProfile.parseDateOnly(dateValue);

    if (localDate) {
      return this.formatDateForInput(localDate);
    }

    const parsedDate = new Date(dateValue);

    return Number.isNaN(parsedDate.getTime())
      ? ''
      : this.formatDateForInput(parsedDate);
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private static noFutureDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate =
      PatientProfile.parseDateOnly(
        String(control.value)
      );

    if (!selectedDate) {
      return {
        invalidDate: true
      };
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate > today
      ? { futureDate: true }
      : null;
  }

  private static parseDateOnly(
    dateValue: string
  ): Date | null {
    const match =
      DATE_ONLY_PATTERN.exec(dateValue.trim());

    if (!match) {
      return null;
    }

    const [, yearText, monthText, dayText] =
      match;

    const year = Number.parseInt(yearText, 10);
    const month = Number.parseInt(monthText, 10);
    const day = Number.parseInt(dayText, 10);

    const parsedDate = new Date(
      year,
      month - 1,
      day
    );

    const isValidDate =
      parsedDate.getFullYear() === year &&
      parsedDate.getMonth() === month - 1 &&
      parsedDate.getDate() === day;

    return isValidDate
      ? parsedDate
      : null;
  }

  private static passwordMatchValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    const newPassword =
      control.get('newPassword')?.value;

    const confirmPassword =
      control.get('confirmPassword')?.value;

    if (!newPassword || !confirmPassword) {
      return null;
    }

    return newPassword === confirmPassword
      ? null
      : { passwordMismatch: true };
  }
}