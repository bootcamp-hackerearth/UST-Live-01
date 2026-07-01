import { Component, OnInit, inject, ChangeDetectorRef, NgZone } from '@angular/core';
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

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);
  private zone = inject(NgZone);

  private doctorsUrl = 'https://localhost:7130/api/doctors';
  private appointmentsUrl = 'https://localhost:7130/api/appointments';
  private patientsUrl = 'https://localhost:7130/api/patients';
  private healthRecordsUrl = 'https://localhost:7130/api/health-records';

  searchText = '';
  selectedStatus = '';
  selectedDateFilter = 'All';
  customDate = '';

  appointments: any[] = [];
  patients: any[] = [];

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

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadAppointments();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadAppointments() {
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
        console.log('Doctor me ✅:', result.doctor);
        console.log('Appointments API ✅:', result.appointments);
        console.log('Doctor patients API ✅:', result.patients);

        const doctorId = Number(result.doctor.doctorId);
        localStorage.setItem('doctorId', String(doctorId));

        this.patients = result.patients.items || result.patients.data || [];

        const mappedAppointments = (result.appointments || [])
          .filter((a: any) => Number(a.doctorId) === doctorId)
          .map((a: any) => {
            const patient = this.patients.find(
              (p: any) => Number(p.patientId) === Number(a.patientId)
            );

            return {
              id: a.appointmentId,
              appointmentId: a.appointmentId,
              patientId: a.patientId,
              doctorId: a.doctorId,
              rawDate: this.toInputDate(a.scheduledDate),
              date: this.toDisplayDate(a.scheduledDate),
              time: a.timeSlot,
              patient: patient ? patient.fullName : `Patient #${a.patientId}`,
              status: this.getStatusName(a.status),
              cancellationReason: a.cancellationReason || '',
              hasRecord: false,
              healthRecord: null
            };
          });

        this.loadHealthRecordFlags(mappedAppointments, headers);
      },
      error: (err: any) => {
        console.error('Doctor appointments load failed ❌:', err);
        alert('Failed to load doctor appointments.');
      }
    });
  }

  private loadHealthRecordFlags(mappedAppointments: any[], headers: HttpHeaders) {
    const patientIds: number[] = Array.from(
      new Set<number>(
        mappedAppointments.map((a: any) => Number(a.patientId))
      )
    );

    if (patientIds.length === 0) {
      this.appointments = mappedAppointments;
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

        this.appointments = mappedAppointments.map((appointment: any) => {
          const record = allRecords.find(
            (r: any) => Number(r.appointmentId) === Number(appointment.appointmentId)
          );

          return {
            ...appointment,
            hasRecord: !!record,
            healthRecord: record || null
          };
        });

        console.log('Mapped doctor appointments with records ✅:', this.appointments);
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Health record flag load failed ❌:', err);

        this.appointments = mappedAppointments;
        this.cdr.detectChanges();
      }
    });
  }

  clearFilters() {
    this.searchText = '';
    this.selectedStatus = '';
    this.selectedDateFilter = 'All';
    this.customDate = '';
  }

  filteredAppointments() {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return this.appointments.filter((a: any) => {
      const appointmentDate = new Date(a.rawDate);
      appointmentDate.setHours(0, 0, 0, 0);

      const matchesSearch =
        !this.searchText ||
        a.patient.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesStatus =
        this.selectedStatus === '' ||
        a.status === this.selectedStatus;

      let matchesDate = true;

      if (this.selectedDateFilter === 'Upcoming') {
        matchesDate = appointmentDate >= today;
      }

      if (this.selectedDateFilter === 'Past') {
        matchesDate = appointmentDate < today;
      }

      if (this.selectedDateFilter === 'Custom') {
        matchesDate = !this.customDate || a.rawDate === this.customDate;
      }

      return matchesSearch && matchesStatus && matchesDate;
    });
  }

  confirmAppointment(appointment: any) {
    this.updateAppointmentStatus(appointment, 1, '');
  }

  completeAppointment(appointment: any) {
    this.updateAppointmentStatus(appointment, 2, '');
  }

  cancelAppointment(appointment: any) {
    this.selectedCancelAppointment = appointment;
    this.cancelReason = '';
    this.showCancelModal = true;
  }

  closeCancelModal() {
    this.showCancelModal = false;
    this.selectedCancelAppointment = null;
    this.cancelReason = '';
  }

  submitCancellation() {
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
  ) {
    if (!appointment) {
      alert('Please select an appointment.');
      return;
    }

    const headers = this.getHeaders();

    const payload = {
      status: statusValue,
      cancellationReason: cancellationReason
    };

    console.log('Status update payload ✅:', {
      appointmentId: appointment.appointmentId,
      payload
    });

    this.http.put<any>(
      `${this.appointmentsUrl}/${appointment.appointmentId}/status`,
      payload,
      { headers }
    ).subscribe({
      next: (res: any) => {
        console.log('Appointment status updated ✅:', res);

        alert('Appointment status updated successfully ✅');
        this.loadAppointments();
      },
      error: (err: any) => {
        console.error('Status update failed ❌:', err);
        alert(this.getErrorMessage(err, 'Failed to update appointment status.'));
      }
    });
  }

  viewRecord(appointment: any) {
    if (!appointment.healthRecord) {
      alert('Health record details not available.');
      return;
    }

    this.selectedHealthRecord = {
      patient: appointment.patient,
      date: appointment.date,
      time: appointment.time,
      diagnosis: appointment.healthRecord.diagnosis,
      prescription: appointment.healthRecord.prescription,
      notes: appointment.healthRecord.notes
    };

    this.showViewRecordModal = true;
    this.cdr.detectChanges();
  }

  closeViewRecordModal() {
    this.showViewRecordModal = false;
    this.selectedHealthRecord = null;
    this.cdr.detectChanges();
  }

  viewHistory(appointment: any) {
    alert(`Patient history for ${appointment.patient} will be connected later.`);
  }

  openAddRecord(appointment: any) {
    if (appointment.status !== 'Completed') {
      alert('Health record can be added only after appointment is completed.');
      return;
    }

    if (appointment.hasRecord) {
      alert('Health record already exists for this appointment.');
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

  closeRecordModal() {
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

  saveHealthRecord() {
    if (this.isSavingRecord) {
      return;
    }

    if (!this.selectedAppointment) {
      alert('Please select an appointment.');
      return;
    }

    if (this.selectedAppointment.status !== 'Completed') {
      alert('Health records can only be created for completed appointments.');
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

    const appointmentToUpdate = this.selectedAppointment;

    const headers = this.getHeaders();

    const payload = {
      appointmentId: appointmentToUpdate.appointmentId,
      diagnosis: this.recordForm.diagnosis,
      prescription: this.recordForm.prescription,
      notes: this.recordForm.notes
    };

    console.log('Create health record payload ✅:', payload);

    this.isSavingRecord = true;
    this.cdr.detectChanges();

    this.http.post<any>(
      this.healthRecordsUrl,
      payload,
      { headers }
    ).subscribe({
      next: (res: any) => {
        console.log('Health record created ✅:', res);

        this.zone.run(() => {
          const appointment = this.appointments.find(
            (a: any) => Number(a.appointmentId) === Number(appointmentToUpdate.appointmentId)
          );

          if (appointment) {
            appointment.hasRecord = true;
            appointment.healthRecord = res;
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
          alert('Health record added successfully ✅');
        }, 0);
      },
      error: (err: any) => {
        console.error('Health record create failed ❌:', err);

        this.zone.run(() => {
          this.isSavingRecord = false;
          this.cdr.detectChanges();
        });

        alert(this.getErrorMessage(err, 'Failed to create health record.'));
      }
    });
  }

  private getErrorMessage(err: any, fallbackMessage: string): string {
    if (err?.error?.message) {
      return err.error.message;
    }

    if (err?.error?.title) {
      return err.error.title;
    }

    if (typeof err?.error === 'string') {
      return err.error;
    }

    if (err?.error?.errors) {
      return Object.values(err.error.errors).flat().join('\n');
    }

    return fallbackMessage;
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