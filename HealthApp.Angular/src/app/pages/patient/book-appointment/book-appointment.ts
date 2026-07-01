import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AuthService } from '../../../core/services/auth.service';
import { AppointmentApiService } from '../../../core/services/appointment-api.service';
import { DoctorApiService } from '../../../core/services/doctor-api.service';
import { PatientApiService } from '../../../core/services/patient-api.service';
import { Doctor, SpecialisationType } from '../../../core/models/doctor.model';
import { Patient } from '../../../core/models/patient.model';

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

  specialisations: SpecialisationType[] = [
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

  doctors: Doctor[] = [];

  selectedSpecialisation: SpecialisationType | '' = '';
  selectedDoctorId = 0;
  selectedDate = '';
  selectedSlot = '';

  availableSlots: string[] = [];

  message = '';
  isError = false;
  isBooking = false;

  today = new Date().toISOString().split('T')[0];

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
    return this.doctors.find(doctor => doctor.doctorId === this.selectedDoctorId);
  }

  onSpecialisationChanged(): void {
    this.selectedDoctorId = 0;
    this.selectedDate = '';
    this.selectedSlot = '';
    this.availableSlots = [];
    this.doctors = [];

    if (!this.selectedSpecialisation) {
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
    this.clearMessage();
  }

  onDateChanged(): void {
    this.selectedSlot = '';
    this.clearMessage();

    if (!this.selectedDoctorId || !this.selectedDate) {
      this.availableSlots = [];
      return;
    }

    this.doctorApi
      .getDoctorAvailability(this.selectedDoctorId, this.selectedDate)
      .subscribe({
        next: availability => {
          this.availableSlots = availability.availableSlots;
          this.cdr.detectChanges();
        },
        error: error => {
          this.showError(this.authService.getErrorMessage(error));
          this.cdr.detectChanges();
        }
      });
  }

  selectSlot(slot: string): void {
    this.selectedSlot = slot;
    this.clearMessage();
  }

  bookAppointment(): void {
    this.clearMessage();

    if (!this.selectedDoctorId || !this.selectedDate || !this.selectedSlot) {
      this.showError('Please select doctor, date, and slot.');
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

  private clearMessage(): void {
    this.message = '';
    this.isError = false;
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }
}