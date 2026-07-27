import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../../../core/constants/api.constants';


@Component({
  selector: 'app-my-health-records',
  standalone: false,
  templateUrl: './my-health-records.html',
  styleUrl: './my-health-records.css'
})
export class MyHealthRecords implements OnInit {

  records = signal<any[]>([]);
  message = signal('');

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadRecords();
  }

  goBack(): void {
    this.router.navigate(['/patient/dashboard']);
  }

  loadRecords() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      `${API_BASE_URL}/healthrecords/my`,
      { headers }
    ).subscribe({
      next: (res) => {
        this.records.set(res);
      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load health records ❌'
        );

      }
    });
  }
}
