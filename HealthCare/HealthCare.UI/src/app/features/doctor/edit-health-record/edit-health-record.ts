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
  selector: 'app-edit-health-record',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './edit-health-record.html',
  styleUrl: './edit-health-record.css'
})
export class EditHealthRecordComponent
implements OnInit {

  recordId = 0;

  loading = false;

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private doctorService: DoctorService
  ) { }

  ngOnInit(): void {

    this.recordId =
      Number(
        this.route.snapshot.paramMap.get('id')
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

    this.loadRecord();

  }

  loadRecord() {

    this.loading = true;

    this.doctorService
      .getHealthRecordById(this.recordId)
      .subscribe({

        next: (res: any) => {

          this.form.patchValue({

            diagnosis: res.diagnosis,

            prescription: res.prescription,

            notes: res.notes

          });

          this.loading = false;
        },

        error: () => {

          this.loading = false;

          alert('Unable to load record');
        }

      });

  }

  updateRecord() {

    if (this.form.invalid)
      return;

    this.doctorService
      .updateHealthRecord(
        this.recordId,
        this.form.value
      )
      .subscribe({

        next: () => {

          alert(
            'Health Record Updated Successfully'
          );

          this.router.navigate([
            '/doctor/dashboard'
          ]);

        },

        error: () => {

          alert(
            'Unable to update record'
          );

        }

      });

  }

}