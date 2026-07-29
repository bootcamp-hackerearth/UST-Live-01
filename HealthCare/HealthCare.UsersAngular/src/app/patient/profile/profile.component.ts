import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { PatientSidebarComponent } from '../../shared/patient-sidebar/patient-sidebar.component';
import { PatientEditProfileModalComponent } from '../../shared/patient-edit-profile-modal/patient-edit-profile-modal.component';
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, PatientSidebarComponent, PatientEditProfileModalComponent], 
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  profile: any = null;

  showEditModal = false; 
  editModel: any = {};

  showSuccessModal = false;
  successMessage = '';

  constructor(private readonly http: HttpClient, private readonly cdr: ChangeDetectorRef) { }

  ngOnInit() {
    this.getProfile();
  }

  getProfile() {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get('/api/patients/profile', { headers })
      .subscribe({
        next: (res: any) => {

          this.profile = {
            ...res,
            dateOfBirth: res.dateOfBirth ? new Date(res.dateOfBirth) : null
          };

          this.cdr.detectChanges();
        },

        error: (err) => {
          console.error(err);
        }
      });
  }

  openEdit() {

    this.editModel = {
      ...this.profile,

      dateOfBirth: this.profile?.dateOfBirth
        ? new Date(this.profile.dateOfBirth)
          .toISOString()
          .split('T')[0]
        : ''
    };

    this.showEditModal = true;
  }

  closeModal() {
    this.showEditModal = false;
  }

  saveChanges(updated: any) {

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.put(
      '/api/patients/profile',
      updated,
      { headers }
    ).subscribe({
      next: () => {

        this.successMessage = 'Profile updated successfully!';
        this.showSuccessModal = true;

        this.showEditModal = false;

        this.getProfile();
      },

      error: (err) => {
        console.error(err);
        alert('Failed to update profile');
      }
    });
    }

    closeSuccessModal() {
      this.showSuccessModal = false;

  }
}
