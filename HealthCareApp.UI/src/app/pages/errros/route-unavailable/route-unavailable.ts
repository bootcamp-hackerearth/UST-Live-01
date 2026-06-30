import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-route-unavailable',
  standalone: true,
  templateUrl: './route-unavailable.html',
  styleUrl: './route-unavailable.css'
})
export class RouteUnavailable {
  constructor(private router: Router) {
  }

  goHome(): void {
    this.router.navigate(['/']);
  }
}