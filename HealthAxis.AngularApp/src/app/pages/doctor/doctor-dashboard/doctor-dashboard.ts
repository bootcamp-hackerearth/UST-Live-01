import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: false,
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})
export class DoctorDashboard {

  auth = authState;

  constructor(private router: Router) { }

  logout() {

    authState.set({
      token: '',
      role: '',
      name: '',
      isLoggedIn: false
    });

    localStorage.removeItem('auth');

    this.router.navigate(['/login']);
  }
}
