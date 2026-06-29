import { Component } from '@angular/core';
import { HealthRecordDto } from '../../../../../shared/models/health-record.models';
import { PatientFakeDataService } from '../../../../../core/services/patient-fake-data.service';

@Component({
  selector: 'app-health-records',
  standalone: true,
  templateUrl: './patient-health-records.html',
  styleUrl: './patient-health-records.css'
})
export class PatientHealthRecords {

  records: HealthRecordDto[] = [];

  // ✅ modal state
  selectedRecord?: HealthRecordDto;
  isModalOpen = false;

  constructor(private service: PatientFakeDataService) {
    this.loadRecords();
  }

  loadRecords(): void {
    this.records = this.service.getHealthRecords().sort(
      (a: HealthRecordDto, b: HealthRecordDto) =>
        new Date(b.visitDate).getTime() -
        new Date(a.visitDate).getTime()
    );
  }

  // ✅ open modal
  openDetails(record: HealthRecordDto): void {
    this.selectedRecord = record;
    this.isModalOpen = true;
  }

  // ✅ close modal
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
}