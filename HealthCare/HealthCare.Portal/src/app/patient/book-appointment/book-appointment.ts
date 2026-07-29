import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppointmentService } from '../../core/services/appointment.service';
import { DoctorService } from '../../core/services/doctor.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment {
  minDate = this.getTomorrowDate();

  selectedDate = signal('');
  selectedSpecialisation = signal('');
  selectedDoctorId = signal<number | null>(null);
  selectedTimeSlot = signal('');

  successMessage = signal('');
  errorMessage = signal('');
  isBooking = signal(false);

  specialisations = [
    
    'Cardiologist',
    'Dermatologist',
    'Neurologist',
    'Orthopedic',
    'Pediatrician',
    'Psychiatrist',
    'General Physician',
    'ENT'];

  showNoDoctorsMessage = computed(() =>
    this.selectedDate() &&
    this.selectedSpecialisation() &&
    !this.doctorService.isLoading() &&
    this.doctorService.availableDoctors().length === 0
  );

  showNoSlotsMessage = computed(() =>
    this.selectedDoctorId() &&
    this.doctorService.slotsLoaded() &&
    this.doctorService.availableSlots().length === 0
  );

  constructor(
    public doctorService: DoctorService,
    private readonly appointmentService: AppointmentService
  ) { }

  getTomorrowDate(): string {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    const year = tomorrow.getFullYear();
    const month = String(tomorrow.getMonth() + 1).padStart(2, '0');
    const day = String(tomorrow.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  onDateOrSpecialisationChange(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    this.selectedDoctorId.set(null);
    this.selectedTimeSlot.set('');

    this.doctorService.availableDoctors.set([]);
    this.doctorService.availableSlots.set([]);
    this.doctorService.slotsLoaded.set(false);

    if (!this.selectedDate() || !this.selectedSpecialisation()) {
      return;
    }

    this.doctorService.loadAvailableDoctors(
      this.selectedSpecialisation(),
      this.selectedDate()
    );
  }

  onDoctorChange(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    this.selectedTimeSlot.set('');

    this.doctorService.availableSlots.set([]);
    this.doctorService.slotsLoaded.set(false);

    if (!this.selectedDoctorId() || !this.selectedDate()) {
      return;
    }

    this.doctorService.loadAvailableSlots(
      Number(this.selectedDoctorId()),
      this.selectedDate()
    );
  }

  bookAppointment(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    if (
      !this.selectedDate() ||
      !this.selectedSpecialisation() ||
      !this.selectedDoctorId() ||
      !this.selectedTimeSlot()
    ) {
      this.errorMessage.set('Please fill all fields');
      return;
    }

    this.isBooking.set(true);

    this.appointmentService.bookAppointment({
      doctorId: Number(this.selectedDoctorId()),
      scheduledDate: this.selectedDate(),
      timeSlot: this.selectedTimeSlot()
    }).subscribe({
      next: () => {
        this.successMessage.set('Appointment booked successfully');

        this.selectedDate.set('');
        this.selectedSpecialisation.set('');
        this.selectedDoctorId.set(null);
        this.selectedTimeSlot.set('');

        this.doctorService.availableDoctors.set([]);
        this.doctorService.availableSlots.set([]);
        this.doctorService.slotsLoaded.set(false);

        this.isBooking.set(false);
      },
      error: (error) => {
        this.errorMessage.set(
          error?.error?.message || 'Failed to book appointment'
        );

        this.isBooking.set(false);
      }
    });
  }
}
