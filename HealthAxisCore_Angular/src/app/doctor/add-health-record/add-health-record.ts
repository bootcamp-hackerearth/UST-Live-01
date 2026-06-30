import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-add-health-record',
  imports: [
    RouterLink,
    ReactiveFormsModule
  ],
  templateUrl: './add-health-record.html',
  styleUrl: './add-health-record.css'
})
export class AddHealthRecord {
  appointmentId = 0;

  recordForm: FormGroup;

  submitted = false;

  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder
  ) {
    this.appointmentId = Number(this.route.snapshot.paramMap.get('appointmentId')) || 1;

    this.recordForm = this.formBuilder.group({
      diagnosis: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],
      prescription: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],
      notes: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ]
    });
  }

  get diagnosis() {
    return this.recordForm.get('diagnosis');
  }

  get prescription() {
    return this.recordForm.get('prescription');
  }

  get notes() {
    return this.recordForm.get('notes');
  }

  submitRecord(): void {
    this.submitted = true;
    this.successMessage = '';

    if (this.recordForm.invalid) {
      this.recordForm.markAllAsTouched();
      return;
    }

    console.log('Health record template submitted:', {
      appointmentId: this.appointmentId,
      ...this.recordForm.value
    });

    this.successMessage = 'Health record saved successfully. API connection will be added later.';
  }
}
