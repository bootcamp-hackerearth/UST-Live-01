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

  constructor(private router: Router) {}

  logout(): void {
    this.popupTitle = 'Logout';
    this.popupMessage = 'Are you sure you want to logout from HealthAxis?';
    this.popupType = 'warning';
    this.popupVisible = true;
  }

  closePopup(): void {
    this.popupVisible = false;
  }

  confirmLogout(): void {
    localStorage.clear();
    sessionStorage.clear();

    this.popupVisible = false;

    this.router.navigate(['/']);
  }
}