import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Sidebar } from '../shared/d_sidebar/d_sidebar';
import { AppointmentService } from '../../Doctor.service/appointmentservice';
import { HealthRecordService } from '../../Doctor.service/health-recordservice';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './d_appointments.html',
  styleUrls: ['./d_appointments.css']
})
export class DoctorAppointments implements OnInit {

  appointments = signal<any[]>([]);
  filteredAppointments = signal<any[]>([]);
  paginatedAppointments = signal<any[]>([]);

  errorMessage = signal('');
  successMessage = signal('');

  selected = signal<any>(null);

  totalCount = signal(0);
  pendingCount = signal(0);
  confirmedCount = signal(0);
  completedCount = signal(0);

  pageNumber = signal(1);
  totalPages = signal(1);

  readonly pageSize = 5;

  showRecordForm = signal(false);
  selectedAppointmentForRecord = signal<any>(null);
  isSavingRecord = signal(false);

  healthRecordForm = {
    appointmentId: null as number | null,
    patientId: null as number | null,
    doctorId: null as number | null,
    patientName: '',
    doctorName: '',
    visitDate: '',
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  constructor(
    private readonly appointmentService: AppointmentService,
    private readonly healthRecordService: HealthRecordService
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  private loadAppointments(): void {
    this.errorMessage.set('');

    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (res) => {
        const appointments = this.formatAppointments(res);

        this.appointments.set(appointments);
        this.filteredAppointments.set(appointments);

        this.pageNumber.set(1);
        this.updateCounts(appointments);
        this.updatePagination();
      },
      error: (err) => {
        this.errorMessage.set(
          err?.error?.message ?? 'Failed to load doctor appointments'
        );
      }
    });
  }

  private formatAppointments(res: any): any[] {
    const list = res?.data ?? res ?? [];

    return list.map((appointment: any) => ({
      ...appointment,
      scheduledDate: appointment.scheduledDate
        ? new Date(appointment.scheduledDate)
        : null
    }));
  }

  private updateCounts(appointments: any[]): void {
    this.totalCount.set(appointments.length);

    const statuses = appointments.reduce(
      (acc, appointment) => {
        const status = appointment.status || 'Unknown';
        acc[status] = (acc[status] || 0) + 1;
        return acc;
      },
      {} as Record<string, number>
    );

    this.pendingCount.set(statuses['Pending'] || 0);
    this.confirmedCount.set(statuses['Confirmed'] || 0);
    this.completedCount.set(statuses['Completed'] || 0);
  }

  private updatePagination(): void {
    const pages = Math.ceil(this.filteredAppointments().length / this.pageSize);

    this.totalPages.set(pages <= 0 ? 1 : pages);

    if (this.pageNumber() > this.totalPages()) {
      this.pageNumber.set(this.totalPages());
    }

    this.paginate();
  }

  private paginate(): void {
    const start = (this.pageNumber() - 1) * this.pageSize;

    this.paginatedAppointments.set(
      this.filteredAppointments().slice(start, start + this.pageSize)
    );
  }

  changePage(offset: number): void {
    const nextPage = this.pageNumber() + offset;

    if (nextPage >= 1 && nextPage <= this.totalPages()) {
      this.pageNumber.set(nextPage);
      this.paginate();
    }
  }

  prevPage(): void {
    this.changePage(-1);
  }

  nextPage(): void {
    this.changePage(1);
  }

  view(data: any): void {
    this.selected.set(data);
  }

  closeView(): void {
    this.selected.set(null);
  }

  confirm(id: number): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    this.appointmentService.confirmAppointment(id).subscribe({
      next: () => {
        this.successMessage.set('Appointment confirmed successfully.');
        this.loadAppointments();
      },
      error: (err) => {
        this.errorMessage.set(
          err?.error?.message ?? 'Failed to confirm appointment'
        );
      }
    });
  }

  /**
   * For confirmed appointment:
   * Open record form first.
   * Appointment becomes Completed only after record saves successfully.
   */
  openRecordForm(appointment: any): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    const patientId =
      appointment.patientId ??
      appointment.patient?.patientId ??
      null;

    const doctorId =
      appointment.doctorId ??
      appointment.doctor?.doctorId ??
      null;

    this.selectedAppointmentForRecord.set(appointment);

    this.healthRecordForm = {
      appointmentId: appointment.appointmentId ?? null,
      patientId,
      doctorId,
      patientName: appointment.patientName ?? appointment.patient?.fullName ?? '',
      doctorName: appointment.doctorName ?? appointment.doctor?.fullName ?? '',
      visitDate: this.formatDateForInput(appointment.scheduledDate),
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    console.log('HEALTH RECORD FORM:', this.healthRecordForm);

    this.showRecordForm.set(true);
  }

  closeRecordForm(): void {
    if (this.isSavingRecord()) return;

    this.showRecordForm.set(false);
    this.selectedAppointmentForRecord.set(null);
    this.resetHealthRecordForm();
  }

  saveHealthRecord(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    const appointment = this.selectedAppointmentForRecord();

    if (!appointment) {
      this.errorMessage.set('Appointment details are missing.');
      return;
    }

    if (!this.healthRecordForm.patientId) {
      this.errorMessage.set('Patient ID is missing from appointment response.');
      console.error('Missing patientId in appointment:', appointment);
      return;
    }

    if (!this.healthRecordForm.doctorId) {
      this.errorMessage.set('Doctor ID is missing from appointment response.');
      console.error('Missing doctorId in appointment:', appointment);
      return;
    }

    if (!this.healthRecordForm.diagnosis.trim()) {
      this.errorMessage.set('Diagnosis is required.');
      return;
    }

    const payload = {
      patientId: Number(this.healthRecordForm.patientId),
      doctorId: Number(this.healthRecordForm.doctorId),
      patientName: this.healthRecordForm.patientName,
      doctorName: this.healthRecordForm.doctorName,
      visitDate: this.healthRecordForm.visitDate
        ? new Date(this.healthRecordForm.visitDate)
        : new Date(),
      diagnosis: this.healthRecordForm.diagnosis.trim(),
      prescription: this.healthRecordForm.prescription.trim(),
      notes: this.healthRecordForm.notes.trim()
    };

    console.log('CREATE HEALTH RECORD PAYLOAD:', payload);

    this.isSavingRecord.set(true);

    this.healthRecordService.createRecord(payload).subscribe({
      next: () => {
        this.completeAppointmentAfterRecordSaved(appointment.appointmentId);
      },
      error: (err) => {
        this.isSavingRecord.set(false);

        console.error('Create health record failed:', err);
        console.error('Backend validation:', err.error);

        const message = this.getErrorMessage(
          err,
          'Failed to create health record'
        );

        this.errorMessage.set(message);
      }
    });
  }

  private completeAppointmentAfterRecordSaved(appointmentId: number): void {
    this.appointmentService.completeAppointment(appointmentId).subscribe({
      next: () => {
        this.isSavingRecord.set(false);

        this.successMessage.set(
          'Health record created and appointment completed successfully.'
        );

        this.closeRecordForm();
        this.loadAppointments();
      },
      error: (err) => {
        this.isSavingRecord.set(false);

        console.error('Complete appointment failed after record save:', err);

        this.errorMessage.set(
          err?.error?.message ??
          'Health record saved, but appointment completion failed.'
        );
      }
    });
  }

  private resetHealthRecordForm(): void {
    this.healthRecordForm = {
      appointmentId: null,
      patientId: null,
      doctorId: null,
      patientName: '',
      doctorName: '',
      visitDate: '',
      diagnosis: '',
      prescription: '',
      notes: ''
    };
  }

  private formatDateForInput(dateValue: any): string {
    if (!dateValue) return '';

    const date = new Date(dateValue);

    if (Number.isNaN(date.getTime())) return '';

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private getErrorMessage(err: any, fallback: string): string {
    const errors = err?.error?.errors;

    if (errors) {
      const firstKey = Object.keys(errors)[0];

      if (firstKey && errors[firstKey]?.length) {
        return errors[firstKey][0];
      }
    }

    if (err?.error?.message) {
      return err.error.message;
    }

    if (typeof err?.error === 'string') {
      return err.error;
    }

    return fallback;
  }
}