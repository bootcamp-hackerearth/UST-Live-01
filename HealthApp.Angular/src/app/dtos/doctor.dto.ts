export interface DoctorDto {
  doctorId: number;
  fullName: string;
  specialisation: string;
  doctorPhoneNo: string;
  doctorEmail: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface DoctorCreateDto {
  fullName: string;
  specialisation: string;
  doctorPhoneNo: string;
  doctorEmail: string;
  yearsOfExperience: number;
  consultationFee: number;
}

export interface DoctorUpdateDto {
  fullName: string;
  specialisation: string;
  doctorPhoneNo: string;
  doctorEmail: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface SpecialisationOption {
  label: string;
  value: string;
}

export interface DoctorStatusUpdateResponse {
  message: string;
}

export type DoctorAvailabilitySlotStatus =
  | 'Available'
  | 'Booked'
  | 'DoctorOnLeave';

export interface DoctorAvailabilitySlotDto {
  timeSlot: string;
  isAvailable: boolean;
  status: DoctorAvailabilitySlotStatus;
}

export interface DoctorAvailabilityDto {
  doctorId: number;
  date: string;
  isDoctorOnLeave: boolean;
  message: string;
  slots: DoctorAvailabilitySlotDto[];
}