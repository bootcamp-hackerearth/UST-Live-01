import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';

import { AppointmentBookingDto } from '../../../dtos/appointment.dto';
import { DoctorDto } from '../../../dtos/doctor.dto';

import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { NotificationService } from '../../../core/services/notification.service';

interface SpecialisationOption {
  label: string;
  value: string;
}

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule],
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

  doctorSlots: Record<number, string[]> = {};
  selectedSlots: Record<number, string> = {};
  slotPageIndexes: Record<number, number> = {};

  readonly slotPageSize = 4;

  minDate = this.getTodayDate();

  specialisations: SpecialisationOption[] = [
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

  sortOptions = [
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
    const doctorsCopy = [...this.doctors];

    switch (this.sortBy) {
      case 'feeAsc':
        return doctorsCopy.sort(
          (a, b) => a.consultationFee - b.consultationFee
        );

      case 'feeDesc':
        return doctorsCopy.sort(
          (a, b) => b.consultationFee - a.consultationFee
        );

      case 'experienceDesc':
        return doctorsCopy.sort(
          (a, b) => b.yearsOfExperience - a.yearsOfExperience
        );

      case 'experienceAsc':
        return doctorsCopy.sort(
          (a, b) => a.yearsOfExperience - b.yearsOfExperience
        );

      case 'nameAsc':
      default:
        return doctorsCopy.sort((a, b) =>
          a.fullName.localeCompare(b.fullName)
        );
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

          this.expandedDetailsDoctorId = null;
          this.expandedSlotsDoctorId = null;

          this.doctorSlots = {};
          this.selectedSlots = {};
          this.slotPageIndexes = {};

          this.cdr.markForCheck();
        },
        error: () => {
          this.doctors = [];
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
    this.selectedDate = '';
    this.sortBy = 'nameAsc';

    this.expandedDetailsDoctorId = null;
    this.expandedSlotsDoctorId = null;

    this.doctorSlots = {};
    this.selectedSlots = {};
    this.slotPageIndexes = {};

    this.loadDoctors();
  }

  onDateChanged(): void {
    this.expandedSlotsDoctorId = null;

    this.doctorSlots = {};
    this.selectedSlots = {};
    this.slotPageIndexes = {};

    this.cdr.markForCheck();

    if (this.selectedDate) {
      this.notificationService.info(
        'Select a doctor and click View Slots to check availability.'
      );
    }
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

    if (this.areSlotsLoaded(doctor.doctorId)) {
      return;
    }

    this.loadAvailability(doctor.doctorId);
  }

  loadAvailability(doctorId: number): void {
    if (!this.selectedDate) {
      this.notificationService.warning('Please select an appointment date first.');
      return;
    }

    this.loadingSlotsDoctorId = doctorId;
    this.doctorSlots[doctorId] = [];
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
        next: slots => {
          this.doctorSlots[doctorId] = slots ?? [];

          if (!slots || slots.length === 0) {
            this.notificationService.info(
              'No slots available for the selected date.'
            );
          }

          this.cdr.markForCheck();
        },
        error: () => {
          this.doctorSlots[doctorId] = [];
          this.cdr.markForCheck();
        }
      });
  }

  selectSlot(doctorId: number, slot: string): void {
    this.selectedSlots[doctorId] = slot;
    this.cdr.markForCheck();
  }

  getSlots(doctorId: number): string[] {
    return this.doctorSlots[doctorId] ?? [];
  }

  areSlotsLoaded(doctorId: number): boolean {
    return Object.hasOwn(this.doctorSlots, doctorId);
  }

  getSlotCount(doctorId: number): number {
    return this.getSlots(doctorId).length;
  }

  getCurrentSlotPage(doctorId: number): number {
    return this.slotPageIndexes[doctorId] ?? 0;
  }

  getTotalSlotPages(doctorId: number): number {
    const slotCount = this.getSlotCount(doctorId);

    if (slotCount === 0) {
      return 1;
    }

    return Math.ceil(slotCount / this.slotPageSize);
  }

  getPagedSlots(doctorId: number): string[] {
    const slots = this.getSlots(doctorId);
    const currentPage = this.getCurrentSlotPage(doctorId);

    const startIndex = currentPage * this.slotPageSize;
    const endIndex = startIndex + this.slotPageSize;

    return slots.slice(startIndex, endIndex);
  }

  goToPreviousSlotPage(doctorId: number): void {
    const currentPage = this.getCurrentSlotPage(doctorId);

    if (currentPage <= 0) {
      return;
    }

    this.slotPageIndexes[doctorId] = currentPage - 1;
    this.cdr.markForCheck();
  }

  goToNextSlotPage(doctorId: number): void {
    const currentPage = this.getCurrentSlotPage(doctorId);
    const totalPages = this.getTotalSlotPages(doctorId);

    if (currentPage >= totalPages - 1) {
      return;
    }

    this.slotPageIndexes[doctorId] = currentPage + 1;
    this.cdr.markForCheck();
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

    if (!selectedSlot) {
      this.notificationService.warning('Please select a time slot.');
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

  private getTodayDate(): string {
    const today = new Date();

    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
