import { CommonModule } from '@angular/common';
import { Component, OnInit, computed, signal } from '@angular/core';
import { HealthRecordService } from '../../core/services/health-record.service';
import { HealthRecordResponse } from '../../core/models/portal.models';

@Component({
  selector: 'app-health-records',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './health-records.html',
  styleUrl: './health-records.css'
})
export class HealthRecords implements OnInit {
  records = signal<HealthRecordResponse[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');

  hasRecords = computed(() => this.records().length > 0);

  showEmptyMessage = computed(() =>
    !this.isLoading() &&
    !this.hasRecords() &&
    !this.errorMessage()
  );

  constructor(private readonly healthRecordService: HealthRecordService) { }

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');
    this.records.set([]);

    this.healthRecordService.getMyRecords().subscribe({
      next: (response) => {
        console.log('Health records received:', response);

        this.records.set(response ?? []);
        this.isLoading.set(false);

        console.log(
          'Health records count:',
          this.records().length
        );
      },
      error: (error) => {
        console.error(
          'Failed to load health records:',
          error
        );

        this.records.set([]);

        this.errorMessage.set(
          error?.error?.message ||
          'Failed to load health records.'
        );

        this.isLoading.set(false);
      }
    });
  }
}
