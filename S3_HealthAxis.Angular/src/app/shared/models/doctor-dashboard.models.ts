export interface DoctorScheduleItem {
  appointmentId: number;
  scheduledDate: string;
  timeSlot: number;
  patientId: number;
  patientName: string;
  status: number;
  cancellationReason?: string | null;
  hasHealthRecord: boolean;
}

export interface DoctorPatientProfile {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email?: string;
  insuranceId?: string;
  isActive: boolean;
}

export interface DoctorHealthRecord {
  healthRecordId: number;
  appointmentId: number;
  patientId: number;
  doctorId: number;
  doctorName: string;
  doctorSpecialisation: number;
  createdOn: string;
  diagnosis?: string;
  prescription?: string;
  notes?: string;
}

export interface CreateHealthRecordRequest {
  appointmentId: number;
  patientId: number;
  doctorId: number;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

export interface DoctorPatient {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: number;
  phoneNumber: string;
  email?: string;
  insuranceId?: string;
  isActive: boolean;
  totalAppointments: number;
  lastVisitDate?: string | null;
}
