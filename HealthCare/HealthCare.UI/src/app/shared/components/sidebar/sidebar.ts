import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class SidebarComponent {

  role: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    this.role = this.authService.getRole();
  }

  logout() {
  this.authService.logout();
  this.router.navigate(['/login']);
}
}
