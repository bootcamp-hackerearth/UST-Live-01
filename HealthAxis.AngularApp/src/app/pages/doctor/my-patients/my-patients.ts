import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-my-patients',
  standalone: false,
  templateUrl: './my-patients.html',
  styleUrl: './my-patients.css'
})
export class MyPatients implements OnInit {

  patients = signal<any[]>([]);
  message = signal('');

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadPatients();
  }

  loadPatients() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      'https://localhost:7038/api/healthrecords/doctor/patients',
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
