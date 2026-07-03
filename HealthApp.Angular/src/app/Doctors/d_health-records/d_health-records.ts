import { Component, OnInit } from '@angular/core';
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

  records: any[] = [];

  showForm = false;
  filteredRecords: any[] = [];
  paginatedRecords: any[] = [];
  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  doctor: any = null;
  selected: any = null;

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
    this.selected = data;
  }

  closeView() {
    this.selected = null;
  }

  loadDoctor() {
    this.doctorService.getMyProfile().subscribe({
      next: (res) => {
        this.doctor = res;
        console.log('Doctor loaded:', res);
      },
      error: (err) => console.error('Failed to load doctor', err)
    });
  }

  load() {
    this.service.getDoctorRecords().subscribe({
      next: (res: any) => {

        const list = res?.data || res || [];

        this.records = list;
        this.filteredRecords = list;

        this.updatePagination();
      },
      error: (err) => console.error('Failed to load records', err)
    });
  }

  create() {
    if (this.doctor) {
      this.form.doctorId = this.doctor.doctorId;
      this.form.doctorName = this.doctor.fullName;
    }

    this.service.createRecord(this.form).subscribe({
      next: () => {
        this.showForm = false;

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

  updatePagination() {
    this.totalPages = Math.ceil(this.filteredRecords.length / this.pageSize);
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedRecords = this.filteredRecords.slice(start, end);
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
}