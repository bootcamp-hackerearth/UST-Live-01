import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { HealthRecordDto } from '../../../dtos/health-record.dto';
import { HealthRecordService } from '../../../core/services/health-record.service';

@Component({
  selector: 'app-health-records',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './health-records.html',
  styleUrl: './health-records.css'
})
export class HealthRecords implements OnInit {
  searchText = '';
  selectedDate = '';

  isLoading = false;

  expandedRecordId: number | null = null;

  healthRecords: HealthRecordDto[] = [];

  constructor(
    private readonly healthRecordService: HealthRecordService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadHealthRecords();
  }

  loadHealthRecords(): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    this.healthRecordService
      .getMyHealthRecords()
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: records => {
          this.healthRecords = records ?? [];
          this.expandedRecordId = null;
          this.cdr.markForCheck();
        },
        error: () => {
          this.healthRecords = [];
          this.expandedRecordId = null;
          this.cdr.markForCheck();
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
        this.formatDateOnly(record.visitDate) === this.selectedDate;

      return matchesSearch && matchesDate;
    });
  }

  get totalRecords(): number {
    return this.healthRecords.length;
  }

  toggleRecord(recordId: number): void {
    this.expandedRecordId =
      this.expandedRecordId === recordId ? null : recordId;

    this.cdr.markForCheck();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedDate = '';
    this.cdr.markForCheck();
  }

  refreshRecords(): void {
    this.loadHealthRecords();
  }

  formatDateOnly(dateValue: string): string {
    return dateValue ? dateValue.substring(0, 10) : '';
  }
}
