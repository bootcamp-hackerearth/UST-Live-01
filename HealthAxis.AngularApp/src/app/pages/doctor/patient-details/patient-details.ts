import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { authState } from '../../../core/auth-state';
import { API_BASE_URL } from '../../../core/constants/api.constants';


@Component({
  selector: 'app-patient-details',
  standalone: false,
  templateUrl: './patient-details.html',
  styleUrl: './patient-details.css'
})
export class PatientDetails implements OnInit {

  patient = signal<any>(null);

  message = signal('');

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient
  ) { }

  ngOnInit(): void {

    const patientId =
      Number(this.route.snapshot.paramMap.get('id'));

    this.loadPatient(patientId);
  }

  loadPatient(patientId: number) {

    const headers = {
      Authorization: `Bearer ${authState().token}`
    };

    this.http.get<any>(
      `${API_BASE_URL}/patients/doctor/patient/${patientId}`,
      { headers }
    ).subscribe({
      next: (res) => {

        this.patient.set(res);

      },

      error: (err) => {

        console.error(err);

        this.message.set(
          'Unable to load patient details ❌'
        );

      }
    });
  }
}
