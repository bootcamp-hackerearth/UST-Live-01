import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth } from '../../../services/mock-auth';
import {
  DoctorDto,
  MockPatientData,
  PatientDto,
  SpecialisationType
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-book-appointment',
  imports: [RouterLink, FormsModule],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css',
})
export class BookAppointment {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);

  patient?: PatientDto = this.auth.getCurrentPatient();

  specialisations = this.patientData.specialisations;

  selectedSpecialisation: SpecialisationType | '' = '';
  selectedDoctorId = 0;
  selectedDate = '';
  selectedSlot = '';

  availableSlots: string[] = [];

  message = '';
  isError = false;
  isBooking = false;

  today = this.getTodayDate();

  get filteredDoctors(): DoctorDto[] {
    if (!this.selectedSpecialisation) {
      return [];
    }

    return this.patientData.getDoctorsBySpecialisation(this.selectedSpecialisation);
  }

  get selectedDoctor(): DoctorDto | undefined {
    if (!this.selectedDoctorId) {
      return undefined;
    }

    return this.patientData.getDoctorById(this.selectedDoctorId);
  }

  onSpecialisationChanged(): void {
    this.selectedDoctorId = 0;
    this.selectedDate = '';
    this.selectedSlot = '';
    this.availableSlots = [];
    this.clearMessage();
  }

  onDoctorChanged(): void {
    this.selectedDate = '';
    this.selectedSlot = '';
    this.availableSlots = [];
    this.clearMessage();
  }

  onDateChanged(): void {
    this.selectedSlot = '';
    this.clearMessage();

    if (!this.selectedDoctorId || !this.selectedDate) {
      this.availableSlots = [];
      return;
    }

    this.availableSlots = this.patientData.getAvailableSlots(
      this.selectedDoctorId,
      this.selectedDate
    );
  }

  selectSlot(slot: string): void {
    this.selectedSlot = slot;
    this.clearMessage();
  }

  bookAppointment(): void {
    this.clearMessage();

    if (!this.patient) {
      this.showError('Patient details are missing. Please login again.');
      return;
    }

    if (!this.selectedSpecialisation) {
      this.showError('Please select a specialisation.');
      return;
    }

    if (!this.selectedDoctorId) {
      this.showError('Please select a doctor.');
      return;
    }

    if (!this.selectedDate) {
      this.showError('Please select an appointment date.');
      return;
    }

    if (!this.selectedSlot) {
      this.showError('Please select an available time slot.');
      return;
    }

    this.isBooking = true;

    setTimeout(() => {
      const result = this.patientData.bookAppointment({
        patientId: this.patient!.patientId,
        doctorId: this.selectedDoctorId,
        scheduledDate: this.selectedDate,
        timeSlot: this.selectedSlot
      });

      this.isBooking = false;

      if (!result.success) {
        this.showError(result.message);
        this.onDateChanged();
        return;
      }

      this.message = result.message;
      this.isError = false;

      setTimeout(() => {
        this.router.navigate(['/patient/appointments']);
      }, 1000);
    }, 600);
  }

  goBack(): void {
    this.router.navigate(['/patient/appointments']);
  }

  private clearMessage(): void {
    this.message = '';
    this.isError = false;
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }

  private getTodayDate(): string {
    return new Date().toISOString().split('T')[0];
  }
}