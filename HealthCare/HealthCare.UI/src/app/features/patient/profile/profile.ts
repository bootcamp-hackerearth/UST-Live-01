import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { PatientService } from '../../../core/services/patient.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class ProfileComponent implements OnInit {

  profileForm!: FormGroup;
  isEditMode = false;
  showSuccess = false;
  loading = false;
  saving = false;

  constructor(
    private fb: FormBuilder,
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

   this.profileForm = this.fb.group({

  patientId: [{ value: '', disabled: true }], 

  fullName: ['', Validators.required],

  phoneNumber: [
    '',
    [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]
  ],

  email: [{ value: '', disabled: true }],

  gender: ['', Validators.required],

  hasInsurance: [false]
});

    this.loadProfile();
  }

  loadProfile() {

    this.loading = true;

    this.patientService.getProfile()
      .subscribe({

     
next: (res: any) => {

  this.profileForm.patchValue({

    patientId: res.patientId,
    fullName: res.fullName,
    phoneNumber: res.phoneNumber,
     email: res.email,
    gender: res.gender,
    hasInsurance: res.hasInsurance
  });

  this.loading = false;
  }

      });
  }

 updateProfile() {

  if (this.profileForm.invalid) return;

  this.saving = true;

  this.patientService.updateProfile(
    this.profileForm.getRawValue()
  )
  .subscribe({

    next: () => {
      this.saving = false;

      this.isEditMode = false; 
      this.showSuccess = true;

      setTimeout(() => {
        this.showSuccess = false;
      }, 2500);
    },

    error: () => {
      this.saving = false;
      alert('Update failed');
    }
  });
}

toggleEdit() {
  this.isEditMode = !this.isEditMode;
}


}