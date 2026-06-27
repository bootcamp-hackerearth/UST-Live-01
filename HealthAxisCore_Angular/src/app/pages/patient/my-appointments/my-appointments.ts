import { Component } from '@angular/core';

interface AppointmentCard {
  appointmentId: number;
  doctorName: string;
  specialisation: string;
  scheduledDate: string;
  timeSlot: string;
  status: string;
}

@Component({
  selector: 'app-my-appointments',
  imports: [],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css'
})
export class MyAppointments {
  appointments: AppointmentCard[] = [
    {
      appointmentId: 1,
      doctorName: 'Dr. Isha Nair',
      specialisation: 'Dermatologist',
      scheduledDate: '2026-06-26',
      timeSlot: '04:30 PM',
      status: 'Confirmed'
    },
    {
      appointmentId: 2,
      doctorName: 'Dr. Aravind Menon',
      specialisation: 'Cardiologist',
      scheduledDate: '2026-06-27',
      timeSlot: '10:00 AM',
      status: 'Pending'
    },
    {
      appointmentId: 3,
      doctorName: 'Dr. Meera Thomas',
      specialisation: 'Pediatrician',
      scheduledDate: '2026-06-20',
      timeSlot: '11:30 AM',
      status: 'Completed'
    }
  ];

  cancelAppointment(appointment: AppointmentCard): void {
    appointment.status = 'Cancelled';

    console.log('Cancelled appointment template:', appointment);
  }

  getStatusClass(status: string): string {
    return `status-badge status-${status.toLowerCase()}`;
  }
}
