import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-patient-layout',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './patient-layout.html',
  styleUrls: ['./patient-layout.css']
})
export class PatientLayout implements OnInit {

  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  patientName = 'Patient';
  patientEmail = 'patient@healthaxis.com';
  patientInitial = 'P';

  private readonly patientUrl = 'https://localhost:7130/api/patients/me';

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadPatient();
    }
  }

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token');

    return new HttpHeaders({
      Authorization: `Bearer ${token}`
    });
  }

  loadPatient() {
    this.http.get<any>(this.patientUrl, { headers: this.getHeaders() })
      .subscribe({
        next: (res: any) => {
          console.log('Sidebar patient ✅:', res);

          this.patientName = res.fullName || 'Patient';
          this.patientEmail = res.email || 'patient@healthaxis.com';
          this.patientInitial = this.patientName.charAt(0).toUpperCase();

          localStorage.setItem('patientId', String(res.patientId));
        },
        error: (err: any) => {
          console.error('Sidebar patient load failed ❌:', err);
        }
      });
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}