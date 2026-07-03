import { Injectable, signal } from '@angular/core';

export type ToastType = 'success' | 'error' | 'warning' | 'info';

export interface ToastNotification {
  id: number;
  type: ToastType;
  message: string;
  title: string;
  duration: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private readonly _toasts = signal<ToastNotification[]>([]);

  readonly toasts = this._toasts.asReadonly();

  private counter = 0;

  success(message: string, title = 'Success', duration = 3500): void {
    this.show('success', message, title, duration);
  }

  error(message: string, title = 'Error', duration = 4000): void {
    this.show('error', message, title, duration);
  }

  warning(message: string, title = 'Warning', duration = 3500): void {
    this.show('warning', message, title, duration);
  }

  info(message: string, title = 'Info', duration = 3500): void {
    this.show('info', message, title, duration);
  }

  remove(id: number): void {
    this._toasts.update(toasts => toasts.filter(toast => toast.id !== id));
  }

  clear(): void {
    this._toasts.set([]);
  }

  private show(
    type: ToastType,
    message: string,
    title: string,
    duration: number
  ): void {
    const toast: ToastNotification = {
      id: ++this.counter,
      type,
      message,
      title,
      duration
    };

    this._toasts.update(toasts => [...toasts, toast]);

    globalThis.setTimeout(() => {
      this.remove(toast.id);
    }, duration);
  }
}
