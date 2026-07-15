import {
  ChangeDetectionStrategy,
  Component,
  HostListener,
  inject,
  signal
} from '@angular/core';
import {
  Router,
  RouterOutlet
} from '@angular/router';

import { AuthService } from '../../core/services/auth.service';
import { DoctorStatusStateService } from '../../core/services/doctor-status-state.service';
import { Sidebar } from '../../shared/sidebar/sidebar';
import { Topbar } from '../../shared/topbar/topbar';

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
  private readonly router = inject(Router);

  readonly isSidebarClosed = signal(false);
  readonly isLogoutDialogOpen = signal(false);

  toggleSidebar(): void {
    this.isSidebarClosed.update(
      (isClosed) => !isClosed
    );
  }

  openLogoutDialog(): void {
    this.isLogoutDialogOpen.set(true);
  }

  closeLogoutDialog(): void {
    this.isLogoutDialogOpen.set(false);
  }

  confirmLogout(): void {
    this.isLogoutDialogOpen.set(false);

    this.authService.logout();
    this.doctorStatusState.clearStatus();

    void this.router.navigate(['/login']);
  }

  @HostListener('document:keydown.escape')
  closeLogoutDialogWithEscape(): void {
    if (this.isLogoutDialogOpen()) {
      this.closeLogoutDialog();
    }
  }
}