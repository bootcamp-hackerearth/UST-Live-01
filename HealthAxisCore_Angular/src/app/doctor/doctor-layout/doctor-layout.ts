import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { DoctorSession } from '../../services/doctor-session';

@Component({
  selector: 'app-doctor-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './doctor-layout.html',
  styleUrl: './doctor-layout.css'
})
export class DoctorLayout {
  doctorName = '';

  constructor(
    private doctorSession: DoctorSession,
    private router: Router
  ) {
    const doctor = this.doctorSession.getCurrentDoctor();

    this.doctorName = doctor.doctorName;
  }

  logout(): void {
    localStorage.removeItem('healthaxis_access_token');
    localStorage.removeItem('healthaxis_refresh_token');
    localStorage.removeItem('healthaxis_user');
    localStorage.removeItem('healthaxis_role');

    this.router.navigate(['/login']);
  }
}
