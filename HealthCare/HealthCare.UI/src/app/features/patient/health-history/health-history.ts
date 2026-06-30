import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientService }
from '../../../core/services/patient.service';

import { HealthRecord }
from '../../../core/models/health-record.model';

@Component({
  selector: 'app-health-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './health-history.html',
  styleUrl: './health-history.css'
})
export class HealthHistoryComponent
implements OnInit {

  records: any[] = []

  loading = false;

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.loadRecords();

  }

  loadRecords() {

    this.loading = true;

    this.patientService
      .getHealthRecords()
      .subscribe({

        next: (res) => {

          this.records = res ?? [];

          this.loading = false;
        },

        error: err => {

          console.log(err);
          this.records = [];
          this.loading = false;
        }

      });

  }

}