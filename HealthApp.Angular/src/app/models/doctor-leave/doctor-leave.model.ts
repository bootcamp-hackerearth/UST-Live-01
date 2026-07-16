export interface DoctorLeaveCreate {
  startDate: string;
  endDate: string;
  reason: string;
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