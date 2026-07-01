import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { HealthRecordService } from '../../../core/services/health-record-service';
import { HealthRecord } from '../../../core/models/health-record';

@Component({
  selector: 'app-doctor-patient-history',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './doctor-patient-history.html',
  styleUrl: './doctor-patient-history.css'
})
export class DoctorPatientHistory implements OnInit {

  private route = inject(ActivatedRoute);

  private router = inject(Router);

  private toastr = inject(ToastrService);

  private healthRecordService = inject(HealthRecordService);

  private cdr = inject(ChangeDetectorRef);

  records: HealthRecord[] = [];

  isLoading = false;

  ngOnInit(): void {

    const appointmentId = Number(
      this.route.snapshot.paramMap.get('appointmentId')
    );

    if (!appointmentId) {

      this.toastr.error('Invalid appointment.');

      this.router.navigate(['/doctor/appointments']);

      return;

    }

    this.loadHistory(appointmentId);

  }

  loadHistory(appointmentId: number): void {

    this.isLoading = true;

    this.healthRecordService
      .getPatientHistory(appointmentId)
      .subscribe({

        next: (response) => {

          this.records = response;

          this.isLoading = false;

          this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;

          this.toastr.error(
            err?.error?.message ??
            'Unable to load patient history.'
          );

          this.router.navigate([
            '/doctor/appointments'
          ]);

        }

      });

  }

  back(): void {

    this.router.navigate([
      '/doctor/appointments'
    ]);

  }

}