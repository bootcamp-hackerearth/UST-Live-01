import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Sidebar } from '../shared/d_sidebar/d_sidebar';
import { AuthService } from '../../service/auth.service';
import { AppointmentService } from '../../Doctor.service/appointmentservice';
import { DoctorService } from '../../Doctor.service/doctorservice';

@Component({
  selector: 'app-doctor-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './d_doctor-dashboard.html',
  styleUrls: ['./d_doctor-dashboard.css']
})
export class DoctorDashboard implements OnInit {

  appointments: any[] = [];
  filteredAppointments: any[] = [];
  latestAppointments: any[] = [];

  upcomingCount = 0;
  pendingCount = 0;
  confirmedCount = 0;
  completedCount = 0;

  doctor: any = null;

  showProfile = false;
  showPasswordModal = false;

  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  constructor(
    private readonly appointmentService: AppointmentService,
    private readonly doctorService: DoctorService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadAppointments();
    this.loadProfile();
  }

  loadAppointments() {
    this.appointmentService.getMyDoctorAppointments().subscribe({
      next: (res: any) => {

        const list = res?.data || res || [];

        const data = list.map((a: any) => ({
          ...a,
          scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
        }));

        this.appointments = data;
        this.filteredAppointments = data;

        this.upcomingCount = data.length;
        this.pendingCount = data.filter((a: any) => a.status === 'Pending').length;
        this.confirmedCount = data.filter((a: any) => a.status === 'Confirmed').length;
        this.completedCount = data.filter((a: any) => a.status === 'Completed').length;

        this.setLatestAppointments();
      },
      error: (err) => console.error(err)
    });
  }

  setLatestAppointments() {
    this.latestAppointments = [...this.filteredAppointments]
      .sort((a: any, b: any) => {
        const dateA = a.scheduledDate ? new Date(a.scheduledDate).getTime() : 0;
        const dateB = b.scheduledDate ? new Date(b.scheduledDate).getTime() : 0;

        return dateB - dateA;
      })
      .slice(0, 4);
  }

  loadProfile() {
    this.doctorService.getMyProfile().subscribe({
      next: (res) => this.doctor = res,
      error: (err) => console.error(err)
    });
  }

  openProfile() {
    this.showProfile = true;
  }

  closeProfile() {
    this.showProfile = false;
  }

  openPasswordModal() {
    this.showPasswordModal = true;
  }

  closePasswordModal() {
    this.showPasswordModal = false;
    this.clearPassword();
  }

  confirm(id: number) {
    this.appointmentService.confirmAppointment(id).subscribe(() => {
      this.loadAppointments();
    });
  }

  complete(id: number) {
    this.appointmentService.completeAppointment(id).subscribe(() => {
      this.loadAppointments();
    });
  }

  changePassword() {

    if (!this.passwordForm.currentPassword ||
        !this.passwordForm.newPassword ||
        !this.passwordForm.confirmPassword) {
      alert('All fields are required');
      return;
    }

    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      alert('Passwords do not match');
      return;
    }

    this.authService.changePassword(this.passwordForm).subscribe({
      next: (res: any) => {
        alert(res.message || 'Password updated successfully');
        this.closePasswordModal();
      },
      error: (err) => {
        alert(err.error?.message || 'Password change failed');
      }
    });
  }

  clearPassword() {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }
}