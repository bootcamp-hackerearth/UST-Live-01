import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { HealthRecordDto } from '../../../../../shared/models/health-record.models';
import { DoctorFakeDataService } from '../../../../../core/services/doctor-fake-data.service';

@Component({
  selector: 'app-doctor-health-records',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './doctor-health-records.html',
  styleUrl: './doctor-health-records.css'
})
export class DoctorHealthRecords {
  records: HealthRecordDto[] = [];

  searchTerm = '';
  selectedVisitDate = '';

  selectedRecord?: HealthRecordDto;
  isModalOpen = false;

  constructor(private service: DoctorFakeDataService) {
    this.loadRecords();
  }

  get filteredRecords(): HealthRecordDto[] {
    const term = this.searchTerm.trim().toLowerCase();

    return this.records.filter((record: HealthRecordDto) =>
      this.matchesSearchTerm(record, term) &&
      this.matchesVisitDateFilter(record)
    );
  }

  get hasActiveFilters(): boolean {
    return (
      this.searchTerm.trim().length > 0 ||
      !!this.selectedVisitDate
    );
  }

  loadRecords(): void {
    this.records = this.service.getDoctorHealthRecords();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedVisitDate = '';
  }

  openDetails(record: HealthRecordDto): void {
    this.selectedRecord = record;
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedRecord = undefined;
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private matchesSearchTerm(
    record: HealthRecordDto,
    term: string
  ): boolean {
    if (!term) {
      return true;
    }

    return (
      record.patientName.toLowerCase().includes(term) ||
      record.diagnosis.toLowerCase().includes(term) ||
      record.prescription.toLowerCase().includes(term) ||
      record.appointmentId.toString().includes(term)
    );
  }

  private matchesVisitDateFilter(record: HealthRecordDto): boolean {
    if (!this.selectedVisitDate) {
      return true;
    }

    return record.visitDate === this.selectedVisitDate;
  }
}