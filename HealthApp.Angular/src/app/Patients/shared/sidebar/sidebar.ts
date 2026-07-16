import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AppPopupComponent } from '../../../shared/app-popup/app-popup';


@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterModule, CommonModule, AppPopupComponent],
  templateUrl: './sidebar.html',
  styleUrls: ['./sidebar.css']
})
export class Sidebar {

  popupVisible = false;
  popupTitle = '';
  popupMessage = '';
  popupType: 'success' | 'error' | 'warning' = 'warning';

  showLogoutConfirm = false;

  constructor(private router: Router) {}

  logout(): void {
    this.popupTitle = 'Logout';
    this.popupMessage = 'Are you sure you want to logout from HealthSphere?';
    this.popupType = 'warning';

    this.popupVisible = true;
    this.showLogoutConfirm = true;
  }

  closePopup(): void {
    this.popupVisible = false;
    this.showLogoutConfirm = false;
  }

  confirmLogout(): void {
    localStorage.clear();
    sessionStorage.clear();

    this.popupVisible = false;
    this.showLogoutConfirm = false;

    this.router.navigate(['/']);
  }
}