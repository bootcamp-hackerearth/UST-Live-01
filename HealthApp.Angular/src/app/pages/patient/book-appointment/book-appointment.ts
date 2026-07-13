import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { AppointmentBookingDto } from '../../../dtos/appointment.dto';
import {
  DoctorAvailabilityDto,
  DoctorAvailabilitySlotDto,
  DoctorDto
} from '../../../dtos/doctor.dto';

import { AppointmentService } from '../../../core/services/appointment.service';
import { DoctorService } from '../../../core/services/doctor.service';
import { NotificationService } from '../../../core/services/notification.service';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

interface SpecialisationOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment implements OnInit {
  searchText = '';
  selectedSpecialisation = 'All';
  selectedDate = '';
  sortBy = 'nameAsc';

  doctors: DoctorDto[] = [];
  isLoadingDoctors = false;

  expandedDetailsDoctorId: number | null = null;
  expandedSlotsDoctorId: number | null = null;
  loadingSlotsDoctorId: number | null = null;
  bookingDoctorId: number | null = null;

  doctorAvailability: Record<number, DoctorAvailabilityDto> = {};
  selectedSlots: Record<number, string> = {};
  slotPageIndexes: Record<number, number> = {};

  readonly slotPageSize = 4;
  readonly minDate = this.getTodayDate();

  readonly specialisations: SpecialisationOption[] = [
    { label: 'All Specialisations', value: 'All' },
    { label: 'General Physician', value: 'GeneralPhysician' },
    { label: 'Cardiologist', value: 'Cardiologist' },
    { label: 'Dermatologist', value: 'Dermatologist' },
    { label: 'Neurologist', value: 'Neurologist' },
    { label: 'Orthopedic', value: 'Orthopedic' },
    { label: 'Pediatrician', value: 'Pediatrician' },
    { label: 'Psychiatrist', value: 'Psychiatrist' },
    { label: 'ENT', value: 'ENT' },
    { label: 'Gynecologist', value: 'Gynecologist' }
  ];

  readonly sortOptions = [
    { label: 'Name: A to Z', value: 'nameAsc' },
    { label: 'Fee: Low to High', value: 'feeAsc' },
    { label: 'Fee: High to Low', value: 'feeDesc' },
    { label: 'Experience: High to Low', value: 'experienceDesc' },
    { label: 'Experience: Low to High', value: 'experienceAsc' }
  ];

  constructor(
    private readonly doctorService: DoctorService,
    private readonly appointmentService: AppointmentService,
    private readonly notificationService: NotificationService,
    private readonly router: Router,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadDoctors();
  }

  get sortedDoctors(): DoctorDto[] {
    const doctors = [...this.doctors];

    switch (this.sortBy) {
      case 'feeAsc':
        return doctors.sort((a, b) => a.consultationFee - b.consultationFee);
      case 'feeDesc':
        return doctors.sort((a, b) => b.consultationFee - a.consultationFee);
      case 'experienceDesc':
        return doctors.sort((a, b) => b.yearsOfExperience - a.yearsOfExperience);
      case 'experienceAsc':
        return doctors.sort((a, b) => a.yearsOfExperience - b.yearsOfExperience);
      case 'nameAsc':
      default:
        return doctors.sort((a, b) => a.fullName.localeCompare(b.fullName));
    }
  }

  loadDoctors(): void {
    this.isLoadingDoctors = true;
    this.cdr.markForCheck();

    const specialisation =
      this.selectedSpecialisation === 'All'
        ? undefined
        : this.selectedSpecialisation;

    this.doctorService
      .getDoctors(this.searchText, specialisation, true)
      .pipe(
        finalize(() => {
          this.isLoadingDoctors = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: doctors => {
          this.doctors = doctors ?? [];
          this.resetDoctorPanels();
          this.cdr.markForCheck();
        },
        error: () => {
          this.doctors = [];
          this.resetDoctorPanels();
          this.cdr.markForCheck();
        }
      });
  }

  searchDoctors(): void {
    this.loadDoctors();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedSpecialisation = 'All';
    this.sortBy = 'nameAsc';
    this.loadDoctors();
  }

  onDateChanged(): void {
    this.expandedSlotsDoctorId = null;
    this.doctorAvailability = {};
    this.selectedSlots = {};
    this.slotPageIndexes = {};
    this.cdr.markForCheck();
  }

  toggleDetails(doctorId: number): void {
    this.expandedDetailsDoctorId =
      this.expandedDetailsDoctorId === doctorId ? null : doctorId;
    this.cdr.markForCheck();
  }

  toggleSlots(doctor: DoctorDto): void {
    if (!doctor.isActive) {
      this.notificationService.warning('This doctor is currently unavailable.');
      return;
    }

    if (!this.selectedDate) {
      this.notificationService.warning('Please select an appointment date first.');
      return;
    }

    if (this.expandedSlotsDoctorId === doctor.doctorId) {
      this.expandedSlotsDoctorId = null;
      this.cdr.markForCheck();
      return;
    }

    this.expandedSlotsDoctorId = doctor.doctorId;
    this.cdr.markForCheck();

    if (!this.areSlotsLoaded(doctor.doctorId)) {
      this.loadAvailability(doctor.doctorId);
    }
  }

  loadAvailability(doctorId: number): void {
    if (!this.selectedDate) {
      this.notificationService.warning('Please select an appointment date first.');
      return;
    }

    this.loadingSlotsDoctorId = doctorId;
    delete this.doctorAvailability[doctorId];
    this.selectedSlots[doctorId] = '';
    this.slotPageIndexes[doctorId] = 0;
    this.cdr.markForCheck();

    this.doctorService
      .getDoctorAvailability(doctorId, this.selectedDate)
      .pipe(
        finalize(() => {
          this.loadingSlotsDoctorId = null;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: availability => {
          this.doctorAvailability[doctorId] = availability;

          if (availability.isDoctorOnLeave) {
            this.selectedSlots[doctorId] = '';
          } else if (!availability.slots.some(slot => slot.isAvailable)) {
            this.notificationService.info('No slots are available for the selected date.');
          }

          this.cdr.markForCheck();
        },
        error: () => {
          delete this.doctorAvailability[doctorId];
          this.selectedSlots[doctorId] = '';
          this.cdr.markForCheck();
        }
      });
  }

  selectSlot(doctorId: number, slot: DoctorAvailabilitySlotDto): void {
    if (!slot.isAvailable || this.isDoctorOnLeave(doctorId)) {
      return;
    }

    this.selectedSlots[doctorId] = slot.timeSlot;
    this.cdr.markForCheck();
  }

  getAvailability(doctorId: number): DoctorAvailabilityDto | null {
    return this.doctorAvailability[doctorId] ?? null;
  }

  getAllSlots(doctorId: number): DoctorAvailabilitySlotDto[] {
    return this.getAvailability(doctorId)?.slots ?? [];
  }

  getPagedSlots(doctorId: number): DoctorAvailabilitySlotDto[] {
    const slots = this.getAllSlots(doctorId);
    const startIndex = this.getCurrentSlotPage(doctorId) * this.slotPageSize;
    return slots.slice(startIndex, startIndex + this.slotPageSize);
  }

  getAvailableSlotCount(doctorId: number): number {
    return this.getAllSlots(doctorId).filter(slot => slot.isAvailable).length;
  }

  isDoctorOnLeave(doctorId: number): boolean {
    return this.getAvailability(doctorId)?.isDoctorOnLeave ?? false;
  }

  getAvailabilityMessage(doctorId: number): string {
    return this.getAvailability(doctorId)?.message ?? '';
  }

  areSlotsLoaded(doctorId: number): boolean {
    return Object.hasOwn(this.doctorAvailability, doctorId);
  }

  getCurrentSlotPage(doctorId: number): number {
    return this.slotPageIndexes[doctorId] ?? 0;
  }

  getTotalSlotPages(doctorId: number): number {
    const count = this.getAllSlots(doctorId).length;
    return Math.max(1, Math.ceil(count / this.slotPageSize));
  }

  goToPreviousSlotPage(doctorId: number): void {
    const currentPage = this.getCurrentSlotPage(doctorId);
    if (currentPage > 0) {
      this.slotPageIndexes[doctorId] = currentPage - 1;
      this.cdr.markForCheck();
    }
  }

  goToNextSlotPage(doctorId: number): void {
    const currentPage = this.getCurrentSlotPage(doctorId);
    const totalPages = this.getTotalSlotPages(doctorId);

    if (currentPage < totalPages - 1) {
      this.slotPageIndexes[doctorId] = currentPage + 1;
      this.cdr.markForCheck();
    }
  }

  getSlotClass(slot: DoctorAvailabilitySlotDto): string {
    if (slot.status === 'DoctorOnLeave') {
      return 'slot-on-leave';
    }

    if (slot.status === 'Booked' || !slot.isAvailable) {
      return 'slot-booked';
    }

    return 'slot-available';
  }

  getSlotLabel(slot: DoctorAvailabilitySlotDto): string {
    if (slot.status === 'DoctorOnLeave') {
      return 'Doctor On Leave';
    }

    return slot.isAvailable ? 'Available' : 'Booked';
  }

  bookAppointment(doctor: DoctorDto): void {
    const selectedSlot = this.selectedSlots[doctor.doctorId];

    if (!doctor.isActive) {
      this.notificationService.warning('This doctor is currently unavailable.');
      return;
    }

    if (!this.selectedDate) {
      this.notificationService.warning('Please select an appointment date.');
      return;
    }

    if (this.isDoctorOnLeave(doctor.doctorId)) {
      this.notificationService.warning(
        this.getAvailabilityMessage(doctor.doctorId) ||
          'This doctor is on leave for the selected date.'
      );
      return;
    }

    if (!selectedSlot) {
      this.notificationService.warning('Please select an available time slot.');
      return;
    }

    const selectedAvailabilitySlot = this.getAllSlots(doctor.doctorId)
      .find(slot => slot.timeSlot === selectedSlot);

    if (!selectedAvailabilitySlot?.isAvailable) {
      this.selectedSlots[doctor.doctorId] = '';
      this.notificationService.warning('The selected slot is no longer available.');
      return;
    }

    const payload: AppointmentBookingDto = {
      doctorId: doctor.doctorId,
      scheduledDate: this.selectedDate,
      timeSlot: selectedSlot
    };

    this.bookingDoctorId = doctor.doctorId;
    this.cdr.markForCheck();

    this.appointmentService
      .bookAppointment(payload)
      .pipe(
        finalize(() => {
          this.bookingDoctorId = null;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            `Appointment request sent to ${doctor.fullName} for ${this.selectedDate} at ${selectedSlot}.`
          );

          this.selectedSlots[doctor.doctorId] = '';

          setTimeout(() => {
            this.router.navigate(['/patient/appointments']);
          }, 700);
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  private resetDoctorPanels(): void {
    this.expandedDetailsDoctorId = null;
    this.expandedSlotsDoctorId = null;
    this.doctorAvailability = {};
    this.selectedSlots = {};
    this.slotPageIndexes = {};
  }

  private getTodayDate(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}