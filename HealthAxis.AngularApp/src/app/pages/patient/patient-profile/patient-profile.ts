import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';
import { API_BASE_URL } from '../../../core/constants/api.constants';

@Component({
  selector: 'app-patient-profile',
  standalone: false,
  templateUrl: './patient-profile.html',
  styleUrl: './patient-profile.css'
})
export class PatientProfile implements OnInit {

  patient = signal<any | null>(null);

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      `${API_BASE_URL}/patients/profile`,
      { headers }
    )
      .subscribe({
        next: (res) => {
          this.patient.set(res);
        },
        error: (err) => {
          console.error(err);
        }
      });

  }

  goBack(): void {
    this.router.navigate(['/patient/dashboard']);
  }
}
