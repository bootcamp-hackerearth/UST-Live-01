import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/sidebar/sidebar';

import { AppointmentService } from '../../Patient.service/appointmentservice';

@Component({
  selector: 'app-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './appointments.html',
  styleUrls: ['./appointments.css']
})
export class Appointments implements OnInit {

  allAppointments: any[] = [];
  filteredAppointments: any[] = [];
  paginatedAppointments: any[] = [];

  // FILTERS
  nameSearch = '';
  statusFilter = '';
  dateSearch = '';

  //  PAGINATION
  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  // BOOKING
  showBookingModal = false;
  selectedDoctorId: number | null = null;
  selectedDate = '';
  selectedSlot = '';
  selectedDoctor: any = null;
  availableSlots: string[] = [];
  allSlots: string[] = [  '09:00 AM',  '10:00 AM',  '11:00 AM',  '12:00 PM',  '01:00 PM',
  '02:00 PM',  '03:00 PM',  '04:00 PM',  '05:00 PM'];

  constructor(private appointmentService: AppointmentService) {}

ngOnInit(): void {
  this.loadAppointments();

  const data = localStorage.getItem('selectedDoctor');

  if (data) {
    this.selectedDoctor = JSON.parse(data);

    this.selectedDoctorId = this.selectedDoctor.doctorId;

    this.showBookingModal = true;
  }
}

  // LOAD
  loadAppointments() {
    this.appointmentService.getMyAppointments().subscribe(res => {
      this.allAppointments = (res || []).map((a: any) => ({
        ...a,
        scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
      }));

      this.filteredAppointments = this.allAppointments;
      this.updatePagination();
    });
  }

  // FILTER
filterAppointments() {
  this.filteredAppointments = this.allAppointments.filter(a => {

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


  this.pageNumber = 1;
  this.updatePagination();
  }

  // PAGINATION
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredAppointments.length / this.pageSize);
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber - 1) * this.pageSize;
    const end = start + this.pageSize;

    this.paginatedAppointments = this.filteredAppointments.slice(start, end);
  }

  nextPage() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.paginate();
    }
  }

  // CANCEL
  cancel(id: number) {
    const reason = prompt('Enter cancel reason');
    if (!reason) return;

    this.appointmentService.cancelAppointment(id, reason).subscribe(() => {
      this.loadAppointments();
    });
  }

  // BOOKING MODAL
  openBooking() {
    this.showBookingModal = true;
  }

  closeBooking() {
    this.showBookingModal = false;
    this.selectedDoctorId = null;
    this.selectedDate = '';
    this.selectedSlot = '';
    this.availableSlots = [];
  }

  // SLOT CHECK
checkSlots() {

  if (!this.selectedDoctorId || !this.selectedDate) return;

  const dateObj = new Date(this.selectedDate);

  this.appointmentService
    .checkDoctorAvailability(this.selectedDoctorId, dateObj)
    .subscribe((bookedSlots: string[]) => {

      this.availableSlots = this.allSlots.filter(
        slot => !bookedSlots.includes(slot)
      );

    });
}
  // BOOK
  bookAppointment() {
    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      alert('Fill all fields');
      return;
    }

  const data = {
  doctorId: this.selectedDoctorId,
  scheduledDate: this.selectedDate,
  timeSlot: this.selectedSlot
};


this.appointmentService.bookAppointment(data).subscribe({
  next: () => {
    alert('Appointment booked');
    this.closeBooking();
    this.loadAppointments();
  },
  error: (err: any) => {
  console.error("FULL ERROR:", err);
  console.error("ERROR BODY:", err.error);
      console.error("Validation error:", err.error?.errors);

}
});
  }

  
}