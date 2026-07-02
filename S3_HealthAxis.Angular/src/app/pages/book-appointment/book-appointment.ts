import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { AppointmentService } from '../../core/services/appointment.service';
import { DoctorService } from '../../core/services/doctor.service';
import { TokenService } from '../../core/services/token.service';

import { Doctor } from '../../shared/models/doctor.models';
import { AppointmentTimeSlot } from '../../shared/models/patient-dashboard.models';

@Component({
  selector: 'app-book-appointment',
  imports: [
    FormsModule,
    RouterLink,
    DatePipe
  ],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment implements OnInit {
  doctorId = 0;
  patientId: number | null = null;

  doctor?: Doctor;

  selectedDate = '';
  selectedSlot: number | null = null;

  availableSlots: number[] = [];

  allSlots: number[] = [
    AppointmentTimeSlot.TenAM,
    AppointmentTimeSlot.TenThirtyAM,
    AppointmentTimeSlot.ElevenAM,
    AppointmentTimeSlot.ElevenThirtyAM,
    AppointmentTimeSlot.TwelvePM,
    AppointmentTimeSlot.TwelveThirtyPM,
    AppointmentTimeSlot.OnePM,
    AppointmentTimeSlot.OneThirtyPM,
    AppointmentTimeSlot.TwoPM,
    AppointmentTimeSlot.TwoThirtyPM,
    AppointmentTimeSlot.ThreePM,
    AppointmentTimeSlot.ThreeThirtyPM
  ];

  loadingDoctor = true;
  loadingSlots = false;
  booking = false;

  errorMessage = '';
  successMessage = '';

  showReviewModal = false;
  showSuccessModal = false;

  minDate = this.getTodayDateString();
  maxDate = this.getMaxBookingDateString();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private doctorService: DoctorService,
    private appointmentService: AppointmentService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    this.patientId = this.tokenService.getReferenceId();

    if (!this.patientId) {
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      this.loadingDoctor = false;
      return;
    }

    const doctorIdFromRoute = this.route.snapshot.paramMap.get('doctorId');
    this.doctorId = Number(doctorIdFromRoute);

    if (!this.doctorId || Number.isNaN(this.doctorId)) {
      this.errorMessage = 'Invalid doctor selected.';
      this.loadingDoctor = false;
      return;
    }

    this.loadDoctor();
  }

  loadDoctor(): void {
    this.loadingDoctor = true;
    this.errorMessage = '';

    this.doctorService.getDoctorById(this.doctorId).subscribe({
      next: (doctor) => {
        this.doctor = doctor;
        this.loadingDoctor = false;
      },
      error: (error) => {
        this.loadingDoctor = false;

        if (error.status === 404) {
          this.errorMessage = 'Doctor not found.';
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to view this doctor. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not load doctor details. Please try again.';
      }
    });
  }

  onDateChange(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.selectedSlot = null;
    this.availableSlots = [];
    this.showReviewModal = false;
    this.showSuccessModal = false;

    if (!this.selectedDate) {
      return;
    }

    if (!this.isDateWithinAllowedRange(this.selectedDate)) {
      this.errorMessage = 'Appointments can be booked only from today up to the next 30 days.';
      return;
    }

    this.loadAvailability();
  }

  loadAvailability(): void {
    this.loadingSlots = true;
    this.errorMessage = '';

    this.doctorService
      .getDoctorAvailability(this.doctorId, this.selectedDate)
      .subscribe({
        next: (slots) => {
          this.availableSlots = slots ?? [];
          this.loadingSlots = false;

          if (!this.getBookableSlots().length) {
            this.errorMessage = 'No bookable slots available for the selected date.';
          }
        },
        error: (error) => {
          this.loadingSlots = false;

          if (error.status === 401 || error.status === 403) {
            this.errorMessage = 'You are not authorized to view available slots. Please login again.';
            return;
          }

          if (error.status === 0) {
            this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
            return;
          }

          this.errorMessage = 'Could not load available slots. Please try another date.';
        }
      });
  }

  selectSlot(slot: number): void {
    if (!this.isSlotBookable(slot)) {
      return;
    }

    this.selectedSlot = slot;
    this.errorMessage = '';
    this.successMessage = '';
  }

  openReviewModal(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.patientId) {
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      return;
    }

    if (!this.doctor) {
      this.errorMessage = 'Doctor details are not available.';
      return;
    }

    if (!this.selectedDate) {
      this.errorMessage = 'Please select an appointment date.';
      return;
    }

    if (!this.isDateWithinAllowedRange(this.selectedDate)) {
      this.errorMessage = 'Appointments can be booked only from today up to the next 30 days.';
      return;
    }

    if (!this.selectedSlot) {
      this.errorMessage = 'Please select an available time slot.';
      return;
    }

    if (!this.isSlotBookable(this.selectedSlot)) {
      this.errorMessage = 'Selected slot is no longer available. Please choose another slot.';
      this.selectedSlot = null;
      return;
    }

    this.showReviewModal = true;
  }

  closeReviewModal(): void {
    if (this.booking) {
      return;
    }

    this.showReviewModal = false;
  }

  confirmFinalBooking(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.patientId) {
      this.errorMessage = 'Unable to identify patient account. Please login again.';
      this.showReviewModal = false;
      return;
    }

    if (!this.doctor) {
      this.errorMessage = 'Doctor details are not available.';
      this.showReviewModal = false;
      return;
    }

    if (!this.selectedDate) {
      this.errorMessage = 'Please select an appointment date.';
      this.showReviewModal = false;
      return;
    }

    if (!this.isDateWithinAllowedRange(this.selectedDate)) {
      this.errorMessage = 'Appointments can be booked only from today up to the next 30 days.';
      this.showReviewModal = false;
      return;
    }

    if (!this.selectedSlot) {
      this.errorMessage = 'Please select an available time slot.';
      this.showReviewModal = false;
      return;
    }

    if (!this.isSlotBookable(this.selectedSlot)) {
      this.errorMessage = 'Selected slot is no longer available. Please choose another slot.';
      this.selectedSlot = null;
      this.showReviewModal = false;
      return;
    }

    this.booking = true;

    this.appointmentService.createAppointment({
      patientId: this.patientId,
      doctorId: this.doctorId,
      scheduledDate: this.selectedDate,
      timeSlot: this.selectedSlot
    }).subscribe({
      next: () => {
        this.booking = false;
        this.showReviewModal = false;
        this.showSuccessModal = true;
        this.successMessage = 'Appointment booked successfully.';
      },
      error: (error) => {
        this.booking = false;
        this.showReviewModal = false;

        if (error.status === 400 && typeof error.error === 'string') {
          this.errorMessage = error.error;
          return;
        }

        if (error.status === 401 || error.status === 403) {
          this.errorMessage = 'You are not authorized to book this appointment. Please login again.';
          return;
        }

        if (error.status === 0) {
          this.errorMessage = 'Could not connect to the API. Please make sure the API is running.';
          return;
        }

        this.errorMessage = 'Could not book appointment. Please try again.';
      }
    });
  }

  goToAppointments(): void {
    this.router.navigate(['/patient/appointments'], {
      queryParams: {
        booked: 'true'
      }
    });
  }

  bookAnother(): void {
    this.showSuccessModal = false;
    this.successMessage = '';
    this.errorMessage = '';

    this.selectedDate = '';
    this.selectedSlot = null;
    this.availableSlots = [];
  }

  isDateWithinAllowedRange(date: string): boolean {
    return date >= this.minDate && date <= this.maxDate;
  }

  isAvailableSlot(slot: number): boolean {
    return this.availableSlots.includes(slot);
  }

  isPastSlotForToday(slot: number): boolean {
    if (!this.selectedDate || this.selectedDate !== this.minDate) {
      return false;
    }

    const now = new Date();
    const currentMinutes = now.getHours() * 60 + now.getMinutes();
    const slotStartMinutes = this.getSlotStartMinutes(slot);

    return slotStartMinutes < currentMinutes;
  }

  isSlotBookable(slot: number): boolean {
    return (
      !!this.selectedDate &&
      this.isDateWithinAllowedRange(this.selectedDate) &&
      this.isAvailableSlot(slot) &&
      !this.isPastSlotForToday(slot)
    );
  }

  getSlotStatusText(slot: number): string {
    if (this.selectedSlot === slot && this.isSlotBookable(slot)) {
      return 'Selected';
    }

    if (this.isPastSlotForToday(slot)) {
      return 'Time passed';
    }

    if (!this.isAvailableSlot(slot)) {
      return 'Unavailable';
    }

    return 'Available';
  }

  getSlotClass(slot: number): string {
    if (this.selectedSlot === slot && this.isSlotBookable(slot)) {
      return 'selected';
    }

    if (this.isSlotBookable(slot)) {
      return 'available';
    }

    return 'unavailable';
  }

  getBookableSlots(): number[] {
    return this.allSlots.filter(slot => this.isSlotBookable(slot));
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

  private getSlotStartMinutes(timeSlot: number): number {
    switch (timeSlot) {
      case AppointmentTimeSlot.TenAM:
        return 10 * 60;

      case AppointmentTimeSlot.TenThirtyAM:
        return 10 * 60 + 30;

      case AppointmentTimeSlot.ElevenAM:
        return 11 * 60;

      case AppointmentTimeSlot.ElevenThirtyAM:
        return 11 * 60 + 30;

      case AppointmentTimeSlot.TwelvePM:
        return 12 * 60;

      case AppointmentTimeSlot.TwelveThirtyPM:
        return 12 * 60 + 30;

      case AppointmentTimeSlot.OnePM:
        return 13 * 60;

      case AppointmentTimeSlot.OneThirtyPM:
        return 13 * 60 + 30;

      case AppointmentTimeSlot.TwoPM:
        return 14 * 60;

      case AppointmentTimeSlot.TwoThirtyPM:
        return 14 * 60 + 30;

      case AppointmentTimeSlot.ThreePM:
        return 15 * 60;

      case AppointmentTimeSlot.ThreeThirtyPM:
        return 15 * 60 + 30;

      default:
        return 0;
    }
  }

  private getTodayDateString(): string {
    const today = new Date();

    const year = today.getFullYear();
    const month = `${today.getMonth() + 1}`.padStart(2, '0');
    const day = `${today.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private getMaxBookingDateString(): string {
    const maxDate = new Date();
    maxDate.setDate(maxDate.getDate() + 30);

    const year = maxDate.getFullYear();
    const month = `${maxDate.getMonth() + 1}`.padStart(2, '0');
    const day = `${maxDate.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
