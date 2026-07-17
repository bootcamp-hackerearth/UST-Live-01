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
  selected = signal<any>(null);

  totalCount = signal(0);
  pendingCount = signal(0);
  confirmedCount = signal(0);
  completedCount = signal(0);

  pageNumber = signal(1);
  totalPages = signal(0);

  readonly pageSize = 5;

  constructor(
    private readonly appointmentService: AppointmentService
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
  }

  private loadAppointments(): void {
    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (res) => {
        const appointments = this.formatAppointments(res);

        this.appointments.set(appointments);
        this.filteredAppointments.set(appointments);

        this.updateCounts(appointments);
        this.updatePagination();
      },
      error: (err) => {
        this.errorMessage.set(
          err?.error?.message ?? 'Failed to load doctor appointments'
        );
      }
    });
  }

  private formatAppointments(res: any): any[] {
    const list = res?.data ?? res ?? [];

    return list.map((appointment: any) => ({
      ...appointment,
      scheduledDate: appointment.scheduledDate
        ? new Date(appointment.scheduledDate)
        : null
    }));
  }

  private updateCounts(appointments: any[]): void {
    this.totalCount.set(appointments.length);

    const statuses = appointments.reduce(
      (acc, appointment) => {
        acc[appointment.status] = (acc[appointment.status] || 0) + 1;
        return acc;
      },
      {} as Record<string, number>
    );

    this.pendingCount.set(statuses['Pending'] || 0);
    this.confirmedCount.set(statuses['Confirmed'] || 0);
    this.completedCount.set(statuses['Completed'] || 0);
  }

  private updatePagination(): void {
    this.totalPages.set(
      Math.ceil(this.filteredAppointments().length / this.pageSize)
    );

    this.paginate();
  }

  private paginate(): void {
    const start = (this.pageNumber() - 1) * this.pageSize;

    this.paginatedAppointments.set(
      this.filteredAppointments().slice(start, start + this.pageSize)
    );
  }

  changePage(offset: number): void {
    const nextPage = this.pageNumber() + offset;

    if (nextPage >= 1 && nextPage <= this.totalPages()) {
      this.pageNumber.set(nextPage);
      this.paginate();
    }
  }

  prevPage(): void {
    this.changePage(-1);
  }

  nextPage(): void {
    this.changePage(1);
  }

  view(data: any): void {
    this.selected.set(data);
  }

  closeView(): void {
    this.selected.set(null);
  }

  updateAppointment(
    id: number,
    action: (id: number) => any
  ): void {
    action.call(this.appointmentService, id)
      .subscribe(() => this.loadAppointments());
  }

  confirm(id: number): void {
    this.updateAppointment(
      id,
      this.appointmentService.confirmAppointment
    );
  }

  complete(id: number): void {
    this.updateAppointment(
      id,
      this.appointmentService.completeAppointment
    );
  }
}