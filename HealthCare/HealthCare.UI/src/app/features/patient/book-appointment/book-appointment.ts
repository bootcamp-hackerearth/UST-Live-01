import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointmentComponent
implements OnInit {

  date = '';

  specialisation = '';

  doctorId = 0;

  selectedSlot = '';

  specialisations: string[] = [];

  doctors: any[] = [];

  slots: string[] = [];

  constructor(
    private doctorService: DoctorService,
    private appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {

    this.loadSpecialisations();
  }

  loadSpecialisations() {

    this.doctorService
      .getSpecialisations()
      .subscribe(res => {

        this.specialisations = res;
      });
  }

  loadDoctors() {

    if (!this.date || !this.specialisation)
      return;

    this.doctorService
      .getAvailableDoctors(
        this.specialisation,
        this.date
      )
      .subscribe(res => {

        this.doctors = res;

        this.slots = [];

        this.selectedSlot = '';
      });
  }

  loadSlots() {

    if (!this.doctorId)
      return;

    this.appointmentService
      .getAvailableSlots(
        this.doctorId,
        this.date
      )
      .subscribe(res => {

        this.slots = res;
      });
  }

  bookAppointment() {

    const appointment = {

      doctorId: this.doctorId,

      appointmentDate: this.date,

      timeSlot: this.selectedSlot
    };

    this.appointmentService
      .bookAppointment(appointment)
      .subscribe({

        next: () => {

          alert(
            'Appointment booked successfully'
          );
        },

        error: () => {

          alert(
            'Unable to book appointment'
          );
        }
      });
  }
}