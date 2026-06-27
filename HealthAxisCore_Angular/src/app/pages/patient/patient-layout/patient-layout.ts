import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { PatientSession } from '../../../services/patient-session';

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
  patientName = '';

  constructor(
    private patientSession: PatientSession,
    private router: Router
  ) {
    const patient = this.patientSession.getCurrentPatient();

    this.patientName = patient.fullName;
  }

  logout(): void {
    localStorage.removeItem('healthaxis_access_token');
    localStorage.removeItem('healthaxis_refresh_token');
    localStorage.removeItem('healthaxis_user');
    localStorage.removeItem('healthaxis_role');

    this.router.navigate(['/login']);
  }
}
