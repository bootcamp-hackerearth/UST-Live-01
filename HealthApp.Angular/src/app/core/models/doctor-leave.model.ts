export interface CreateDoctorLeaveRequest {
  startDate: string;
  endDate: string;
  reason: string;
  confirmAppointmentCancellation: boolean;
}

export interface DoctorLeave {
  doctorLeaveId: number;
  doctorId: number;
  doctorName: string;
  startDate: string;
  endDate: string;
  reason: string;
  createdDate: string;
}

export interface DoctorLeaveImpact {
  affectedAppointmentCount: number;
  requiresConfirmation: boolean;
  message: string;
}

export interface DoctorLeaveCreationResult {
  leave: DoctorLeave;
  cancelledAppointmentCount: number;
  message: string;
}

export interface DoctorLeaveStatus {
  doctorId: number;
  date: string;
  isOnLeave: boolean;
  message: string;
  leave: DoctorLeave | null;
}
