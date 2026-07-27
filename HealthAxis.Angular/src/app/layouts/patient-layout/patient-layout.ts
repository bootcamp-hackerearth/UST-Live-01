import {
  Component,
  HostListener,
  OnInit
} from '@angular/core';

import {
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet
} from '@angular/router';

import { filter } from 'rxjs';

import { TokenService } from '../../core/models/token.service';

@Component({
  selector: 'app-patient-layout',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './patient-layout.html',
  styleUrls: ['./patient-layout.css']
})
export class PatientLayout implements OnInit {
  displayName = 'Patient';
  userEmail = '';

  isUserMenuOpen = false;
  isSidebarOpen = false;

  constructor(
    private readonly tokenService: TokenService,
    private readonly router: Router
  ) {
    this.router.events
      .pipe(
        filter(
          event => event instanceof NavigationEnd
        )
      )
      .subscribe(() => {
        this.closeSidebar();
        this.closeUserMenu();
      });
  }

  ngOnInit(): void {
    const email =
      this.tokenService.getUserEmail();

    if (email) {
      this.userEmail = email;
      this.displayName =
        this.getFirstNameFromEmail(email);
    }
  }

  toggleSidebar(event: MouseEvent): void {
    event.stopPropagation();

    this.isSidebarOpen =
      !this.isSidebarOpen;

    this.isUserMenuOpen = false;
    this.updateBodyScroll();
  }

  closeSidebar(): void {
    this.isSidebarOpen = false;
    this.updateBodyScroll();
  }

  toggleUserMenu(event: MouseEvent): void {
    event.stopPropagation();

    this.isUserMenuOpen =
      !this.isUserMenuOpen;

    this.isSidebarOpen = false;
    this.updateBodyScroll();
  }

  closeUserMenu(): void {
    this.isUserMenuOpen = false;
  }

  logout(): void {
    this.closeSidebar();
    this.closeUserMenu();

    this.tokenService.clearAuthData();

    this.router.navigate(['/login']);
  }

  @HostListener('document:click')
  onDocumentClick(): void {
    this.closeUserMenu();
  }

  @HostListener('window:resize')
  onWindowResize(): void {
    if (window.innerWidth > 980) {
      this.closeSidebar();
    }
  }

  @HostListener('document:keydown.escape')
  onEscapePressed(): void {
    this.closeSidebar();
    this.closeUserMenu();
  }

  private updateBodyScroll(): void {
    document.body.style.overflow =
      this.isSidebarOpen
        ? 'hidden'
        : '';
  }

  private getFirstNameFromEmail(
    email: string
  ): string {
    const namePart =
      email.split('@')[0];

    const firstName =
      namePart
        .split(/[._-]/)
        .filter(Boolean)[0];

    return firstName || 'Patient';
  }
}