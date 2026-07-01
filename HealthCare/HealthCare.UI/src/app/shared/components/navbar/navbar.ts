import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink], 
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent {

  showLogoutConfirm = false;
  role: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
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