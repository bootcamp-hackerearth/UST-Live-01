import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  OnInit,
  Output
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { timeout } from 'rxjs';

import { BookAppointmentDto } from '../../../../../shared/models/appointment.models';
import { DoctorDto } from '../../../../../shared/models/doctor.models';

import { PatientApiService } from '../../../../../core/services/patient-api.service';
import { DoctorApiService } from '../../../../../core/services/doctor-api.service';
import { AppointmentApiService } from '../../../../../core/services/appointment-api.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-book-appointment.html',
  styleUrl: './patient-book-appointment.css'
})
export class PatientBookAppointment implements OnInit {
  currentPatientId = 0;

  specialisations: string[] = [];
  doctors: DoctorDto[] = [];
  filteredDoctors: DoctorDto[] = [];
  timeSlots: string[] = [];

  selectedSpecialisation = '';
  specialisationSearchTerm = '';
  isSpecialisationDropdownOpen = false;

  todayDate = '';
  maxBookingDate = '';

  message = '';
  isLoadingPatient = false;
  isLoadingDoctors = false;
  isLoadingSlots = false;
  isSubmitting = false;
  isBookingConfirmOpen = false;

  form: BookAppointmentDto = {
    patientId: 0,
    doctorId: null!,
    scheduledDate: '',
    timeSlot: ''
  };

  @Output() bookingSuccess = new EventEmitter<void>();

  constructor(
    private patientApiService: PatientApiService,
    private doctorApiService: DoctorApiService,
    private appointmentApiService: AppointmentApiService,
    private cdr: ChangeDetectorRef
  ) {
    this.todayDate = new Date().toISOString().split('T')[0];
    this.maxBookingDate = this.getDateAfterDays(30);
  }

  ngOnInit(): void {
    this.loadCurrentPatient();
    this.loadDoctors();
  }

  get filteredSpecialisations(): string[] {
    const searchTerm = this.specialisationSearchTerm.trim().toLowerCase();

    if (!searchTerm) {
      return this.specialisations;
    }

    return this.specialisations.filter((specialisation: string) =>
      specialisation.toLowerCase().includes(searchTerm)
    );
  }

  get visibleSpecialisations(): string[] {
    return this.filteredSpecialisations.slice(0, 6);
  }

  get selectedDoctor(): DoctorDto | undefined {
    return this.doctors.find(
      (doctor: DoctorDto) => doctor.doctorId === this.form.doctorId
    );
  }

  loadCurrentPatient(): void {
    this.isLoadingPatient = true;
    this.message = '';
    this.cdr.detectChanges();

    this.patientApiService.getMyProfile().pipe(
      timeout(15000)
    ).subscribe({
      next: (patient) => {
        this.currentPatientId = patient.patientId;
        this.form.patientId = patient.patientId;

        this.isLoadingPatient = false;
        this.cdr.detectChanges();
      },
      error: (error: unknown) => {
        console.log('Current patient API error:', error);

        this.currentPatientId = 0;
        this.form.patientId = 0;
        this.isLoadingPatient = false;
        this.message = 'Unable to load patient profile for booking. Please reload and try again.';

        this.cdr.detectChanges();
      }
    });
  }

  loadDoctors(): void {
    this.isLoadingDoctors = true;
    this.message = '';
    this.cdr.detectChanges();

    this.doctorApiService.getAllActiveDoctors().pipe(
      timeout(15000)
    ).subscribe({
      next: (doctors: DoctorDto[]) => {
        this.doctors = doctors ?? [];

        this.specialisations = Array.from(
          new Set(
            this.doctors
              .filter((doctor: DoctorDto) => doctor.isActive)
              .map((doctor: DoctorDto) => doctor.specialisation)
          )
        ).sort();

        if (this.doctors.length === 0) {
          this.message = 'No active doctors are available for booking right now.';
        }

        this.isLoadingDoctors = false;
        this.cdr.detectChanges();
      },
      error: (error: unknown) => {
        console.log('Active doctors API error:', error);

        this.doctors = [];
        this.specialisations = [];
        this.filteredDoctors = [];
        this.isLoadingDoctors = false;
        this.message = this.getErrorMessage(error);

        this.cdr.detectChanges();
      }
    });
  }

  openSpecialisationDropdown(): void {
    if (this.isLoadingDoctors) {
      return;
    }

    this.isSpecialisationDropdownOpen = true;
    this.scrollDownForSpecialisationSearch();
  }

  closeSpecialisationDropdown(): void {
    setTimeout(() => {
      this.isSpecialisationDropdownOpen = false;
      this.cdr.detectChanges();
    }, 150);
  }

  selectSpecialisation(specialisation: string): void {
    this.selectedSpecialisation = specialisation;
    this.specialisationSearchTerm = specialisation;

    this.filteredDoctors = this.doctors.filter(
      (doctor: DoctorDto) =>
        doctor.specialisation === specialisation &&
        doctor.isActive
    );

    this.isSpecialisationDropdownOpen = false;

    this.form.doctorId = null!;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.timeSlots = [];
    this.message = '';

    this.scrollToSection('doctor-section');
    this.cdr.detectChanges();
  }

  clearSpecialisation(): void {
    this.selectedSpecialisation = '';
    this.specialisationSearchTerm = '';
    this.filteredDoctors = [];
    this.isSpecialisationDropdownOpen = false;

    this.form.doctorId = null!;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.timeSlots = [];
    this.message = '';

    this.cdr.detectChanges();
  }

  selectDoctor(doctorId: number): void {
    this.form.doctorId = doctorId;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.timeSlots = [];
    this.message = '';

    this.loadDoctorAvailability(doctorId);
    this.scrollToSection('date-section');
    this.cdr.detectChanges();
  }

  loadDoctorAvailability(doctorId: number): void {
    this.isLoadingSlots = true;
    this.message = '';
    this.cdr.detectChanges();

    this.doctorApiService.getDoctorAvailability(doctorId).pipe(
      timeout(15000)
    ).subscribe({
      next: (slots: string[]) => {
        this.timeSlots = slots ?? [];

        if (this.timeSlots.length === 0) {
          this.message = 'No time slots are available for the selected doctor.';
        }

        this.isLoadingSlots = false;
        this.cdr.detectChanges();
      },
      error: (error: unknown) => {
        console.log('Doctor availability API error:', error);

        this.timeSlots = [];
        this.isLoadingSlots = false;
        this.message = this.getErrorMessage(error);

        this.cdr.detectChanges();
      }
    });
  }

  onDateChange(): void {
    this.form.timeSlot = '';
    this.message = '';

    if (this.form.scheduledDate) {
      this.scrollToSection('slot-section');
    }

    this.cdr.detectChanges();
  }

  selectTimeSlot(slot: string): void {
    if (this.isTimeSlotDisabled(slot)) {
      return;
    }

    this.form.timeSlot = slot;
    this.message = '';

    this.scrollToSection('submit-section');
    this.cdr.detectChanges();
  }

  isTimeSlotBooked(slot: string): boolean {
    return false;
  }

  isTimeSlotDisabled(slot: string): boolean {
    return (
      !this.form.doctorId ||
      !this.form.scheduledDate ||
      this.isSubmitting ||
      this.isLoadingSlots ||
      this.isTimeSlotBooked(slot)
    );
  }

  getSlotClass(slot: string): string {
    if (this.isTimeSlotBooked(slot)) {
      return 'ba-slot booked';
    }

    if (this.form.timeSlot === slot) {
      return 'ba-slot selected';
    }

    if (
      !this.form.doctorId ||
      !this.form.scheduledDate ||
      this.isLoadingSlots
    ) {
      return 'ba-slot disabled';
    }

    return 'ba-slot available';
  }

  getSlotStatus(slot: string): string {
    if (this.isTimeSlotBooked(slot)) {
      return 'Booked';
    }

    if (this.form.timeSlot === slot) {
      return 'Selected';
    }

    return 'Available';
  }

  submit(): void {
    this.message = '';

    if (!this.isBookingFormValid()) {
      return;
    }

    this.isBookingConfirmOpen = true;
    this.cdr.detectChanges();
  }

  closeBookingConfirm(): void {
    if (this.isSubmitting) {
      return;
    }

    this.isBookingConfirmOpen = false;
    this.cdr.detectChanges();
  }

  confirmBooking(): void {
    this.message = '';
    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.appointmentApiService.bookAppointment({
      patientId: this.currentPatientId,
      doctorId: this.form.doctorId,
      scheduledDate: this.form.scheduledDate,
      timeSlot: this.form.timeSlot
    }).pipe(
      timeout(15000)
    ).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.isBookingConfirmOpen = false;

        this.resetForm();
        this.bookingSuccess.emit();

        this.cdr.detectChanges();
      },
      error: (error: unknown) => {
        console.log('Book appointment API error:', error);

        this.isSubmitting = false;
        this.isBookingConfirmOpen = false;
        this.message = this.getErrorMessage(error);

        this.cdr.detectChanges();
      }
    });
  }

  private isBookingFormValid(): boolean {
    if (this.isLoadingPatient) {
      this.message = 'Please wait while patient profile is loading.';
      return false;
    }

    if (this.currentPatientId <= 0) {
      this.message = 'Patient profile is not loaded. Please reload and try again.';
      return false;
    }

    if (this.isLoadingDoctors) {
      this.message = 'Please wait while doctors are loading.';
      return false;
    }

    if (!this.selectedSpecialisation) {
      this.message = 'Please select a specialisation.';
      return false;
    }

    if (!this.form.doctorId) {
      this.message = 'Please select a doctor.';
      return false;
    }

    if (!this.form.scheduledDate) {
      this.message = 'Please select an appointment date.';
      return false;
    }

    if (this.form.scheduledDate < this.todayDate) {
      this.message = 'Past dates are not allowed. Please select today or a future date.';
      return false;
    }

    if (this.form.scheduledDate > this.maxBookingDate) {
      this.message = 'Appointments can only be booked within the next 30 days.';
      return false;
    }

    if (this.isLoadingSlots) {
      this.message = 'Please wait while available slots are loading.';
      return false;
    }

    if (!this.form.timeSlot) {
      this.message = 'Please select an available time slot.';
      return false;
    }

    return true;
  }

  private resetForm(): void {
    this.selectedSpecialisation = '';
    this.specialisationSearchTerm = '';
    this.isSpecialisationDropdownOpen = false;
    this.filteredDoctors = [];
    this.timeSlots = [];

    this.form = {
      patientId: this.currentPatientId,
      doctorId: null!,
      scheduledDate: '',
      timeSlot: ''
    };
  }

  private scrollDownForSpecialisationSearch(): void {
    setTimeout(() => {
      window.scrollBy({
        top: 260,
        behavior: 'smooth'
      });
    }, 120);
  }

  private scrollToSection(sectionId: string): void {
    setTimeout(() => {
      const section = document.getElementById(sectionId);

      if (!section) {
        return;
      }

      section.scrollIntoView({
        behavior: 'smooth',
        block: 'start'
      });
    }, 250);
  }

  private getErrorMessage(error: unknown): string {
    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          errors?: Record<string, string[]>;
        };
        name?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (apiError.error?.message) {
        return apiError.error.message;
      }

      if (apiError.error?.Message) {
        return apiError.error.Message;
      }

      if (apiError.error?.errors) {
        const firstError = Object.values(apiError.error.errors)[0]?.[0];

        if (firstError) {
          return firstError;
        }
      }
    }

    return 'Something went wrong while booking the appointment.';
  }

  private getDateAfterDays(days: number): string {
    const date = new Date();

    date.setDate(date.getDate() + days);

    return date.toISOString().split('T')[0];
  }
}