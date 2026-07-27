import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { authState } from '../../../core/auth-state';
import { API_BASE_URL } from '../../../core/constants/api.constants';


@Component({
  selector: 'app-create-health-record',
  standalone: false,
  templateUrl: './create-health-record.html',
  styleUrl: './create-health-record.css'
})
export class CreateHealthRecord {

  diagnosis = signal('');
  prescription = signal('');
  notes = signal('');

  healthRecordId = signal<number | null>(null);

  message = signal('');

  constructor(
    private http: HttpClient,
    private router: Router
  ) {

    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras.state as any;

    const appointmentId = state?.appointmentId;

    if (appointmentId) {
      this.loadHealthRecord(appointmentId);
    }
    else {
      this.message.set('Appointment information not found ❌');
    }
  }

  loadHealthRecord(appointmentId: number) {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      `${API_BASE_URL}/healthrecords/appointment/${appointmentId}`,
      { headers }
    ).subscribe({
      next: (res) => {

        this.healthRecordId.set(
          res.healthRecordId
        );

      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load health record ❌'
        );

      }
    });
  }

  save() {

    if (!this.diagnosis().trim()) {
      this.message.set('Diagnosis is required ❌');
      return;
    }

    if (!this.prescription().trim()) {
      this.message.set('Prescription is required ❌');
      return;
    }

    if (!this.healthRecordId()) {
      this.message.set('Health record not loaded ❌');
      return;
    }

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    const payload = {
      diagnosis: this.diagnosis(),
      prescription: this.prescription(),
      notes: this.notes()
    };

    this.http.put(
      `${API_BASE_URL}/healthrecords/${this.healthRecordId()}`,
      payload,
      { headers }
    ).subscribe({
      next: () => {

        this.message.set(
          'Health Record saved successfully ✅'
        );

        setTimeout(() => {

          this.router.navigateByUrl(
            '/doctor/appointments'
          );

        }, 1500);

      },

      error: (err) => {

        const msg =
          err?.error?.message ||
          'Unable to save health record ❌';

        this.message.set(msg);

      }
    });
  }
}
