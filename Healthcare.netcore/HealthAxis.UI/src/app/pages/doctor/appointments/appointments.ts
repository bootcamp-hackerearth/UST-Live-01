import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef,
  NgZone
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './appointments.html',
  styleUrls: ['./appointments.css']
})
export class Appointments implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly zone = inject(NgZone);

  private readonly doctorsUrl =
    '/api/doctors';

  private readonly appointmentsUrl =
    '/api/appointments';

  private readonly patientsUrl =
    '/api/patients';

  private readonly healthRecordsUrl =
    '/api/health-records';

  searchText = '';
  selectedStatus = '';
  selectedDateFilter = 'All';
  customDate = '';

  appointments: any[] = [];
  patients: any[] = [];

  currentPage = 1;
  pageSize = 5;

  showRecordModal = false;
  selectedAppointment: any = null;
  isSavingRecord = false;

  showCancelModal = false;
  selectedCancelAppointment: any = null;
  cancelReason = '';

  showViewRecordModal = false;
  selectedHealthRecord: any = null;

  recordForm = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

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
    const headers = this.getHeaders();

    forkJoin({
      doctor: this.http.get<any>(
        `${this.doctorsUrl}/me`,
        { headers }
      ),

      appointments: this.http.get<any[]>(
        this.appointmentsUrl,
        { headers }
      ),

      patients: this.http.get<any>(
        `${this.patientsUrl}/doctor?pageNumber=1&pageSize=100`,
        { headers }
      )
    }).subscribe({
      next: (result: any) => {
        console.log('Doctor profile:', result.doctor);
        console.log('Appointments API:', result.appointments);
        console.log('Doctor patients API:', result.patients);

        const doctorId = Number(result.doctor.doctorId);

        localStorage.setItem(
          'doctorId',
          String(doctorId)
        );

        this.patients =
          result.patients?.items ||
          result.patients?.data ||
          [];

        const mappedAppointments =
          (result.appointments || [])
            .filter(
              (appointment: any) =>
                Number(appointment.doctorId) === doctorId
            )
            .map((appointment: any) => {
              const patient = this.patients.find(
                (currentPatient: any) =>
                  Number(currentPatient.patientId) ===
                  Number(appointment.patientId)
              );

              return {
                id: appointment.appointmentId,
                appointmentId: appointment.appointmentId,
                patientId: appointment.patientId,
                doctorId: appointment.doctorId,

                rawDate: this.toInputDate(
                  appointment.scheduledDate
                ),

                date: this.toDisplayDate(
                  appointment.scheduledDate
                ),

                time: appointment.timeSlot,

                patient: patient
                  ? patient.fullName
                  : `Patient #${appointment.patientId}`,

                status: this.getStatusName(
                  appointment.status
                ),

                cancellationReason:
                  appointment.cancellationReason || '',

                hasRecord: false,
                healthRecord: null
              };
            });

        this.loadHealthRecordFlags(
          mappedAppointments,
          headers
        );
      },

      error: (error: any) => {
        console.error(
          'Doctor appointments load failed:',
          error
        );

        alert('Failed to load doctor appointments.');
      }
    });
  }

  private loadHealthRecordFlags(
    mappedAppointments: any[],
    headers: HttpHeaders
  ): void {

    const patientIds: number[] = Array.from(
      new Set<number>(
        mappedAppointments.map(
          (appointment: any) =>
            Number(appointment.patientId)
        )
      )
    );

    if (patientIds.length === 0) {
      this.appointments = mappedAppointments;
      this.adjustCurrentPage();
      this.cdr.detectChanges();
      return;
    }

    forkJoin(
      patientIds.map(
        (patientId: number) =>
          this.http.get<any[]>(
            `${this.healthRecordsUrl}/patient/${patientId}`,
            { headers }
          )
      )
    ).subscribe({
      next: (recordGroups: any[]) => {
        const allRecords = recordGroups.flat();

        this.appointments = mappedAppointments.map(
          (appointment: any) => {
            const record = allRecords.find(
              (currentRecord: any) =>
                Number(currentRecord.appointmentId) ===
                Number(appointment.appointmentId)
            );

            return {
              ...appointment,
              hasRecord: Boolean(record),
              healthRecord: record || null
            };
          }
        );

        this.adjustCurrentPage();

        console.log(
          'Mapped doctor appointments with records:',
          this.appointments
        );

        this.cdr.detectChanges();
      },

      error: (error: any) => {
        console.error(
          'Health record flag load failed:',
          error
        );

        this.appointments = mappedAppointments;
        this.adjustCurrentPage();
        this.cdr.detectChanges();
      }
    });
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedStatus = '';
    this.selectedDateFilter = 'All';
    this.customDate = '';
    this.currentPage = 1;
  }

  onFiltersChanged(): void {
    this.currentPage = 1;
  }

  filteredAppointments(): any[] {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return this.appointments.filter(
      (appointment: any) => {

        const appointmentDate =
          new Date(appointment.rawDate);

        appointmentDate.setHours(0, 0, 0, 0);

        const normalizedSearchText =
          this.searchText.trim().toLowerCase();

        const patientName =
          String(appointment.patient || '')
            .toLowerCase();

        const matchesSearch =
          normalizedSearchText === '' ||
          patientName.includes(normalizedSearchText);

        const matchesStatus =
          this.selectedStatus === '' ||
          appointment.status === this.selectedStatus;

        let matchesDate = true;

        if (this.selectedDateFilter === 'Upcoming') {
          matchesDate = appointmentDate >= today;
        }

        if (this.selectedDateFilter === 'Past') {
          matchesDate = appointmentDate < today;
        }

        if (this.selectedDateFilter === 'Custom') {
          matchesDate =
            this.customDate === '' ||
            appointment.rawDate === this.customDate;
        }

        return (
          matchesSearch &&
          matchesStatus &&
          matchesDate
        );
      }
    );
  }

  pagedAppointments(): any[] {
    const filteredAppointments =
      this.filteredAppointments();

    const startIndex =
      (this.currentPage - 1) * this.pageSize;

    const endIndex =
      startIndex + this.pageSize;

    return filteredAppointments.slice(
      startIndex,
      endIndex
    );
  }

  get totalPages(): number {
    const filteredCount =
      this.filteredAppointments().length;

    if (filteredCount === 0) {
      return 0;
    }

    return Math.ceil(
      filteredCount / this.pageSize
    );
  }

  get pageNumbers(): number[] {
    return Array.from(
      { length: this.totalPages },
      (_, index) => index + 1
    );
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.currentPage = page;
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  private adjustCurrentPage(): void {
    const availablePages = this.totalPages;

    if (availablePages === 0) {
      this.currentPage = 1;
      return;
    }

    if (this.currentPage > availablePages) {
      this.currentPage = availablePages;
    }

    if (this.currentPage < 1) {
      this.currentPage = 1;
    }
  }

  confirmAppointment(appointment: any): void {
    this.updateAppointmentStatus(
      appointment,
      1,
      ''
    );
  }

  completeAppointment(appointment: any): void {
    this.updateAppointmentStatus(
      appointment,
      2,
      ''
    );
  }

  cancelAppointment(appointment: any): void {
    this.selectedCancelAppointment = appointment;
    this.cancelReason = '';
    this.showCancelModal = true;
  }

  closeCancelModal(): void {
    this.showCancelModal = false;
    this.selectedCancelAppointment = null;
    this.cancelReason = '';
  }

  submitCancellation(): void {
    if (!this.cancelReason.trim()) {
      alert('Cancellation reason is required.');
      return;
    }

    this.updateAppointmentStatus(
      this.selectedCancelAppointment,
      3,
      this.cancelReason
    );

    this.closeCancelModal();
  }

  private updateAppointmentStatus(
    appointment: any,
    statusValue: number,
    cancellationReason: string
  ): void {

    if (!appointment) {
      alert('Please select an appointment.');
      return;
    }

    const headers = this.getHeaders();

    const payload = {
      status: statusValue,
      cancellationReason
    };

    console.log(
      'Status update payload:',
      {
        appointmentId:
          appointment.appointmentId,
        payload
      }
    );

    this.http.put<any>(
      `${this.appointmentsUrl}/${appointment.appointmentId}/status`,
      payload,
      { headers }
    ).subscribe({
      next: (response: any) => {
        console.log(
          'Appointment status updated:',
          response
        );

        alert(
          'Appointment status updated successfully.'
        );

        this.loadAppointments();
      },

      error: (error: any) => {
        console.error(
          'Status update failed:',
          error
        );

        alert(
          this.getErrorMessage(
            error,
            'Failed to update appointment status.'
          )
        );
      }
    });
  }

  viewRecord(appointment: any): void {
    if (!appointment.healthRecord) {
      alert(
        'Health record details are not available.'
      );

      return;
    }

    this.selectedHealthRecord = {
      patient: appointment.patient,
      date: appointment.date,
      time: appointment.time,
      diagnosis:
        appointment.healthRecord.diagnosis,
      prescription:
        appointment.healthRecord.prescription,
      notes:
        appointment.healthRecord.notes
    };

    this.showViewRecordModal = true;
    this.cdr.detectChanges();
  }

  closeViewRecordModal(): void {
    this.showViewRecordModal = false;
    this.selectedHealthRecord = null;
    this.cdr.detectChanges();
  }

  viewHistory(appointment: any): void {
    alert(
      `Patient history for ${appointment.patient} will be connected later.`
    );
  }

  openAddRecord(appointment: any): void {
    if (appointment.status !== 'Completed') {
      alert(
        'Health record can be added only after the appointment is completed.'
      );

      return;
    }

    if (appointment.hasRecord) {
      alert(
        'Health record already exists for this appointment.'
      );

      return;
    }

    this.selectedAppointment = appointment;

    this.recordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    this.showRecordModal = true;
    this.cdr.detectChanges();
  }

  closeRecordModal(): void {
    this.showRecordModal = false;
    this.selectedAppointment = null;
    this.isSavingRecord = false;

    this.recordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    this.cdr.detectChanges();
  }

  saveHealthRecord(): void {
    if (this.isSavingRecord) {
      return;
    }

    if (!this.selectedAppointment) {
      alert('Please select an appointment.');
      return;
    }

    if (
      this.selectedAppointment.status !==
      'Completed'
    ) {
      alert(
        'Health records can only be created for completed appointments.'
      );

      return;
    }

    if (!this.recordForm.diagnosis.trim()) {
      alert('Diagnosis is required.');
      return;
    }

    if (!this.recordForm.prescription.trim()) {
      alert('Prescription is required.');
      return;
    }

    const appointmentToUpdate =
      this.selectedAppointment;

    const headers = this.getHeaders();

    const payload = {
      appointmentId:
        appointmentToUpdate.appointmentId,

      diagnosis:
        this.recordForm.diagnosis.trim(),

      prescription:
        this.recordForm.prescription.trim(),

      notes:
        this.recordForm.notes.trim()
    };

    console.log(
      'Create health record payload:',
      payload
    );

    this.isSavingRecord = true;
    this.cdr.detectChanges();

    this.http.post<any>(
      this.healthRecordsUrl,
      payload,
      { headers }
    ).subscribe({
      next: (response: any) => {
        console.log(
          'Health record created:',
          response
        );

        this.zone.run(() => {
          const appointment =
            this.appointments.find(
              (currentAppointment: any) =>
                Number(
                  currentAppointment.appointmentId
                ) ===
                Number(
                  appointmentToUpdate.appointmentId
                )
            );

          if (appointment) {
            appointment.hasRecord = true;
            appointment.healthRecord = response;
          }

          this.showRecordModal = false;
          this.selectedAppointment = null;
          this.isSavingRecord = false;

          this.recordForm = {
            diagnosis: '',
            prescription: '',
            notes: ''
          };

          this.cdr.detectChanges();
        });

        setTimeout(() => {
          alert(
            'Health record added successfully.'
          );
        }, 0);
      },

      error: (error: any) => {
        console.error(
          'Health record creation failed:',
          error
        );

        this.zone.run(() => {
          this.isSavingRecord = false;
          this.cdr.detectChanges();
        });

        alert(
          this.getErrorMessage(
            error,
            'Failed to create health record.'
          )
        );
      }
    });
  }

  private getErrorMessage(
    error: any,
    fallbackMessage: string
  ): string {

    if (error?.error?.message) {
      return error.error.message;
    }

    if (error?.error?.title) {
      return error.error.title;
    }

    if (typeof error?.error === 'string') {
      return error.error;
    }

    if (error?.error?.errors) {
      return Object.values(
        error.error.errors
      )
        .flat()
        .join('\n');
    }

    return fallbackMessage;
  }

  private getStatusName(value: any): string {
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
}