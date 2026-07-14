import {
  Component,
  EventEmitter,
  OnInit,
  Output
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { finalize, timeout } from 'rxjs';

import {
  CreateDoctorLeaveRequest,
  DoctorLeaveDto
} from '../../../../../shared/models/doctor-leave.models';

import { DoctorLeaveApiService } from '../../../../../core/services/doctor-leave-api.service';

type DoctorLeaveToastType = 'success' | 'info' | 'warning' | 'error';

interface DoctorLeaveToastEvent {
  message: string;
  type: DoctorLeaveToastType;
}

@Component({
  selector: 'app-doctor-leave',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './doctor-leave.html',
  styleUrl: './doctor-leave.css'
})
export class DoctorLeave implements OnInit {
  leaves: DoctorLeaveDto[] = [];

  isLoadingLeaves = false;
  isSubmitting = false;

  isConfirmModalOpen = false;

  hasSubmitted = false;

  message = '';

  todayDate = '';

  form: CreateDoctorLeaveRequest = {
    startDate: '',
    endDate: '',
    reason: ''
  };

  @Output() leaveCreated = new EventEmitter<void>();

  @Output() doctorToast = new EventEmitter<DoctorLeaveToastEvent>();

  constructor(
    private readonly doctorLeaveApiService: DoctorLeaveApiService
  ) {
    this.todayDate = this.formatDateForInput(new Date());
  }

  ngOnInit(): void {
    this.loadLeaves();
  }

  get latestLeave(): DoctorLeaveDto | undefined {
    return this.leaves[0];
  }

  get isStartDateInvalid(): boolean {
    if (!this.form.startDate) {
      return true;
    }

    return this.form.startDate < this.todayDate;
  }

  get isEndDateInvalid(): boolean {
    if (!this.form.endDate) {
      return true;
    }

    if (!this.form.startDate) {
      return false;
    }

    return this.form.endDate < this.form.startDate;
  }

  get isReasonInvalid(): boolean {
    const reason = this.form.reason.trim();

    return reason.length === 0 || reason.length > 500;
  }

  get isFormInvalid(): boolean {
    return (
      this.isStartDateInvalid ||
      this.isEndDateInvalid ||
      this.isReasonInvalid
    );
  }

  loadLeaves(): void {
    this.isLoadingLeaves = true;
    this.message = '';

    this.doctorLeaveApiService.getMyLeaves().pipe(
      timeout(15000),
      finalize(() => {
        this.isLoadingLeaves = false;
      })
    ).subscribe({
      next: (leaves: DoctorLeaveDto[]) => {
        this.leaves = leaves ?? [];
      },
      error: (error: unknown) => {
        console.log('Doctor leave history API error:', error);

        this.leaves = [];
        this.message = this.getErrorMessage(error);

        this.doctorToast.emit({
          message: this.message,
          type: 'error'
        });
      }
    });
  }

  submitLeave(): void {
    this.message = '';
    this.hasSubmitted = true;

    if (this.isFormInvalid) {
      this.message = 'Please correct the highlighted leave fields.';

      this.doctorToast.emit({
        message: this.message,
        type: 'warning'
      });

      return;
    }

    this.isConfirmModalOpen = true;
  }

  closeConfirmModal(): void {
    if (this.isSubmitting) {
      return;
    }

    this.isConfirmModalOpen = false;
  }

  confirmCreateLeave(): void {
    if (this.isSubmitting) {
      return;
    }

    this.isSubmitting = true;
    this.message = '';

    const request: CreateDoctorLeaveRequest = {
      startDate: this.form.startDate,
      endDate: this.form.endDate,
      reason: this.form.reason.trim()
    };

    this.doctorLeaveApiService.createMyLeave(request).pipe(
      timeout(15000),
      finalize(() => {
        this.isSubmitting = false;
      })
    ).subscribe({
      next: () => {
        this.isConfirmModalOpen = false;
        this.hasSubmitted = false;

        this.resetForm();
        this.loadLeaves();

        this.doctorToast.emit({
          message: 'Doctor leave created successfully.',
          type: 'success'
        });

        this.leaveCreated.emit();
      },
      error: (error: unknown) => {
        console.log('Doctor leave create API error:', error);

        this.isConfirmModalOpen = false;
        this.message = this.getErrorMessage(error);

        this.doctorToast.emit({
          message: this.message,
          type: 'error'
        });
      }
    });
  }

  formatDisplayDate(dateValue: string): string {
    if (!dateValue) {
      return 'Not Available';
    }

    const date = new Date(dateValue);

    if (Number.isNaN(date.getTime())) {
      return dateValue;
    }

    return date.toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  formatCreatedDate(dateValue: string): string {
    if (!dateValue) {
      return 'Not Available';
    }

    const date = new Date(dateValue);

    if (Number.isNaN(date.getTime())) {
      return dateValue;
    }

    return date.toLocaleString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  private resetForm(): void {
    this.form = {
      startDate: '',
      endDate: '',
      reason: ''
    };
  }

  private formatDateForInput(date: Date): string {
    const year = date.getFullYear();
    const month = `${date.getMonth() + 1}`.padStart(2, '0');
    const day = `${date.getDate()}`.padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  private getErrorMessage(error: unknown): string {
    if (typeof error === 'string' && error.trim()) {
      return error;
    }

    if (
      typeof error === 'object' &&
      error !== null &&
      'error' in error
    ) {
      const apiError = error as {
        error?: {
          message?: string;
          Message?: string;
          errors?: Record<string, string[]>;
          title?: string;
        } | string;
        name?: string;
        message?: string;
      };

      if (apiError.name === 'TimeoutError') {
        return 'The server is taking too long to respond. Please try again.';
      }

      if (typeof apiError.error === 'string' && apiError.error.trim()) {
        return apiError.error;
      }

      if (typeof apiError.error === 'object' && apiError.error !== null) {
        if (apiError.error.message) {
          return apiError.error.message;
        }

        if (apiError.error.Message) {
          return apiError.error.Message;
        }

        if (apiError.error.title) {
          return apiError.error.title;
        }

        if (apiError.error.errors) {
          const firstError = Object.values(apiError.error.errors)[0]?.[0];

          if (firstError) {
            return firstError;
          }
        }
      }

      if (apiError.message) {
        return apiError.message;
      }
    }

    return 'Something went wrong while processing doctor leave.';
  }
}