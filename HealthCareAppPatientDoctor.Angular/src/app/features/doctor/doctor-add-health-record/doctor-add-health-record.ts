import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { HealthRecordService } from '../../../core/services/health-record-service';
import { Appointment } from '../../../core/models/appointment';
import { AddHealthRecord } from '../../../core/models/add-health-record';
import { AppointmentService } from '../../../core/services/appointment-service';

@Component({
  selector: 'app-doctor-add-health-record',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './doctor-add-health-record.html',
  styleUrl: './doctor-add-health-record.css'
})
export class DoctorAddHealthRecord implements OnInit {

  private fb = inject(FormBuilder);

  private router = inject(Router);

  private route = inject(ActivatedRoute);

  private toastr = inject(ToastrService);

  private healthRecordService = inject(HealthRecordService);

  private appointmentService = inject(AppointmentService);
  private cdr = inject(ChangeDetectorRef);

  appointment?: Appointment;

  isLoading = false;

  healthRecordForm = this.fb.group({

    diagnosis: [
      '',
      [
        Validators.required,
        Validators.maxLength(500)
      ]
    ],

    prescription: [
      '',
      [
        Validators.required,
        Validators.maxLength(500)
      ]
    ],

    notes: [
      '',
      Validators.maxLength(1000)
    ]

  });

  ngOnInit(): void {

    const appointmentId = Number(
      this.route.snapshot.paramMap.get('appointmentId')
    );

    if (!appointmentId) {

      this.toastr.error('Invalid appointment.');

      this.router.navigate(['/doctor/appointments']);

      return;

    }

    this.loadAppointment(appointmentId);

  }

  loadAppointment(appointmentId: number): void {

    this.isLoading = true;

    this.appointmentService
      .getAppointmentById(appointmentId)
      .subscribe({

        next: (response) => {

          console.log('Appointment Loaded:', response);

          this.appointment = response;

          this.isLoading = false;
           this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;

          this.toastr.error(
            'Unable to load appointment.'
          );

          this.router.navigate([
            '/doctor/appointments'
          ]);

        }

      });

  }

  save(): void {

    if (!this.appointment) {

      this.toastr.error('Appointment not loaded.');

      return;

    }

    if (this.healthRecordForm.invalid) {

      this.healthRecordForm.markAllAsTouched();

      return;

    }

    this.isLoading = true;

    const request: AddHealthRecord = {

      patientId: this.appointment.patientId,

      appointmentId: this.appointment.appointmentId,

      diagnosis:
        this.healthRecordForm.value.diagnosis!,

      prescription:
        this.healthRecordForm.value.prescription!,

      notes:
        this.healthRecordForm.value.notes ?? '',

      visitDate:
        new Date().toISOString()

    };

    console.log('Health Record Request:', request);

    this.healthRecordService
      .addHealthRecord(request)
      .subscribe({

        next: () => {

          this.isLoading = false;

              this.cdr.detectChanges();

          this.toastr.success(
            'Health record added successfully.'
          );

          this.router.navigate([
            '/doctor/appointments'
          ]);

        },

        error: (err) => {

          this.isLoading = false;

              this.cdr.detectChanges();

          console.log(err);

          this.toastr.error(
            err?.error?.message ??
            'Unable to add health record.'
          );

        }

      });

  }

  cancel(): void {

    this.router.navigate([
      '/doctor/appointments'
    ]);

  }

}