import { Component, OnInit, signal } from '@angular/core';
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

  appointments = signal<any[]>([]);
  filteredAppointments = signal<any[]>([]);
  paginatedAppointments = signal<any[]>([]);

  errorMessage = signal('');

  totalCount = signal(0);
  pendingCount = signal(0);
  confirmedCount = signal(0);
  completedCount = signal(0);

  pageNumber = signal(1);
  pageSize = 5;
  totalPages = signal(0);

  selected = signal<any>(null);

  constructor(private readonly appointmentService: AppointmentService) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {
    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (res: any) => {
        const appointments = this.formatAppointments(res);

        this.appointments.set(appointments);
        this.filteredAppointments.set(appointments);

        this.updateCounts(appointments);
        this.updatePagination();
      },
      error: (err) => {
        this.errorMessage.set(err?.error?.message || 'Failed to load doctor appointments');
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
    this.totalCount.set(appointments.length);
    this.pendingCount.set(this.getStatusCount(appointments, 'Pending'));
    this.confirmedCount.set(this.getStatusCount(appointments, 'Confirmed'));
    this.completedCount.set(this.getStatusCount(appointments, 'Completed'));
  }

  private getStatusCount(appointments: any[], status: string): number {
    return appointments.filter((appointment: any) => appointment.status === status).length;
  }

  updatePagination() {
    this.totalPages.set(Math.ceil(this.filteredAppointments().length / this.pageSize));
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

  view(data: any) {
    this.selected.set(data);
  }

  closeView() {
    this.selected.set(null);
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