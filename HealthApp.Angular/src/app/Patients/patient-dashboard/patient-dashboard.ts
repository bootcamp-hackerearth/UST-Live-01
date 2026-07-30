import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Sidebar } from '../shared/sidebar/sidebar';

import { AuthService } from '../../service/auth.service';
import { PatientService } from '../../Patient.service/patientservice';
import { AppointmentService } from '../../Patient.service/appointmentservice';
import { HealthRecordService } from '../../Patient.service/health-recordservice';

import { Patient } from '../../models/patient/patient.model';
import { Appointment } from '../../models/appointment/appointment.model';
import { HealthRecord } from '../../models/health-record/health-record.model';

import { NotificationService } from '../../Patient.service/notificationservice';
import { Notification } from '../../models/notification/notification.model';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboard implements OnInit {

  unreadNotifications: Notification[] = [];
  currentNotification: Notification | null = null;
  showNotificationPopup = false;
  isReadingNotification = false;

  patient = signal<Patient | null>(null);
  editPatient: Patient | null = null;

  appointments = signal<Appointment[]>([]);
  records = signal<HealthRecord[]>([]);

  upcomingCount = signal(0);
  pendingCount = signal(0);

  showProfile = false;
  editMode = false;
  isSavingProfile = false;

  filteredAppointments = signal<Appointment[]>([]);
  paginatedAppointments = signal<Appointment[]>([]);

  pageNumber = signal(1);
  readonly pageSize = 5;
  totalPages = signal(1);

  passwordForm = {
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  };

  showAppPopup = signal(false);
  popupTitle = signal('');
  popupMessage = signal('');
  popupType = signal<'success' | 'error' | 'warning'>('success');

  showCancelModal = false;
  cancelReason = '';
  selectedAppointmentId: number | null = null;

  constructor(
    private patientService: PatientService,
    private appointmentService: AppointmentService,
    private recordService: HealthRecordService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) { }


  ngOnInit(): void {
    this.loadProfile();
    this.loadUnreadNotifications();
  }

  openProfile() {
    this.showProfile = true;
    this.editMode = false;
    this.editPatient = null;

    this.loadUnreadNotifications();
  }
  loadUnreadNotifications() {
    this.notificationService.getMyUnreadNotifications().subscribe({
      next: (res) => {
        console.log('UNREAD NOTIFICATIONS:', res);

        this.unreadNotifications = res || [];

        if (this.unreadNotifications.length > 0) {
          this.currentNotification = this.unreadNotifications[0];
          this.showNotificationPopup = true;
        }
      },
      error: (err) => {
        console.error('Failed to load unread notifications', err);
      }
    });
  }

  markCurrentNotificationAsRead() {
    if (!this.currentNotification || this.isReadingNotification) return;

    const notificationId = this.currentNotification.notificationId;

    this.isReadingNotification = true;

    this.notificationService.markAsRead(notificationId).subscribe({
      next: () => {
        this.isReadingNotification = false;

        this.unreadNotifications = this.unreadNotifications.filter(
          n => n.notificationId !== notificationId
        );

        if (this.unreadNotifications.length > 0) {
          this.currentNotification = this.unreadNotifications[0];
          this.showNotificationPopup = true;
        } else {
          this.currentNotification = null;
          this.showNotificationPopup = false;
        }
      },
      error: (err) => {
        this.isReadingNotification = false;
        console.error('Failed to mark notification as read', err);
      }
    });
  }

  closeNotificationPopupWithoutRead() {
    this.showNotificationPopup = false;
  }
  closeProfile() {
    this.showProfile = false;
    this.editMode = false;
    this.editPatient = null;
    this.isSavingProfile = false;
    this.resetPasswordForm();
  }

  loadProfile() {
    this.patientService.getMyProfile().subscribe({
      next: (res) => {
        this.patient.set(res);
        this.loadAppointments();
        this.loadRecords();
      },
      error: (err) => {
        console.error('Profile load failed', err);
        this.openAppPopup('Profile Error', 'Unable to load your profile details. Please try again.', 'error');
      }
    });
  }

  loadAppointments() {
    this.appointmentService.getMyAppointments().subscribe({
      next: (res) => {
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        const data = (res || [])

          .map((a: any) => ({
            ...a,
            scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
          }))
          .filter((a: any) => {
            const isUpcoming = a.scheduledDate && a.scheduledDate >= today;
            const isValidStatus = a.status === 'Pending' || a.status === 'Confirmed';
            return isUpcoming && isValidStatus;
          })
          .sort((a, b) =>
            new Date(a.scheduledDate).getTime() -
            new Date(b.scheduledDate).getTime()
          );


        this.appointments.set(data);
        this.filteredAppointments.set(data);
        this.upcomingCount.set(data.length);
        this.pendingCount.set(data.filter(a => a.status === 'Pending').length);
        this.pageNumber.set(1);
        this.updatePagination();
      },
      error: (err) => {
        console.error('Appointments load failed', err);
      }
    });
  }

  loadRecords() {
    this.recordService.getMyRecords().subscribe({
      next: (res) => {
        this.records.set(res || []);
      },
      error: (err) => {
        console.error('Records load failed', err);
      }
    });
  }

  cancel(id: number): void {
    this.selectedAppointmentId = id;
    this.cancelReason = '';
    this.showCancelModal = true;
  }
  toggleEdit() {
    if (!this.patient()) return;
    this.editMode = true;
    this.editPatient = {
      ...this.patient()!,
      dateOfBirth: this.patient()!.dateOfBirth
        ? this.formatDateForInput(this.patient()!.dateOfBirth) as any
        : null as any
    };
  }

  cancelEdit() {
    if (this.isSavingProfile) return;
    this.editMode = false;
    this.editPatient = null;
  }
  closeCancelModal(): void {
    this.showCancelModal = false;
    this.cancelReason = '';
    this.selectedAppointmentId = null;
  }
  confirmCancel(): void {

    if (!this.selectedAppointmentId) {
      return;
    }

    if (!this.cancelReason.trim()) {
      this.openAppPopup(
        'Missing Reason',
        'Please enter a cancellation reason.',
        'warning'
      );
      return;
    }

    this.appointmentService
      .cancelAppointment(
        this.selectedAppointmentId,
        this.cancelReason
      )
      .subscribe({
        next: () => {

          this.closeCancelModal();
          this.loadAppointments();

          this.openAppPopup(
            'Appointment Cancelled',
            'Appointment cancelled successfully.',
            'success'
          );
        },
        error: (err) => {
          console.error(err);

          this.openAppPopup(
            'Cancel Failed',
            err.error?.message ||
            'Unable to cancel appointment.',
            'error'
          );
        }
      });
  }
  update() {
    if (!this.editPatient || !this.patient() || this.isSavingProfile) return;

    this.isSavingProfile = true;

    const payload = {
      fullName: this.editPatient.fullName,
      phoneNumber: this.editPatient.phoneNumber,
      dateOfBirth: this.normalizeDate(this.editPatient.dateOfBirth),
      insuranceId: this.editPatient.insuranceId || null
    };

    console.log('UPDATE PROFILE PAYLOAD:', payload);

    this.patientService.updateMyProfile(payload).subscribe({
      next: (res: Patient) => {
        this.patient.set({
          ...this.patient()!,
          ...payload,
          ...res
        } as Patient);

        this.isSavingProfile = false;
        this.showProfile = false;
        this.editMode = false;
        this.editPatient = null;

        this.openAppPopup('Profile Updated', 'Your profile details have been updated successfully.', 'success');
      },
      error: (err) => {
        this.isSavingProfile = false;
        console.error('Update failed', err);
        console.log('Update failed details:', err.error);
        this.openAppPopup('Update Failed', this.getErrorMessage(err, 'Profile update failed. Please try again.'), 'error');
      }
    });
  }

  changePassword() {
    if (!this.passwordForm.currentPassword || !this.passwordForm.newPassword || !this.passwordForm.confirmPassword) {
      this.openAppPopup('Missing Details', 'Please fill all password fields.', 'warning');
      return;
    }

    if (this.passwordForm.newPassword !== this.passwordForm.confirmPassword) {
      this.openAppPopup('Password Mismatch', 'New password and confirm password do not match.', 'warning');
      return;
    }

    this.authService.changePassword(this.passwordForm).subscribe({
      next: (res: any) => {
        this.resetPasswordForm();
        this.openAppPopup('Password Updated', res.message || 'Password updated successfully.', 'success');
      },
      error: (err) => {
        this.openAppPopup('Password Update Failed', this.getErrorMessage(err, 'Password update failed.'), 'error');
      }
    });
  }

  private normalizeDate(dateValue: any): string | null {
    if (!dateValue) return null;
    const date = new Date(dateValue);
    if (Number.isNaN(date.getTime())) return null;
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private formatDateForInput(dateValue: any): string {
    if (!dateValue) return '';
    const date = new Date(dateValue);
    if (Number.isNaN(date.getTime())) return '';
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  updatePagination() {
    const totalPages = Math.ceil(this.filteredAppointments().length / this.pageSize);
    this.totalPages.set(totalPages <= 0 ? 1 : totalPages);
    if (this.pageNumber() > this.totalPages()) this.pageNumber.set(this.totalPages());
    this.paginate();
  }

  paginate() {
    const start = (this.pageNumber() - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.paginatedAppointments.set(this.filteredAppointments().slice(start, end));
  }

  nextPage() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(value => value + 1);
      this.paginate();
    }
  }

  prevPage() {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(value => value - 1);
      this.paginate();
    }
  }

  private resetPasswordForm() {
    this.passwordForm = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: ''
    };
  }

  private openAppPopup(title: string, message: string, type: 'success' | 'error' | 'warning' = 'success') {
    this.popupTitle.set(title);
    this.popupMessage.set(message);
    this.popupType.set(type);
    this.showAppPopup.set(true);
  }

  closeAppPopup() {
    this.showAppPopup.set(false);
    this.popupTitle.set('');
    this.popupMessage.set('');
    this.popupType.set('success');
  }

  private getErrorMessage(err: any, fallback: string): string {
    const errors = err?.error?.errors;
    if (errors) {
      const firstKey = Object.keys(errors)[0];
      if (firstKey && errors[firstKey]?.length) return errors[firstKey][0];
    }
    if (err?.error?.message) return err.error.message;
    if (typeof err?.error === 'string') return err.error;
    return fallback;
  }
}
