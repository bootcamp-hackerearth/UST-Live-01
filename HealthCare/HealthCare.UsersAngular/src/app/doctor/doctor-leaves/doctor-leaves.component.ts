import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DoctorSidebarComponent } from '../../shared/doctor-sidebar/doctor-sidebar';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-doctor-leaves',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    DoctorSidebarComponent
  ],
  templateUrl: './doctor-leaves.component.html',
  styleUrls: ['./doctor-leaves.component.css']
})
export class DoctorLeavesComponent {

  constructor(private readonly http: HttpClient) { }

  leaveDates: string[] = [''];
  todayString = new Date().toISOString().split('T')[0];

  reason: string = '';

  showSuccessModal = false;
  showErrorModal = false;

  errorMessage = '';

  addDate() {
    this.leaveDates.push('');
  }

  removeDate(i: number) {
    this.leaveDates.splice(i, 1);
  }

  closeSuccessModal() {
    this.showSuccessModal = false;
  }

  closeErrorModal() {
    this.showErrorModal = false;
  }

  submit() {

    const token = localStorage.getItem('token');

    if (!token) {
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    const dates = this.leaveDates
      .map(d => d?.trim())
      .filter(d => d && d !== '');

    const uniqueDates = [...new Set(dates)];

    if (uniqueDates.length !== dates.length) {

      this.showSuccessModal = false;

      this.errorMessage =
        'Duplicate dates are not allowed';

      this.showErrorModal = true;

      return;
    }

    const body = dates.map(d => ({
      leaveDate: d,
      reason: this.reason?.trim()
        ? this.reason.trim()
        : null
    }));

    console.log('Sending:', body);

    this.http.post(
      '/api/doctors/leaves',
      body,
      { headers }
    )
      .subscribe({

        next: (res: any) => {

          const skipped = res?.skippedDates ?? [];

          this.showSuccessModal = false;
          this.showErrorModal = false;

          // Leave already exists in DB
          if (skipped.length > 0) {

            this.errorMessage =
              'Leave already exists for: ' +
              skipped.join(', ');

            this.showErrorModal = true;
            return;
          }

          // Success
          this.leaveDates = [''];
          this.reason = '';

          this.showSuccessModal = true;
        },

        error: (err: any) => {

          console.error(err);

          this.showSuccessModal = false;
          this.showErrorModal = false;

          // Session expired handled by interceptor
          if (err.status === 401) {
            return;
          }

          this.errorMessage =
            err.error?.message ||
            'Something went wrong. Please try again.';

          this.showErrorModal = true;
        }
      });
  }
}
