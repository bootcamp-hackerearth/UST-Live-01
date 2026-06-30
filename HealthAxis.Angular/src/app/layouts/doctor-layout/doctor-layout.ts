import { Component, HostListener, OnInit } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import { TokenService } from '../../core/models/token.service';

@Component({
  selector: 'app-doctor-layout',
  imports: [RouterOutlet,RouterLink,RouterLinkActive],
  templateUrl: './doctor-layout.html',
  styleUrls: ['./doctor-layout.css']
})
export class DoctorLayout implements OnInit {
  displayName = 'Doctor';
  userEmail = '';
  isUserMenuOpen = false;

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

  toggleUserMenu(event: MouseEvent): void {
    event.stopPropagation();
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }

  closeUserMenu(): void {
    this.isUserMenuOpen = false;
  }

  logout(): void {
    this.tokenService.clearAuthData();
    this.router.navigate(['/login']);
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.isUserMenuOpen = false;
  }

  private getFirstNameFromEmail(email: string): string {
    const namePart = email.split('@')[0];

    const firstName =
      namePart
        .split(/[._-]/)
        .filter(Boolean)[0];

    return firstName || 'Doctor';
  }
}