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
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
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
  readonly doctorStatusState = inject(DoctorStatusStateService);

  private readonly router = inject(Router);
  private readonly doctorService = inject(DoctorService);

  readonly currentTime = signal(new Date());
  readonly isProfileMenuOpen = signal(false);
  readonly statusUpdating = signal(false);
  readonly statusError = signal('');

  private readonly timerId = window.setInterval(() => {
    this.currentTime.set(new Date());
  }, 1000);

  constructor() {
    this.loadDoctorStatus();
  }

  ngOnDestroy(): void {
    window.clearInterval(this.timerId);
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

  getDoctorStatusText(): string {
    const status = this.doctorStatusState.isActive();

    if (status === null) {
      return 'Loading...';
    }

    return status ? 'Active' : 'Inactive';
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
    this.doctorStatusState.clearStatus();
    this.router.navigate(['/login']);
  }

  toggleDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    const currentStatus = this.doctorStatusState.isActive();

    if (currentStatus === null) {
      this.statusError.set('Doctor status is still loading.');
      return;
    }

    const nextStatus = !currentStatus;

    this.statusUpdating.set(true);
    this.statusError.set('');

    this.doctorService.updateMyStatus(nextStatus).subscribe({
      next: (response) => {
        this.statusUpdating.set(false);
        this.doctorStatusState.setStatus(response.isActive);
      },
      error: (error: unknown) => {
        this.statusUpdating.set(false);
        this.statusError.set(
          getFriendlyErrorMessage(error, 'Could not update doctor status.')
        );

        window.setTimeout(() => {
          this.statusError.set('');
        }, 3000);
      }
    });
  }

  private loadDoctorStatus(): void {
    if (this.authService.role() !== 'Doctor') {
      return;
    }

    this.doctorService.getMyDoctorProfile().subscribe({
      next: (doctor) => {
        this.doctorStatusState.setStatus(Boolean(doctor.isActive));
      },
      error: () => {
        this.doctorStatusState.clearStatus();
      }
    });
  }
}