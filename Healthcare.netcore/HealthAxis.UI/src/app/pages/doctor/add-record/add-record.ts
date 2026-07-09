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

  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly doctorsUrl = 'https://localhost:7130/api/doctors';
  private readonly appointmentsUrl = 'https://localhost:7130/api/appointments';
  private readonly patientsUrl = 'https://localhost:7130/api/patients';
  private readonly healthRecordsUrl = 'https://localhost:7130/api/health-records';

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
        this.handleBaseHealthRecordData(result, headers);
      },
      error: (err: any) => {
        console.error('Doctor health records base load failed ❌:', err);
        alert('Failed to load doctor health records.');
      }
    });
  }

  private handleBaseHealthRecordData(result: any, headers: HttpHeaders): void {
    console.log('Doctor health records base data ✅:', result);

    this.doctorId = Number(result.doctor.doctorId);
    localStorage.setItem('doctorId', String(this.doctorId));

    const allAppointments = result.appointments || [];
    const patients = this.extractPatients(result.patients);
    const doctorAppointments = this.getDoctorAppointments(allAppointments);
    const patientIds = this.getPatientIds(doctorAppointments);

    if (patientIds.length === 0) {
      this.createdRecords = [];
      this.cdr.detectChanges();
      return;
    }

    this.loadPatientHealthRecords(patientIds, patients, headers);
  }

  private extractPatients(patientResponse: any): any[] {
    return patientResponse?.items || patientResponse?.data || [];
  }

  private getDoctorAppointments(allAppointments: any[]): any[] {
    return allAppointments.filter((appointment: any) =>
      Number(appointment.doctorId) === Number(this.doctorId)
    );
  }

  private getPatientIds(doctorAppointments: any[]): number[] {
    return Array.from(
      new Set<number>(
        doctorAppointments.map((appointment: any) => Number(appointment.patientId))
      )
    );
  }

  private loadPatientHealthRecords(
    patientIds: number[],
    patients: any[],
    headers: HttpHeaders
  ): void {
    forkJoin(
      patientIds.map((patientId: number) =>
        this.http.get<any[]>(
          `${this.healthRecordsUrl}/patient/${patientId}`,
          { headers }
        )
      )
    ).subscribe({
      next: (recordGroups: any[]) => {
        this.handleHealthRecordGroups(recordGroups, patients);
      },
      error: (err: any) => {
        console.error('Health records load failed ❌:', err);
        alert('Failed to load health records.');
      }
    });
  }

  private handleHealthRecordGroups(recordGroups: any[], patients: any[]): void {
    const allRecords = recordGroups.flat();
    const patientMap = this.createPatientMap(patients);

    this.createdRecords = allRecords
      .filter((record: any) => this.isCurrentDoctorRecord(record))
      .map((record: any) => this.mapCreatedRecord(record, patientMap));

    console.log('Created records mapped ✅:', this.createdRecords);
    this.cdr.detectChanges();
  }

  private createPatientMap(patients: any[]): Map<number, any> {
    return new Map(
      patients.map((patient: any) => [
        Number(patient.patientId),
        patient
      ])
    );
  }

  private isCurrentDoctorRecord(record: any): boolean {
    return Number(record.doctorId) === Number(this.doctorId);
  }

  private mapCreatedRecord(record: any, patientMap: Map<number, any>): any {
    const patient = patientMap.get(Number(record.patientId));

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