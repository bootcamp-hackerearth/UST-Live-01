import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../shared/sidebar/sidebar';
import { FormsModule } from '@angular/forms';

import { HealthRecordService } from '../../Patient.service/health-recordservice';
import { HealthRecord } from '../../models/health-record/health-record.model';

@Component({
  selector: 'app-health-records',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './health-records.html',
  styleUrls: ['./health-records.css']
})
export class HealthRecords implements OnInit {

  allRecords: HealthRecord[] = [];
  paginatedRecords: HealthRecord[] = [];
  nameSearch: string = '';
  filteredTemp: HealthRecord[] = [];
  dateSearch: string = '';

  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(private readonly recordService: HealthRecordService) {}

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords() {
    this.recordService.getMyRecords().subscribe({
      next: (res) => {
        this.allRecords = res || [];
        this.filteredTemp = this.allRecords;
        this.calculateTotalPages();
        this.paginate();
      },
      error: (err) => {
        console.error('Failed to load records', err);
      }
    });
  }

  paginate() {
    const start = (this.pageNumber - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedRecords = this.filteredTemp.slice(start, end);
  }

  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.paginate();
    }
  }

  calculateTotalPages() {
    this.totalPages = Math.ceil(this.allRecords.length / this.pageSize);
  }

  filterRecords() {
    const filtered = this.allRecords.filter(r => {

      const matchName =
        !this.nameSearch ||
        r.doctorName?.toLowerCase().includes(this.nameSearch.toLowerCase());

      const matchDate =
        !this.dateSearch ||
        (r.visitDate &&
          new Date(r.visitDate).toISOString().slice(0, 10) === this.dateSearch);

      return matchName && matchDate;
    });

    this.filteredTemp = filtered;

    this.pageNumber = 1;
    this.totalPages = Math.ceil(filtered.length / this.pageSize);

    this.paginate();
  }
}