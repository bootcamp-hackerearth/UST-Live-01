import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../shared/d_sidebar/d_sidebar';

import { AppointmentService } from '../../Doctor.service/appointmentservice';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, Sidebar],
  templateUrl: './d_appointments.html',
  styleUrls: ['./d_appointments.css']
})
export class DoctorAppointments implements OnInit {

  appointments: any[] = [];
  filteredAppointments: any[] = [];
  paginatedAppointments: any[] = [];

  totalCount = 0;
  pendingCount = 0;
  confirmedCount = 0;
  completedCount = 0;

  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  selected: any = null;

  constructor(private appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {
  this.appointmentService.getMyDoctorAppointments().subscribe({
    next: (res: any) => {  

      console.log("API response:", res);

      const list = res?.data || res || [];

      const data = list.map((a: any) => ({
        ...a,
        scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
      }));

      this.appointments = data;
      this.filteredAppointments = data;

      this.totalCount = data.length;

      this.pendingCount = data.filter((a: any) => a.status === 'Pending').length;

      this.confirmedCount = data.filter((a: any) => a.status === 'Confirmed').length;

      this.completedCount = data.filter((a: any) => a.status === 'Completed').length;

      this.updatePagination();
    },

    error: (err) => {
      console.error('Failed to load doctor appointments', err);
    }
  });
}

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

  // VIEW MODAL
  view(data: any) {
    this.selected = data;
  }

  closeView() {
    this.selected = null;
  }

  // ACTIONS
  confirm(id: number) {
    this.appointmentService.confirmAppointment(id).subscribe(() => {
      this.loadAppointments();
    });
  }

  complete(id: number) {
    this.appointmentService.completeAppointment(id).subscribe(() => {
      this.loadAppointments();
    });
  }
}