import { Injectable } from '@angular/core';

export interface ConfirmOptions {
  title?: string;
  message?: string;
  confirmText?: string;
  cancelText?: string;
  type?: 'default' | 'danger' | 'success';
}

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {

  visible = false;

  title = 'Are you sure?';
  message = 'Please confirm this action.';
  confirmText = 'Confirm';
  cancelText = 'Cancel';
  type: 'default' | 'danger' | 'success' = 'default';

  private resolver: ((value: boolean) => void) | null = null;

  confirm(options: ConfirmOptions): Promise<boolean> {
    this.title = options.title || 'Are you sure?';
    this.message = options.message || 'Please confirm this action.';
    this.confirmText = options.confirmText || 'Confirm';
    this.cancelText = options.cancelText || 'Cancel';
    this.type = options.type || 'default';

    this.visible = true;

    return new Promise<boolean>((resolve) => {
      this.resolver = resolve;
    });
  }

  accept() {
    this.visible = false;

    if (this.resolver) {
      this.resolver(true);
      this.resolver = null;
    }
  }

  reject() {
    this.visible = false;

    if (this.resolver) {
      this.resolver(false);
      this.resolver = null;
    }
  }
}