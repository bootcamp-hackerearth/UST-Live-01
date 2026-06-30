import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import { RouterLink } from '@angular/router';

import { AppointmentService } from '../../core/services/appointment.service';
import { Doctor } from '../../core/models/doctor.model';
import { DoctorService } from '../../core/services/doctor.service';
import { Patient } from '../../core/models/patient.model';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

interface AppointmentSuccessDialog {
  doctorName: string;
  specialisation: string;
  scheduledDate: string;
  timeSlot: string;
  status: string;
}

const TIME_SLOTS: readonly string[] = [
  '09:00 AM - 09:30 AM',
  '09:30 AM - 10:00 AM',
  '10:00 AM - 10:30 AM',
  '10:30 AM - 11:00 AM',
  '11:00 AM - 11:30 AM',
  '11:30 AM - 12:00 PM',
  '02:00 PM - 02:30 PM',
  '02:30 PM - 03:00 PM',
  '03:00 PM - 03:30 PM',
  '03:30 PM - 04:00 PM',
  '04:00 PM - 04:30 PM',
  '04:30 PM - 05:00 PM'
];

@Component({
  selector: 'app-book-appointment',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BookAppointment {
  private readonly formBuilder = inject(FormBuilder);
  private readonly patientService = inject(PatientService);
  private readonly doctorService = inject(DoctorService);
  private readonly appointmentService = inject(AppointmentService);

  readonly patient = signal<Patient | null>(null);
  readonly doctors = signal<Doctor[]>([]);
  readonly selectedSpecialisation = signal('');
  readonly selectedDoctorId = signal(0);
  readonly doctorSearch = signal('');

  readonly loading = signal(false);
  readonly submitting = signal(false);
  readonly errorMessage = signal('');
  readonly successMessage = signal('');
  readonly successDialog = signal<AppointmentSuccessDialog | null>(null);

  readonly timeSlots = TIME_SLOTS;
  readonly minDate = this.formatDateForInput(new Date());
  readonly maxDate = this.getMaxBookingDate();

  readonly bookingForm = this.formBuilder.nonNullable.group({
    specialisation: [''],
    doctorId: [0, [Validators.required, Validators.min(1)]],
    scheduledDate: [
      '',
      [
        Validators.required,
        BookAppointment.notPastDateValidator,
        BookAppointment.maxSixMonthsValidator
      ]
    ],
    timeSlot: ['', [Validators.required]]
  });

  readonly activeDoctors = computed(() =>
    this.doctors().filter((doctor) => doctor.isActive)
  );

  readonly specialisations = computed(() =>
    [...new Set(this.activeDoctors().map((doctor) => doctor.specialisation))]
      .sort((first, second) => first.localeCompare(second))
  );

  readonly filteredDoctors = computed(() => {
    const specialisation = this.selectedSpecialisation();
    const searchText = this.doctorSearch().trim().toLowerCase();

    return this.activeDoctors()
      .filter((doctor) =>
        this.isDoctorMatchingFilters(doctor, specialisation, searchText)
      )
      .sort((first, second) => first.fullName.localeCompare(second.fullName));
  });

  readonly selectedDoctor = computed(() => {
    const doctorId = this.selectedDoctorId();

    if (!doctorId) {
      return undefined;
    }

    return this.doctors().find((doctor) => doctor.doctorId === doctorId);
  });

  constructor() {
    this.loadInitialData();
  }

  get doctorId() {
    return this.bookingForm.controls.doctorId;
  }

  get scheduledDate() {
    return this.bookingForm.controls.scheduledDate;
  }

  get timeSlot() {
    return this.bookingForm.controls.timeSlot;
  }

  onSpecialisationChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const specialisation = selectElement.value;

    this.selectedSpecialisation.set(specialisation);
    this.bookingForm.controls.specialisation.setValue(specialisation);
    this.clearSelectedDoctor();
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.doctorSearch.set(input.value);
  }

  selectDoctor(doctorId: number): void {
    this.selectedDoctorId.set(doctorId);
    this.bookingForm.controls.doctorId.setValue(doctorId);
  }

  clearFilters(): void {
    this.selectedSpecialisation.set('');
    this.doctorSearch.set('');
    this.bookingForm.controls.specialisation.setValue('');
    this.clearSelectedDoctor();
  }

  closeSuccessDialog(): void {
    this.successDialog.set(null);
  }

  bookAppointment(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (this.bookingForm.invalid) {
      this.bookingForm.markAllAsTouched();
      this.errorMessage.set('Please select doctor, date, and time slot.');
      return;
    }

    const currentPatient = this.patient();
    const doctor = this.selectedDoctor();

    if (!currentPatient) {
      this.errorMessage.set('Patient profile not loaded. Please login again.');
      return;
    }

    if (!doctor) {
      this.errorMessage.set('Please select a valid doctor.');
      return;
    }

    if (!doctor.isActive) {
      this.errorMessage.set('Selected doctor is inactive. Please choose another doctor.');
      return;
    }

    const formValue = this.bookingForm.getRawValue();

    this.submitting.set(true);

    this.appointmentService.createAppointment({
      patientId: currentPatient.patientId,
      doctorId: Number(formValue.doctorId),
      scheduledDate: formValue.scheduledDate,
      timeSlot: formValue.timeSlot
    }).subscribe({
      next: () => {
        this.submitting.set(false);

        this.successDialog.set({
          doctorName: doctor.fullName,
          specialisation: doctor.specialisation,
          scheduledDate: formValue.scheduledDate,
          timeSlot: formValue.timeSlot,
          status: 'Pending'
        });

        this.resetBookingSelection();
      },
      error: (error: unknown) => {
        this.submitting.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Appointment booking failed. Please choose another date or time slot.'
          )
        );
      }
    });
  }

  private loadInitialData(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    this.patientService.getMyProfile().subscribe({
      next: (patient) => {
        this.patient.set(patient);
      },
      error: (error: unknown) => {
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Could not load patient profile.')
        );
      }
    });

    this.doctorService.getAllDoctors().subscribe({
      next: (doctors) => {
        this.doctors.set(doctors);
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(error, 'Unable to load doctors.')
        );
      }
    });
  }

  private isDoctorMatchingFilters(
    doctor: Doctor,
    specialisation: string,
    searchText: string
  ): boolean {
    const matchesSpecialisation =
      !specialisation || doctor.specialisation === specialisation;

    const matchesSearch =
      !searchText ||
      doctor.fullName.toLowerCase().includes(searchText) ||
      doctor.specialisation.toLowerCase().includes(searchText);

    return matchesSpecialisation && matchesSearch;
  }

  private resetBookingSelection(): void {
    this.bookingForm.patchValue({
      doctorId: 0,
      scheduledDate: '',
      timeSlot: ''
    });

    this.bookingForm.markAsPristine();
    this.bookingForm.markAsUntouched();

    this.selectedDoctorId.set(0);
  }

  private clearSelectedDoctor(): void {
    this.selectedDoctorId.set(0);
    this.bookingForm.controls.doctorId.setValue(0);
  }

  private getMaxBookingDate(): string {
    const maxDate = new Date();
    maxDate.setMonth(maxDate.getMonth() + 6);

    return this.formatDateForInput(maxDate);
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private static notPastDateValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate = new Date(control.value);
    const today = new Date();

    selectedDate.setHours(0, 0, 0, 0);
    today.setHours(0, 0, 0, 0);

    if (selectedDate < today) {
      return {
        pastDate: true
      };
    }

    return null;
  }

  private static maxSixMonthsValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate = new Date(control.value);
    const maxDate = new Date();

    maxDate.setMonth(maxDate.getMonth() + 6);
    selectedDate.setHours(0, 0, 0, 0);
    maxDate.setHours(0, 0, 0, 0);

    if (selectedDate > maxDate) {
      return {
        beyondSixMonths: true
      };
    }

    return null;
  }
}