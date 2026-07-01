import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { AppointmentCreateRequest } from '../../../core/models/appointment-create-request';
import { Doctor } from '../../../core/models/doctor.model';
import { PatientService } from '../../../core/services/patient.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-book-appointment',
  imports: [FormsModule, RouterLink],
  templateUrl: './book-appointment.html',
  styleUrls: ['./book-appointment.css']
})
export class BookAppointment implements OnInit {
  doctorId = 0;
  doctor: Doctor | null = null;

  minDate = new Date().toISOString().split('T')[0];

  selectedDate = '';
  selectedSlot = '';

  availableSlots: string[] = [];

  isLoadingDoctor = false;
  isLoadingSlots = false;
  isBooking = false;

  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private patientService: PatientService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    const idFromRoute =
      this.route.snapshot.paramMap.get('doctorId');

    this.doctorId = Number(idFromRoute);

    if (!this.doctorId) {
      this.errorMessage = 'Invalid doctor selected.';
      return;
    }

    this.loadDoctor();
  }

  loadDoctor(): void {
    this.isLoadingDoctor = true;
    this.errorMessage = '';

    this.patientService.getDoctorById(this.doctorId).subscribe({
      next: (doctor: Doctor) => {
        this.doctor = doctor;
      },

      error: (error: HttpErrorResponse) => {
        console.log('Doctor detail error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load doctor details. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoadingDoctor = false;
      }
    });
  }

  onDateChanged(): void {
    this.errorMessage = '';
    this.selectedSlot = '';
    this.availableSlots = [];

    if (!this.selectedDate) {
      return;
    }

    if (this.isPastDate(this.selectedDate)) {
      this.errorMessage = 'Past dates are not allowed.';
      return;
    }

    this.loadAvailableSlots();
  }

  loadAvailableSlots(): void {
    this.isLoadingSlots = true;
    this.errorMessage = '';

    this.patientService
      .getDoctorAvailability(this.doctorId, this.selectedDate)
      .subscribe({
        next: (slots: string[]) => {
          console.log('Availability response:', slots);

          this.availableSlots =
            (slots ?? []).filter(slot =>
              !this.isPastSlot(this.selectedDate, slot)
            );
        },

        error: (error: HttpErrorResponse) => {
          console.log('Availability error:', error);
          console.log('Availability response body:', error.error);

          this.availableSlots = [];

          this.errorMessage =
            error.error?.message ??
            this.getValidationErrors(error) ??
            `Unable to load available slots. Status: ${error.status}`;
        },

        complete: () => {
          this.isLoadingSlots = false;
        }
      });
  }

  bookAppointment(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.selectedDate) {
      this.errorMessage = 'Please select an appointment date.';
      return;
    }

    if (this.isPastDate(this.selectedDate)) {
      this.errorMessage = 'Past dates are not allowed.';
      return;
    }

    if (!this.selectedSlot) {
      this.errorMessage = 'Please select an available time slot.';
      return;
    }

    if (this.isPastSlot(this.selectedDate, this.selectedSlot)) {
      this.errorMessage = 'Past time slots are not allowed.';
      return;
    }

    const patientReferenceId =
      this.tokenService.getReferenceId();

    if (!patientReferenceId) {
      this.errorMessage =
        'Patient details were not found. Please login again.';
      return;
    }

    const request: AppointmentCreateRequest = {
      patientId: Number(patientReferenceId),
      doctorId: this.doctorId,
      scheduledDate: this.selectedDate,
      timeSlot: this.selectedSlot
    };

    this.isBooking = true;

    this.patientService.bookAppointment(request).subscribe({
      next: () => {
        this.successMessage =
          'Appointment booked successfully.';

        setTimeout(() => {
          this.router.navigate(['/patient/appointments']);
        }, 1000);
      },

      error: (error: HttpErrorResponse) => {
        console.log('Book appointment error:', error);
        console.log('Book appointment response body:', error.error);

        this.errorMessage =
          error.error?.message ??
          this.getValidationErrors(error) ??
          `Unable to book appointment. Status: ${error.status}`;
      },

      complete: () => {
        this.isBooking = false;
      }
    });
  }

  private isPastDate(date: string): boolean {
    const selected =
      new Date(date);

    selected.setHours(0, 0, 0, 0);

    const today =
      new Date();

    today.setHours(0, 0, 0, 0);

    return selected.getTime() < today.getTime();
  }

  private isPastSlot(date: string, slot: string): boolean {
    const todayString =
      new Date().toISOString().split('T')[0];

    if (date !== todayString) {
      return false;
    }

    const startTime =
      slot.split('-')[0];

    const [hours, minutes] =
      startTime.split(':').map(Number);

    const slotDateTime =
      new Date();

    slotDateTime.setHours(hours, minutes, 0, 0);

    return slotDateTime.getTime() <= new Date().getTime();
  }

  private getValidationErrors(error: HttpErrorResponse): string | null {
    const validationErrors = error.error?.errors;

    if (!validationErrors) {
      return null;
    }

    const messages: string[] = [];

    Object.keys(validationErrors).forEach((key) => {
      const fieldErrors = validationErrors[key];

      if (Array.isArray(fieldErrors)) {
        messages.push(...fieldErrors);
      }
    });

    return messages.length > 0
      ? messages.join(' ')
      : null;
  }

  getSpecialisationName(value: number | string): string {
    if (typeof value === 'string' && isNaN(Number(value))) {
      return value;
    }

    const specialisationMap: Record<number, string> = {
      0: 'Cardiology',
      1: 'General Medicine',
      2: 'Dermatology',
      3: 'Pediatrics',
      4: 'Orthopedics',
      5: 'Neurology'
    };

    return specialisationMap[Number(value)] ??
      `Specialisation ${value}`;
  }

  formatFee(value: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(value);
  }
}