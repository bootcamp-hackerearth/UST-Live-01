import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  inject,
  signal
} from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { Sidebar } from '../../shared/sidebar/sidebar';
import { Topbar } from '../../shared/topbar/topbar';

const MOBILE_BREAKPOINT_IN_PIXELS = 980;

@Component({
  selector: 'app-dashboard-layout',
  imports: [
    RouterOutlet,
    Sidebar,
    Topbar
  ],
  templateUrl: './dashboard-layout.html',
  styleUrl: './dashboard-layout.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardLayout {
  private readonly authService = inject(AuthService);

  private readonly doctorStatusState =
    inject(DoctorStatusStateService);

  private wasCompactViewport =
    this.isCompactViewport();

  readonly isSidebarClosed = signal(
    this.wasCompactViewport
  );

  readonly isLogoutDialogOpen = signal(false);

  toggleSidebar(): void {
    this.isSidebarClosed.update(
      (isClosed) => !isClosed
    );
  }

  closeSidebar(): void {
    this.isSidebarClosed.set(true);
  }

  openLogoutDialog(): void {
    this.closeSidebarOnCompactViewport();
    this.isLogoutDialogOpen.set(true);
  }

  closeLogoutDialog(): void {
    this.isLogoutDialogOpen.set(false);
  }

  confirmLogout(): void {
    this.isLogoutDialogOpen.set(false);
    this.doctorStatusState.clearStatus();
    this.authService.logout();
  }

  @HostListener('document:keydown.escape')
  handleEscapeKey(): void {
    if (this.isLogoutDialogOpen()) {
      this.closeLogoutDialog();
      return;
    }

    if (
      this.isCompactViewport() &&
      !this.isSidebarClosed()
    ) {
      this.closeSidebar();
    }
  }

  @HostListener('window:resize')
  handleWindowResize(): void {
    const isCompactViewport =
      this.isCompactViewport();

    if (
      isCompactViewport &&
      !this.wasCompactViewport
    ) {
      this.closeSidebar();
    }

    this.wasCompactViewport =
      isCompactViewport;
  }

  private closeSidebarOnCompactViewport(): void {
    if (this.isCompactViewport()) {
      this.closeSidebar();
    }
  }

  private isCompactViewport(): boolean {
    return (
      typeof globalThis.innerWidth === 'number' &&
      globalThis.innerWidth <=
        MOBILE_BREAKPOINT_IN_PIXELS
    );
  }
}