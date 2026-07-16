export interface DoctorSlot {
  timeSlot: string;
  isAvailable: boolean;
  status: string;
}

export interface DoctorAvailabilityResponse {
  doctorId: number;
  date: string;
  isDoctorOnLeave: boolean;
  message: string;
  slots: DoctorSlot[];
}