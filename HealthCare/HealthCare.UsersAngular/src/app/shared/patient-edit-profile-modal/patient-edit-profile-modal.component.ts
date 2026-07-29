import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnChanges,
  SimpleChanges
} from '@angular/core';

import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule
} from '@angular/forms';

@Component({
  selector: 'app-patient-edit-profile-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './patient-edit-profile-modal.component.html',
  styleUrls: ['./patient-edit-profile-modal.component.css']
})
export class PatientEditProfileModalComponent implements OnChanges {

  @Input() visible = false;
  @Input() model: any = {};

  @Output() onClose = new EventEmitter<void>();
  @Output() onSave = new EventEmitter<any>();

  editForm: FormGroup;

  constructor(private readonly fb: FormBuilder) {

    this.editForm = this.fb.group({

      fullName: [
        '',
        [
          Validators.required,
          Validators.pattern('^[A-Za-z ]+$')
        ]
      ],

      gender: [
        '',
        Validators.required
      ],

      phoneNumber: [
        '',
        [
          Validators.required,
          Validators.pattern('^[6-9][0-9]{9}$')
        ]
      ],

      insuranceId: [
        '',
        [
          Validators.pattern('^[A-Za-z0-9]*$')
        ]
      ]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {

    if (changes['model'] && this.model) {

      this.editForm.patchValue({
        fullName: this.model.fullName || '',
        gender: this.model.gender || '',
        phoneNumber: this.model.phoneNumber || '',
        insuranceId: this.model.insuranceId || ''
      });
    }
  }

  saveChanges(): void {

    if (this.editForm.invalid) {

      this.editForm.markAllAsTouched();
      return;
    }

    this.onSave.emit(this.editForm.value);
  }

  close(): void {
    this.onClose.emit();
  }

  closeOnBackdrop(event: MouseEvent): void {
    if (event.target === event.currentTarget) {
      this.close();
    } 
  }

  get f() {
    return this.editForm.controls;
  }
}
