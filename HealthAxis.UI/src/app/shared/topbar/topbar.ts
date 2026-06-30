import {
  ChangeDetectionStrategy,
  Component,
  DestroyRef,
  HostListener,
  computed,
  inject,
  signal
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-topbar',
  imports: [DatePipe, RouterLink],
  templateUrl: './topbar.html',
  styleUrl: './topbar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Topbar {
  readonly authService = inject(AuthService);
  readonly currentTime = signal(new Date());
  readonly isProfileMenuOpen = signal(false);

  private readonly destroyRef = inject(DestroyRef);

  readonly profileRoute = computed(() => {
    const role = this.authService.role();

    if (role === 'Patient') {
      return '/patient/profile';
    }

    if (role === 'Doctor') {
      return '/doctor/dashboard';
    }

    return '/login';
  });

  constructor() {
    const timerId = window.setInterval(() => {
      this.currentTime.set(new Date());
    }, 30000);

    this.destroyRef.onDestroy(() => {
      window.clearInterval(timerId);
    });
  }

  @HostListener('document:click')
  closeProfileMenu(): void {
    this.isProfileMenuOpen.set(false);
  }

  getGreeting(): string {
    const hour = this.currentTime().getHours();

    if (hour >= 5 && hour < 12) {
      return 'Good Morning';
    }

    if (hour >= 12 && hour < 17) {
      return 'Good Afternoon';
    }

    if (hour >= 17 && hour < 21) {
      return 'Good Evening';
    }

    return 'Welcome';
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen.update((isOpen) => !isOpen);
  }

  logout(): void {
    this.isProfileMenuOpen.set(false);
    this.authService.logout();
  }
}