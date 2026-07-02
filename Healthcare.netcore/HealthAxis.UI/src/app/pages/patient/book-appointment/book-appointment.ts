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

  private doctorService = inject(DoctorService);
  private appointmentService = inject(AppointmentService);
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router);

  appointmentDate = '';
  selectedSpecialisation = '';
  searchText = '';

  selectedDoctor: any = null;
  selectedSlot = '';

  submitted = false;
  isBooking = false;

  doctors: any[] = [];

  slots = ['09:00', '09:30', '10:00', '14:00'];

  specialisations = [
    { value: '', label: 'All specialisations' },
    { value: 0, label: 'General Medicine' },
    { value: 1, label: 'Pediatrician' },
    { value: 2, label: 'Cardiology' },
    { value: 3, label: 'Dermatology' },
    { value: 4, label: 'Orthopaedics' }
  ];

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadDoctors();
    }
  }
  
  loadDoctors() {
    this.doctorService.getDoctors().subscribe({
      next: (res: any) => {
        console.log('Doctors for booking ✅:', res);

        this.doctors = res.items || res.data || [];
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        console.error('Doctor load failed ❌:', err);
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
    switch (value) {
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

  clearFilters() {
    this.appointmentDate = '';
    this.selectedSpecialisation = '';
    this.searchText = '';
    this.selectedDoctor = null;
    this.selectedSlot = '';
    this.submitted = false;
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
    alert('Patient profile not loaded. Please open Profile once and try again.');
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

      //  stop loading first
      this.isBooking = false;
      this.submitted = false;
      this.selectedDoctor = null;
      this.selectedSlot = '';

      //  show success
      alert('Appointment booked successfully ✅');

      //  go to appointments page so it reloads data
      this.router.navigate(['/patient/appointments']);
    },
    error: (err: any) => {
      console.error('Booking failed ❌:', err);

      this.isBooking = false;

      const message =
        err?.error?.message ||
        err?.error ||
        'Booking failed. Please check selected date/time.';

      alert(message);
    }
  });
}
}