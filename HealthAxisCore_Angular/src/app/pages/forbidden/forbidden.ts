import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

import { ThemeToggle } from '../../shared/theme-toggle/theme-toggle';

@Component({
  selector: 'app-forbidden',
  imports: [
    RouterLink,
    ThemeToggle
  ],
  templateUrl: './forbidden.html',
  styleUrl: './forbidden.css'
})
export class Forbidden {
}
