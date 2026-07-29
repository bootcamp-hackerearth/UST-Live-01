import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PatientSidebarComponent } from '../../shared/patient-sidebar/patient-sidebar.component';


@Component({
  selector: 'app-medical-history',
  standalone: true,
  imports: [CommonModule, PatientSidebarComponent],
  templateUrl: './medical-history.component.html',
  styleUrls: ['./medical-history.component.css']
})
export class MedicalHistoryComponent implements OnInit {

  records: any[] = [];
  loading: boolean = true;

  constructor(private readonly http: HttpClient, private readonly cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.loadMedicalHistory();
  }

  loadMedicalHistory() {

    const token = localStorage.getItem('token');

    if (!token) {
      console.error("Token missing");
      this.loading = false;
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<any[]>(
      '/api/records/my-records',
      { headers }
    ).subscribe({
      next: (res) => {
        console.log("Medical History Records:", res);


        this.records = [...res];

        this.loading = false;
        this.cd.detectChanges();
      },
      error: (err) => {
        console.error("Medical History Error:", err);
        this.records = [];
        this.loading = false;
      }
    });
  }

  formatDate(date: string): string {
    if (!date) return '';

    const d = new Date(date);
    return d.toLocaleDateString('en-IN');
  }
}
