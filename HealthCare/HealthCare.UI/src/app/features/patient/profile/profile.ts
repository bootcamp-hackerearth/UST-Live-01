import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PatientService } from '../../../core/services/patient.service';

export interface PatientProfile {
  patientId: string;
  fullName: string;
  phoneNumber: string;
  email: string;
  gender: string;
  hasInsurance: boolean;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent implements OnInit {

  profileForm!: FormGroup;

  patient = signal<PatientProfile | null>(null);

  isEditMode = signal(false);
  loading = signal(true);
  saving = signal(false);

  constructor(
    private fb: FormBuilder,
    private readonly patientService: PatientService,
    private readonly toastr: ToastrService
  ) {}

  ngOnInit(): void {

    this.profileForm = this.fb.group({

      patientId: [{ value: '', disabled: true }],

      fullName: ['', Validators.required],

      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[6-9]\d{9}$/)
        ]
      ],

      email: [{ value: '', disabled: true }],

      gender: ['', Validators.required],

      hasInsurance: [false]

    });

    this.loadProfile();
  }

  loadProfile(): void {

    this.loading.set(true);

    this.patientService.getProfile().subscribe({

  next: (res: any) => {

    this.patient.set(res);

    this.profileForm.patchValue({
      patientId: res.patientId,
      fullName: res.fullName,
      phoneNumber: res.phoneNumber,
      email: res.email,
      gender: res.gender,
      hasInsurance: res.hasInsurance
    });

    this.loading.set(false);
  },

  error: () => {
    this.loading.set(false);
  }
});
        
      
  }

  updateProfile(): void {

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.saving.set(true);

    const payload = this.profileForm.getRawValue();

    this.patientService.updateProfile(payload).subscribe({

      next: () => {

        this.patient.set({
          ...this.patient()!,
          ...payload
        });

        this.saving.set(false);
        this.isEditMode.set(false);

        this.toastr.success(
          'Profile updated successfully',
          'Success'
        );
      },

      error: () => {

        this.saving.set(false);

        this.toastr.error(
          'Failed to update profile',
          'Error'
        );
      }
    });
  }

  toggleEdit(): void {

    if (!this.isEditMode() && this.patient()) {

      this.profileForm.patchValue(this.patient()!);
    }

    this.isEditMode.update(v => !v);
  }

  getInitials(): string {

    const name = this.patient()?.fullName ?? '';

    return name
      .split(' ')
      .filter(Boolean)
      .map(x => x[0])
      .join('')
      .substring(0, 2)
      .toUpperCase();
  }
}