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

export const DOCTOR_SPECIALISATION_OPTIONS: DoctorSpecialisationOption[] = [
  {
    value: DoctorSpecialisation.GeneralPractitioner,
    label: 'General Practitioner'
  },
  {
    value: DoctorSpecialisation.Cardiologist,
    label: 'Cardiologist'
  },
  {
    value: DoctorSpecialisation.Dermatologist,
    label: 'Dermatologist'
  },
  {
    value: DoctorSpecialisation.Neurologist,
    label: 'Neurologist'
  },
  {
    value: DoctorSpecialisation.Pediatrician,
    label: 'Pediatrician'
  },
  {
    value: DoctorSpecialisation.Psychiatrist,
    label: 'Psychiatrist'
  },
  {
    value: DoctorSpecialisation.OrthopedicSurgeon,
    label: 'Orthopedic Surgeon'
  },
  {
    value: DoctorSpecialisation.Gynecologist,
    label: 'Gynecologist'
  },
  {
    value: DoctorSpecialisation.Oncologist,
    label: 'Oncologist'
  },
  {
    value: DoctorSpecialisation.Endocrinologist,
    label: 'Endocrinologist'
  }
];

export function getDoctorSpecialisationText(value: number | string | null | undefined): string {
  const numericValue = Number(value);

  const option = DOCTOR_SPECIALISATION_OPTIONS.find(
    item => item.value === numericValue
  );

  return option?.label ?? 'Unknown';
}