import { Injectable } from '@angular/core';

export type ToastType = 'success' | 'error' | 'warning' | 'info';

export interface ToastMessage {
  id: number;
  type: ToastType;
  title: string;
  message?: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  toasts: ToastMessage[] = [];
  private counter = 0;

  show(type: ToastType, title: string, message?: string, duration: number = 3000) {
    const toast: ToastMessage = {
      id: ++this.counter,
      type,
      title,
      message,
      duration
    };

    this.toasts.push(toast);

    setTimeout(() => {
      this.remove(toast.id);
    }, duration);
  }

  success(title: string, message?: string) {
    this.show('success', title, message);
  }

  error(title: string, message?: string) {
    this.show('error', title, message, 4000);
  }

  warning(title: string, message?: string) {
    this.show('warning', title, message, 4000);
  }

  info(title: string, message?: string) {
    this.show('info', title, message);
  }

  remove(id: number) {
    this.toasts = this.toasts.filter(t => t.id !== id);
  }
}