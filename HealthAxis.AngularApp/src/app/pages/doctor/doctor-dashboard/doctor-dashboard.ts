import { Component } from '@angular/core';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: false,
  templateUrl: './doctor-dashboard.html',
  styleUrl: './doctor-dashboard.css',
})
export class DoctorDashboard {
  auth = authState;
}
