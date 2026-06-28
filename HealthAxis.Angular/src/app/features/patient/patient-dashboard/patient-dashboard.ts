import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-patient-dashboard',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboard implements OnInit {
  displayName = 'Patient';
  userEmail = '';

  constructor(
    private tokenService: TokenService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const email = this.tokenService.getUserEmail();

    if (email) {
      this.userEmail = email;
      this.displayName = this.getFirstNameFromEmail(email);
    }
  }

  logout(): void {
    this.tokenService.clearAuthData();
    this.router.navigate(['/login']);
  }

  private getFirstNameFromEmail(email: string): string {
    const namePart = email.split('@')[0];

    const firstName =
      namePart
        .split(/[._-]/)
        .filter(Boolean)[0];

    return firstName || 'Patient';
  }
}