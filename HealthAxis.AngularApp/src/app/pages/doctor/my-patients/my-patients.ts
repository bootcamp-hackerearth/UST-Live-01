import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../../../core/constants/api.constants';

@Component({
  selector: 'app-my-patients',
  standalone: false,
  templateUrl: './my-patients.html',
  styleUrl: './my-patients.css'
})
export class MyPatients implements OnInit {

  patients = signal<any[]>([]);
  message = signal('');

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadPatients();
  }

  goBack(): void {
    this.router.navigate(['/doctor/dashboard']);
  }

  loadPatients() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      `${API_BASE_URL}/healthrecords/doctor/patients`,
      { headers }
    ).subscribe({
      next: (res) => {
        this.patients.set(res);
      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load patients ❌'
        );

      }
    });

  }
}
