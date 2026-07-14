export interface CreateDoctorLeaveRequest {
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
  createdDateUtc: string;
}

export interface DoctorLeaveStatusDto {
  doctorId: number;
  date: string;
  isDoctorOnLeave: boolean;
  message: string;
}