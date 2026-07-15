import { Component, computed } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-patient-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './patient-layout.html',
  styleUrl: './patient-layout.css'
})
export class PatientLayout {
  patientName = computed(() =>
    this.authService.currentUser()?.fullName ?? 'Patient'
  );

  constructor(private readonly authService: AuthService) {
  }

  logout(): void {
    this.authService.logout();
  }
}
