import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { HealthRecordDto } from '../../../dtos/health-record.dto';
import { HealthRecordService } from '../../../core/services/health-record.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-doctor-health-records',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './doctor-health-records.html',
  styleUrl: './doctor-health-records.css'
})
export class DoctorHealthRecords implements OnInit {
  patientId: number | null = null;

  searchText = '';
  selectedDate = '';

  isLoading = false;

  healthRecords: HealthRecordDto[] = [];

  expandedRecordId: number | null = null;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly healthRecordService: HealthRecordService,
    private readonly notificationService: NotificationService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const patientIdParam = params.get('patientId');

      this.patientId = null;
      this.healthRecords = [];
      this.expandedRecordId = null;
      this.clearFilters();

      if (!patientIdParam) {
        this.cdr.markForCheck();
        return;
      }

      const parsedPatientId = Number(patientIdParam);

      if (Number.isNaN(parsedPatientId) || parsedPatientId <= 0) {
        this.notificationService.warning('Invalid patient selected.');
        this.cdr.markForCheck();
        return;
      }

      this.patientId = parsedPatientId;
      this.loadPatientHealthRecords(parsedPatientId);
    });
  }

  loadPatientHealthRecords(patientId: number): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    this.healthRecordService
      .getPatientHealthRecords(patientId)
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: records => {
          this.healthRecords = records ?? [];
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  get filteredRecords(): HealthRecordDto[] {
    const search = this.searchText.trim().toLowerCase();

    return this.healthRecords.filter(record => {
      const matchesSearch =
        !search ||
        record.recordId.toString().includes(search) ||
        record.patientName.toLowerCase().includes(search) ||
        record.doctorName.toLowerCase().includes(search) ||
        record.diagnosis.toLowerCase().includes(search) ||
        record.prescription.toLowerCase().includes(search);

      const matchesDate =
        !this.selectedDate ||
        record.visitDate.substring(0, 10) === this.selectedDate;

      return matchesSearch && matchesDate;
    });
  }

  toggleRecord(recordId: number): void {
    this.expandedRecordId =
      this.expandedRecordId === recordId ? null : recordId;
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedDate = '';
  }

  refreshRecords(): void {
    if (!this.patientId) {
      return;
    }

    this.loadPatientHealthRecords(this.patientId);
  }
}