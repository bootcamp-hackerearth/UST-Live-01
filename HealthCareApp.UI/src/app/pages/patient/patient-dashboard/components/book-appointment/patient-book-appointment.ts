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
  todayDate = '';

  form: BookAppointmentDto = {
    patientId: 1,
    doctorId: null!,
    scheduledDate: '',
    timeSlot: ''
  };

  message = '';
  isSubmitting = false;

  @Output() bookingSuccess = new EventEmitter<void>();

  constructor(private service: PatientFakeDataService) {
    this.specialisations = this.service.getSpecialisations();
    this.doctors = this.service.getActiveDoctors();
    this.timeSlots = this.service.getAvailableTimeSlots();
    this.todayDate = new Date().toISOString().split('T')[0];
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

  get selectedDoctor(): DoctorDto | undefined {
    return this.doctors.find(
      (doctor: DoctorDto) => doctor.doctorId === this.form.doctorId
    );
  }

  selectSpecialisation(specialisation: string): void {
    this.selectedSpecialisation = specialisation;
    this.specialisationSearchTerm = specialisation;
    this.filteredDoctors = this.service.getDoctorsBySpecialisation(specialisation);

    this.form.doctorId = null!;
    this.form.timeSlot = '';
    this.message = '';
  }

  clearSpecialisation(): void {
    this.selectedSpecialisation = '';
    this.specialisationSearchTerm = '';
    this.filteredDoctors = [];

    this.form.doctorId = null!;
    this.form.timeSlot = '';
    this.message = '';
  }

  selectDoctor(doctorId: number): void {
    this.form.doctorId = doctorId;
    this.form.timeSlot = '';
    this.message = '';
  }

  selectTimeSlot(slot: string): void {
    if (this.isTimeSlotDisabled(slot)) {
      return;
    }

    this.form.timeSlot = slot;
    this.message = '';
  }

  onDateChange(): void {
    this.form.timeSlot = '';
    this.message = '';
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

    if (!this.selectedSpecialisation) {
      this.message = 'Please select a specialisation.';
      return;
    }

    if (!this.form.doctorId) {
      this.message = 'Please select a doctor.';
      return;
    }

    if (!this.form.scheduledDate) {
      this.message = 'Please select an appointment date.';
      return;
    }

    if (!this.form.timeSlot) {
      this.message = 'Please select an available time slot.';
      return;
    }

    this.isSubmitting = true;

    try {
      this.service.bookAppointment(this.form);

      this.resetForm();
      this.bookingSuccess.emit();
    } catch (error: unknown) {
      this.message = this.getErrorMessage(error);
    }

    this.isSubmitting = false;
  }

  private resetForm(): void {
    this.selectedSpecialisation = '';
    this.specialisationSearchTerm = '';
    this.filteredDoctors = [];

    this.form = {
      patientId: 1,
      doctorId: null!,
      scheduledDate: '',
      timeSlot: ''
    };
  }

  private getErrorMessage(error: unknown): string {
    if (error instanceof Error) {
      return error.message;
    }

    return 'Something went wrong while booking the appointment.';
  }
}
