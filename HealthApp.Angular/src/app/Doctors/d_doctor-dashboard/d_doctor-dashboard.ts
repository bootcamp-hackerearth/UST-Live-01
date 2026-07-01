import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Sidebar } from '../shared/d_sidebar/d_sidebar';

import { AppointmentService } from '../../Doctor.service/appointmentservice';
import { doctorservice } from '../../Doctor.service/doctorservice';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, Sidebar],
  templateUrl: './d_doctor-dashboard.html',
  styleUrls: ['./d_doctor-dashboard.css']
})
export class DoctorDashboard implements OnInit {

  appointments: any[] = [];

  upcomingCount = 0;
  pendingCount = 0;
  confirmedCount = 0;
  completedCount = 0;

  doctor: any = null;

  showProfile = false;
  editMode = false;
  filteredAppointments: any[] = [];
  paginatedAppointments: any[] = [];
  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(
    private appointmentService: AppointmentService,
    private doctorService: doctorservice
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
    this.loadProfile();
  }

  loadAppointments() {
  this.appointmentService.getMyDoctorAppointments().subscribe({
    next: (res: any) => {

      const list = res?.data || res || [];

      const data = list.map((a: any) => ({
        ...a,
        scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
      }));

      this.appointments = data;
      this.filteredAppointments = data;

      this.upcomingCount = data.length;
      this.pendingCount = data.filter((a: any) => a.status === 'Pending').length;
      this.confirmedCount = data.filter((a: any) => a.status === 'Confirmed').length;
      this.completedCount = data.filter((a: any) => a.status === 'Completed').length;

      this.updatePagination();
    },
    error: (err) => console.error(err)
  });
}

  loadProfile() {
    this.doctorService.getMyProfile().subscribe({
      next: (res) => this.doctor = res,
      error: (err) => console.error(err)
    });
  }

  openProfile() { this.showProfile = true; }
  closeProfile() { this.showProfile = false; }

  toggleEdit() { this.editMode = !this.editMode; }

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

}