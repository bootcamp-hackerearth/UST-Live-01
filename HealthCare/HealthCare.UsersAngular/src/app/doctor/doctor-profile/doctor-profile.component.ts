import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DoctorSidebarComponent } from '../../shared/doctor-sidebar/doctor-sidebar';
@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [CommonModule, DoctorSidebarComponent], 
  templateUrl: './doctor-profile.component.html',
  styleUrls: ['./doctor-profile.component.css']
})
export class DoctorProfileComponent implements OnInit {

  profile: any = null;

  showEditModal = false; 
  editModel: any = {}; 

  constructor(private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.getProfile();
  }

  getProfile() {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get(
      '/api/doctors/profile',
      { headers }
    ).subscribe({
      next: (res: any) => {
        this.profile = res;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  openEdit() {
    this.editModel = { ...this.profile };
    this.showEditModal = true;
  }

  closeModal() {
    this.showEditModal = false;
  }

  saveChanges(updated: any) {
    console.log("Updated Doctor:", updated);

    this.showEditModal = false;

    this.getProfile();
  }
}
