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

  constructor(private readonly appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (res: any) => {
        const appointments = this.formatAppointments(res);

        this.appointments = appointments;
        this.filteredAppointments = appointments;

        this.updateCounts(appointments);
        this.updatePagination();
      },
      error: (err) => {
        console.error('Failed to load doctor appointments', err);
      }
    });
  }

  private formatAppointments(res: any): any[] {
    const list = res?.data || res || [];

    return list.map((appointment: any) => ({
      ...appointment,
      scheduledDate: appointment.scheduledDate
        ? new Date(appointment.scheduledDate)
        : null
    }));
  }

  private updateCounts(appointments: any[]): void {
    this.totalCount = appointments.length;
    this.pendingCount = this.getStatusCount(appointments, 'Pending');
    this.confirmedCount = this.getStatusCount(appointments, 'Confirmed');
    this.completedCount = this.getStatusCount(appointments, 'Completed');
  }

  private getStatusCount(appointments: any[], status: string): number {
    return appointments.filter((appointment: any) => appointment.status === status).length;
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

  view(data: any) {
    this.selected = data;
  }

  closeView() {
    this.selected = null;
  }

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