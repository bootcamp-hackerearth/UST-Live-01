export interface PatientProfile {
  patientId: number;
  fullName: string;
  dateOfBirth?: string;
  gender?: number | string;
  phoneNumber?: string;
  email?: string;
  insuranceNumber?: string;
  isActive?: boolean;
}

export interface PatientAppointment {
  appointmentId: number;
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  scheduledDate: string;
  timeSlot: number;
  status: number;
  cancellationReason?: string | null;
}

export interface PatientHealthRecord {
  healthRecordId?: number;
  appointmentId?: number;
  visitDate?: string;
  createdDate?: string;
  doctorName?: string;
  doctorFullName?: string;
  specialisation?: string;
  doctorSpecialisation?: string;
  diagnosis?: string;
  prescription?: string;
  prescribedMedication?: string;
  medication?: string;
  notes?: string;
}

export enum AppointmentStatus {
  Pending = 0,
  Confirmed = 1,
  Cancelled = 2,
  Completed = 3
}

export enum AppointmentTimeSlot {
  TenAM = 1,
  TenThirtyAM = 2,
  ElevenAM = 3,
  ElevenThirtyAM = 4,
  TwelvePM = 5,
  TwelveThirtyPM = 6,
  OnePM = 7,
  OneThirtyPM = 8,
  TwoPM = 9,
  TwoThirtyPM = 10,
  ThreePM = 11,
  ThreeThirtyPM = 12
}


