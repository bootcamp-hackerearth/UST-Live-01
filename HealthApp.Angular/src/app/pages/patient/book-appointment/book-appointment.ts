import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { Doctor, SpecialisationType } from '../../../core/models/doctor.model';
import { Patient } from '../../../core/models/patient.model';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { AuthService } from '../../../core/services/auth.service';
import { DoctorApiService } from '../../../core/services/doctor-api.service';
import { PatientApiService } from '../../../core/services/patient-api.service';

@Component({
  selector: 'app-book-appointment',
  imports: [FormsModule],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css',
})
export class BookAppointment implements OnInit {
  private readonly cdr = inject(ChangeDetectorRef);
  private readonly authService = inject(AuthService);
  private readonly doctorApi = inject(DoctorApiService);
  private readonly patientApi = inject(PatientApiService);
  private readonly appointmentApi = inject(AppointmentApiService);
  private readonly router = inject(Router);

  patient?: Patient;

  readonly specialisations: SpecialisationType[] = [
    'Endocrinologist',
    'Oncologist',
    'Gynecologist',
    'OrthopedicSurgeon',
    'Psychiatrist',
    'Pediatrician',
    'Neurologist',
    'Dermatologist',
    'Cardiologist',
    'GeneralPractitioner'
  ];

  // Used only on a leave date so every slot can remain visible and disabled.
  readonly allTimeSlots: string[] = [
    '09:00 AM - 09:30 AM',
    '09:30 AM - 10:00 AM',
    '10:00 AM - 10:30 AM',
    '10:30 AM - 11:00 AM',
    '11:00 AM - 11:30 AM',
    '11:30 AM - 12:00 PM',
    '12:00 PM - 12:30 PM',
    '02:00 PM - 02:30 PM',
    '02:30 PM - 03:00 PM',
    '03:00 PM - 03:30 PM',
    '03:30 PM - 04:00 PM',
    '04:00 PM - 04:30 PM',
    '04:30 PM - 05:00 PM',
    '05:00 PM - 05:30 PM',
    '05:30 PM - 06:00 PM'
  ];

  doctors: Doctor[] = [];

  selectedSpecialisation: SpecialisationType | '' = '';
  selectedDoctorId = 0;
  selectedDate = '';
  selectedSlot = '';

  availableSlots: string[] = [];

  isDoctorOnLeave = false;
  leaveMessage = '';

  message = '';
  isError = false;

  isLoadingAvailability = false;
  isBooking = false;

  readonly today = new Date().toISOString().split('T')[0];

  ngOnInit(): void {
    const user = this.authService.currentUser();

    if (!user?.patientId) {
      this.router.navigate(['/login']);
      return;
    }

    this.patientApi.getPatientById(user.patientId).subscribe({
      next: patient => {
        this.patient = patient;
        this.cdr.detectChanges();
      },
      error: error => {
        this.showError(this.authService.getErrorMessage(error));
        this.cdr.detectChanges();
      }
    });
  }

  get filteredDoctors(): Doctor[] {
    return this.doctors;
  }

  get selectedDoctor(): Doctor | undefined {
    return this.doctors.find(
      doctor => doctor.doctorId === this.selectedDoctorId
    );
  }

  get canBookAppointment(): boolean {
    return (
      !this.isBooking &&
      !this.isLoadingAvailability &&
      !this.isDoctorOnLeave &&
      this.selectedDoctorId > 0 &&
      Boolean(this.selectedDate) &&
      Boolean(this.selectedSlot)
    );
  }

  onSpecialisationChanged(): void {
    this.selectedDoctorId = 0;
    this.selectedDate = '';
    this.selectedSlot = '';
    this.doctors = [];
    this.availableSlots = [];
    this.resetLeaveState();
    this.clearMessage();

    if (!this.selectedSpecialisation) {
      this.cdr.detectChanges();
      return;
    }

    this.doctorApi.getDoctors(this.selectedSpecialisation).subscribe({
      next: doctors => {
        this.doctors = doctors;
        this.cdr.detectChanges();
      },
      error: error => {
        this.showError(this.authService.getErrorMessage(error));
        this.cdr.detectChanges();
      }
    });
  }

  onDoctorChanged(): void {
    this.selectedDate = '';
    this.selectedSlot = '';
    this.availableSlots = [];
    this.resetLeaveState();
    this.clearMessage();
    this.cdr.detectChanges();
  }

  onDateChanged(): void {
    this.selectedSlot = '';
    this.availableSlots = [];
    this.resetLeaveState();
    this.clearMessage();

    if (!this.selectedDoctorId || !this.selectedDate) {
      this.cdr.detectChanges();
      return;
    }

    this.isLoadingAvailability = true;

    this.doctorApi
      .getDoctorAvailability(this.selectedDoctorId, this.selectedDate)
      .subscribe({
        next: availability => {
          this.isLoadingAvailability = false;
          this.isDoctorOnLeave = availability.isOnLeave;
          this.leaveMessage = availability.message || '';
          this.availableSlots = availability.availableSlots ?? [];

          if (this.isDoctorOnLeave) {
            this.selectedSlot = '';

            if (!this.leaveMessage.trim()) {
              this.leaveMessage = 'Doctor On Leave';
            }
          }

          this.cdr.detectChanges();
        },
        error: error => {
          this.isLoadingAvailability = false;
          this.availableSlots = [];
          this.selectedSlot = '';
          this.resetLeaveState();
          this.showError(this.authService.getErrorMessage(error));
          this.cdr.detectChanges();
        }
      });
  }

  selectSlot(slot: string): void {
    if (
      this.isDoctorOnLeave ||
      this.isLoadingAvailability ||
      !this.availableSlots.includes(slot)
    ) {
      return;
    }

    this.selectedSlot = slot;
    this.clearMessage();
  }

  bookAppointment(): void {
    this.clearMessage();

    if (this.isDoctorOnLeave) {
      this.selectedSlot = '';
      this.showError(
        'The selected doctor is on leave on this date. Please choose another date.'
      );
      this.cdr.detectChanges();
      return;
    }

    if (this.isLoadingAvailability) {
      this.showError(
        'Please wait while doctor availability is being checked.'
      );
      this.cdr.detectChanges();
      return;
    }

    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      this.showError('Please select doctor, date, and slot.');
      this.cdr.detectChanges();
      return;
    }

    if (!this.availableSlots.includes(this.selectedSlot)) {
      this.selectedSlot = '';
      this.showError(
        'The selected time slot is no longer available. Please select another slot.'
      );
      this.cdr.detectChanges();
      return;
    }

    this.isBooking = true;

    this.appointmentApi.bookAppointment({
      doctorId: this.selectedDoctorId,
      scheduledDate: this.selectedDate,
      timeSlot: this.selectedSlot
    }).subscribe({
      next: () => {
        this.isBooking = false;
        this.message = 'Appointment booked successfully.';
        this.isError = false;
        this.cdr.detectChanges();

        setTimeout(() => {
          this.router.navigate(['/patient/appointments']);
        }, 800);
      },
      error: error => {
        this.isBooking = false;
        this.showError(this.authService.getErrorMessage(error));
        this.cdr.detectChanges();
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/patient/appointments']);
  }

  private resetLeaveState(): void {
    this.isDoctorOnLeave = false;
    this.leaveMessage = '';
  }

  private clearMessage(): void {
    this.message = '';
    this.isError = false;
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }
}
