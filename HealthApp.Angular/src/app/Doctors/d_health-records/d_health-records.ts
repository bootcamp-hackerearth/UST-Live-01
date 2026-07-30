import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/d_sidebar/d_sidebar';

import { HealthRecordService } from '../../Doctor.service/health-recordservice';
import { DoctorService } from '../../Doctor.service/doctorservice';

@Component({
  selector: 'app-doctor-healthrecords',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './d_health-records.html',
  styleUrls: ['./d_health-records.css']
})
export class DoctorHealthRecords implements OnInit {

  records = signal<any[]>([]);

  showForm = signal(false);
  filteredRecords = signal<any[]>([]);
  paginatedRecords = signal<any[]>([]);
  pageNumber = signal(1);
  pageSize = 5;
  totalPages = signal(0);

  doctor = signal<any>(null);
  selected = signal<any>(null);

  searchName = '';
searchDate = '';

  form: any = {
    patientId: '',
    diagnosis: '',
    notes: ''
  };

  constructor(
    private readonly service: HealthRecordService,
    private readonly doctorService: DoctorService
  ) {}

  ngOnInit(): void {
    this.load();
    this.loadDoctor();
  }

  view(data: any) {
    this.selected.set(data);
  }

  closeView() {
    this.selected.set(null);
  }

  loadDoctor() {
    this.doctorService.getMyProfile().subscribe({
      next: (res) => {
        this.doctor.set(res);
        console.log('Doctor loaded:', res);
      },
      error: (err) => console.error('Failed to load doctor', err)
    });
  }

  load() {
  this.service.getDoctorRecords().subscribe({
    next: (res: any) => {

      const list = res?.data || res || [];

      this.records.set(list);
      this.filteredRecords.set(list);

      this.pageNumber.set(1);

      this.updatePagination();
    },
    error: (err) =>
      console.error('Failed to load records', err)
  });
}

  create() {
    if (this.doctor()) {
      this.form.doctorId = this.doctor().doctorId;
      this.form.doctorName = this.doctor().fullName;
    }

    this.service.createRecord(this.form).subscribe({
      next: () => {
        this.showForm.set(false);

        this.form = {
          patientId: '',
          doctorId: '',
          patientName: '',
          doctorName: '',
          visitDate: '',
          diagnosis: '',
          prescription: '',
          notes: ''
        };

        this.load();
      },
      error: (err) => {
        console.error('Create error:', err.error);
      }
    });
  }
applyFilters(): void {

  const filtered = this.records().filter(record => {

    const matchName =
      !this.searchName ||
      (record.patientName ?? '')
        .toLowerCase()
        .includes(this.searchName.toLowerCase());

    let matchDate = true;

    if (this.searchDate) {

      const visitDate =
        new Date(record.visitDate)
          .toISOString()
          .split('T')[0];

      matchDate =
        visitDate === this.searchDate;
    }

    return matchName && matchDate;
  });

  this.filteredRecords.set(filtered);

  this.pageNumber.set(1);

  this.updatePagination();
}
resetFilters(): void {

  this.searchName = '';
  this.searchDate = '';

  this.filteredRecords.set(
    this.records()
  );

  this.pageNumber.set(1);

  this.updatePagination();
}

  updatePagination() {
    this.totalPages.set(Math.ceil(this.filteredRecords().length / this.pageSize));
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedRecords.set(this.filteredRecords().slice(start, end));
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
}