export type SpecialisationType =
  | 'Endocrinologist'
  | 'Oncologist'
  | 'Gynecologist'
  | 'OrthopedicSurgeon'
  | 'Psychiatrist'
  | 'Pediatrician'
  | 'Neurologist'
  | 'Dermatologist'
  | 'Cardiologist'
  | 'GeneralPractitioner';

export interface Doctor {
  doctorId: number;
  fullName: string;
  specialisation: SpecialisationType;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface DoctorAvailability {
  doctorId: number;
  date: string;
  availableSlots: string[];
}