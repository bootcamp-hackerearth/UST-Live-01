import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { BookAppointmentComponent } from '../../../features/patient/book-appointment/book-appointment';


@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive,
    BookAppointmentComponent
  ],
  templateUrl: './sidebar.html'
})
export class SidebarComponent {

  role: string | null = null;
  showBookingModal = false;
  isSidebarOpen = false;
  menuItems: any[] = [];
  showLogoutConfirm = false;


  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.role = this.authService.getRole();
    this.setupMenu();
  }

  setupMenu() {

    const menusByRole: any = {

      Patient: [
        { label: 'Dashboard', icon: '🏠', link: '/patient/dashboard' },
        { label: 'Search Doctors', icon: '🔍', link: '/patient/search-doctors' },

        {
          label: 'Book Appointment',
          icon: '📅',
          type: 'action',
          action: () => this.showBookingModal = true
        },

        { label: 'My Appointments', icon: '🗓️', link: '/patient/my-appointments' },
        { label: 'Health History', icon: '📄', link: '/patient/health-history' },
        { label: 'My Profile', icon: '👤', link: '/patient/profile' }
      ],

      Doctor: [
        { label: 'Dashboard', icon: '🏠', link: '/doctor/dashboard' },
        { label: "My Schedule", icon: '📅', link: '/doctor/schedule' },
        //{ label: 'Add Health Record', icon: '🩺', link: '/doctor/health-record' },
        { label: 'Add Leave', icon: '🌴', link: '/doctor/leave' },
        { label: 'My Profile', icon: '👤', link: '/doctor/profile' }
      ]
    };

    //  assign role menu
    this.menuItems = menusByRole[this.role || ''] || [];

    //  add logout always at bottom
    this.menuItems.push({
      label: 'Logout',
      icon: '🚪',
      type: 'action',
      action: () => this.openLogoutConfirm()
    });
  }

  openLogoutConfirm() {
  this.showLogoutConfirm = true;
}

confirmLogout() {
  this.authService.logout();
  this.router.navigate(['/']);
}

cancelLogout() {
  this.showLogoutConfirm = false;
}

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }
}