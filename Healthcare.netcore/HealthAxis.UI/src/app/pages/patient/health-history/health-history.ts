import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-health-history',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './health-history.html',
  styleUrls: ['./health-history.css']
})
export class HealthHistory implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly healthRecordsUrl = 'https://localhost:7130/api/health-records';
  private readonly doctorsUrl = 'https://localhost:7130/api/doctors';

  showFilters = false;

  searchText = '';
  selectedSpecialisation = '';
  selectedDate = '';

  records: any[] = [];
  doctors: any[] = [];

  showDetailsModal = false;
  selectedRecord: any = null;

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadHealthRecords();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadHealthRecords() {
    const patientId = Number(localStorage.getItem('patientId'));

    if (!patientId) {
      alert('Patient profile not loaded. Please open Profile once and try again.');
      return;
    }

    const headers = this.getHeaders();

    forkJoin({
      healthRecords: this.http.get<any[]>(
        `${this.healthRecordsUrl}/patient/${patientId}`,
        { headers }
      ),
      doctorResponse: this.http.get<any>(
        this.doctorsUrl,
        { headers }
      )
    }).subscribe({
      next: (result: any) => {
        console.log('Patient health records API ✅:', result.healthRecords);
        console.log('Doctors API ✅:', result.doctorResponse);

        this.doctors =
          result.doctorResponse.items ||
          result.doctorResponse.data ||
          [];

        const apiRecords = result.healthRecords || [];

        this.records = apiRecords.map((record: any) => {
          const doctor = this.doctors.find(
            (d: any) => Number(d.doctorId) === Number(record.doctorId)
          );

          return {
            id: record.recordId,
            recordId: record.recordId,
            appointmentId: record.appointmentId,
            doctorId: record.doctorId,
            patientId: record.patientId,

            visitDate: this.toInputDate(record.visitDate),
            displayDate: this.toDisplayDate(record.visitDate),

            doctorName: doctor ? doctor.fullName : `Doctor #${record.doctorId}`,
            specialisation: doctor
              ? this.getSpecialisationName(doctor.specialisation)
              : 'Not available',

            diagnosis: record.diagnosis || 'No diagnosis added',
            prescription: record.prescription || 'No prescription added',
            notes: record.notes || 'No notes added'
          };
        });

        console.log('Mapped patient health records ✅:', this.records);
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Health records load failed ❌:', err);
        alert('Failed to load health records.');
      }
    });
  }

  get latestVisitText(): string {
    if (this.records.length === 0) {
      return 'No visits';
    }

    return this.records[0].displayDate;
  }

  get careHistoryText(): string {
    return this.records.length > 0 ? 'Active' : 'Empty';
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  clearFilters() {
    this.searchText = '';
    this.selectedSpecialisation = '';
    this.selectedDate = '';
  }

  filteredRecords() {
    return this.records.filter((record: any) => {
      const matchesSearch =
        !this.searchText ||
        record.doctorName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        record.diagnosis.toLowerCase().includes(this.searchText.toLowerCase()) ||
        record.prescription.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesSpecialisation =
        this.selectedSpecialisation === '' ||
        record.specialisation === this.selectedSpecialisation;

      const matchesDate =
        this.selectedDate === '' ||
        record.visitDate === this.selectedDate;

      return matchesSearch && matchesSpecialisation && matchesDate;
    });
  }

  viewDetails(record: any) {
    this.selectedRecord = record;
    this.showDetailsModal = true;
  }

  closeDetailsModal() {
    this.showDetailsModal = false;
    this.selectedRecord = null;
  }

  private toInputDate(dateValue: string): string {
    if (!dateValue) {
      return '';
    }

    return dateValue.split('T')[0];
  }

  private toDisplayDate(dateValue: string): string {
    if (!dateValue) {
      return '';
    }

    const date = new Date(dateValue);

    return date.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private getSpecialisationName(value: number): string {
    switch (Number(value)) {
      case 0: return 'General Medicine';
      case 1: return 'Pediatrician';
      case 2: return 'Cardiology';
      case 3: return 'Dermatology';
      case 4: return 'Orthopaedics';
      default: return 'Other';
    }
  }
}