import { CommonModule, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  PatientPortalService,
  PatientProfileDetails,
  UpdatePatientProfileRequest
} from '../../core/services/patient-portal.service';

import { AppointmentService } from '../../core/services/appointment.service';
import { HealthHistoryService } from '../../core/services/health-history.service';
import { TokenService } from '../../core/services/token.service';

import {
  AppointmentStatus,
  AppointmentTimeSlot
} from '../../shared/models/patient-dashboard.models';

@Component({
  selector: 'app-patient-profile',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    FormsModule
  ],
  templateUrl: './patient-profile.html',
  styleUrls: ['./patient-profile.css']
})
export class PatientProfile implements OnInit {
  patientId: number | null = null;

  loading = true;
  saving = false;

  errorMessage = '';
  successMessage = '';

  patient?: PatientProfileDetails;
  appointments: any[] = [];
  healthRecords: any[] = [];

  editMode = false;

  showSaveConfirmModal = false;
  pendingUpdateRequest?: UpdatePatientProfileRequest;

  editForm = {
    fullName: '',
    dateOfBirth: '',
    gender: 1,
    phoneNumber: '',
    insuranceNumber: ''
  };

  constructor(
    private tokenService: TokenService,
    private patientPortalService: PatientPortalService,
    private appointmentService: AppointmentService,
    private healthHistoryService: HealthHistoryService
  ) {}

  ngOnInit(): void {
    this.patientId = this.tokenService.getReferenceId();

    if (!this.patientId) {
      this.loading = false;
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      return;
    }

    this.loadProfileData();
  }

  loadProfileData(): void {
    if (!this.patientId) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientPortalService.getPatientProfile(this.patientId).subscribe({
      next: (patient: PatientProfileDetails) => {
        this.patient = patient;
        this.populateEditForm(patient);
        this.loadAppointmentsAndHealthRecords();
      },
      error: (error: any) => {
        this.loading = false;

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view this profile.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load patient profile.';
      }
    });
  }

  loadAppointmentsAndHealthRecords(): void {
    if (!this.patientId) {
      return;
    }

    this.appointmentService.getPatientAppointments(this.patientId).subscribe({
      next: (appointments: any[]) => {
        this.appointments = appointments ?? [];

        this.healthHistoryService.getPatientHealthRecords(this.patientId!).subscribe({
          next: (records: any[]) => {
            this.healthRecords = records ?? [];
            this.loading = false;
          },
          error: () => {
            this.healthRecords = [];
            this.loading = false;
          }
        });
      },
      error: () => {
        this.appointments = [];
        this.healthRecords = [];
        this.loading = false;
      }
    });
  }

  populateEditForm(patient: PatientProfileDetails): void {
    this.editForm = {
      fullName: patient.fullName,
      dateOfBirth: patient.dateOfBirth,
      gender: Number(patient.gender),
      phoneNumber: patient.phoneNumber,
      insuranceNumber: patient.insuranceNumber ?? patient.insuranceId ?? ''
    };
  }

  enableEdit(): void {
    if (!this.patient) {
      return;
    }

    this.populateEditForm(this.patient);
    this.editMode = true;
    this.errorMessage = '';
    this.successMessage = '';
  }

  cancelEdit(): void {
    this.editMode = false;
    this.errorMessage = '';
    this.successMessage = '';
    this.showSaveConfirmModal = false;
    this.pendingUpdateRequest = undefined;

    if (this.patient) {
      this.populateEditForm(this.patient);
    }
  }

  saveProfile(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.patient || !this.patientId) {
      this.errorMessage = 'Patient profile is not available.';
      return;
    }

    if (!this.editForm.fullName.trim()) {
      this.errorMessage = 'Full name is required.';
      return;
    }

    if (!this.isValidFullName(this.editForm.fullName)) {
      this.errorMessage = 'Full name can contain only letters and spaces.';
      return;
    }

    if (!this.editForm.dateOfBirth) {
      this.errorMessage = 'Date of birth is required.';
      return;
    }

    if (!this.editForm.phoneNumber.trim()) {
      this.errorMessage = 'Phone number is required.';
      return;
    }

    this.pendingUpdateRequest = {
      fullName: this.editForm.fullName.trim(),
      dateOfBirth: this.editForm.dateOfBirth,
      gender: Number(this.editForm.gender),
      phoneNumber: this.editForm.phoneNumber.trim(),
      email: this.patient.email,
      insuranceNumber: this.editForm.insuranceNumber.trim() || undefined
    };

    this.showSaveConfirmModal = true;
  }

  cancelSaveConfirmation(): void {
    this.showSaveConfirmModal = false;
    this.pendingUpdateRequest = undefined;
  }

  confirmSaveProfile(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.patientId || !this.pendingUpdateRequest) {
      this.errorMessage = 'Profile update details are not available.';
      this.showSaveConfirmModal = false;
      return;
    }

    this.saving = true;
    this.showSaveConfirmModal = false;

    this.patientPortalService.updatePatientProfile(
      this.patientId,
      this.pendingUpdateRequest
    ).subscribe({
      next: () => {
        this.saving = false;
        this.editMode = false;
        this.pendingUpdateRequest = undefined;
        this.successMessage = 'Profile updated successfully.';
        this.loadProfileData();
      },
      error: (error: any) => {
        this.saving = false;
        this.pendingUpdateRequest = undefined;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to update this profile.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not update profile. Please try again.';
      }
    });
  }

  isValidFullName(fullName: string): boolean {
    return /^[A-Za-z ]+$/.test(fullName.trim());
  }

  get totalAppointments(): number {
    return this.appointments.length;
  }

  get activeAppointments(): number {
    return this.appointments.filter(appointment =>
      Number(appointment.status) === AppointmentStatus.Pending ||
      Number(appointment.status) === AppointmentStatus.Confirmed
    ).length;
  }

  get completedAppointments(): number {
    return this.appointments.filter(
      appointment => Number(appointment.status) === AppointmentStatus.Completed
    ).length;
  }

  get cancelledAppointments(): number {
    return this.appointments.filter(
      appointment => Number(appointment.status) === AppointmentStatus.Cancelled
    ).length;
  }

  get totalHealthRecords(): number {
    return this.healthRecords.length;
  }

  get mostVisitedDoctor(): string {
    if (!this.appointments.length) {
      return 'Not available';
    }

    const doctorVisitCount = new Map<string, number>();

    this.appointments.forEach(appointment => {
      if (!appointment.doctorName) {
        return;
      }

      doctorVisitCount.set(
        appointment.doctorName,
        (doctorVisitCount.get(appointment.doctorName) ?? 0) + 1
      );
    });

    return [...doctorVisitCount.entries()]
      .sort((a, b) => b[1] - a[1])[0]?.[0] ?? 'Not available';
  }

  get lastAppointment(): any | undefined {
    if (!this.appointments.length) {
      return undefined;
    }

    return [...this.appointments]
      .sort((a, b) => {
        const dateA = new Date(a.scheduledDate).getTime();
        const dateB = new Date(b.scheduledDate).getTime();

        if (dateB !== dateA) {
          return dateB - dateA;
        }

        return Number(b.timeSlot) - Number(a.timeSlot);
      })[0];
  }

  get nextAppointment(): any | undefined {
    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return [...this.appointments]
      .filter(appointment =>
        new Date(appointment.scheduledDate).getTime() >= today.getTime() &&
        (
          Number(appointment.status) === AppointmentStatus.Pending ||
          Number(appointment.status) === AppointmentStatus.Confirmed
        )
      )
      .sort((a, b) => {
        const dateA = new Date(a.scheduledDate).getTime();
        const dateB = new Date(b.scheduledDate).getTime();

        if (dateA !== dateB) {
          return dateA - dateB;
        }

        return Number(a.timeSlot) - Number(b.timeSlot);
      })[0];
  }

  genderText(value: number): string {
    switch (Number(value)) {
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

  statusText(status: number): string {
    switch (Number(status)) {
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

  timeSlotText(timeSlot: number): string {
    switch (Number(timeSlot)) {
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
}
