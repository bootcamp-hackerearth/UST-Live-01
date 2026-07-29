import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DoctorSidebarComponent } from '../../shared/doctor-sidebar/doctor-sidebar';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    DoctorSidebarComponent
  ],
  templateUrl: './doctor-appointments.component.html',
  styleUrls: ['./doctor-appointments.component.css']
})
export class DoctorAppointmentsComponent implements OnInit {

  appointments: any[] = [];
  loading = true;

  showAddModal = false;
  showViewModal = false;
  showCancelModal = false;
  showSuccessModal = false;

  cancelSubmitted = false;

  selectedAppointmentDate: Date | null = null;

  selectedAppointmentId = 0;
  selectedPatientRecords: any[] = [];

  cancellationReason = '';
  today = new Date();

  submitted = false;

  recordForm: FormGroup;

  constructor(
    private readonly http: HttpClient,
    private readonly cd: ChangeDetectorRef,
    private readonly fb: FormBuilder
  ) {

    this.recordForm = this.fb.group({

      diagnosis: [
        '',
        [
          Validators.required,
          Validators.maxLength(500),
          Validators.pattern('^[A-Za-z0-9 ]+$')
        ]
      ],

      prescription: [
        '',
        [
          Validators.required,
          Validators.maxLength(500),
          Validators.pattern('^[A-Za-z0-9 ]+$')
        ]
      ],

      notes: [
        '',
        [
          Validators.maxLength(1000),
          Validators.pattern('^[A-Za-z0-9 ]*$')
        ]
      ]

    });
  }

  get f() {
    return this.recordForm.controls;
  }

  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments() {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<any[]>(
      '/api/appointments/doctor/upcoming',
      { headers }
    ).subscribe({

      next: (res) => {

        console.log('Doctor Appointments:', res);

        this.appointments = [...res];

        this.loading = false;

        this.cd.detectChanges();
      },

      error: (err) => {

        console.error(err);

        this.loading = false;
      }

    });
  }

  updateStatus(id: number, status: string) {

    if (status === 'Cancelled') {

      this.openCancelModal(id);

      return;
    }

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.put(
      `/api/appointments/${id}/status`,
      {
        status: 'Confirmed'
      },
      { headers }
    ).subscribe({

      next: () => {
        this.getAppointments();
      },

      error: (err) => {
        console.error(err);
      }

    });
  }

  openAddRecord(id: number) {

    const appointment = this.appointments.find(
      a => a.appointmentId === id
    );

    this.selectedAppointmentId = id;

    this.selectedAppointmentDate =
      appointment?.scheduledDate
        ? new Date(appointment.scheduledDate)
        : null;

    this.recordForm.reset();

    this.submitted = false;

    this.showAddModal = true;
  }

  saveRecord() {

    this.submitted = true;

    if (this.recordForm.invalid) {
      return;
    }

    const token = localStorage.getItem('token');

    const headers = {
      Authorization: `Bearer ${token}`
    };

    const body = {
      appointmentId: this.selectedAppointmentId,
      diagnosis: this.recordForm.value.diagnosis,
      prescription: this.recordForm.value.prescription,
      notes: this.recordForm.value.notes
    };

    this.http.post(
      '/api/records/create',
      body,
      { headers }
    ).subscribe({

      next: () => {

        this.showAddModal = false;

        this.showSuccessModal = true;

        this.recordForm.reset();

        this.submitted = false;

        this.getAppointments();
      },

      error: (err) => {

        console.error('Create Record Error:', err);
      }

    });
  }

  openCancelModal(id: number) {

    this.selectedAppointmentId = id;

    this.cancellationReason = '';

    this.cancelSubmitted = false;

    this.showCancelModal = true;
  }

  closeCancelModal() {

    this.showCancelModal = false;

    this.cancellationReason = '';

    this.cancelSubmitted = false;
  }

  submitCancellation() {

    this.cancelSubmitted = true;

    if (!this.cancellationReason.trim()) {
      return;
    }

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    const body = {
      status: 'Cancelled',
      cancellationReason: this.cancellationReason
    };

    this.http.put(
      `/api/appointments/${this.selectedAppointmentId}/status`,
      body,
      { headers }
    ).subscribe({

      next: () => {

        this.showCancelModal = false;

        this.cancellationReason = '';

        this.cancelSubmitted = false;

        this.getAppointments();
      },

      error: (err) => {
        console.error(err);
      }

    });
  }

  openView(patientId: number) {

    const token = localStorage.getItem('token');

    const headers = {
      Authorization: `Bearer ${token}`
    };

    this.http.get<any[]>(
      `/api/records/by-patient/${patientId}`,
      { headers }
    ).subscribe({

      next: (res) => {

        this.selectedPatientRecords = res;

        this.showViewModal = true;
      },

      error: (err) => {

        console.error(err);
      }

    });
  }

  closeModal() {

    this.showAddModal = false;

    this.showViewModal = false;

    this.selectedPatientRecords = [];

    this.recordForm.reset();

    this.submitted = false;
  }
}
