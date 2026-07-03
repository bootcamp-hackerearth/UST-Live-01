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

  constructor(private readonly router: Router) {}

  goToPatientLogin(): void {
    this.router.navigate(['/login']);
  }

  goToDoctorLogin(): void {
    this.router.navigate(['/login']);
  }

  openAdminPortal(): void {
    globalThis.location.href = this.adminPortalUrl;
  }
}