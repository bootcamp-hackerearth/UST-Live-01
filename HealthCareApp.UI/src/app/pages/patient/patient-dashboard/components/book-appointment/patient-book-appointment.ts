import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { BookAppointmentDto } from '../../../../../shared/models/appointment.models';
import { DoctorDto } from '../../../../../shared/models/doctor.models';
import { PatientFakeDataService } from '../../../../../core/services/patient-fake-data.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './patient-book-appointment.html',
  styleUrl: './patient-book-appointment.css'
})
export class PatientBookAppointment {
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
  isSubmitting = false;
  isBookingConfirmOpen = false;

  form: BookAppointmentDto = {
    patientId: 1,
    doctorId: null!,
    scheduledDate: '',
    timeSlot: ''
  };

  @Output() bookingSuccess = new EventEmitter<void>();

  constructor(private service: PatientFakeDataService) {
    this.specialisations = this.service.getSpecialisations();
    this.doctors = this.service.getActiveDoctors();
    this.timeSlots = this.service.getAvailableTimeSlots();

    this.todayDate = new Date().toISOString().split('T')[0];
    this.maxBookingDate = this.getDateAfterDays(30);
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
    return this.filteredSpecialisations;
  }

  get selectedDoctor(): DoctorDto | undefined {
    return this.doctors.find(
      (doctor: DoctorDto) => doctor.doctorId === this.form.doctorId
    );
  }

  openSpecialisationDropdown(): void {
    this.isSpecialisationDropdownOpen = true;
    this.scrollDownForSpecialisationSearch();
  }

  closeSpecialisationDropdown(): void {
    setTimeout(() => {
      this.isSpecialisationDropdownOpen = false;
    }, 150);
  }

  selectSpecialisation(specialisation: string): void {
    this.selectedSpecialisation = specialisation;
    this.specialisationSearchTerm = specialisation;
    this.filteredDoctors = this.service.getDoctorsBySpecialisation(specialisation);
    this.isSpecialisationDropdownOpen = false;

    this.form.doctorId = null!;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.message = '';

    this.scrollToSection('doctor-section');
  }

  clearSpecialisation(): void {
    this.selectedSpecialisation = '';
    this.specialisationSearchTerm = '';
    this.filteredDoctors = [];
    this.isSpecialisationDropdownOpen = false;

    this.form.doctorId = null!;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.message = '';
  }

  selectDoctor(doctorId: number): void {
    this.form.doctorId = doctorId;
    this.form.scheduledDate = '';
    this.form.timeSlot = '';
    this.message = '';

    this.scrollToSection('date-section');
  }

  onDateChange(): void {
    this.form.timeSlot = '';
    this.message = '';

    if (this.form.scheduledDate) {
      this.scrollToSection('slot-section');
    }
  }

  selectTimeSlot(slot: string): void {
    if (this.isTimeSlotDisabled(slot)) {
      return;
    }

    this.form.timeSlot = slot;
    this.message = '';

    this.scrollToSection('submit-section');
  }

  isTimeSlotBooked(slot: string): boolean {
    if (!this.form.doctorId || !this.form.scheduledDate) {
      return false;
    }

    return this.service.isSlotBooked(
      this.form.doctorId,
      this.form.scheduledDate,
      slot
    );
  }

  isTimeSlotDisabled(slot: string): boolean {
    return (
      !this.form.doctorId ||
      !this.form.scheduledDate ||
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

    if (!this.form.doctorId || !this.form.scheduledDate) {
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
  }

  closeBookingConfirm(): void {
    this.isBookingConfirmOpen = false;
  }

  confirmBooking(): void {
    this.message = '';
    this.isSubmitting = true;

    try {
      this.service.bookAppointment(this.form);

      this.closeBookingConfirm();
      this.resetForm();
      this.bookingSuccess.emit();
    } catch (error: unknown) {
      this.message = this.getErrorMessage(error);
      this.closeBookingConfirm();
    }

    this.isSubmitting = false;
  }

  private isBookingFormValid(): boolean {
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

    this.form = {
      patientId: 1,
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
    if (error instanceof Error) {
      return error.message;
    }

    return 'Something went wrong while booking the appointment.';
  }

  private getDateAfterDays(days: number): string {
    const date = new Date();

    date.setDate(date.getDate() + days);

    return date.toISOString().split('T')[0];
  }
}