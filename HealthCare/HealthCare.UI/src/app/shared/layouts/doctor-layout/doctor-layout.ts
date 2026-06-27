import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { NavbarComponent } from '../../components/navbar/navbar';
import { SidebarComponent } from '../../components/sidebar/sidebar';

@Component({
  selector: 'app-doctor-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent,
    SidebarComponent
  ],
  templateUrl: './doctor-layout.html',
  styleUrl: './doctor-layout.css'
})
export class DoctorLayoutComponent {
}