import { Component, OnInit, ChangeDetectorRef, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DoctorService } from '../../../services/doctor';
import { AppointmentService } from '../../../services/appointment';
import { Router } from '@angular/router';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './book-appointment.html',
  styleUrls: ['./book-appointment.css']
})
export class BookAppointment implements OnInit {

  private readonly doctorService = inject(DoctorService);
  private readonly appointmentService = inject(AppointmentService);
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly router = inject(Router);

  appointmentDate = '';
  selectedSpecialisation = '';
  searchText = '';

  selectedDoctor: any = null;
  selectedSlot = '';

  submitted = false;
  isBooking = false;

  successMessage = '';
  errorMessage = '';

  doctors: any[] = [];

  currentPage = 1;
  pageSize = 3;

  specialisations = [
    { value: '', label: 'All specialisations' },
    { value: 0, label: 'General Medicine' },
    { value: 1, label: 'Pediatrician' },
    { value: 2, label: 'Cardiology' },
    { value: 3, label: 'Dermatology' },
    { value: 4, label: 'Orthopaedics' }
  ];

  ngOnInit() {
    if (globalThis.window !== undefined) {
      this.loadDoctors();
    }
  }

  loadDoctors() {
    this.doctorService.getDoctors().subscribe({
      next: (res: any) => {
        console.log('Doctors for booking ✅:', res);

        this.doctors = (Array.isArray(res) ? res : res.items || res.data || [])
          .map((doctor: any) => ({
            ...doctor,
            availableSlots: []
          }));

        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Doctor load failed ❌:', err);
        this.showError('Unable to load doctors. Please try again.');
      }
    });
  }

  get todayDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  get dateError(): string {
    if (!this.submitted) {
      return '';
    }

    if (!this.appointmentDate) {
      return 'Please select appointment date.';
    }

    if (this.appointmentDate < this.todayDate) {
      return 'Past date is not allowed.';
    }

    return '';
  }

  get slotError(): string {
    if (!this.submitted) {
      return '';
    }

    if (!this.selectedDoctor || !this.selectedSlot) {
      return 'Please select a doctor slot.';
    }

    return '';
  }

  getSpecialisationName(value: number): string {
    switch (Number(value)) {
      case 0: return 'General Medicine';
      case 1: return 'Pediatrician';
      case 2: return 'Cardiology';
      case 3: return 'Dermatology';
      case 4: return 'Orthopaedics';
      default: return 'Other';
    }
  }

  get isValidAppointmentDate(): boolean {
    return !!this.appointmentDate && this.appointmentDate >= this.todayDate;
  }

  get displayAppointmentDate(): string {
    if (!this.isValidAppointmentDate) {
      return 'selected date';
    }

    const date = new Date(this.appointmentDate);

    return date.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  filteredDoctors() {
    return this.doctors.filter((d: any) => {
      const matchesSearch =
        !this.searchText ||
        d.fullName.toLowerCase().includes(this.searchText.toLowerCase());

      const matchesSpec =
        this.selectedSpecialisation === '' ||
        d.specialisation == this.selectedSpecialisation;

      return matchesSearch && matchesSpec;
    });
  }

  pagedDoctors() {
    const filtered = this.filteredDoctors();
    const startIndex = (this.currentPage - 1) * this.pageSize;

    return filtered.slice(startIndex, startIndex + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredDoctors().length / this.pageSize);
  }

  get pageNumbers(): number[] {
    return Array.from({ length: this.totalPages }, (_, index) => index + 1);
  }

  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.clearAvailableSlots();
    this.refreshAvailabilityForDisplayedDoctors();
  }

  previousPage(): void {
    if (this.currentPage <= 1) {
      return;
    }

    this.currentPage--;
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.clearAvailableSlots();
    this.refreshAvailabilityForDisplayedDoctors();
  }

  nextPage(): void {
    if (this.currentPage >= this.totalPages) {
      return;
    }

    this.currentPage++;
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.clearAvailableSlots();
    this.refreshAvailabilityForDisplayedDoctors();
  }

  onSearchCriteriaChanged(): void {
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.currentPage = 1;

    this.clearAvailableSlots();

    this.refreshAvailabilityForDisplayedDoctors();
  }

  clearAvailableSlots(): void {
    this.doctors = this.doctors.map((doctor: any) => ({
      ...doctor,
      availableSlots: []
    }));
  }

  refreshAvailabilityForDisplayedDoctors(): void {
    if (!this.isValidAppointmentDate) {
      return;
    }

    if (this.selectedSpecialisation === '') {
      console.log('Select specialisation to load doctor availability.');
      return;
    }

    const visibleDoctors = this.pagedDoctors();

    visibleDoctors.forEach((doctor: any) => {
      this.doctorService
        .getDoctorAvailability(doctor.doctorId, this.appointmentDate)
        .subscribe({
          next: availability => {
            console.log('Doctor availability from Garnet/API ✅:', availability);

            doctor.isActive = availability.isActive;
            doctor.availableSlots = availability.availableSlots || [];

            if (
              this.selectedDoctor?.doctorId === doctor.doctorId &&
              this.selectedSlot &&
              !doctor.availableSlots.includes(this.selectedSlot)
            ) {
              this.selectedDoctor = null;
              this.selectedSlot = '';
            }

            this.cdr.detectChanges();
          },
          error: err => {
            console.error('Doctor availability load failed ❌:', err);
            this.showError('Unable to load doctor availability.');
          }
        });
    });
  }

  clearFilters() {
    this.appointmentDate = '';
    this.selectedSpecialisation = '';
    this.searchText = '';
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.submitted = false;
    this.currentPage = 1;

    this.clearAvailableSlots();
  }

  selectSlot(doctor: any, slot: string) {
    this.selectedDoctor = doctor;
    this.selectedSlot = slot;
  }

  confirmAppointment(doctor: any) {
    this.submitted = true;

    if (!this.appointmentDate || this.appointmentDate < this.todayDate) {
      return;
    }

    if (!this.selectedSlot || this.selectedDoctor?.doctorId !== doctor.doctorId) {
      return;
    }

    const patientId = Number(localStorage.getItem('patientId'));

    if (!patientId) {
      this.showError('Patient profile not loaded. Please open Profile once and try again.');
      return;
    }

    const payload = {
      patientId: patientId,
      doctorId: doctor.doctorId,
      scheduledDate: this.appointmentDate,
      timeSlot: this.selectedSlot
    };

    console.log('Booking payload ✅:', payload);

    this.isBooking = true;

    this.appointmentService.bookAppointment(payload).subscribe({
      next: (res: any) => {
        console.log('Appointment booked ✅:', res);

        this.isBooking = false;
        this.submitted = false;
        this.selectedDoctor = null;
        this.selectedSlot = '';

        this.successMessage = 'Appointment booked successfully';
        this.errorMessage = '';

        this.cdr.detectChanges();

        setTimeout(() => {
          this.router.navigate(['/patient/appointments']);
        }, 2500);
      },
      error: (err: any) => {
        console.error('Booking failed ❌:', err);

        this.isBooking = false;

        const message =
          err?.error?.message ||
          err?.error ||
          'Booking failed. Please check selected date/time.';

        this.showError(message);
      }
    });
  }

  private showError(message: string): void {
    this.errorMessage = message;
    this.successMessage = '';

    this.cdr.detectChanges();

    setTimeout(() => {
      this.errorMessage = '';
      this.cdr.detectChanges();
    }, 3000);
  }
}