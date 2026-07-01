import { Component, OnInit, inject, NgZone, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { DoctorProfileService } from '../../services/doctor-profile';

@Component({
  selector: 'app-doctor-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './doctor-layout.html',
  styleUrls: ['./doctor-layout.css']
})
export class DoctorLayout implements OnInit {

  private doctorProfileService = inject(DoctorProfileService);
  private router = inject(Router);
  private zone = inject(NgZone);
  private cdr = inject(ChangeDetectorRef);

  doctorName = 'Doctor';
  doctorEmail = 'doctor@healthaxis.com';
  doctorInitial = 'D';

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.loadDoctor();
    }
  }

  loadDoctor() {
    this.doctorProfileService.getCurrentDoctor().subscribe({
      next: (res: any) => {
        console.log('Doctor layout profile ✅:', res);

        const email = this.getEmailFromToken();

        this.zone.run(() => {
          this.doctorName = res.fullName || 'Doctor';
          this.doctorEmail = email || 'doctor@healthaxis.com';
          this.doctorInitial = this.doctorName.charAt(0).toUpperCase();

          localStorage.setItem('doctorId', String(res.doctorId));

          this.cdr.detectChanges();
        });
      },
      error: (err: any) => {
        console.error('Doctor layout profile failed ❌:', err);
      }
    });
  }

  getEmailFromToken(): string {
    const token = localStorage.getItem('token');

    if (!token) {
      return '';
    }

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));

      return (
        payload.email ||
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ||
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/emailaddress'] ||
        ''
      );
    } catch {
      return '';
    }
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}