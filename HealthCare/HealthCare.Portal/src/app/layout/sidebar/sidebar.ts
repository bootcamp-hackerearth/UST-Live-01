import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar {
  role = localStorage.getItem('role')?.toUpperCase();

  constructor(private readonly router: Router) { }

  logout(): void {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
