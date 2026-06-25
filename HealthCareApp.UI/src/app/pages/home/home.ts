import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class HomeComponent {
  constructor(private router: Router) {
  }

  goToPatientLogin(): void {
    this.router.navigate(['/patient/login']);
  }

  goToDoctorLogin(): void {
    this.router.navigate(['/doctor/login']);
  }
}
