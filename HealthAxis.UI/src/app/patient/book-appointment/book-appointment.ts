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
import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';
import { forkJoin } from 'rxjs';

import { AppointmentService } from '../../core/services/appointment.service';
import { Doctor } from '../../core/models/doctor.model';
import {
  DoctorAvailabilityResponse,
  DoctorService
} from '../../core/services/doctor.service';
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

interface TimeSlotView {
  value: string;
  disabled: boolean;
  reason: string;
}

const PRINT_DELAY_IN_MS = 350;
const PRINT_WINDOW_FEATURES =
  'width=900,height=700';

const DATE_ONLY_PATTERN =
  /^(\d{4})-(\d{2})-(\d{2})$/;

const TIME_SLOTS: readonly string[] = [
  '09:00 AM - 10:00 AM',
  '10:00 AM - 11:00 AM',
  '11:00 AM - 12:00 PM',
  '12:00 PM - 01:00 PM',
  '02:00 PM - 03:00 PM',
  '03:00 PM - 04:00 PM',
  '04:00 PM - 05:00 PM',
  '05:00 PM - 06:00 PM',
  '06:00 PM - 07:00 PM',
  '07:00 PM - 08:00 PM',
  '08:00 PM - 09:00 PM',
  '09:00 PM - 10:00 PM'
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
  private readonly appointmentService =
    inject(AppointmentService);

  private readonly route =
    inject(ActivatedRoute);

  private readonly doctorPageSize = 6;
  private availabilityRequestId = 0;

  readonly patient = signal<Patient | null>(null);
  readonly doctors = signal<Doctor[]>([]);

  readonly selectedSpecialisation = signal('');
  readonly selectedDoctorId = signal(0);
  readonly doctorSearch = signal('');
  readonly currentDoctorPage = signal(1);
  readonly selectedDate = signal('');
  readonly availableSlots = signal<readonly string[]>([]);
  readonly availabilityLoaded = signal(false);
  readonly loadingAvailability = signal(false);

  readonly loading = signal(false);
  readonly submitting = signal(false);
  readonly errorMessage = signal('');
  readonly successDialog = signal<AppointmentSuccessDialog | null>(null);

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

  readonly activeDoctorCount = computed(() =>
    this.doctors().filter((doctor) => doctor.isActive).length
  );

  readonly specialisations = computed(() =>
    [...new Set(this.doctors().map((doctor) => doctor.specialisation))]
      .filter(Boolean)
      .sort((first, second) => first.localeCompare(second))
  );

  readonly filteredDoctors = computed(() => {
    const specialisation = this.selectedSpecialisation();
    const searchText = this.doctorSearch().trim().toLowerCase();

    return this.doctors()
      .filter((doctor) =>
        this.isDoctorMatchingFilters(doctor, specialisation, searchText)
      )
      .sort((first, second) => first.fullName.localeCompare(second.fullName));
  });

  readonly totalDoctorPages = computed(() => {
    const totalDoctors = this.filteredDoctors().length;

    if (totalDoctors === 0) {
      return 1;
    }

    return Math.ceil(totalDoctors / this.doctorPageSize);
  });

  readonly pagedDoctors = computed(() => {
    const pageNumber = this.currentDoctorPage();
    const startIndex = (pageNumber - 1) * this.doctorPageSize;

    return this.filteredDoctors().slice(
      startIndex,
      startIndex + this.doctorPageSize
    );
  });

  readonly selectedDoctor = computed(() => {
    const doctorId = this.selectedDoctorId();

    if (!doctorId) {
      return undefined;
    }

    return this.doctors().find((doctor) => doctor.doctorId === doctorId);
  });

  readonly timeSlotViews = computed(() =>
    TIME_SLOTS.map((slot) => this.createTimeSlotView(slot))
  );

  readonly noAvailableTimeSlots = computed(() => {
    if (
      !this.selectedDoctorId() ||
      !this.selectedDate() ||
      this.loadingAvailability() ||
      !this.availabilityLoaded()
    ) {
      return false;
    }

    return this.timeSlotViews().every(
      (slot) => slot.disabled
    );
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
    this.resetDoctorPage();
  }

  onSearchInput(event: Event): void {
    const input = event.target as HTMLInputElement;

    this.doctorSearch.set(input.value);
    this.resetDoctorPage();
  }

  onDateChange(): void {
    const dateValue =
      this.bookingForm.controls.scheduledDate.value;

    this.selectedDate.set(dateValue);
    this.clearSelectedTimeSlot();
    this.loadSelectedDoctorAvailability();
  }

  selectDoctor(doctor: Doctor): void {
    if (!doctor.isActive) {
      this.errorMessage.set(
        'This doctor is currently inactive. Please choose an available doctor.'
      );
      return;
    }

    this.errorMessage.set('');
    this.selectedDoctorId.set(doctor.doctorId);
    this.bookingForm.controls.doctorId.setValue(
      doctor.doctorId
    );
    this.bookingForm.controls.doctorId.markAsTouched();
    this.clearSelectedTimeSlot();
    this.loadSelectedDoctorAvailability();
  }

  selectTimeSlot(slot: TimeSlotView): void {
    if (slot.disabled) {
      return;
    }

    this.bookingForm.controls.timeSlot.setValue(slot.value);
    this.bookingForm.controls.timeSlot.markAsTouched();
  }

  goToPreviousDoctorPage(): void {
    if (this.currentDoctorPage() > 1) {
      this.currentDoctorPage.update((page) => page - 1);
    }
  }

  goToNextDoctorPage(): void {
    if (this.currentDoctorPage() < this.totalDoctorPages()) {
      this.currentDoctorPage.update((page) => page + 1);
    }
  }

  clearFilters(): void {
    this.selectedSpecialisation.set('');
    this.doctorSearch.set('');
    this.bookingForm.controls.specialisation.setValue('');

    this.clearSelectedDoctor();
    this.resetDoctorPage();
  }

  closeSuccessDialog(): void {
    this.successDialog.set(null);
  }

  closeErrorDialog(): void {
    this.errorMessage.set('');
  }

  bookAppointment(): void {
    this.errorMessage.set('');

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
      this.errorMessage.set(
        'Selected doctor is inactive. Please choose another doctor.'
      );
      return;
    }

    if (this.isSelectedTimeSlotInvalid()) {
      this.errorMessage.set('Past time slot cannot be booked.');
      return;
    }

    const formValue = this.bookingForm.getRawValue();

    const selectedSlotIsAvailable =
      this.availabilityLoaded() &&
      !this.loadingAvailability() &&
      this.availableSlots().includes(
        formValue.timeSlot
      );

    if (!selectedSlotIsAvailable) {
      this.errorMessage.set(
        'Selected time slot is no longer available. Please choose another time slot.'
      );

      this.clearSelectedTimeSlot();
      this.loadSelectedDoctorAvailability();
      return;
    }

    this.submitting.set(true);

    this.appointmentService.createAppointment({
      patientId: currentPatient.patientId,
      doctorId: Number(formValue.doctorId),
      scheduledDate: formValue.scheduledDate,
      timeSlot: formValue.timeSlot
    }).subscribe({
      next: (createdAppointment) => {
        this.submitting.set(false);

        this.successDialog.set({
          doctorName: this.getDoctorDisplayName(doctor.fullName),
          specialisation: doctor.specialisation,
          scheduledDate: formValue.scheduledDate,
          timeSlot: formValue.timeSlot,
          status:
            createdAppointment.status?.trim() ||
            'Pending'
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

  getDoctorStatusText(doctor: Doctor): string {
    return doctor.isActive ? 'Available' : 'Unavailable';
  }

  getDoctorDisplayName(name: string): string {
    const cleanName = name.trim();

    if (!cleanName) {
      return 'Doctor not assigned';
    }

    if (/^dr\.?\s/i.test(cleanName)) {
      return cleanName;
    }

    return `Dr. ${cleanName}`;
  }

  getDateErrorMessage(): string {
    if (this.scheduledDate.hasError('required')) {
      return 'Appointment date is required.';
    }

    if (this.scheduledDate.hasError('invalidDate')) {
      return 'Enter a valid appointment date.';
    }

    if (this.scheduledDate.hasError('pastDate')) {
      return 'Past date is not allowed.';
    }

    if (this.scheduledDate.hasError('beyondSixMonths')) {
      return 'Booking is allowed only within the next 6 months.';
    }

    return '';
  }

  private loadInitialData(): void {
    this.loading.set(true);
    this.errorMessage.set('');

    forkJoin({
      patient: this.patientService.getMyProfile(),
      doctors: this.doctorService.getAllDoctors()
    }).subscribe({
      next: ({ patient, doctors }) => {
        const normalizedDoctors =
          this.extractDoctors(doctors);

        this.patient.set(patient);
        this.doctors.set(normalizedDoctors);
        this.applyRequestedDoctorSelection(
          normalizedDoctors
        );
        this.loading.set(false);
      },
      error: (error: unknown) => {
        this.loading.set(false);
        this.errorMessage.set(
          getFriendlyErrorMessage(
            error,
            'Unable to load booking details. Please try again.'
          )
        );
      }
    });
  }

  private applyRequestedDoctorSelection(
    doctors: readonly Doctor[]
  ): void {
    const queryParams =
      this.route.snapshot.queryParamMap;

    const requestedDoctorId =
      Number(queryParams.get('doctorId'));

    const requestedSpecialisation =
      queryParams
        .get('specialisation')
        ?.trim() ?? '';

    const requestedDoctor =
      Number.isInteger(
        requestedDoctorId
      ) &&
      requestedDoctorId > 0
        ? doctors.find(
            (doctor) =>
              doctor.doctorId ===
                requestedDoctorId &&
              doctor.isActive
          )
        : undefined;

    if (requestedDoctor) {
      this.setRequestedSpecialisation(
        requestedDoctor.specialisation
      );

      this.selectDoctor(requestedDoctor);
      this.moveToSelectedDoctorPage(
        requestedDoctor
      );

      return;
    }

    if (requestedSpecialisation) {
      const matchingSpecialisation =
        this.specialisations().find(
          (specialisation) =>
            specialisation
              .toLowerCase() ===
            requestedSpecialisation
              .toLowerCase()
        );

      if (matchingSpecialisation) {
        this.setRequestedSpecialisation(
          matchingSpecialisation
        );
      }
    }
  }

  private setRequestedSpecialisation(
    specialisation: string
  ): void {
    this.selectedSpecialisation.set(
      specialisation
    );

    this.bookingForm.controls
      .specialisation
      .setValue(specialisation);

    this.resetDoctorPage();
  }

  private moveToSelectedDoctorPage(
    doctor: Doctor
  ): void {
    const doctorIndex =
      this.filteredDoctors().findIndex(
        (item) =>
          item.doctorId ===
          doctor.doctorId
      );

    if (doctorIndex < 0) {
      return;
    }

    this.currentDoctorPage.set(
      Math.floor(
        doctorIndex /
        this.doctorPageSize
      ) + 1
    );
  }

  private extractDoctors(response: unknown): Doctor[] {
    if (Array.isArray(response)) {
      return response.map((doctor) => this.normalizeDoctor(doctor));
    }

    if (!response || typeof response !== 'object') {
      return [];
    }

    const pageResponse = response as Record<string, unknown>;

    const items =
      pageResponse['items'] ??
      pageResponse['Items'] ??
      pageResponse['data'] ??
      pageResponse['Data'];

    if (!Array.isArray(items)) {
      return [];
    }

    return items.map((doctor) => this.normalizeDoctor(doctor));
  }

  private normalizeDoctor(response: unknown): Doctor {
    const doctor = response as Record<string, unknown>;

    return {
      doctorId: Number(doctor['doctorId'] ?? doctor['DoctorId'] ?? 0),
      fullName: this.getStringValue(
        doctor['fullName'],
        doctor['FullName']
      ),
      specialisation: this.getStringValue(
        doctor['specialisation'],
        doctor['Specialisation']
      ),
      yearsOfExperience: Number(
        doctor['yearsOfExperience'] ?? doctor['YearsOfExperience'] ?? 0
      ),
      consultationFee: Number(
        doctor['consultationFee'] ?? doctor['ConsultationFee'] ?? 0
      ),
      isActive: Boolean(doctor['isActive'] ?? doctor['IsActive'])
    };
  }

  private getStringValue(
    ...values: unknown[]
  ): string {
    for (const value of values) {
      if (typeof value === 'string') {
        return value;
      }
    }

    return '';
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

  private createTimeSlotView(
    slot: string
  ): TimeSlotView {
    const doctorId = this.selectedDoctorId();
    const selectedAppointmentDate =
      this.selectedDate() ||
      this.bookingForm.controls.scheduledDate.value;

    if (!doctorId) {
      return {
        value: slot,
        disabled: true,
        reason: 'Select doctor first'
      };
    }

    if (!selectedAppointmentDate) {
      return {
        value: slot,
        disabled: true,
        reason: 'Select date first'
      };
    }

    if (this.loadingAvailability()) {
      return {
        value: slot,
        disabled: true,
        reason: 'Checking availability'
      };
    }

    if (
      this.isPastTimeSlot(
        selectedAppointmentDate,
        slot
      )
    ) {
      return {
        value: slot,
        disabled: true,
        reason: 'Past time'
      };
    }

    if (
      this.availabilityLoaded() &&
      !this.availableSlots().includes(slot)
    ) {
      return {
        value: slot,
        disabled: true,
        reason: 'Already booked'
      };
    }

    return {
      value: slot,
      disabled: false,
      reason: 'Available'
    };
  }

  private loadSelectedDoctorAvailability(): void {
    const doctorId = this.selectedDoctorId();
    const date = this.selectedDate();

    const requestId =
      ++this.availabilityRequestId;

    this.availableSlots.set([]);
    this.availabilityLoaded.set(false);

    if (!doctorId || !date) {
      this.loadingAvailability.set(false);
      return;
    }

    this.loadingAvailability.set(true);
    this.errorMessage.set('');

    this.doctorService
      .getDoctorAvailability(doctorId, date)
      .subscribe({
        next: (availability) => {
          if (
            requestId !==
            this.availabilityRequestId
          ) {
            return;
          }

          this.applyDoctorAvailability(availability);
          this.loadingAvailability.set(false);
          this.availabilityLoaded.set(true);
          this.clearUnavailableSelectedTimeSlot();
        },
        error: (error: unknown) => {
          if (
            requestId !==
            this.availabilityRequestId
          ) {
            return;
          }

          this.loadingAvailability.set(false);
          this.availabilityLoaded.set(false);
          this.availableSlots.set([]);

          this.errorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Unable to load doctor availability.'
            )
          );
        }
      });
  }

  private applyDoctorAvailability(
    availability: DoctorAvailabilityResponse
  ): void {
    const slots = Array.isArray(
      availability.availableSlots
    )
      ? availability.availableSlots
      : [];

    this.availableSlots.set(slots);

    if (!availability.isActive) {
      this.errorMessage.set(
        availability.message ||
          'This doctor is currently unavailable.'
      );
    }
  }

  private clearUnavailableSelectedTimeSlot(): void {
    const selectedTimeSlot =
      this.bookingForm.controls.timeSlot.value;

    if (
      selectedTimeSlot &&
      !this.availableSlots().includes(
        selectedTimeSlot
      )
    ) {
      this.clearSelectedTimeSlot();
    }
  }

  private clearSelectedTimeSlot(): void {
    this.bookingForm.controls.timeSlot.setValue('');
  }

  private isSelectedTimeSlotInvalid(): boolean {
    const dateValue = this.bookingForm.controls.scheduledDate.value;
    const slotValue = this.bookingForm.controls.timeSlot.value;

    return this.isPastTimeSlot(dateValue, slotValue);
  }

  private clearPastSelectedTimeSlot(): void {
    const slotValue = this.bookingForm.controls.timeSlot.value;

    if (!slotValue) {
      return;
    }

    if (this.isPastTimeSlot(this.selectedDate(), slotValue)) {
      this.bookingForm.controls.timeSlot.setValue('');
    }
  }

  private isPastTimeSlot(dateValue: string, slot: string): boolean {
    if (!dateValue || !slot) {
      return false;
    }

    if (!this.isToday(dateValue)) {
      return false;
    }

    const slotStartMinutes = this.getSlotStartMinutes(slot);
    const now = new Date();
    const currentMinutes = now.getHours() * 60 + now.getMinutes();

    return slotStartMinutes <= currentMinutes;
  }

  private getSlotStartMinutes(slot: string): number {
    const startTime = slot.split('-')[0].trim();
    const [time, period] = startTime.split(' ');
    const [hourText, minuteText] = time.split(':');

    let hour = Number(hourText);
    const minute = Number(minuteText);

    if (period === 'PM' && hour !== 12) {
      hour += 12;
    }

    if (period === 'AM' && hour === 12) {
      hour = 0;
    }

    return hour * 60 + minute;
  }

  private isToday(dateValue: string): boolean {
    return dateValue === this.formatDateForInput(new Date());
  }

  private resetBookingSelection(): void {
    this.availabilityRequestId += 1;

    this.bookingForm.patchValue({
      doctorId: 0,
      scheduledDate: '',
      timeSlot: ''
    });

    this.bookingForm.markAsPristine();
    this.bookingForm.markAsUntouched();

    this.selectedDoctorId.set(0);
    this.selectedDate.set('');
    this.availableSlots.set([]);
    this.availabilityLoaded.set(false);
    this.loadingAvailability.set(false);
  }

  private clearSelectedDoctor(): void {
    this.availabilityRequestId += 1;
    this.selectedDoctorId.set(0);
    this.bookingForm.controls.doctorId.setValue(0);
    this.availableSlots.set([]);
    this.availabilityLoaded.set(false);
    this.loadingAvailability.set(false);
    this.clearSelectedTimeSlot();
  }

  private resetDoctorPage(): void {
    this.currentDoctorPage.set(1);
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

    const selectedDate =
      BookAppointment.parseDateOnly(
        String(control.value)
      );

    if (!selectedDate) {
      return {
        invalidDate: true
      };
    }

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    return selectedDate < today
      ? { pastDate: true }
      : null;
  }
  printBookedAppointment(
    dialog: AppointmentSuccessDialog
  ): void {
    const patientName = this.escapeHtml(
      this.patient()?.fullName ?? 'Patient'
    );

    const doctorName =
      this.escapeHtml(dialog.doctorName);

    const specialisation =
      this.escapeHtml(dialog.specialisation);

    const scheduledDate =
      this.escapeHtml(dialog.scheduledDate);

    const timeSlot =
      this.escapeHtml(dialog.timeSlot);

    const status =
      this.escapeHtml(dialog.status);

    const generatedOn = this.escapeHtml(
      new Date().toLocaleString()
    );

    const printableDocument = `
    <!DOCTYPE html>
    <html>
      <head>
        <title>HealthAxis Appointment</title>

        <style>
          body {
            margin: 0;
            background: #f8fafc;
            color: #0f172a;
            font-family: Arial, sans-serif;
          }

          .document {
            width: 800px;
            margin: 24px auto;
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 18px;
            padding: 32px;
          }

          .header {
            background: linear-gradient(135deg, #0f172a, #0284c7);
            color: #ffffff;
            padding: 24px;
            border-radius: 16px;
            margin-bottom: 24px;
          }

          .header h1 {
            margin: 0;
            font-size: 28px;
          }

          .header p {
            margin: 8px 0 0;
            color: #dbeafe;
          }

          .badge {
            display: inline-block;
            margin-top: 14px;
            padding: 8px 12px;
            border-radius: 999px;
            background: #dcfce7;
            color: #15803d;
            font-weight: bold;
          }

          .grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 14px;
          }

          .box {
            border: 1px solid #dbeafe;
            background: #f8fafc;
            border-radius: 14px;
            padding: 16px;
          }

          .box span {
            display: block;
            color: #64748b;
            font-size: 12px;
            font-weight: bold;
            margin-bottom: 7px;
          }

          .box strong {
            font-size: 16px;
            color: #0f172a;
          }

          .note {
            margin-top: 22px;
            padding: 16px;
            border-radius: 14px;
            background: #eff6ff;
            color: #334155;
            line-height: 1.6;
            font-size: 14px;
          }

          .footer {
            margin-top: 24px;
            padding-top: 14px;
            border-top: 1px solid #e2e8f0;
            color: #64748b;
            font-size: 12px;
          }

          @media print {
            body {
              background: #ffffff;
            }

            .document {
              width: auto;
              margin: 0;
              border: none;
              border-radius: 0;
            }
          }
        </style>
      </head>

      <body>
        <main class="document">
          <section class="header">
            <h1>HealthAxis Appointment Request</h1>
            <p>Patient appointment booking confirmation document</p>
            <span class="badge">${status}</span>
          </section>

          <section class="grid">
            <div class="box">
              <span>Patient Name</span>
              <strong>${patientName}</strong>
            </div>

            <div class="box">
              <span>Doctor Name</span>
              <strong>${doctorName}</strong>
            </div>

            <div class="box">
              <span>Specialisation</span>
              <strong>${specialisation}</strong>
            </div>

            <div class="box">
              <span>Appointment Date</span>
              <strong>${scheduledDate}</strong>
            </div>

            <div class="box">
              <span>Time Slot</span>
              <strong>${timeSlot}</strong>
            </div>

            <div class="box">
              <span>Generated On</span>
              <strong>${generatedOn}</strong>
            </div>
          </section>

          <section class="note">
            Your appointment request has been submitted successfully.
            Please check My Appointments for confirmation status updates.
          </section>

          <section class="footer">
            This document is generated by HealthAxis Patient Portal.
          </section>
        </main>

      </body>
    </html>
  `;

    this.openPrintableDocument(
      printableDocument,
      'Please allow popups to download or print the appointment.'
    );
  }

  private openPrintableDocument(
    printContent: string,
    popupErrorMessage: string
  ): void {
    const printWindow = globalThis.open(
      '',
      '_blank',
      PRINT_WINDOW_FEATURES
    );

    if (!printWindow) {
      this.errorMessage.set(popupErrorMessage);
      return;
    }

    const parsedDocument = new DOMParser().parseFromString(
      printContent,
      'text/html'
    );

    const importedDocumentElement =
      printWindow.document.importNode(
        parsedDocument.documentElement,
        true
      );

    printWindow.document.replaceChild(
      importedDocumentElement,
      printWindow.document.documentElement
    );

    printWindow.onafterprint = (): void => {
      printWindow.close();
    };

    printWindow.focus();

    globalThis.setTimeout(() => {
      if (!printWindow.closed) {
        printWindow.print();
      }
    }, PRINT_DELAY_IN_MS);
  }

  private escapeHtml(value: string): string {
    return value
      .replaceAll('&', '&amp;')
      .replaceAll('<', '&lt;')
      .replaceAll('>', '&gt;')
      .replaceAll('"', '&quot;')
      .replaceAll("'", '&#039;');
  }

  private static maxSixMonthsValidator(
    control: AbstractControl
  ): ValidationErrors | null {
    if (!control.value) {
      return null;
    }

    const selectedDate =
      BookAppointment.parseDateOnly(
        String(control.value)
      );

    if (!selectedDate) {
      return {
        invalidDate: true
      };
    }

    const maxDate = new Date();
    maxDate.setMonth(maxDate.getMonth() + 6);
    maxDate.setHours(0, 0, 0, 0);

    return selectedDate > maxDate
      ? { beyondSixMonths: true }
      : null;
  }

  private static parseDateOnly(
    value: string
  ): Date | null {
    const match =
      DATE_ONLY_PATTERN.exec(value.trim());

    if (!match) {
      return null;
    }

    const [, yearText, monthText, dayText] =
      match;

    const year = Number.parseInt(yearText, 10);
    const month = Number.parseInt(monthText, 10);
    const day = Number.parseInt(dayText, 10);

    const parsedDate = new Date(
      year,
      month - 1,
      day
    );

    const isValidDate =
      parsedDate.getFullYear() === year &&
      parsedDate.getMonth() === month - 1 &&
      parsedDate.getDate() === day;

    return isValidDate
      ? parsedDate
      : null;
  }
}