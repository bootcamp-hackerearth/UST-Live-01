export interface DoctorLeaveCreateDto {
  startDate: string;
  endDate: string;
  reason: string;
}

export interface DoctorLeaveDto {
  doctorLeaveId: number;
  doctorId: number;
  doctorName: string;
  startDate: string;
  endDate: string;
  reason: string;
  createdAtUtc: string;
}

export interface DoctorLeaveCreationResultDto {
  leave: DoctorLeaveDto;
  cancelledAppointmentCount: number;
  cancelledAppointmentIds: number[];
}

export interface DoctorLeaveCreationResponse {
  message: string;
  data: DoctorLeaveCreationResultDto;
}

export interface DoctorLeavePreviewDto {
  doctorId: number;
  startDate: string;
  endDate: string;
  pendingAppointmentCount: number;
  confirmedAppointmentCount: number;
  totalAffectedAppointmentCount: number;
  message: string;
}