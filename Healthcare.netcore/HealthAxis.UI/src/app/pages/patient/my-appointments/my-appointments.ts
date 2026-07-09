import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './my-appointments.html',
  styleUrls: ['./my-appointments.css']
})
export class MyAppointments implements OnInit {

  private readonly http = inject(HttpClient);

  private readonly appointmentsUrl = 'https://localhost:7130/api/appointments';
  private readonly doctorsUrl = 'https://localhost:7130/api/doctors';
  private readonly healthRecordsUrl = 'https://localhost:7130/api/health-records';

  showFilters = false;

  selectedStatus = '';
  selectedDate = '';
  searchText = '';

  appointments: any[] = [];
  doctors: any[] = [];
  healthRecords: any[] = [];

  currentPage = 1;
  pageSize = 5;

  showRecordModal = false;
  selectedHealthRecord: any = null;

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadAppointments();
    }
  }

  getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadAppointments() {
    const patientId = Number(localStorage.getItem('patientId'));

    if (!patientId) {
      alert('Patient profile not loaded. Please open Profile once and try again.');
      return;
    }

    const headers = this.getHeaders();

    forkJoin({
      appointmentResponse: this.http.get<any[]>(this.appointmentsUrl, { headers }),
      doctorResponse: this.http.get<any>(this.doctorsUrl, { headers }),
      healthRecords: this.http.get<any[]>(
        `${this.healthRecordsUrl}/patient/${patientId}`,
        { headers }
      )
    }).subscribe({
      next: (result: any) => {
        console.log('Appointments API ✅:', result.appointmentResponse);
        console.log('Doctors API ✅:', result.doctorResponse);
        console.log('Patient health records API ✅:', result.healthRecords);

        const allAppointments = result.appointmentResponse || [];

        this.doctors =
          result.doctorResponse.items ||
          result.doctorResponse.data ||
          [];

        this.healthRecords = result.healthRecords || [];

        this.appointments = allAppointments
          .filter((a: any) => Number(a.patientId) === Number(patientId))
          .map((a: any) => {
            const doctor = this.doctors.find(
              (d: any) => Number(d.doctorId) === Number(a.doctorId)
            );

            const record = this.healthRecords.find(
              (r: any) => Number(r.appointmentId) === Number(a.appointmentId)
            );

            const hasRecord = !!record;

            const finalStatus = hasRecord
              ? 'Completed'
              : this.getStatusName(a.status);

            return {
              id: a.appointmentId,
              appointmentId: a.appointmentId,
              patientId: a.patientId,
              doctorId: a.doctorId,

              date: this.toInputDate(a.scheduledDate),
              displayDate: this.toDisplayDate(a.scheduledDate),

              time: a.timeSlot,
              doctorName: doctor ? doctor.fullName : `Doctor #${a.doctorId}`,
              specialisation: doctor
                ? this.getSpecialisationName(doctor.specialisation)
                : 'Not available',

              status: finalStatus,
              cancellationReason: a.cancellationReason || '',
              hasRecord: hasRecord,
              healthRecord: record || null
            };
          });

        this.currentPage = 1;

        console.log('Mapped patient appointments ✅:', this.appointments);
      },
      error: (err: any) => {
        console.error('Appointments load failed ❌:', err);
        alert('Failed to load appointments.');
      }
    });
  }

  toggleFilters() {
    this.showFilters = !this.showFilters;
  }

  clearFilters() {
    this.selectedStatus = '';
    this.selectedDate = '';
    this.searchText = '';
    this.currentPage = 1;
  }

  onFilterChanged(): void {
    this.currentPage = 1;
  }

  filteredAppointments() {
    return this.appointments.filter((a: any) => {
      const matchesSearch =
        !this.searchText ||
        a.doctorName.toLowerCase().includes(this.searchText.toLowerCase()) ||
        a.specialisation.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesStatus =
        this.selectedStatus === '' ||
        a.status === this.selectedStatus;

      const matchesDate =
        this.selectedDate === '' ||
        a.date === this.selectedDate;

      return matchesSearch && matchesStatus && matchesDate;
    });
  }

  pagedAppointments() {
    const filtered = this.filteredAppointments();
    const startIndex = (this.currentPage - 1) * this.pageSize;

    return filtered.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredAppointments().length / this.pageSize);
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
  }

  previousPage(): void {
    if (this.currentPage <= 1) {
      return;
    }

    this.currentPage--;
  }

  nextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
  }

  viewRecord(id: number) {
    const appointment = this.appointments.find(
      (a: any) => Number(a.appointmentId) === Number(id)
    );

    if (!appointment) {
      alert('Appointment not found.');
      return;
    }

    if (!appointment.healthRecord) {
      alert('Health record is not added by doctor yet.');
      return;
    }

    this.selectedHealthRecord = {
      doctorName: appointment.doctorName,
      date: appointment.displayDate,
      time: appointment.time,
      diagnosis: appointment.healthRecord.diagnosis,
      prescription: appointment.healthRecord.prescription,
      notes: appointment.healthRecord.notes
    };

    this.showRecordModal = true;
  }

  closeRecordModal() {
    this.showRecordModal = false;
    this.selectedHealthRecord = null;
  }

  viewReason(id: number) {
    const appointment = this.appointments.find(
      (a: any) => Number(a.appointmentId) === Number(id)
    );

    alert(
      appointment?.cancellationReason ||
      'Cancellation reason not available.'
    );
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

  private getStatusName(value: any): string {
    if (typeof value === 'string') {
      return value;
    }

    switch (Number(value)) {
      case 0: return 'Pending';
      case 1: return 'Confirmed';
      case 2: return 'Completed';
      case 3: return 'Cancelled';
      default: return 'Pending';
    }
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