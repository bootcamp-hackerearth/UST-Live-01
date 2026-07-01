import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { forkJoin } from 'rxjs';

import { DoctorDashboardService } from '../../core/services/doctor-dashboard.service';
import { TokenService } from '../../core/services/token.service';

import {
  CreateHealthRecordRequest,
  DoctorHealthRecord,
  DoctorPatientProfile,
  DoctorScheduleItem
} from '../../shared/models/doctor-dashboard.models';

import {
  AppointmentStatus,
  AppointmentTimeSlot
} from '../../shared/models/patient-dashboard.models';

@Component({
  selector: 'app-doctor-dashboard',
  imports: [
    DatePipe,
    FormsModule
  ],
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css'
})
export class DoctorDashboard implements OnInit {
  doctorId: number | null = null;

  loading = true;
  actionLoading = false;
  errorMessage = '';
  successMessage = '';

  schedule: DoctorScheduleItem[] = [];

  selectedAppointment?: DoctorScheduleItem;
  selectedPatient?: DoctorPatientProfile;
  selectedPatientRecords: DoctorHealthRecord[] = [];

  showPatientModal = false;
  loadingPatient = false;

  showHealthRecordPanel = false;
  savingHealthRecord = false;

  healthRecordForm = {
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  constructor(
    private tokenService: TokenService,
    private doctorDashboardService: DoctorDashboardService
  ) {}

  ngOnInit(): void {
    this.doctorId = this.tokenService.getReferenceId();

    if (!this.doctorId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    this.loadTodaySchedule();
  }

  loadTodaySchedule(): void {
    if (!this.doctorId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.doctorDashboardService.getTodaySchedule(this.doctorId).subscribe({
      next: (schedule) => {
        this.schedule = this.sortSchedule(schedule ?? []);
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view doctor dashboard. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load today’s schedule. Please try again.';
      }
    });
  }

  confirmAppointment(appointment: DoctorScheduleItem): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.actionLoading = true;

    this.doctorDashboardService.confirmAppointment(appointment.appointmentId).subscribe({
      next: () => {
        this.actionLoading = false;
        this.successMessage = 'Appointment confirmed successfully.';
        this.loadTodaySchedule();
      },
      error: (error) => {
        this.actionLoading = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to confirm this appointment.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not confirm appointment. Please try again.';
      }
    });
  }

  completeAppointment(appointment: DoctorScheduleItem): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.actionLoading = true;

    this.doctorDashboardService.completeAppointment(appointment.appointmentId).subscribe({
      next: () => {
        this.actionLoading = false;
        this.successMessage = 'Appointment marked as completed. You can now add a health record.';
        this.loadTodaySchedule();
      },
      error: (error) => {
        this.actionLoading = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to complete this appointment.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not complete appointment. Please try again.';
      }
    });
  }

  openPatientProfile(appointment: DoctorScheduleItem): void {
    this.selectedAppointment = appointment;
    this.selectedPatient = undefined;
    this.selectedPatientRecords = [];
    this.showPatientModal = true;
    this.loadingPatient = true;
    this.errorMessage = '';

    forkJoin({
      patient: this.doctorDashboardService.getPatientProfile(appointment.patientId),
      records: this.doctorDashboardService.getPatientHealthRecords(appointment.patientId)
    }).subscribe({
      next: ({ patient, records }) => {
        this.selectedPatient = patient;
        this.selectedPatientRecords = records ?? [];
        this.loadingPatient = false;
      },
      error: (error) => {
        this.loadingPatient = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view this patient profile.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load patient profile or health history.';
      }
    });
  }

  closePatientModal(): void {
    if (this.loadingPatient) {
      return;
    }

    this.showPatientModal = false;
    this.selectedAppointment = undefined;
    this.selectedPatient = undefined;
    this.selectedPatientRecords = [];
  }

  openHealthRecordPanel(appointment: DoctorScheduleItem): void {
    this.selectedAppointment = appointment;

    this.healthRecordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };

    this.errorMessage = '';
    this.successMessage = '';
    this.showHealthRecordPanel = true;
  }

  closeHealthRecordPanel(): void {
    if (this.savingHealthRecord) {
      return;
    }

    this.showHealthRecordPanel = false;

    this.healthRecordForm = {
      diagnosis: '',
      prescription: '',
      notes: ''
    };
  }

  submitHealthRecord(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.doctorId) {
      this.errorMessage = 'Unable to identify doctor account. Please login again.';
      return;
    }

    if (!this.selectedAppointment) {
      this.errorMessage = 'No appointment selected.';
      return;
    }

    if (this.selectedAppointment.status !== AppointmentStatus.Completed) {
      this.errorMessage = 'Health record can only be added after completing the appointment.';
      return;
    }

    if (!this.healthRecordForm.diagnosis.trim()) {
      this.errorMessage = 'Diagnosis is required.';
      return;
    }

    if (!this.healthRecordForm.prescription.trim()) {
      this.errorMessage = 'Prescription is required.';
      return;
    }

    const request: CreateHealthRecordRequest = {
      appointmentId: this.selectedAppointment.appointmentId,
      patientId: this.selectedAppointment.patientId,
      doctorId: this.doctorId,
      diagnosis: this.healthRecordForm.diagnosis.trim(),
      prescription: this.healthRecordForm.prescription.trim(),
      notes: this.healthRecordForm.notes.trim() || undefined
    };

    this.savingHealthRecord = true;

    this.doctorDashboardService.createHealthRecord(request).subscribe({
      next: () => {
        this.savingHealthRecord = false;
        this.showHealthRecordPanel = false;

        this.successMessage = 'Health record added successfully.';

        this.healthRecordForm = {
          diagnosis: '',
          prescription: '',
          notes: ''
        };

        this.selectedAppointment = undefined;
        this.loadTodaySchedule();
      },
      error: (error) => {
        this.savingHealthRecord = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to add health records.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not add health record. Please try again.';
      }
    });
  }

  get totalAppointments(): number {
    return this.schedule.length;
  }

  get pendingAppointments(): number {
    return this.schedule.filter(
      appointment => appointment.status === AppointmentStatus.Pending
    ).length;
  }

  get confirmedAppointments(): number {
    return this.schedule.filter(
      appointment => appointment.status === AppointmentStatus.Confirmed
    ).length;
  }

  get completedAppointments(): number {
    return this.schedule.filter(
      appointment => appointment.status === AppointmentStatus.Completed
    ).length;
  }

  get cancelledAppointments(): number {
    return this.schedule.filter(
      appointment => appointment.status === AppointmentStatus.Cancelled
    ).length;
  }

  canConfirmAppointment(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Pending;
  }

  canCompleteAppointment(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Confirmed;
  }

  canAddHealthRecord(appointment: DoctorScheduleItem): boolean {
    return appointment.status === AppointmentStatus.Completed;
  }

  statusText(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'Pending';

      case AppointmentStatus.Confirmed:
        return 'Confirmed';

      case AppointmentStatus.Cancelled:
        return 'Cancelled';

      case AppointmentStatus.Completed:
        return 'Completed';

      default:
        return 'Unknown';
    }
  }

  statusClass(status: number): string {
    switch (status) {
      case AppointmentStatus.Pending:
        return 'pending';

      case AppointmentStatus.Confirmed:
        return 'confirmed';

      case AppointmentStatus.Cancelled:
        return 'cancelled';

      case AppointmentStatus.Completed:
        return 'completed';

      default:
        return 'unknown';
    }
  }

  timeSlotText(timeSlot: number): string {
    switch (timeSlot) {
      case AppointmentTimeSlot.TenAM:
        return '10:00 AM - 10:30 AM';

      case AppointmentTimeSlot.TenThirtyAM:
        return '10:30 AM - 11:00 AM';

      case AppointmentTimeSlot.ElevenAM:
        return '11:00 AM - 11:30 AM';

      case AppointmentTimeSlot.ElevenThirtyAM:
        return '11:30 AM - 12:00 PM';

      case AppointmentTimeSlot.TwelvePM:
        return '12:00 PM - 12:30 PM';

      case AppointmentTimeSlot.TwelveThirtyPM:
        return '12:30 PM - 01:00 PM';

      case AppointmentTimeSlot.OnePM:
        return '01:00 PM - 01:30 PM';

      case AppointmentTimeSlot.OneThirtyPM:
        return '01:30 PM - 02:00 PM';

      case AppointmentTimeSlot.TwoPM:
        return '02:00 PM - 02:30 PM';

      case AppointmentTimeSlot.TwoThirtyPM:
        return '02:30 PM - 03:00 PM';

      case AppointmentTimeSlot.ThreePM:
        return '03:00 PM - 03:30 PM';

      case AppointmentTimeSlot.ThreeThirtyPM:
        return '03:30 PM - 04:00 PM';

      default:
        return `Slot ${timeSlot}`;
    }
  }

  genderText(value: number): string {
    switch (value) {
      case 1:
        return 'Male';

      case 2:
        return 'Female';

      case 3:
        return 'Non-binary';

      case 4:
        return 'Prefer not to say';

      default:
        return 'Not specified';
    }
  }

  specialisationText(value: number): string {
    switch (value) {
      case 1:
        return 'General Practitioner';

      case 2:
        return 'Cardiologist';

      case 3:
        return 'Dermatologist';

      case 4:
        return 'Neurologist';

      case 5:
        return 'Pediatrician';

      case 6:
        return 'Psychiatrist';

      case 7:
        return 'Orthopedic Surgeon';

      case 8:
        return 'Gynecologist';

      case 9:
        return 'Oncologist';

      case 10:
        return 'Endocrinologist';

      default:
        return 'Specialist';
    }
  }

  private sortSchedule(schedule: DoctorScheduleItem[]): DoctorScheduleItem[] {
    return [...schedule].sort((a, b) => {
      const dateA = new Date(a.scheduledDate).getTime();
      const dateB = new Date(b.scheduledDate).getTime();

      if (dateA !== dateB) {
        return dateA - dateB;
      }

      return a.timeSlot - b.timeSlot;
    });
  }
}

