import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  signal
} from '@angular/core';
import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';
import { FormsModule } from '@angular/forms';

import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { DoctorDto } from '../../../core/models/doctor.model';
import { AuthService } from '../../../core/services/auth.service';

interface SlotView {
  time: string;
  isAvailable: boolean;
  isPast: boolean;
  isBooked: boolean;
  isPatientBusy: boolean;
  label: string;
}

@Component({
  selector: 'app-book-appointment',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment {
  doctorId = 0;

  doctor = signal<DoctorDto | null>
    (null);

  selectedDate = signal('');

  selectedSlot = signal('');

  availableSlots = signal<string[]>
    ([]);

  patientBookedSlots = signal<string[]>
    ([]);

  isLoading = signal(false);

  isSaving = signal(false);

  errorMessage = signal('');

  successMessage = signal('');

  today = this.getTodayDate();

  private readonly workingSlots = [
    '09:00',
    '10:00',
    '11:00',
    '12:00',
    '13:00',
    '14:00',
    '15:00',
    '16:00'
  ];

  slotViews = computed<SlotView[]>
    (() => {
      const selectedDate = this.selectedDate();

      const availableSlots = this.availableSlots();

      const patientBookedSlots = this.patientBookedSlots();

      return this.workingSlots.map(slot => {
        const isPast = this.isPastSlot(selectedDate, slot);

        const isReturnedAsAvailable = availableSlots.includes(slot);

        const isPatientBusy = patientBookedSlots.includes(slot);

        const isBooked =
          !!selectedDate &&
          !isPast &&
          !isReturnedAsAvailable;

        const isAvailable =
          !!selectedDate &&
          isReturnedAsAvailable &&
          !isPast &&
          !isPatientBusy;

        let label = 'Available';

        if (!selectedDate) {
          label = 'Select date';
        } else if (isPast) {
          label = 'Past slot';
        } else if (isPatientBusy) {
          label = 'You already have another appointment at this time';
        } else if (isBooked) {
          label = 'Booked';
        }

        return {
          time: slot,
          isAvailable,
          isPast,
          isBooked,
          isPatientBusy,
          label
        };
      });
    });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private doctorService: DoctorService,
    private appointmentService: AppointmentService,
    private authService: AuthService
  ) {
    this.doctorId = Number(this.route.snapshot.paramMap.get('doctorId'));

    this.loadDoctor();
  }

  loadDoctor(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.doctorService.getById(this.doctorId).subscribe({
      next: doctor => {
        this.doctor.set(doctor);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.isLoading.set(false);
      }
    });
  }

  loadSlots(): void {
    this.errorMessage.set('');
    this.successMessage.set('');
    this.selectedSlot.set('');
    this.availableSlots.set([]);
    this.patientBookedSlots.set([]);

    if (!this.selectedDate()) {
      return;
    }

    if (this.selectedDate() < this.today) {
      this.errorMessage.set('Cannot check availability for a past date.');
      return;
    }

    this.doctorService.getAvailability(
      this.doctorId,
      this.selectedDate()
    ).subscribe({
      next: slots => {
        this.availableSlots.set(slots);
        this.loadPatientBookedSlots();
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  private loadPatientBookedSlots(): void {
    const patientId = this.authService.patientId();

    if (!patientId) {
      return;
    }

    this.appointmentService.getAppointments({
      patientId: patientId,
      date: this.selectedDate()
    }).subscribe({
      next: appointments => {
        const bookedSlots = appointments
          .filter(appointment =>
            appointment.status !== 'Cancelled'
          )
          .map(appointment => appointment.timeSlot);

        this.patientBookedSlots.set(bookedSlots);
      },
      error: error => {
        this.errorMessage.set(this.authService.getErrorMessage(error));
      }
    });
  }

  selectSlot(slot: SlotView): void {
    this.errorMessage.set('');

    if (!slot.isAvailable) {
      this.selectedSlot.set('');

      if (slot.isPast) {
        this.errorMessage.set('This time slot has already passed for today.');
        return;
      }

      if (slot.isPatientBusy) {
        this.errorMessage.set('You already have another appointment at this time.');
        return;
      }

      if (slot.isBooked) {
        this.errorMessage.set('This time slot is already booked by another patient.');
        return;
      }

      this.errorMessage.set('Please select an available slot.');
      return;
    }

    this.selectedSlot.set(slot.time);
  }

  bookAppointment(): void {
    this.errorMessage.set('');
    this.successMessage.set('');

    if (!this.selectedDate()) {
      this.errorMessage.set('Please select an appointment date.');
      return;
    }

    if (!this.selectedSlot()) {
      this.errorMessage.set('Please select an available time slot.');
      return;
    }

    const selectedSlotView = this.slotViews().find(slot =>
      slot.time === this.selectedSlot()
    );

    if (!selectedSlotView?.isAvailable) {
      this.errorMessage.set('Selected slot is not available. Please choose another slot.');
      return;
    }

    if (selectedSlotView.isPatientBusy) {
      this.errorMessage.set('You already have another appointment at this time.');
      return;
    }

    this.isSaving.set(true);

    this.appointmentService.create({
      doctorId: this.doctorId,
      scheduledDate: this.selectedDate(),
      timeSlot: this.selectedSlot()
    }).subscribe({
      next: () => {
        this.isSaving.set(false);
        this.successMessage.set('Appointment booked successfully.');
        this.router.navigate(['/patient/appointments']);
      },
      error: error => {
        this.isSaving.set(false);
        this.errorMessage.set(this.authService.getErrorMessage(error));
        this.loadSlots();
      }
    });
  }

  canConfirmAppointment(): boolean {
    if (this.isSaving()) {
      return false;
    }

    if (!this.selectedDate() || !this.selectedSlot()) {
      return false;
    }

    const selectedSlotView = this.slotViews().find(slot =>
      slot.time === this.selectedSlot()
    );

    return !!selectedSlotView?.isAvailable;
  }

  getSlotClass(slot: SlotView): string {
    if (this.selectedSlot() === slot.time && slot.isAvailable) {
      return 'slot-button selected';
    }

    if (slot.isPast) {
      return 'slot-button unavailable past-slot';
    }

    if (slot.isPatientBusy) {
      return 'slot-button unavailable patient-busy-slot';
    }

    if (slot.isBooked) {
      return 'slot-button unavailable booked-slot';
    }

    if (!slot.isAvailable) {
      return 'slot-button unavailable';
    }

    return 'slot-button';
  }

  private isPastSlot(selectedDate: string, slot: string): boolean {
    if (!selectedDate) {
      return false;
    }

    if (selectedDate !== this.today) {
      return false;
    }

    const now = new Date();

    const currentMinutes =
      now.getHours() * 60 +
      now.getMinutes();

    const slotMinutes = this.convertSlotToMinutes(slot);

    return slotMinutes <= currentMinutes;
  }

  private convertSlotToMinutes(slot: string): number {
    const [hour, minute] = slot.split(':').map(Number);

    return hour * 60 + minute;
  }

  private getTodayDate(): string {
    const today = new Date();

    const year = today.getFullYear();

    const month = String(today.getMonth() + 1).padStart(2, '0');

    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
