export interface DoctorDto {
  doctorId: number;
  doctorName: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface SlotAvailabilityDto {
  timeSlot: string;
  isBooked: boolean;
}

export interface DoctorAvailabilityResponseDto {
  doctorId: number;
  date: string;
  isDoctorOnLeave: boolean;
  message: string;
  slots: SlotAvailabilityDto[];
}

export interface ApiSlotAvailabilityDto {
  timeSlot?: string;
  TimeSlot?: string;
  isBooked?: boolean;
  IsBooked?: boolean;
}

export interface ApiDoctorAvailabilityResponseDto {
  doctorId?: number;
  DoctorId?: number;
  date?: string;
  Date?: string;
  isDoctorOnLeave?: boolean;
  IsDoctorOnLeave?: boolean;
  message?: string;
  Message?: string;
  slots?: ApiSlotAvailabilityDto[];
  Slots?: ApiSlotAvailabilityDto[];
}