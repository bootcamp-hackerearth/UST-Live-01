import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/sidebar/sidebar';

import { AppointmentService } from '../../Patient.service/appointmentservice';
import { DoctorSlot } from '../../models/appointment/doctor-availability.model';
import { AppPopupComponent } from '../../shared/app-popup/app-popup';

interface AppointmentViewModel {
  appointmentId: number;
  patientId?: number;
  doctorId: number;
  patientName?: string;
  doctorName?: string;
  scheduledDate: Date | null;
  timeSlot?: string;
  status?: string;
  cancellationReason?: string;
}

type PopupType = 'success' | 'error' | 'warning';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar, AppPopupComponent],
  templateUrl: './appointments.html',
  styleUrls: ['./appointments.css']
})
export class Appointments implements OnInit {
  readonly pageSize = 5;
  private readonly statusClassMap: Record<string, string> = {
    Pending: 'pending',
    Confirmed: 'confirmed',
    Completed: 'completed',
    Cancelled: 'cancelled'
  };

  allAppointments = signal<AppointmentViewModel[]>([]);
  filteredAppointments = signal<AppointmentViewModel[]>([]);
  paginatedAppointments = signal<AppointmentViewModel[]>([]);
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
  popupType = signal<PopupType>('success');

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

  getStatusClass(status?: string): string {
    return this.statusClassMap[status ?? ''] ?? 'pending';
  }

  private showPopup(title: string, message: string, type: PopupType = 'success') {
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

  private getDateOnly(value: string | Date | null): Date | null {
    if (!value) {
      return null;
    }


  const parsedDate = new Date(value);

    if (Number.isNaN(parsedDate.getTime())) {
      return null;
    }

    parsedDate.setHours(0, 0, 0, 0);
    return parsedDate;
  }

  private isPastDate(dateValue: string): boolean {
    const selectedDate = this.getDateOnly(dateValue);
    const today = this.getDateOnly(new Date());

    return !!selectedDate && !!today && selectedDate < today;
  }

  private isToday(dateValue: string): boolean {
    const selectedDate = this.getDateOnly(dateValue);
    const today = this.getDateOnly(new Date());

    return !!selectedDate && !!today && selectedDate.getTime() === today.getTime();
  }

  private convertSlotToDate(timeSlot: string, dateValue: string): Date | null {
    if (!timeSlot || !dateValue) {
      return null;
    }

    const selectedDate = this.getDateOnly(dateValue);

    if (!selectedDate) {
      return null;
    }

    const cleanedSlot = timeSlot.trim().toUpperCase();
    const timeRegex = /^(\d{1,2}):(\d{2})\s?(AM|PM)$/;
    const twelveHourMatch = timeRegex.exec(cleanedSlot);

    let hours = 0;
    let minutes = 0;

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
      const twentyFourHourRegex = /^(\d{1,2}):(\d{2})$/;
      const twentyFourHourMatch = twentyFourHourRegex.exec(cleanedSlot);

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

  private normalizeAppointments(appointments: AppointmentViewModel[]): AppointmentViewModel[] {
    return appointments.map((appointment) => ({
      ...appointment,
      scheduledDate: appointment.scheduledDate ? new Date(appointment.scheduledDate) : null
    }));
  }

  private updatePagination() {
    const totalPages = Math.ceil(this.filteredAppointments().length / this.pageSize);
    this.totalPages.set(totalPages <= 0 ? 1 : totalPages);
    this.paginate();
  }

  private applyFilters() {
    const filtered = this.allAppointments().filter((appointment) => {
      const matchName = !this.nameSearch || appointment.doctorName?.toLowerCase().includes(this.nameSearch.toLowerCase());
      const matchStatus = !this.statusFilter || appointment.status === this.statusFilter;

      let matchDate = true;

      if (this.dateSearch) {
        const selectedDate = this.getDateOnly(this.dateSearch);
        const appointmentDate = this.getDateOnly(appointment.scheduledDate);
        matchDate = !!selectedDate && !!appointmentDate && appointmentDate.getTime() === selectedDate.getTime();
      }

      return matchName && matchStatus && matchDate;
    });

    this.filteredAppointments.set(filtered);
    this.pageNumber.set(1);
    this.updatePagination();
  }

  private resetBookingState() {
    this.availableSlots.set([]);
    this.availabilityMessage.set('');
    this.isDoctorOnLeave.set(false);
    this.bookingErrorMessage.set('');
    this.bookingSuccessMessage.set('');
  }

  private getBookingValidationError(): string | null {
    if (this.isDoctorOnLeave()) {
      return 'Doctor is on leave for selected date.';
    }

    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      return 'Please fill all fields.';
    }

    if (this.isPastDate(this.selectedDate)) {
      return 'Previous date appointment booking is not allowed.';
    }

    if (!this.isFutureSlotForToday(this.selectedSlot)) {
      return 'Selected time slot has already passed.';
    }

    return null;
  }

  loadAppointments() {
    this.appointmentService.getMyAppointments().subscribe((res) => {
      const appointments = this.normalizeAppointments((res || []) as AppointmentViewModel[]);

      this.allAppointments.set(appointments);
      this.filteredAppointments.set(appointments);
      this.updatePagination();
    });
  }

  filterAppointments() {
    this.applyFilters();
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedAppointments.set(this.filteredAppointments().slice(start, end));
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((value) => value + 1);
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((value) => value - 1);
      this.paginate();
    }
  }

  cancel(id: number): void {
    this.selectedAppointmentId = id;
    this.cancelReason = '';
    this.showCancelModal.set(true);
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
      .cancelAppointment(this.selectedAppointmentId, this.cancelReason)
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
    this.resetBookingState();

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

          const slots = (res.slots || []).map((slot: DoctorSlot) => ({
            ...slot,
            isAvailable: slot.isAvailable && this.isFutureSlotForToday(slot.timeSlot),
            status: slot.status
          }));

          this.availableSlots.set(slots);

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

    const validationError = this.getBookingValidationError();

    if (validationError) {
      this.bookingErrorMessage.set(validationError);
      return;
    }

    if (!this.selectedDoctorId) {
      this.bookingErrorMessage.set('Please select a doctor before booking.');
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