import { Component, OnInit, signal } from '@angular/core';
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

  allRecords = signal<HealthRecord[]>([]);
  paginatedRecords = signal<HealthRecord[]>([]);
  nameSearch: string = '';
  filteredTemp = signal<HealthRecord[]>([]);
  dateSearch: string = '';

  pageNumber = signal(1);
  pageSize = 5;
  totalPages = signal(0);

  constructor(private readonly recordService: HealthRecordService) {}

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords() {
    this.recordService.getMyRecords().subscribe({
      next: (res) => {
        const records = Array.isArray(res) ? res : [];
        this.allRecords.set(records);
        this.filteredTemp.set([...records]);
        this.calculateTotalPages();
        this.pageNumber.set(1);
        this.paginate();
      },
      error: (err) => {
        console.error('Failed to load records', err);
        this.allRecords.set([]);
        this.filteredTemp.set([]);
        this.paginatedRecords.set([]);
        this.totalPages.set(0);
        this.pageNumber.set(1);
      }
    });
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedRecords.set(this.filteredTemp().slice(start, end));
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(value => value + 1);
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(value => value - 1);
      this.paginate();
    }
  }

  calculateTotalPages() {
    this.totalPages.set(Math.ceil(this.allRecords().length / this.pageSize));
  }

  filterRecords() {
    const filtered = this.allRecords().filter(r => {

      const matchName =
        !this.nameSearch ||
        r.doctorName?.toLowerCase().includes(this.nameSearch.toLowerCase());

      const matchDate =
        !this.dateSearch ||
        (r.visitDate &&
          new Date(r.visitDate).toISOString().slice(0, 10) === this.dateSearch);

      return matchName && matchDate;
    });

    this.filteredTemp.set(filtered);

    this.pageNumber.set(1);
    this.totalPages.set(Math.ceil(filtered.length / this.pageSize));

    this.paginate();
  }
}