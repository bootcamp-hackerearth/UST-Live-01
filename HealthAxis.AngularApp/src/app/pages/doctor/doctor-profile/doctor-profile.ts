import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';
import { API_BASE_URL } from '../../../core/constants/api.constants';

@Component({
  selector: 'app-doctor-profile',
  standalone: false,
  templateUrl: './doctor-profile.html',
  styleUrl: './doctor-profile.css'
})
export class DoctorProfile implements OnInit {

  doctor = signal<any | null>(null);

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      `${API_BASE_URL}/doctors/profile`,
      { headers }
    )
      .subscribe({
        next: (res) => {
          this.doctor.set(res);
        },
        error: (err) => {
          console.error(err);
        }
      });
  }

  goBack(): void {
    this.router.navigate(['/doctor/dashboard']);
  }
}
