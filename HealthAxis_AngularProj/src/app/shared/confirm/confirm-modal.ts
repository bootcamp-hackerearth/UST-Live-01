import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmService } from './confirm.service';

@Component({
  selector: 'app-confirm-modal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './confirm-modal.html',
  styleUrls: ['./confirm-modal.css']
})
export class ConfirmModalComponent {

  constructor(public confirmService: ConfirmService) {}

  accept() {
    this.confirmService.accept();
  }

  reject() {
    this.confirmService.reject();
  }
}