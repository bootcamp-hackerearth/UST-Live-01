import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/sidebar/sidebar';

import { AppointmentService } from '../../Patient.service/appointmentservice';
import { DoctorSlot } from '../../models/appointment/doctor-availability.model';
import { AppPopupComponent } from '../../shared/app-popup/app-popup';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar, AppPopupComponent],
  templateUrl: './appointments.html',
  styleUrls: ['./appointments.css']
})
export class Appointments implements OnInit {

  allAppointments = signal<any[]>([]);
  filteredAppointments = signal<any[]>([]);
  paginatedAppointments = signal<any[]>([]);
  minBookingDate = signal('');
  bookingErrorMessage = signal('');
  bookingSuccessMessage = signal('');

  showCancelModal = signal(false);
  cancelReason = '';
  selectedAppointmentId: number | null = null;

  availabilityMessage = signal('');
  isDoctorOnLeave = signal(false);

  nameSearch = '';
  statusFilter = '';
  dateSearch = '';

  pageNumber = signal(1);
  pageSize = 5;
  totalPages = signal(0);

  showBookingModal = signal(false);
  selectedDoctorId: number | null = null;
  selectedDate = '';
  selectedSlot = '';
  selectedDoctor: any = null;

  availableSlots = signal<DoctorSlot[]>([]);

  popupVisible = signal(false);
  popupTitle = signal('');
  popupMessage = signal('');
  popupType = signal<'success' | 'error' | 'warning'>('success');

  constructor(private readonly appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.minBookingDate.set(this.getTodayDateString());

    this.loadAppointments();

    const data = localStorage.getItem('selectedDoctor');

    if (data) {
      this.selectedDoctor = JSON.parse(data);
      this.selectedDoctorId = this.selectedDoctor.doctorId;
      this.showBookingModal.set(true);
    }
  }

  private showPopup(title: string, message: string, type: 'success' | 'error' | 'warning' = 'success') {
    this.popupTitle.set(title);
    this.popupMessage.set(message);
    this.popupType.set(type);
    this.popupVisible.set(true);
  }

  closePopup() {
    this.popupVisible.set(false);
    this.popupTitle.set('');
    this.popupMessage.set('');
  }

  private getTodayDateString(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private isPastDate(dateValue: string): boolean {
    const selectedDate = new Date(dateValue);
    selectedDate.setHours(0, 0, 0, 0);

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate < today;
  }

  private isToday(dateValue: string): boolean {
    const selectedDate = new Date(dateValue);
    selectedDate.setHours(0, 0, 0, 0);

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate.getTime() === today.getTime();
  }

  private convertSlotToDate(timeSlot: string, dateValue: string): Date | null {
    if (!timeSlot || !dateValue) {
      return null;
    }

    const selectedDate = new Date(dateValue);
    let hours = 0;
    let minutes = 0;

    const cleanedSlot = timeSlot.trim().toUpperCase();

    const twelveHourMatch = cleanedSlot.match(/^(\d{1,2}):(\d{2})\s?(AM|PM)$/);

    if (twelveHourMatch) {
      hours = Number(twelveHourMatch[1]);
      minutes = Number(twelveHourMatch[2]);
      const meridian = twelveHourMatch[3];

      if (meridian === 'PM' && hours !== 12) {
        hours += 12;
      }

      if (meridian === 'AM' && hours === 12) {
        hours = 0;
      }
    } else {
      const twentyFourHourMatch = cleanedSlot.match(/^(\d{1,2}):(\d{2})$/);

      if (!twentyFourHourMatch) {
        return null;
      }

      hours = Number(twentyFourHourMatch[1]);
      minutes = Number(twentyFourHourMatch[2]);
    }

    selectedDate.setHours(hours, minutes, 0, 0);

    return selectedDate;
  }

  private isFutureSlotForToday(timeSlot: string): boolean {
    if (!this.selectedDate) {
      return false;
    }

    if (!this.isToday(this.selectedDate)) {
      return true;
    }

    const slotDateTime = this.convertSlotToDate(timeSlot, this.selectedDate);

    if (!slotDateTime) {
      return false;
    }

    const now = new Date();

    return slotDateTime > now;
  }

  loadAppointments() {
    this.appointmentService.getMyAppointments().subscribe(res => {
      const appointments = (res || []).map((a: any) => ({
        ...a,
        scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
      }));

      this.allAppointments.set(appointments);
      this.filteredAppointments.set(appointments);
      this.updatePagination();
    });
  }

  filterAppointments() {
    const filtered = this.allAppointments().filter(a => {
      const matchName =
        !this.nameSearch ||
        a.doctorName?.toLowerCase().includes(this.nameSearch.toLowerCase());

      const matchStatus =
        !this.statusFilter || a.status === this.statusFilter;

      let matchDate = true;

      if (this.dateSearch) {
        const selectedDate = new Date(this.dateSearch);
        selectedDate.setHours(0, 0, 0, 0);

        const appointmentDate = new Date(a.scheduledDate);
        appointmentDate.setHours(0, 0, 0, 0);

        matchDate = appointmentDate.getTime() === selectedDate.getTime();
      }

      return matchName && matchStatus && matchDate;
    });

    this.filteredAppointments.set(filtered);
    this.pageNumber.set(1);
    this.updatePagination();
  }

  updatePagination() {
    const totalPages = Math.ceil(this.filteredAppointments().length / this.pageSize);
    this.totalPages.set(totalPages <= 0 ? 1 : totalPages);
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedAppointments.set(this.filteredAppointments().slice(start, end));
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(value => value + 1);
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(value => value - 1);
      this.paginate();
    }
  }

  cancel(id: number): void {
    this.selectedAppointmentId = id;
    this.cancelReason = '';
    this.showCancelModal.set(true);
    console.log('Clicked appointment', id);
  }

  confirmCancel(): void {
    if (!this.selectedAppointmentId) {
      return;
    }

    if (!this.cancelReason.trim()) {
      this.showPopup('Missing Reason', 'Please enter a cancellation reason.', 'warning');
      return;
    }

    this.appointmentService
      .cancelAppointment(
        this.selectedAppointmentId,
        this.cancelReason
      )
      .subscribe({
        next: () => {
          this.closeCancelModal();
          this.loadAppointments();
        }
      });
  }

  closeCancelModal() {
    this.showCancelModal.set(false);
    this.cancelReason = '';
    this.selectedAppointmentId = null;
  }

  openBooking() {
    this.showBookingModal.set(true);
  }

  closeBooking() {
    this.showBookingModal.set(false);
    this.selectedDoctorId = null;
    this.selectedDate = '';
    this.selectedSlot = '';
    this.selectedDoctor = null;

    this.availableSlots.set([]);
    this.availabilityMessage.set('');
    this.isDoctorOnLeave.set(false);

    this.bookingErrorMessage.set('');
    this.bookingSuccessMessage.set('');

    localStorage.removeItem('selectedDoctor');
  }

  checkSlots() {
    if (!this.selectedDoctorId || !this.selectedDate) {
      return;
    }

    if (this.isPastDate(this.selectedDate)) {
      this.showPopup('Booking Error', 'Previous date appointment booking is not allowed.', 'warning');
      this.selectedDate = '';
      this.availableSlots.set([]);
      this.selectedSlot = '';
      return;
    }

    this.appointmentService
      .checkDoctorAvailability(this.selectedDoctorId, this.selectedDate)
      .subscribe({
        next: (res) => {
          this.isDoctorOnLeave.set(res.isDoctorOnLeave);
          this.availabilityMessage.set(res.message);

          const slots = res.slots || [];

          this.availableSlots.set(slots.map((slot: DoctorSlot) => {
            const isFutureSlot = this.isFutureSlotForToday(slot.timeSlot);

            return {
              ...slot,
              isAvailable: slot.isAvailable && isFutureSlot,
              status: !isFutureSlot
                ? 'Time Passed'
                : slot.status
            };
          }));

          if (res.isDoctorOnLeave) {
            this.selectedSlot = '';
          }
        },
        error: (err) => {
          console.error(err);
          this.showPopup('Availability Error', 'Failed to check doctor availability.', 'error');
        }
      });
  }

  selectSlot(slot: DoctorSlot) {
    if (!slot.isAvailable) {
      return;
    }

    if (!this.isFutureSlotForToday(slot.timeSlot)) {
      this.showPopup('Past Time Slot', 'This time slot has already passed.', 'warning');
      return;
    }

    this.selectedSlot = slot.timeSlot;
  }

  bookAppointment() {
    this.bookingErrorMessage.set('');
    this.bookingSuccessMessage.set('');

    if (this.isDoctorOnLeave()) {
      this.bookingErrorMessage.set('Doctor is on leave for selected date.');
      return;
    }

    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      this.bookingErrorMessage.set('Please fill all fields.');
      return;
    }

    if (this.isPastDate(this.selectedDate)) {
      this.bookingErrorMessage.set('Previous date appointment booking is not allowed.');
      return;
    }

    if (!this.isFutureSlotForToday(this.selectedSlot)) {
      this.bookingErrorMessage.set('Selected time slot has already passed.');
      return;
    }

    const data = {
      doctorId: this.selectedDoctorId,
      scheduledDate: this.selectedDate,
      timeSlot: this.selectedSlot
    };

    this.appointmentService.bookAppointment(data).subscribe({
      next: () => {
        this.showPopup('Appointment Booked', 'Appointment booked successfully.', 'success');
        this.bookingSuccessMessage.set('Appointment booked successfully.');

        setTimeout(() => {
          this.closeBooking();
          this.loadAppointments();
        }, 1200);
      },
      error: (err: any) => {
        this.bookingErrorMessage.set(err.error?.message || err.error || 'Booking failed.');
      }
    });
  }
}