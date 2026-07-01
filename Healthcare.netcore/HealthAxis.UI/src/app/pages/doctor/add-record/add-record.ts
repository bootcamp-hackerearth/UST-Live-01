import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-add-record',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-record.html',
  styleUrls: ['./add-record.css']
})
export class AddRecord implements OnInit {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  private doctorsUrl = 'https://localhost:7130/api/doctors';
  private appointmentsUrl = 'https://localhost:7130/api/appointments';
  private patientsUrl = 'https://localhost:7130/api/patients';
  private healthRecordsUrl = 'https://localhost:7130/api/health-records';

  searchText = '';
  fromDate = '';
  toDate = '';

  doctorId = 0;

  createdRecords: any[] = [];

  showRecordDetailsModal = false;
  selectedRecord: any = null;

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadHealthRecordData();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadHealthRecordData() {
    const headers = this.getHeaders();

    forkJoin({
      doctor: this.http.get<any>(`${this.doctorsUrl}/me`, { headers }),
      appointments: this.http.get<any[]>(this.appointmentsUrl, { headers }),
      patients: this.http.get<any>(
        `${this.patientsUrl}/doctor?pageNumber=1&pageSize=100`,
        { headers }
      )
    }).subscribe({
      next: (result: any) => {
        console.log('Doctor health records base data ✅:', result);

        this.doctorId = Number(result.doctor.doctorId);
        localStorage.setItem('doctorId', String(this.doctorId));

        const allAppointments = result.appointments || [];
        const patients = result.patients.items || result.patients.data || [];

        const doctorAppointments = allAppointments.filter(
          (appointment: any) => Number(appointment.doctorId) === Number(this.doctorId)
        );

        const patientIds: number[] = Array.from(
          new Set<number>(
            doctorAppointments.map((appointment: any) => Number(appointment.patientId))
          )
        );

        if (patientIds.length === 0) {
          this.createdRecords = [];
          this.cdr.detectChanges();
          return;
        }

        forkJoin(
          patientIds.map((patientId: number) =>
            this.http.get<any[]>(
              `${this.healthRecordsUrl}/patient/${patientId}`,
              { headers }
            )
          )
        ).subscribe({
          next: (recordGroups: any[]) => {
            const allRecords = recordGroups.flat();

            this.createdRecords = allRecords
              .filter((record: any) => Number(record.doctorId) === Number(this.doctorId))
              .map((record: any) => {
                const patient = patients.find(
                  (p: any) => Number(p.patientId) === Number(record.patientId)
                );

                return {
                  id: record.recordId,
                  recordId: record.recordId,
                  appointmentId: record.appointmentId,
                  patientId: record.patientId,
                  doctorId: record.doctorId,
                  date: this.toDisplayDate(record.visitDate),
                  rawDate: this.toInputDate(record.visitDate),
                  patient: patient ? patient.fullName : `Patient #${record.patientId}`,
                  diagnosis: record.diagnosis || 'No diagnosis added',
                  prescription: record.prescription || 'No prescription added',
                  notes: record.notes || 'No notes added'
                };
              });

            console.log('Created records mapped ✅:', this.createdRecords);
            this.cdr.detectChanges();
          },
          error: (err: any) => {
            console.error('Health records load failed ❌:', err);
            alert('Failed to load health records.');
          }
        });
      },
      error: (err: any) => {
        console.error('Doctor health records base load failed ❌:', err);
        alert('Failed to load doctor health records.');
      }
    });
  }

  clearFilters() {
    this.searchText = '';
    this.fromDate = '';
    this.toDate = '';
  }

  filteredCreatedRecords() {
    return this.createdRecords.filter((record: any) => {
      const matchesSearch =
        !this.searchText ||
        record.patient.toLowerCase().includes(this.searchText.toLowerCase()) ||
        record.diagnosis.toLowerCase().includes(this.searchText.toLowerCase()) ||
        record.prescription.toLowerCase().includes(this.searchText.toLowerCase());

      let matchesFromDate = true;
      let matchesToDate = true;

      if (this.fromDate) {
        matchesFromDate = record.rawDate >= this.fromDate;
      }

      if (this.toDate) {
        matchesToDate = record.rawDate <= this.toDate;
      }

      return matchesSearch && matchesFromDate && matchesToDate;
    });
  }

  viewRecord(record: any) {
    this.selectedRecord = record;
    this.showRecordDetailsModal = true;
  }

  closeRecordDetailsModal() {
    this.showRecordDetailsModal = false;
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
}