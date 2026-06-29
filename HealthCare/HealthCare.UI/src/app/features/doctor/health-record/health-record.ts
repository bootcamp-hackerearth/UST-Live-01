import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  ActivatedRoute,
  Router
} from '@angular/router';

import { DoctorService }
from '../../../core/services/doctor.service';

@Component({
  selector: 'app-health-record',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './health-record.html',
  styleUrl: './health-record.css'
})
export class HealthRecordComponent
implements OnInit {

  form!: FormGroup;

  appointmentId = 0;

  patientId = 0;

  loading = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.appointmentId =
      Number(
        this.route.snapshot.queryParamMap
          .get('appointmentId')
      );

    this.patientId =
      Number(
        this.route.snapshot.queryParamMap
          .get('patientId')
      );

    this.form = this.fb.group({

      diagnosis: [
        '',
        Validators.required
      ],

      prescription: [
        '',
        Validators.required
      ],

      notes: ['']

    });

  }

  saveRecord() {

    if (this.form.invalid)
      return;

    const payload = {

      appointmentId: this.appointmentId,

      patientId: this.patientId,

      visitDate: new Date(),

      ...this.form.value

    };

    this.loading = true;

    this.doctorService
      .createHealthRecord(payload)
      .subscribe({

        next: () => {

          this.loading = false;

          alert(
            'Health Record Added Successfully'
          );

          this.router.navigate([
            '/doctor/schedule'
          ]);

        },

        error: err => {

          console.log(err);

          this.loading = false;

          alert(
            'Unable to save record'
          );

        }

      });

  }

}