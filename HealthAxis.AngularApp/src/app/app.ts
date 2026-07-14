import { Component, signal } from '@angular/core';
import { authState } from './core/auth-state';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: false,
  styleUrl: './app.css'
})
export class App {

  protected readonly title = signal('HealthAxis.AngularApp');

  constructor() {

    const storedAuth = localStorage.getItem('auth');

    if (storedAuth) {

      authState.set(JSON.parse(storedAuth));

      console.log('Auth Restored:', authState());
    }
  }
}
