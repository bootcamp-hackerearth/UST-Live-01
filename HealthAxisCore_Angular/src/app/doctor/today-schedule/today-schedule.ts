import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

interface DoctorAppointment {
  appointmentId: number;
  patientId: number;
  patientName: string;
  age: number;
  timeSlot: string;
  reason: string;
  status: string;
}

@Component({
  selector: 'app-today-schedule',
  imports: [
    RouterLink
  ],
  templateUrl: './today-schedule.html',
  styleUrl: './today-schedule.css'
})
export class TodaySchedule {
  appointments: DoctorAppointment[] = [
    {
      appointmentId: 1,
      patientId: 101,
      patientName: 'Arun Menon',
      age: 28,
      timeSlot: '09:30 AM',
      reason: 'Skin allergy follow-up',
      status: 'Confirmed'
    },
    {
      appointmentId: 2,
      patientId: 102,
      patientName: 'Meera Thomas',
      age: 34,
      timeSlot: '10:30 AM',
      reason: 'Routine consultation',
      status: 'Pending'
    },
    {
      appointmentId: 3,
      patientId: 103,
      patientName: 'Nikhil Rao',
      age: 41,
      timeSlot: '11:30 AM',
      reason: 'Medication review',
      status: 'Completed'
    }
  ];

  markCompleted(appointment: DoctorAppointment): void {
    appointment.status = 'Completed';

    console.log('Marked appointment completed:', appointment);
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }
}
