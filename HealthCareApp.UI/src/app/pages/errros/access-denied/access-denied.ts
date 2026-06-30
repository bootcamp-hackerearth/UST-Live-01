import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-access-denied',
  standalone: true,
  templateUrl: './access-denied.html',
  styleUrl: './access-denied.css'
})
export class AccessDenied {
  constructor(private router: Router) {
  }

  goHome(): void {
    this.router.navigate(['/']);
  }
}