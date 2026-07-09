import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, NgZone, OnInit, PLATFORM_ID, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected readonly title = signal('HealthAxis.UI');

  private readonly zone = inject(NgZone);

  alertMessage = '';
  alertType: 'success' | 'error' | 'info' = 'info';

  constructor(@Inject(PLATFORM_ID) private readonly platformId: object) {}

  ngOnInit(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    window.alert = (message?: any) => {
      this.zone.run(() => {
        const text = String(message ?? '');

        this.alertMessage = text;
        this.alertType = this.getAlertType(text);

        setTimeout(() => {
          this.alertMessage = '';
        }, 3000);
      });
    };
  }

  private getAlertType(message: string): 'success' | 'error' | 'info' {
    const lowerMessage = message.toLowerCase();

    if (
      lowerMessage.includes('success') ||
      lowerMessage.includes('successfully') ||
      lowerMessage.includes('booked') ||
      lowerMessage.includes('updated') ||
      lowerMessage.includes('added') ||
      lowerMessage.includes('registered') ||
      lowerMessage.includes('changed')
    ) {
      return 'success';
    }

    if (
      lowerMessage.includes('failed') ||
      lowerMessage.includes('invalid') ||
      lowerMessage.includes('required') ||
      lowerMessage.includes('not found') ||
      lowerMessage.includes('cannot') ||
      lowerMessage.includes('error') ||
      lowerMessage.includes('already')
    ) {
      return 'error';
    }

    return 'info';
  }
}
