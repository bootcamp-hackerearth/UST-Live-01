import { Component, OnInit, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../../../core/constants/api.constants';

@Component({
  selector: 'app-my-appointments',
  standalone: false,
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css'
})
export class MyAppointments implements OnInit {

  appointments = signal<any[]>([]);
  message = signal('');

  currentPage = signal(1);
  pageSize = 5;


  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
    this.loadAppointments();
  }

  goBack(): void {
    this.router.navigate(['/patient/dashboard']);
  }

  loadAppointments() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any[]>(
      `${API_BASE_URL}/appointments/my?page=${this.currentPage()}&pageSize=${this.pageSize}`,
      { headers }
    ).subscribe({
      next: (res) => {
        this.appointments.set(res);
      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load appointments ❌'
        );

      }
    });
  }

  nextPage() {
    this.currentPage.update(page => page + 1);
    this.loadAppointments();
  }

  previousPage() {
    if (this.currentPage() > 1) {
      this.currentPage.update(page => page - 1);
      this.loadAppointments();
    }
  }
}
