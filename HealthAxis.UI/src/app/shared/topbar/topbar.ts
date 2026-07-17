
import { DatePipe } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  OnDestroy,
  inject,
  output,
  signal
} from '@angular/core';
import {
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { DoctorService } from '../../core/services/doctor.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { PatientService } from '../../core/services/patient.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

const CLOCK_INTERVAL_IN_MS = 1000;
const ERROR_DURATION_IN_MS = 3000;

@Component({
  selector: 'app-topbar',
  imports: [
    DatePipe,
    RouterLink
  ],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Topbar implements OnDestroy {
  readonly authService = inject(AuthService);
  readonly doctorStatusState =
    inject(DoctorStatusStateService);

  private readonly router = inject(Router);
  private readonly doctorService = inject(DoctorService);
  private readonly patientService = inject(PatientService);

  readonly logoutRequested = output<void>();

  readonly currentTime = signal(new Date());
  readonly displayName = signal('');
  readonly isProfileMenuOpen = signal(false);
  readonly statusUpdating = signal(false);
  readonly statusError = signal('');

  private readonly timerId = globalThis.setInterval(() => {
    this.currentTime.set(new Date());
  }, CLOCK_INTERVAL_IN_MS);

  constructor() {
    this.loadCurrentUser();
  }

  ngOnDestroy(): void {
    globalThis.clearInterval(this.timerId);
  }

  @HostListener('document:click')
  closeProfileMenu(): void {
    this.isProfileMenuOpen.set(false);
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen.update(
      (isOpen) => !isOpen
    );
  }

  getGreeting(): string {
    const hour = new Date().getHours();

    if (hour < 12) {
      return 'Good Morning';
    }

    if (hour < 17) {
      return 'Good Afternoon';
    }

    return 'Good Evening';
  }

  getFirstName(): string {
    const name = this.displayName().trim();

    if (!name) {
      return this.authService.role() || 'User';
    }

    return name.split(/\s+/)[0];
  }

  getProfileName(): string {
    const name = this.displayName().trim();

    return name ||
      this.authService.role() ||
      'User';
  }

  getProfileInitial(): string {
    return this.getProfileName()
      .charAt(0)
      .toUpperCase();
  }

  getDoctorStatusText(): string {
    const status =
      this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading';
    }

    return status
      ? 'Available'
      : 'Unavailable';
  }

  profileRoute(): string {
    const role = this.authService.role();

    if (role === 'Doctor') {
      return '/doctor/profile';
    }

    if (role === 'Patient') {
      return '/patient/profile';
    }

    return '/login';
  }

  goToChangePassword(): void {
    this.isProfileMenuOpen.set(false);

    const role = this.authService.role();

    if (role === 'Doctor') {
      void this.router
        .navigate(['/doctor/profile'])
        .then((navigated) => {
          if (!navigated) {
            return;
          }

          globalThis.setTimeout(() => {
            document
              .getElementById('change-password')
              ?.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
              });
          });
        });

      return;
    }

    if (role === 'Patient') {
      void this.router
        .navigate(['/patient/profile'])
        .then((navigated) => {
          if (!navigated) {
            return;
          }

          globalThis.setTimeout(() => {
            document
              .getElementById('change-password')
              ?.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
              });
          });
        });

      return;
    }

    void this.router.navigate(['/login']);
  }

  requestLogout(): void {
    this.isProfileMenuOpen.set(false);
    this.logoutRequested.emit();
  }

  toggleDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    const currentStatus =
      this.doctorStatusState.isActive();

    if (currentStatus === null) {
      this.showStatusError(
        'Doctor status is still loading.'
      );
      return;
    }

    this.statusUpdating.set(true);
    this.statusError.set('');

    this.doctorService
      .updateMyStatus(!currentStatus)
      .subscribe({
        next: (response) => {
          this.statusUpdating.set(false);

          this.doctorStatusState.setStatus(
            response.isActive
          );
        },
        error: (error: unknown) => {
          this.statusUpdating.set(false);

          this.showStatusError(
            getFriendlyErrorMessage(
              error,
              'Could not update doctor status.'
            )
          );
        }
      });
  }

  private loadCurrentUser(): void {
    const role = this.authService.role();

    if (role === 'Patient') {
      this.loadPatientDetails();
      return;
    }

    if (role === 'Doctor') {
      this.loadDoctorDetails();
    }
  }

  private loadPatientDetails(): void {
    this.patientService
      .getMyProfile()
      .subscribe({
        next: (patient) => {
          this.displayName.set(
            patient.fullName
          );
        },
        error: () => {
          this.displayName.set('Patient');
        }
      });
  }

  private loadDoctorDetails(): void {
    this.doctorService
      .getMyDoctorProfile()
      .subscribe({
        next: (doctor) => {
          this.displayName.set(
            doctor.fullName
          );

          this.doctorStatusState.setStatus(
            Boolean(doctor.isActive)
          );
        },
        error: () => {
          this.displayName.set('Doctor');
          this.doctorStatusState.clearStatus();
        }
      });
  }

  private showStatusError(
    message: string
  ): void {
    this.statusError.set(message);

    globalThis.setTimeout(() => {
      this.statusError.set('');
    }, ERROR_DURATION_IN_MS);
  }
}

