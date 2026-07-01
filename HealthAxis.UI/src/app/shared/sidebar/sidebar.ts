import { ChangeDetectionStrategy, Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

interface SidebarMenuItem {
  label: string;
  icon: string;
  path: string;
}

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Sidebar {
  readonly authService = inject(AuthService);

  private readonly patientMenu: readonly SidebarMenuItem[] = [
    {
      label: 'Dashboard',
      icon: '🏠',
      path: '/patient/dashboard'
    },
    {
      label: 'Book Appointment',
      icon: '📅',
      path: '/patient/book-appointment'
    },
    {
      label: 'My Appointments',
      icon: '📋',
      path: '/patient/my-appointments'
    },
    {
      label: 'Health Records',
      icon: '🧾',
      path: '/patient/health-records'
    },
    {
      label: 'My Profile',
      icon: '👤',
      path: '/patient/profile'
    }
  ];

  private readonly doctorMenu: readonly SidebarMenuItem[] = [
    {
      label: 'Dashboard',
      icon: '🏠',
      path: '/doctor/dashboard'
    },
    {
      label: 'Appointments',
      icon: '📅',
      path: '/doctor/upcoming-appointments'
    },
    {
      label: 'Completed Visits',
      icon: '✅',
      path: '/doctor/completed-appointments'
    },
    {
      label: 'Health Records',
      icon: '🧾',
      path: '/doctor/health-records'
    },
    {
      label: 'My Profile',
      icon: '👤',
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

  readonly contactRoute = computed(() => {
    const role = this.authService.role();

    if (role === 'Doctor') {
      return '/doctor/contact-us';
    }

    return '/patient/contact-us';
  });

  logout(): void {
    this.authService.logout();
  }
}