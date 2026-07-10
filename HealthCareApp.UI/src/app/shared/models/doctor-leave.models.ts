export interface CreateMyDoctorLeaveDto {
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
  createdDate: string;
}