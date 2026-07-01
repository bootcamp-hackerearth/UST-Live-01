import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  adminPortalUrl = 'https://localhost:7200';

  constructor(private router: Router) {}

  goToPatientLogin(): void {
    this.router.navigate(['/login']);
  }

  goToDoctorLogin(): void {
    this.router.navigate(['/login']);
  }

  openAdminPortal(): void {
    window.location.href = this.adminPortalUrl;
  }
}