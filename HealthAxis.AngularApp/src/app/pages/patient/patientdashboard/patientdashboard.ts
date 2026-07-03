import { Component } from '@angular/core';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-patientdashboard',
  standalone: false,
  templateUrl: './patientdashboard.html',
  styleUrl: './patientdashboard.css',
})

export class Patientdashboard {

  auth = authState;

}

