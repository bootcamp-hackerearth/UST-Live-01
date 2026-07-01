import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

import { DoctorService } from '../../../core/services/doctor-service';
import { AppointmentService } from '../../../core/services/appointment-service';

import { Doctor } from '../../../core/models/doctor';
import { BookAppointments } from '../../../core/models/book-appointment';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule
  ],
  templateUrl: './book-appointment.html',
  styleUrl: './book-appointment.css'
})
export class BookAppointment implements OnInit {

  private fb = inject(FormBuilder);

  private router = inject(Router);

  private doctorService = inject(DoctorService);

  private appointmentService = inject(AppointmentService);

  doctors: Doctor[] = [];

  filteredDoctors: Doctor[] = [];

  specialisations: number[] = [];

  availableSlots: string[] = [];

  today = new Date().toISOString().split('T')[0];

  isLoading = false;

  slotsLoaded = false;

  appointmentForm = this.fb.group({

    appointmentDate: ['', Validators.required],

    specialisation: ['', Validators.required],

    doctorId: ['', Validators.required],

    timeSlot: ['', Validators.required]

  });

  ngOnInit(): void {

    this.loadDoctors();

    this.appointmentForm.get('doctorId')?.valueChanges.subscribe(() => {

      this.loadAvailableSlots();

    });

    this.appointmentForm.get('appointmentDate')?.valueChanges.subscribe(() => {

      this.loadAvailableSlots();

    });

  }

  loadDoctors(): void {

    this.doctorService.getAllDoctors().subscribe({

      next: (response) => {

        this.doctors = response.filter(x => x.isActive);

        this.specialisations = [
          ...new Set(this.doctors.map(x => x.specialisation))
        ].sort((a, b) => a - b);

      }

    });

  }

  onSpecialisationChange(): void {

    const specialisation =
      Number(this.appointmentForm.value.specialisation);

    this.filteredDoctors =
      this.doctors.filter(
        x => x.specialisation === specialisation
      );

    this.availableSlots = [];

    this.slotsLoaded = false;

    this.appointmentForm.patchValue({

      doctorId: '',

      timeSlot: ''

    });

  }

  loadAvailableSlots(): void {

    const doctorId = Number(this.appointmentForm.value.doctorId);

    const date = this.appointmentForm.value.appointmentDate;

    if (!doctorId || !date) {

      this.availableSlots = [];

      this.slotsLoaded = false;

      return;

    }

    this.isLoading = true;

    this.slotsLoaded = false;

    this.appointmentService
      .getAvailableSlots(doctorId, date)
      .subscribe({

        next: (slots) => {

          console.log(slots);

          this.availableSlots = [...slots];

          this.slotsLoaded = true;

          this.isLoading = false;

          const selectedTime =
            this.appointmentForm.controls.timeSlot.value;

          if (!slots.includes(selectedTime ?? '')) {

            setTimeout(() => {

              this.appointmentForm.patchValue({

                timeSlot: ''

              });

            });

          }

        },

        error: (err) => {

          console.log(err);

          this.availableSlots = [];

          this.slotsLoaded = true;

          this.isLoading = false;

        }

      });

  }

  getSpecialisationName(value: number): string {

    switch (value) {

      case 0:
        return 'Endocrinologist';

      case 1:
        return 'Oncologist';

      case 2:
        return 'Gynecologist';

      case 3:
        return 'Orthopedic Surgeon';

      case 4:
        return 'Psychiatrist';

      case 5:
        return 'Pediatrician';

      case 6:
        return 'Neurologist';

      case 7:
        return 'Dermatologist';

      case 8:
        return 'Cardiologist';

      case 9:
        return 'General Practitioner';

      default:
        return 'Specialist';

    }

  }

  bookAppointment(): void {

    if (this.appointmentForm.invalid) {

      this.appointmentForm.markAllAsTouched();

      return;

    }

    this.isLoading = true;

    const request: BookAppointments = {

      patientId: 1,

      doctorId: Number(this.appointmentForm.value.doctorId),

      scheduledDate: this.appointmentForm.value.appointmentDate!,

      timeSlot: this.appointmentForm.value.timeSlot!

    };

    this.appointmentService
      .bookAppointment(request)
      .subscribe({

        next: () => {

          this.isLoading = false;

          alert('Appointment booked successfully.');

          this.router.navigate(['/appointments']);

        },

        error: (err) => {

          this.isLoading = false;

          alert(err.error.message);

        }

      });

  }

  cancel(): void {

    this.router.navigate(['/patient']);

  }

}