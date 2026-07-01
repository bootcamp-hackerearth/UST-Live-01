export const SPECIALISATION_OPTIONS = [
  { value: '', label: 'All Specialisations' },
  { value: '1', label: 'Cardiology' },
  { value: '2', label: 'Neurology' },
  { value: '3', label: 'Dermatology' },
  { value: '4', label: 'Orthopedics' },
  { value: '5', label: 'Pediatrics' },
  { value: '6', label: 'Gynecology' },
  { value: '7', label: 'Oncology' },
  { value: '8', label: 'Psychiatry' },
  { value: '9', label: 'Ophthalmology' },
  { value: '10', label: 'ENT' },
  { value: '11', label: 'Pulmonology' },
  { value: '12', label: 'Gastroenterology' },
  { value: '13', label: 'Nephrology' },
  { value: '14', label: 'Urology' },
  { value: '15', label: 'Endocrinology' },
  { value: '16', label: 'Radiology' },
  { value: '17', label: 'General Surgery' },
  { value: '18', label: 'Anesthesiology' },
  { value: '19', label: 'Emergency Medicine' },
  { value: '20', label: 'General Medicine' }
];

export function getSpecialisationName(value: number | string): string {
  const option =
    SPECIALISATION_OPTIONS.find(item =>
      item.value === String(value)
    );

  return option?.label ?? `Specialisation ${value}`;
}