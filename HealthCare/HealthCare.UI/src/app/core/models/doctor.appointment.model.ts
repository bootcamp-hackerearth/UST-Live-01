export interface DoctorAppointment {

  appointmentId: number;

  patientId: number;

  healthRecordId?: number;

  patientName: string;

  scheduledDate: string;

  timeSlot: string;

  status: string;

}