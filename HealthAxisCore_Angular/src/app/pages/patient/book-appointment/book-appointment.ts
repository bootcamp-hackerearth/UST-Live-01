import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

interface DoctorDetails {
  doctorId: number;
  doctorName: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
}

@Component({
  selector: 'app-book-appointment',
  imports: [
    FormsModule,
    RouterLink
  ],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment {
  doctorId = 0;

  selectedDate = '';

  selectedSlot = '';

  successMessage = '';

  today = new Date().toISOString().split('T')[0];

  doctor: DoctorDetails = {
    doctorId: 1,
    doctorName: 'Dr. Isha Nair',
    specialisation: 'Dermatologist',
    yearsOfExperience: 10,
    consultationFee: 1000
  };

  availableSlots = [
    '09:00',
    '09:30',
    '10:00',
    '10:30',
    '11:00',
    '11:30',
    '14:00',
    '14:30',
    '15:00',
    '15:30',
    '16:00'
  ];

  constructor(private route: ActivatedRoute) {
    this.doctorId = Number(this.route.snapshot.paramMap.get('doctorId')) || 1;
  }

  selectSlot(slot: string): void {
    this.selectedSlot = slot;
    this.successMessage = '';
  }

  bookAppointment(): void {
    if (!this.selectedDate || !this.selectedSlot) {
      this.successMessage = 'Please select a date and available time slot.';
      return;
    }

    this.successMessage = `Appointment template created for ${this.selectedDate} at ${this.selectedSlot}. API connection will be added later.`;

    console.log('Book appointment:', {
      doctorId: this.doctorId,
      scheduledDate: this.selectedDate,
      timeSlot: this.selectedSlot
    });
  }
}
