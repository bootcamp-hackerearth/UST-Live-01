import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app-popup.html',
  styleUrls: ['./app-popup.css']
})
export class AppPopupComponent {
  @Input() visible = false;
  @Input() title = '';
  @Input() message = '';
  @Input() type: 'success' | 'error' | 'warning' = 'success';

  @Output() closed = new EventEmitter<void>();
  @Output() confirmed = new EventEmitter<void>();

  close(): void {
    this.closed.emit();
  }

  confirm(): void {
    this.confirmed.emit();
  }
}