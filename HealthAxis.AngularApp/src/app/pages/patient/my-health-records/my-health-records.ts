import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-my-health-records',
  standalone: false,
  templateUrl: './my-health-records.html',
  styleUrl: './my-health-records.css'
})
export class MyHealthRecords implements OnInit {

  records = signal<any[]>([]);
  message = signal('');

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadRecords();
  }

  loadRecords() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      'https://localhost:7038/api/healthrecords/my',
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
