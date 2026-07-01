export interface Appointment {

  appointmentId: number;

  patientId: number;

  patientName?: string;

  doctorId: number;

  doctorName?: string;

  scheduledDate: string;

  timeSlot: string;

  status: number;

  cancellationReason?: string;

}