import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

import { AuthService } from '../../../core/services/auth-service';
import { PatientService } from '../../../core/services/patient-service';
import { Patient } from '../../../core/models/patient';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './patient-dashboard.html',
  styleUrl: './patient-dashboard.css'
})
export class PatientDashboard implements OnInit {

  patient?: Patient;

  constructor(
    private authService: AuthService,
    private patientService: PatientService,
    private router: Router,
     private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {

    this.loadProfile();

  }

  loadProfile(): void {

    this.patientService.getMyProfile().subscribe({

      next: (response) => {

        
        this.patient = response;
         this.cdr.detectChanges();

      },

      error: (err) => {

        console.log(err);

      }

    });

  }

  logout(): void {

    this.authService.logout();

    this.router.navigate(['/login']);

  }

}