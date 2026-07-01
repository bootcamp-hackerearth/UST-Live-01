import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { Router, RouterModule } from '@angular/router';

import { ToastrService } from 'ngx-toastr';

import { PatientService } from '../../../core/services/patient-service';

import { HealthRecord } from '../../../core/models/health-record';
import { HealthRecordService } from '../../../core/services/health-record-service';

@Component({
  selector: 'app-patient-health-records',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './patient-health-records.html',
  styleUrl: './patient-health-records.css'
})
export class PatientHealthRecords implements OnInit {

  private patientService = inject(PatientService);

  private healthRecordService = inject(HealthRecordService);

  private toastr = inject(ToastrService);

  private router = inject(Router);

  private cdr = inject(ChangeDetectorRef);

  healthRecords: HealthRecord[] = [];

  isLoading = false;

  ngOnInit(): void {

    this.loadHealthRecords();

  }

  loadHealthRecords(): void {

    this.isLoading = true;

    this.healthRecordService
      .getMyHealthRecords()
      .subscribe({

        next: (response) => {

          this.healthRecords = response;

          this.isLoading = false;

          this.cdr.detectChanges();

        },

        error: (err) => {

          console.log(err);

          this.isLoading = false;

          this.toastr.error(
            'Unable to load health records.'
          );

          this.cdr.detectChanges();

        }

      });

  }

  back(): void {

    this.router.navigate([
      '/patient'
    ]);

  }

}