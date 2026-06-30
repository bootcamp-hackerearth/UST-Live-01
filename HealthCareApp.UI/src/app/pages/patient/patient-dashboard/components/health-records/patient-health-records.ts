import { Component,OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize, timeout } from 'rxjs';

import { HealthRecordDto } from '../../../../../shared/models/health-record.models';
import { HealthRecordApiService } from '../../../../../core/services/health-record-api.service';

@Component({
  selector: 'app-health-records',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-health-records.html',
  styleUrl: './patient-health-records.css'
})
export class PatientHealthRecords {
  records: HealthRecordDto[] = [];

  searchTerm = '';
  selectedVisitDate = '';

  isLoading = false;
  errorMessage = '';

  pageNumber = 1;
  pageSize = 5;
  totalRecords = 0;
  totalPages = 0;

  pageSizeOptions: number[] = [5, 10, 15, 20];

  selectedRecord?: HealthRecordDto;
  isModalOpen = false;

  constructor(private healthRecordApiService: HealthRecordApiService) {
    this.loadRecords();
  }

  get hasActiveFilters(): boolean {
    return (
      this.searchTerm.trim().length > 0 ||
      !!this.selectedVisitDate
    );
  }

  get canGoPrevious(): boolean {
    return this.pageNumber > 1;
  }

  get canGoNext(): boolean {
    return this.pageNumber < this.totalPages;
  }

  loadRecords(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.healthRecordApiService.getMyHealthRecords({
      pageNumber: this.pageNumber,
      pageSize: this.pageSize,
      searchTerm: this.searchTerm,
      visitDate: this.selectedVisitDate
    }).pipe(
      timeout(15000),
      finalize(() => {
        this.isLoading = false;
      })
    ).subscribe({
      next: (response) => {
        this.records = response.items ?? [];
        this.pageNumber = response.pageNumber;
        this.pageSize = response.pageSize;
        this.totalRecords = response.totalRecords;
        this.totalPages = response.totalPages;
      },
      error: (error: unknown) => {
        console.log('Patient health records API error:', error);
        this.records = [];
        this.totalRecords = 0;
        this.totalPages = 0;
        this.errorMessage = this.getErrorMessage(error);
      }
    });
  }

  applyFilters(): void {
    this.pageNumber = 1;
    this.loadRecords();
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.selectedVisitDate = '';
    this.pageNumber = 1;
    this.loadRecords();
  }

  changePageSize(): void {
    this.pageNumber = 1;
    this.loadRecords();
  }

  goToPreviousPage(): void {
    if (!this.canGoPrevious) {
      return;
    }

    this.pageNumber--;
    this.loadRecords();
  }

  goToNextPage(): void {
    if (!this.canGoNext) {
      return;
    }

    this.pageNumber++;
    this.loadRecords();
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
    const parsedDate = new Date(date);

    if (Number.isNaN(parsedDate.getTime())) {
      return 'Not Available';
    }

    return parsedDate.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private getErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          errors?: Record<string, string[]>;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (apiError.error?.message) {
        return apiError.error.message;
      }

      if (apiError.error?.Message) {
        return apiError.error.Message;
      }

      if (apiError.error?.errors) {
        const firstError = Object.values(apiError.error.errors)[0]?.[0];

        if (firstError) {
          return firstError;
        }
      }
    }

    return 'Something went wrong while loading health records.';
  }
}