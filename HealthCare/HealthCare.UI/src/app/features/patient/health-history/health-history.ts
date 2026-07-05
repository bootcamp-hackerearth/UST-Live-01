import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientService }
from '../../../core/services/patient.service';

import { HealthRecord }from '../../../core/models/health-record.model';

@Component({
  selector: 'app-health-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './health-history.html',
  styleUrl: './health-history.css'
})
export class HealthHistoryComponent
implements OnInit {
  records = signal<HealthRecord[]>([]);

  loading = signal(false);

  constructor(
    private readonly patientService: PatientService
  ) {}

  ngOnInit(): void {

    this.loadRecords();

  }

loadRecords() {

  this.loading.set(true);

  this.patientService.getHealthRecords()
    .subscribe({

      next: (res: HealthRecord[]) => {

        this.records.set(res ?? []); 

        this.loading.set(false);

      },

      error: (err) => {

        console.error(err);

        this.records.set([]);

        this.loading.set(false);

      }
    });
}
}

