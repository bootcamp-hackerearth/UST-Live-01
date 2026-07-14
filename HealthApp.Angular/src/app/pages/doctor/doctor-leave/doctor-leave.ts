import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import {
  CreateDoctorLeaveRequest,
  DoctorLeave,
  DoctorLeaveImpact
} from '../../../core/models/doctor-leave.model';
import { AuthService } from '../../../core/services/auth.service';
import { DoctorLeaveApiService } from '../../../core/services/doctor-leave-api.service';
import { DoctorPortalStateService } from '../../../core/services/doctor-portal-state.service';

@Component({
  selector: 'app-doctor-leave',
  imports: [FormsModule],
  templateUrl: './doctor-leave.html',
  styleUrl: './doctor-leave.css',
})
export class DoctorLeavePage implements OnInit {
  readonly doctorState = inject(DoctorPortalStateService);

  private readonly authService = inject(AuthService);
  private readonly doctorLeaveApi = inject(DoctorLeaveApiService);
  private readonly router = inject(Router);

  readonly startDate = signal('');
  readonly endDate = signal('');
  readonly reason = signal('');

  readonly leaveImpact = signal<DoctorLeaveImpact | null>(null);

  readonly isPreviewing = signal(false);
  readonly isSubmitting = signal(false);

  readonly message = signal('');
  readonly isError = signal(false);

  readonly today = this.toDateOnly(new Date());

  readonly leaveHistory = computed(() =>
    [...this.doctorState.doctorLeaves()].sort((a, b) =>
      this.toDateOnly(b.startDate).localeCompare(
        this.toDateOnly(a.startDate)
      )
    )
  );

  readonly latestLeave = computed<DoctorLeave | null>(() =>
    this.leaveHistory()[0] ?? null
  );

  ngOnInit(): void {
    if (this.authService.currentRole() !== 'Doctor') {
      this.router.navigate(['/login']);
      return;
    }

    this.doctorState.loadDoctorLeaves();
  }

  updateStartDate(value: string): void {
    this.startDate.set(value);
    this.clearMessage();

    if (this.endDate() && value && this.endDate() < value) {
      this.endDate.set('');
    }
  }

  updateEndDate(value: string): void {
    this.endDate.set(value);
    this.clearMessage();
  }

  updateReason(value: string): void {
    this.reason.set(value);
    this.clearMessage();
  }

  submitLeave(): void {
    this.clearMessage();

    const validationMessage = this.validateForm();

    if (validationMessage) {
      this.showError(validationMessage);
      return;
    }

    this.isPreviewing.set(true);

    this.doctorLeaveApi
      .previewMyLeaveImpact(this.buildRequest(false))
      .subscribe({
        next: impact => {
          this.isPreviewing.set(false);

          if (impact.requiresConfirmation) {
            this.leaveImpact.set(impact);
            return;
          }

          this.createLeave(false);
        },
        error: error => {
          this.isPreviewing.set(false);
          this.showError(this.authService.getErrorMessage(error));
        }
      });
  }

  closeConfirmationModal(): void {
    if (this.isSubmitting()) {
      return;
    }

    this.leaveImpact.set(null);
  }

  confirmLeave(): void {
    if (!this.leaveImpact() || this.isSubmitting()) {
      return;
    }

    this.createLeave(true);
  }

  getLeaveStatus(leave: DoctorLeave): string {
    const today = this.today;
    const start = this.toDateOnly(leave.startDate);
    const end = this.toDateOnly(leave.endDate);

    if (start <= today && end >= today) {
      return 'Current Leave';
    }

    if (start > today) {
      return 'Upcoming';
    }

    return 'Completed';
  }

  getLeaveStatusClass(leave: DoctorLeave): string {
    const status = this.getLeaveStatus(leave);

    if (status === 'Current Leave') {
      return 'current';
    }

    if (status === 'Upcoming') {
      return 'upcoming';
    }

    return 'completed';
  }

  formatDate(value: string): string {
    return new Date(value).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private createLeave(confirmCancellation: boolean): void {
    this.isSubmitting.set(true);

    this.doctorLeaveApi
      .createMyLeave(this.buildRequest(confirmCancellation))
      .subscribe({
        next: result => {
          this.isSubmitting.set(false);
          this.leaveImpact.set(null);
          this.showSuccess(result.message);
          this.resetForm();
          this.doctorState.refreshAfterLeaveCreation();
        },
        error: error => {
          this.isSubmitting.set(false);
          this.showError(this.authService.getErrorMessage(error));
        }
      });
  }

  private buildRequest(
    confirmAppointmentCancellation: boolean
  ): CreateDoctorLeaveRequest {
    return {
      startDate: this.startDate(),
      endDate: this.endDate(),
      reason: this.reason().trim(),
      confirmAppointmentCancellation
    };
  }

  private validateForm(): string {
    const startDate = this.startDate();
    const endDate = this.endDate();
    const reason = this.reason().trim();

    if (!startDate) {
      return 'Leave start date is required.';
    }

    if (!endDate) {
      return 'Leave end date is required.';
    }

    if (startDate < this.today) {
      return 'Leave start date cannot be in the past.';
    }

    if (endDate < startDate) {
      return 'Leave end date cannot be before the start date.';
    }

    if (!reason) {
      return 'Leave reason is required.';
    }

    if (reason.length < 3) {
      return 'Leave reason must contain at least 3 characters.';
    }

    if (reason.length > 500) {
      return 'Leave reason must not exceed 500 characters.';
    }

    return '';
  }

  private resetForm(): void {
    this.startDate.set('');
    this.endDate.set('');
    this.reason.set('');
  }

  private showSuccess(message: string): void {
    this.message.set(message);
    this.isError.set(false);
  }

  private showError(message: string): void {
    this.message.set(message);
    this.isError.set(true);
  }

  private clearMessage(): void {
    this.message.set('');
    this.isError.set(false);
  }

  private toDateOnly(value: string | Date): string {
    const date = new Date(value);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
