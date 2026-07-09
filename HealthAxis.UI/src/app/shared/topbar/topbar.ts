import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  OnDestroy,
  inject,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { DoctorService } from '../../core/services/doctor.service';
import { getFriendlyErrorMessage } from '../../core/utils/api-error.util';

@Component({
  selector: 'app-topbar',
  imports: [DatePipe, RouterLink],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Topbar implements OnDestroy {
  readonly authService = inject(AuthService);

  private readonly router = inject(Router);
  private readonly doctorService = inject(DoctorService);

  readonly currentTime = signal(new Date());
  readonly isProfileMenuOpen = signal(false);

  readonly isDoctorActive = signal(true);
  readonly statusUpdating = signal(false);

  private readonly timerId = window.setInterval(() => {
    this.currentTime.set(new Date());
  }, 1000);

  private readonly doctorStatusChangedHandler = (event: Event): void => {
    const customEvent = event as CustomEvent<{ isActive: boolean }>;

    if (typeof customEvent.detail?.isActive === 'boolean') {
      this.isDoctorActive.set(customEvent.detail.isActive);
    }
  };

  constructor() {
    this.loadDoctorStatus();

    window.addEventListener(
      'doctor-status-changed',
      this.doctorStatusChangedHandler
    );
  }

  ngOnDestroy(): void {
    window.clearInterval(this.timerId);

    window.removeEventListener(
      'doctor-status-changed',
      this.doctorStatusChangedHandler
    );
  }

  @HostListener('document:click')
  closeProfileMenu(): void {
    this.isProfileMenuOpen.set(false);
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen.update((isOpen) => !isOpen);
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
      this.router.navigate(['/doctor/change-password']);
      return;
    }

    if (role === 'Patient') {
      this.router.navigate(['/patient/change-password']);
      return;
    }

    this.router.navigate(['/login']);
  }

  logout(): void {
    this.isProfileMenuOpen.set(false);
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  toggleDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    const nextStatus = !this.isDoctorActive();

    this.statusUpdating.set(true);

    this.doctorService.updateMyStatus(nextStatus).subscribe({
      next: (response) => {
        this.statusUpdating.set(false);
        this.isDoctorActive.set(response.isActive);

        window.dispatchEvent(
          new CustomEvent('doctor-status-changed', {
            detail: {
              isActive: response.isActive,
              message: response.message
            }
          })
        );
      },
      error: (error: unknown) => {
        this.statusUpdating.set(false);

        window.dispatchEvent(
          new CustomEvent('doctor-status-update-failed', {
            detail: {
              message: getFriendlyErrorMessage(
                error,
                'Could not update doctor status.'
              )
            }
          })
        );
      }
    });
  }

  private loadDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    this.doctorService.getMyDoctorProfile().subscribe({
      next: (doctor) => {
        this.isDoctorActive.set(Boolean(doctor.isActive));
      },
      error: () => {
        this.isDoctorActive.set(true);
      }
    });
  }
}