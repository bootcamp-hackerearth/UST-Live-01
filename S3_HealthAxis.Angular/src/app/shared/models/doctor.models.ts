export interface Doctor {
  doctorId: number;
  fullName: string;
  email: string;
  specialisation: number;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface DoctorSpecialisationOption {
  value: number;
  label: string;
}

export enum DoctorSpecialisation {
  GeneralPractitioner = 1,
  Cardiologist = 2,
  Dermatologist = 3,
  Neurologist = 4,
  Pediatrician = 5,
  Psychiatrist = 6,
  OrthopedicSurgeon = 7,
  Gynecologist = 8,
  Oncologist = 9,
  Endocrinologist = 10
}


