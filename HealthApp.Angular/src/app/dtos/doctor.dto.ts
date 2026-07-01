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