import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder,FormGroup,ReactiveFormsModule,Validators} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
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
  isEditMode = signal(false);
  loading = signal(false);
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
    [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]
  ],

  email: [{ value: '', disabled: true }],

  gender: ['', Validators.required],

  hasInsurance: [false]
});

    this.loadProfile();
  }

  loadProfile() {

  this.loading.set(true);

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

        this.loading.set(false); 
      }
    });
}

 updateProfile() {

  if (this.profileForm.invalid) return;

  this.saving.set(true);

  this.patientService.updateProfile(
    this.profileForm.getRawValue()
  )
  .subscribe({

    next: () => {
      this.saving.set(false);
      this.isEditMode.set(false);
      this.toastr.success('Profile updated successfully ', 'Success');
    },

    error: () => {
      this.saving.set(false);
      this.toastr.error('Failed to update profile','Error ');

    }
  });
}

toggleEdit() {
  this.isEditMode.set(!this.isEditMode());
}


}