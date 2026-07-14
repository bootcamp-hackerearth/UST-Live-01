import {
  Component,
  EventEmitter,
  Input,
  Output
} from '@angular/core';

import {
  PatientNotificationDto,
  PatientNotificationTypes
} from '../../../../../shared/models/patient-notification.models';

@Component({
  selector: 'app-patient-notification-modal',
  standalone: true,
  templateUrl: './patient-notification-modal.html',
  styleUrl: './patient-notification-modal.css'
})
export class PatientNotificationModal {
  @Input() notifications: PatientNotificationDto[] = [];

  @Input() isProcessing = false;

  @Output() markAsRead = new EventEmitter<PatientNotificationDto>();

  @Output() rebookNow = new EventEmitter<PatientNotificationDto>();

  get currentNotification(): PatientNotificationDto | undefined {
    return this.notifications[0];
  }

  get isDoctorLeaveNotification(): boolean {
    return (
      this.currentNotification?.notificationType ===
      PatientNotificationTypes.DoctorLeave
    );
  }

  dismiss(): void {
    if (!this.currentNotification || this.isProcessing) {
      return;
    }

    this.markAsRead.emit(this.currentNotification);
  }

  rebook(): void {
    if (!this.currentNotification || this.isProcessing) {
      return;
    }

    this.rebookNow.emit(this.currentNotification);
  }

  formatCreatedDate(dateValue: string): string {
    if (!dateValue) {
      return 'Just now';
    }

    const parsedDate = new Date(dateValue);

    if (Number.isNaN(parsedDate.getTime())) {
      return dateValue;
    }

    return parsedDate.toLocaleString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
}