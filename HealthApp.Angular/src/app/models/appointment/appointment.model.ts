export interface Appointment {
  appointmentId: number;
  patientId?: number;
  doctorId: number;
  patientName: string;
  doctorName: string;
  scheduledDate: Date;
  
timeSlot:
  | '09:00 AM'
  | '10:00 AM'
  | '11:00 AM'
  | '12:00 PM'
  | '01:00 PM'
  | '02:00 PM'
  | '03:00 PM'
  | '04:00 PM'
  | '05:00 PM';

  status: 'Pending' | 'Confirmed' | 'Completed' | 'Cancelled';
  cancellationReason?: string;
}
