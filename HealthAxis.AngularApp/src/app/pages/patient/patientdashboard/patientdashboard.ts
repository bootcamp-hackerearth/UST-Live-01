import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-patientdashboard',
  standalone: false,
  templateUrl: './patientdashboard.html',
  styleUrl: './patientdashboard.css',
})
export class Patientdashboard {

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
