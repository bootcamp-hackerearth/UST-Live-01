import { Component, computed } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';
import { ThemeToggle } from '../../../shared/theme-toggle/theme-toggle';

@Component({
  selector: 'app-doctor-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    ThemeToggle
  ],
  templateUrl: './doctor-layout.html',
  styleUrl: './doctor-layout.css'
})
export class DoctorLayout {
  doctorName = computed(() =>
    this.authService.currentUser()?.fullName ?? 'Doctor'
  );

  isFirstLogin = computed(() =>
    this.authService.currentUser()?.firstLogin ?? false
  );

  constructor(private authService: AuthService) {
  }

  logout(): void {
    this.authService.logout();
  }
}
