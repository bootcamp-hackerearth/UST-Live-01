import { Component, inject } from '@angular/core';
import {NotificationService,ToastType}from '../../../core/services/notification.service';

@Component({
  selector: 'app-toast-container',
  standalone: true,
  templateUrl: './toast-container.html',
  styleUrl: './toast-container.css'
})
export class ToastContainer {
  private readonly notificationService = inject(NotificationService);

  readonly toasts = this.notificationService.toasts;

  closeToast(id: number): void {
    this.notificationService.remove(id);
  }

  getIcon(type: ToastType): string {
    switch (type) {
      case 'success':
        return 'bi bi-check-circle-fill';
      case 'error':
        return 'bi bi-x-circle-fill';
      case 'warning':
        return 'bi bi-exclamation-triangle-fill';
      case 'info':
        return 'bi bi-info-circle-fill';
      default:
        return 'bi bi-info-circle-fill';
    }
  }
}

  
