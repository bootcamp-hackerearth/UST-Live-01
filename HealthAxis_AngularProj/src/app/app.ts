import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { ToastContainerComponent } from './shared/toast/toast-container';
import { ConfirmModalComponent } from './shared/confirm/confirm-modal';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    ToastContainerComponent,
    ConfirmModalComponent
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {

}