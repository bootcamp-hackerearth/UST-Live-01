import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true, 
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent implements OnInit {

  showLogoutConfirm = false;
  role: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit():void {
    this.role = this.authService.getRole();
  }

openLogoutConfirm() {
  this.showLogoutConfirm = true;
}

confirmLogout() {
  this.authService.logout();
  this.router.navigate(['/']);
}

cancelLogout() {
  this.showLogoutConfirm = false;
}

}