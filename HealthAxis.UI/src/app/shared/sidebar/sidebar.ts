import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  output
} from '@angular/core';
import {
  RouterLink,
  RouterLinkActive
} from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

type SidebarIcon =
  | 'dashboard'
  | 'calendar'
  | 'appointments'
  | 'records'
  | 'profile'
  | 'completed';

interface SidebarMenuItem {
  label: string;
  icon: SidebarIcon;
  path: string;
}

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Sidebar {
  readonly authService = inject(AuthService);

  readonly navigationRequested = output<void>();
  readonly logoutRequested = output<void>();

  private readonly patientMenu:
    readonly SidebarMenuItem[] = [
      {
        label: 'Dashboard',
        icon: 'dashboard',
        path: '/patient/dashboard'
      },
      {
        label: 'Book Appointment',
        icon: 'calendar',
        path: '/patient/book-appointment'
      },
      {
        label: 'My Appointments',
        icon: 'appointments',
        path: '/patient/my-appointments'
      },
      {
        label: 'Health Records',
        icon: 'records',
        path: '/patient/health-records'
      },
      {
        label: 'My Profile',
        icon: 'profile',
        path: '/patient/profile'
      }
    ];

  private readonly doctorMenu:
    readonly SidebarMenuItem[] = [
      {
        label: 'Dashboard',
        icon: 'dashboard',
        path: '/doctor/dashboard'
      },
      {
        label: 'Appointments',
        icon: 'calendar',
        path: '/doctor/upcoming-appointments'
      },
      {
        label: 'Completed Visits',
        icon: 'completed',
        path: '/doctor/completed-appointments'
      },
      {
        label: 'Health Records',
        icon: 'records',
        path: '/doctor/health-records'
      },
      {
        label: 'My Profile',
        icon: 'profile',
        path: '/doctor/profile'
      }
    ];

  readonly menuItems = computed(() => {
    const role = this.authService.role();

    if (role === 'Patient') {
      return this.patientMenu;
    }

    if (role === 'Doctor') {
      return this.doctorMenu;
    }

    return [];
  });

  readonly homeRoute = computed(() =>
    this.getPortalRoute('dashboard')
  );

  readonly contactRoute = computed(() =>
    this.getPortalRoute('contact-us')
  );

  requestNavigation(): void {
    this.navigationRequested.emit();
  }

  requestLogout(): void {
    this.logoutRequested.emit();
  }

  private getPortalRoute(
    routeSegment: string
  ): string {
    const role = this.authService.role();

    if (role === 'Doctor') {
      return `/doctor/${routeSegment}`;
    }

    if (role === 'Patient') {
      return `/patient/${routeSegment}`;
    }

    return '/login';
  }
}