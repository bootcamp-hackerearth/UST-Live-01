import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  HttpClient,
  HttpHeaders
} from '@angular/common/http';

import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './my-appointments.html',
  styleUrls: ['./my-appointments.css']
})
export class MyAppointments implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);

  private readonly appointmentsUrl =
    '/api/appointments';

  private readonly doctorsUrl =
    '/api/doctors';

  private readonly healthRecordsUrl =
    '/api/health-records';

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

  ngOnInit(): void {
    if (globalThis.window !== undefined) {
      this.loadAppointments();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadAppointments(): void {
    const patientId = Number(
      localStorage.getItem('patientId')
    );

    if (!patientId) {
      alert(
        'Patient profile not loaded. Please open Profile once and try again.'
      );

      return;
    }

    const headers = this.getHeaders();

    forkJoin({
      appointmentResponse: this.http.get<any[]>(
        this.appointmentsUrl,
        { headers }
      ),

      doctorResponse: this.http.get<any>(
        this.doctorsUrl,
        { headers }
      ),

      healthRecords: this.http.get<any[]>(
        `${this.healthRecordsUrl}/patient/${patientId}`,
        { headers }
      )
    }).subscribe({
      next: (result: any) => {
        console.log(
          'Appointments API:',
          result.appointmentResponse
        );

        console.log(
          'Doctors API:',
          result.doctorResponse
        );

        console.log(
          'Patient health records API:',
          result.healthRecords
        );

        const allAppointments =
          result.appointmentResponse || [];

        this.doctors =
          result.doctorResponse?.items ||
          result.doctorResponse?.data ||
          result.doctorResponse ||
          [];

        this.healthRecords =
          result.healthRecords || [];

        this.appointments = allAppointments
          .filter(
            (appointment: any) =>
              Number(appointment.patientId) ===
              patientId
          )
          .map((appointment: any) => {
            const doctor = this.doctors.find(
              (currentDoctor: any) =>
                Number(currentDoctor.doctorId) ===
                Number(appointment.doctorId)
            );

            const record = this.healthRecords.find(
              (currentRecord: any) =>
                Number(currentRecord.appointmentId) ===
                Number(appointment.appointmentId)
            );

            const hasRecord = Boolean(record);

            const finalStatus = hasRecord
              ? 'Completed'
              : this.getStatusName(
                  appointment.status
                );

            return {
              id: appointment.appointmentId,
              appointmentId:
                appointment.appointmentId,

              patientId:
                appointment.patientId,

              doctorId:
                appointment.doctorId,

              date: this.toInputDate(
                appointment.scheduledDate
              ),

              displayDate: this.toDisplayDate(
                appointment.scheduledDate
              ),

              time: appointment.timeSlot,

              doctorName: doctor
                ? doctor.fullName
                : `Doctor #${appointment.doctorId}`,

              specialisation: doctor
                ? this.getSpecialisationName(
                    doctor.specialisation
                  )
                : 'Not available',

              status: finalStatus,

              cancellationReason:
                appointment.cancellationReason || '',

              hasRecord,

              healthRecord:
                record || null
            };
          });

        this.currentPage = 1;

        console.log(
          'Mapped patient appointments:',
          this.appointments
        );

        /*
         * This immediately refreshes the screen
         * after the asynchronous API calls finish.
         *
         * Without this line, the appointments may
         * remain invisible until another UI event,
         * such as clicking View filters, occurs.
         */
        this.cdr.detectChanges();
      },

      error: (error: any) => {
        console.error(
          'Appointments load failed:',
          error
        );

        this.appointments = [];
        this.currentPage = 1;

        this.cdr.detectChanges();

        alert('Failed to load appointments.');
      }
    });
  }

  toggleFilters(): void {
    this.showFilters = !this.showFilters;
  }

  clearFilters(): void {
    this.selectedStatus = '';
    this.selectedDate = '';
    this.searchText = '';
    this.currentPage = 1;
  }

  onFilterChanged(): void {
    this.currentPage = 1;
  }

  filteredAppointments(): any[] {
    const normalizedSearchText =
      this.searchText
        .trim()
        .toLowerCase();

    return this.appointments.filter(
      (appointment: any) => {
        const doctorName =
          String(
            appointment.doctorName || ''
          ).toLowerCase();

        const specialisation =
          String(
            appointment.specialisation || ''
          ).toLowerCase();

        const matchesSearch =
          normalizedSearchText === '' ||
          doctorName.includes(
            normalizedSearchText
          ) ||
          specialisation.includes(
            normalizedSearchText
          );

        const matchesStatus =
          this.selectedStatus === '' ||
          appointment.status ===
            this.selectedStatus;

        const matchesDate =
          this.selectedDate === '' ||
          appointment.date ===
            this.selectedDate;

        return (
          matchesSearch &&
          matchesStatus &&
          matchesDate
        );
      }
    );
  }

  pagedAppointments(): any[] {
    const filtered =
      this.filteredAppointments();

    const startIndex =
      (this.currentPage - 1) *
      this.pageSize;

    const endIndex =
      startIndex + this.pageSize;

    return filtered.slice(
      startIndex,
      endIndex
    );
  }

  get totalPages(): number {
    const totalAppointments =
      this.filteredAppointments().length;

    if (totalAppointments === 0) {
      return 0;
    }

    return Math.ceil(
      totalAppointments / this.pageSize
    );
  }

  get pageNumbers(): number[] {
    return Array.from(
      {
        length: this.totalPages
      },
      (_, index) => index + 1
    );
  }

  goToPage(page: number): void {
    if (
      page < 1 ||
      page > this.totalPages ||
      page === this.currentPage
    ) {
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
    if (
      this.currentPage >=
      this.totalPages
    ) {
      return;
    }

    this.currentPage++;
  }

  viewRecord(appointmentId: number): void {
    const appointment =
      this.appointments.find(
        (currentAppointment: any) =>
          Number(
            currentAppointment.appointmentId
          ) ===
          Number(appointmentId)
      );

    if (!appointment) {
      alert('Appointment not found.');
      return;
    }

    if (!appointment.healthRecord) {
      alert(
        'Health record is not added by doctor yet.'
      );

      return;
    }

    this.selectedHealthRecord = {
      doctorName:
        appointment.doctorName,

      date:
        appointment.displayDate,

      time:
        appointment.time,

      diagnosis:
        appointment.healthRecord.diagnosis,

      prescription:
        appointment.healthRecord.prescription,

      notes:
        appointment.healthRecord.notes
    };

    this.showRecordModal = true;
    this.cdr.detectChanges();
  }

  closeRecordModal(): void {
    this.showRecordModal = false;
    this.selectedHealthRecord = null;
    this.cdr.detectChanges();
  }

  viewReason(appointmentId: number): void {
    const appointment =
      this.appointments.find(
        (currentAppointment: any) =>
          Number(
            currentAppointment.appointmentId
          ) ===
          Number(appointmentId)
      );

    alert(
      appointment?.cancellationReason ||
      'Cancellation reason not available.'
    );
  }

  private toInputDate(
    dateValue: string
  ): string {
    if (!dateValue) {
      return '';
    }

    return dateValue.split('T')[0];
  }

  private toDisplayDate(
    dateValue: string
  ): string {
    if (!dateValue) {
      return '';
    }

    const date = new Date(dateValue);

    return date.toLocaleDateString(
      'en-IN',
      {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
      }
    );
  }

  private getStatusName(
    value: any
  ): string {
    if (typeof value === 'string') {
      return value;
    }

    switch (Number(value)) {
      case 0:
        return 'Pending';

      case 1:
        return 'Confirmed';

      case 2:
        return 'Completed';

      case 3:
        return 'Cancelled';

      default:
        return 'Pending';
    }
  }

  private getSpecialisationName(
    value: number
  ): string {
    switch (Number(value)) {
      case 0:
        return 'General Medicine';

      case 1:
        return 'Pediatrician';

      case 2:
        return 'Cardiology';

      case 3:
        return 'Dermatology';

      case 4:
        return 'Orthopaedics';

      default:
        return 'Other';
    }
  }
}