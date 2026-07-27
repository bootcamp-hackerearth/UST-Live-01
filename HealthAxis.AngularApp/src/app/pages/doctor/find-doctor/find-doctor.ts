import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../../../core/constants/api.constants';


@Component({
  selector: 'app-find-doctor',
  standalone: false,
  templateUrl: './find-doctor.html',
  styleUrl: './find-doctor.css',
})
export class FindDoctor implements OnInit {

  name = signal('');
  specialization = signal('');

  doctors = signal<any[]>([]);

  selectedDoctor = signal<any>(null);

  constructor(private http: HttpClient, private router: Router) { }

  goBack(): void {
    this.router.navigate(['/patient/dashboard']);
  }

  search() {

    const params: any = {};

    if (this.name()) params.name = this.name();
    if (this.specialization()) params.specialization = this.specialization();

    if (!this.name() && !this.specialization()) {
      this.loadAllDoctors();
      return;
    }


    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      `${API_BASE_URL}/doctors/filter`,
      { params, headers }
    )
      .subscribe({
        next: (res) => {
          console.log("API response:", res);
          this.doctors.set(res);
        },
        error: (err) => {
          console.error("Search failed:", err);
        }
      });
  }


  ngOnInit() {
    this.loadAllDoctors();
  }

  loadAllDoctors() {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(`${API_BASE_URL}/doctors/active`, { headers })
      .subscribe({
        next: (res) => {
          this.doctors.set(res);
        },
        error: () => {
          console.error("Failed to load doctors");
        }
      });
  }

  book(doc: any) {
    console.log("Book clicked:", doc);

    this.router.navigate(['/patient/book-appointment'], {
      state: { doctor: doc }
    });
  }

}
