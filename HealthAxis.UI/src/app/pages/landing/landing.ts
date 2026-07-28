import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal
} from '@angular/core';
import { Params, RouterLink } from '@angular/router';

import { Doctor } from '../../core/models/doctor.model';
import { DoctorService } from '../../core/services/doctor.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const BOOK_APPOINTMENT_ROUTE =
  '/patient/book-appointment';

@Component({
  selector: 'app-landing',
  imports: [
    RouterLink
  ],
  templateUrl: './landing.html',
  styleUrl: './landing.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Landing {
  private readonly doctorService =
    inject(DoctorService);

  readonly doctors = signal<Doctor[]>([]);
  readonly selectedSpecialisation = signal('');
  readonly selectedDoctorId = signal(0);
  readonly loadingDoctors = signal(false);
  readonly doctorErrorMessage = signal('');

  readonly specialisations = computed(() =>
    [
      ...new Set(
        this.doctors()
          .filter((doctor) => doctor.isActive)
          .map((doctor) =>
            doctor.specialisation.trim()
          )
          .filter(Boolean)
      )
    ].sort((first, second) =>
      first.localeCompare(second)
    )
  );

  readonly availableDoctors = computed(() => {
    const selectedSpecialisation =
      this.selectedSpecialisation()
        .trim()
        .toLowerCase();

    if (!selectedSpecialisation) {
      return [];
    }

    return this.doctors()
      .filter(
        (doctor) =>
          doctor.isActive &&
          doctor.specialisation
            .trim()
            .toLowerCase() ===
            selectedSpecialisation
      )
      .sort((first, second) =>
        first.fullName.localeCompare(
          second.fullName
        )
      );
  });

  readonly selectedDoctor = computed(() => {
    const doctorId = this.selectedDoctorId();

    return this.availableDoctors().find(
      (doctor) =>
        doctor.doctorId === doctorId
    );
  });

  readonly bookingQueryParams = computed<Params>(
    () => {
      const selectedDoctor =
        this.selectedDoctor();

      if (!selectedDoctor) {
        return {
          returnUrl: BOOK_APPOINTMENT_ROUTE
        };
      }

      return {
        returnUrl: BOOK_APPOINTMENT_ROUTE,
        doctorId: selectedDoctor.doctorId,
        specialisation:
          selectedDoctor.specialisation
      };
    }
  );

  constructor() {
    this.loadDoctors();
  }

  retryLoadingDoctors(): void {
    if (!this.loadingDoctors()) {
      this.loadDoctors();
    }
  }

  onSpecialisationChange(
    event: Event
  ): void {
    const selectedValue =
      (event.target as HTMLSelectElement)
        .value;

    this.selectedSpecialisation.set(
      selectedValue
    );

    this.selectedDoctorId.set(0);
    this.doctorErrorMessage.set('');
  }

  selectDoctor(doctor: Doctor): void {
    if (!doctor.isActive) {
      return;
    }

    this.selectedDoctorId.set(
      doctor.doctorId
    );
  }

  scrollToDoctorFinder(): void {
    const doctorFinder =
      globalThis.document?.getElementById(
        'find-doctors'
      );

    doctorFinder?.scrollIntoView({
      behavior: 'smooth',
      block: 'start'
    });
  }

  getDoctorDisplayName(
    name: string
  ): string {
    const cleanName = name.trim();

    if (!cleanName) {
      return 'Doctor';
    }

    return /^dr\.?\s/i.test(cleanName)
      ? cleanName
      : `Dr. ${cleanName}`;
  }

  getDoctorInitial(
    name: string
  ): string {
    const cleanName = name
      .trim()
      .replace(/^dr\.?\s+/i, '');

    return cleanName
      .charAt(0)
      .toUpperCase() || 'D';
  }

  private loadDoctors(): void {
    this.loadingDoctors.set(true);
    this.doctorErrorMessage.set('');

    this.doctorService
      .getPublicActiveDoctors()
      .subscribe({
        next: (doctors) => {
          this.doctors.set(
            Array.isArray(doctors)
              ? doctors
              : []
          );

          this.loadingDoctors.set(false);
        },
        error: (error: unknown) => {
          this.loadingDoctors.set(false);

          this.doctorErrorMessage.set(
            getFriendlyErrorMessage(
              error,
              'Available doctors could not be loaded. Please try again later.'
            )
          );
        }
      });
  }
}