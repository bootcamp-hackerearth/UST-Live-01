export interface PatientProfile {
  patientId: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth: string;
  gender: string;
  insuranceId?: string | null;
}

export interface UpdatePatientProfileRequest {
  fullName: string;
  phoneNumber: string;
  gender: string;
  insuranceId?: string | null;
}

export interface DoctorProfile {
  doctorId: number;
  fullName: string;
  email: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive?: boolean;
}

export interface DoctorList {
  doctorId: number;
  fullName: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface AppointmentList {
  appointmentId: number;
  patientId: number;
  patientName: string;
  doctorName: string;
  scheduledDate: string;
  timeSlot: string;
  status: string;
}

export interface CreateAppointmentRequest {
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface HealthRecord {
  recordId?: number;
  recordTitle?: string;
  title?: string;
  doctorName?: string;
  recordDate?: string;
  date?: string;
  description?: string;
}

export interface CreateLeaveRequest {
  leaveDate: string;
  reason?: string;
}

export interface CreateLeaveResult {
  skippedDates: string[];
  createdWithCancelledAppointments: string[];
}

export interface DoctorDashboardSummary {
  upcomingAppointments: number;
  completedAppointments: number;
  upcomingLeaves: number;
  todaysAppointments: number;
}

export interface CreateHealthRecordRequest {
  appointmentId: number;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}

export interface HealthRecordResponse {
  recordId: number;
  patientName: string;
  doctorName: string;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string | null;
}

export interface HealthRecordListDto {
  recordId: number;
  patientName: string;
  doctorName: string;
  visitDate: string;  
  diagnosis: string;
  prescription: string;
  notes: string;
}
