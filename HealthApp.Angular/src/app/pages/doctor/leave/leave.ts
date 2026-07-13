import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';

import { DoctorLeaveService } from '../../../core/services/doctor-leave.service';
import { NotificationService } from '../../../core/services/notification.service';
import {
  DoctorLeaveCreateDto,
  DoctorLeaveDto,
  DoctorLeavePreviewDto
} from '../../../dtos/doctor-leave.dto';
import { LoadingSpinnerComponent } from '../../../shared/components/loading-spinner/loading-spinner';

@Component({
  selector: 'app-doctor-leave',
  standalone: true,
  imports: [CommonModule, FormsModule, LoadingSpinnerComponent],
  templateUrl: './leave.html',
  styleUrl: './leave.css'
})
export class DoctorLeave implements OnInit {
  isLoading = false;
  isLoadingPreview = false;
  isSubmitting = false;
  showConfirmModal = false;

  leaves: DoctorLeaveDto[] = [];
  leavePreview: DoctorLeavePreviewDto | null = null;
  leaveForm: DoctorLeaveCreateDto = this.getEmptyForm();

  readonly minDate = this.getTodayDate();
  readonly maxReasonLength = 500;

  constructor(
    private readonly doctorLeaveService: DoctorLeaveService,
    private readonly notificationService: NotificationService,
    private readonly cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadLeaves();
  }

  get latestLeave(): DoctorLeaveDto | null {
    return this.sortedLeaves[0] ?? null;
  }

  get sortedLeaves(): DoctorLeaveDto[] {
    return [...this.leaves].sort((a, b) =>
      b.startDate.localeCompare(a.startDate)
    );
  }

  get reasonLength(): number {
    return this.leaveForm.reason?.length ?? 0;
  }

  get effectiveEndDate(): string {
    return this.leaveForm.endDate || this.leaveForm.startDate;
  }

  loadLeaves(): void {
    this.isLoading = true;
    this.cdr.markForCheck();

    this.doctorLeaveService
      .getMyLeaves()
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: leaves => {
          this.leaves = leaves ?? [];
          this.cdr.markForCheck();
        },
        error: () => {
          this.leaves = [];
          this.cdr.markForCheck();
        }
      });
  }

  openConfirmation(): void {
    if (!this.validateForm()) {
      return;
    }

    this.isLoadingPreview = true;
    this.leavePreview = null;
    this.cdr.markForCheck();

    this.doctorLeaveService
      .previewMyLeave(this.buildPayload())
      .pipe(
        finalize(() => {
          this.isLoadingPreview = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: preview => {
          this.leavePreview = preview;
          this.showConfirmModal = true;
          this.cdr.markForCheck();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  closeConfirmation(): void {
    if (this.isSubmitting) {
      return;
    }

    this.showConfirmModal = false;
    this.leavePreview = null;
    this.cdr.markForCheck();
  }

  confirmLeave(): void {
    if (!this.validateForm()) {
      this.closeConfirmation();
      return;
    }

    this.isSubmitting = true;
    this.cdr.markForCheck();

    this.doctorLeaveService
      .createMyLeave(this.buildPayload())
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
          this.cdr.markForCheck();
        })
      )
      .subscribe({
        next: response => {
          this.notificationService.success(
            response.message || 'Doctor leave created successfully.'
          );

          this.showConfirmModal = false;
          this.leavePreview = null;
          this.leaveForm = this.getEmptyForm();
          this.loadLeaves();
        },
        error: () => {
          // Error toast is handled globally by errorInterceptor.
        }
      });
  }

  resetForm(): void {
    if (this.isSubmitting || this.isLoadingPreview) {
      return;
    }

    this.leaveForm = this.getEmptyForm();
    this.showConfirmModal = false;
    this.leavePreview = null;
    this.cdr.markForCheck();
  }

  private buildPayload(): DoctorLeaveCreateDto {
    return {
      startDate: this.leaveForm.startDate,
      endDate: this.effectiveEndDate,
      reason: this.leaveForm.reason.trim()
    };
  }

  private validateForm(): boolean {
    const today = this.getTodayDate();
    const reason = this.leaveForm.reason.trim();

    if (!this.leaveForm.startDate) {
      this.notificationService.warning('Leave start date is required.');
      return false;
    }

    if (this.leaveForm.startDate < today) {
      this.notificationService.warning('Leave start date cannot be in the past.');
      return false;
    }

    if (this.effectiveEndDate < this.leaveForm.startDate) {
      this.notificationService.warning(
        'Leave end date cannot be before the start date.'
      );
      return false;
    }

    if (!reason) {
      this.notificationService.warning('Leave reason is required.');
      return false;
    }

    if (reason.length < 3) {
      this.notificationService.warning(
        'Leave reason must be at least 3 characters long.'
      );
      return false;
    }

    if (reason.length > this.maxReasonLength) {
      this.notificationService.warning(
        `Leave reason cannot exceed ${this.maxReasonLength} characters.`
      );
      return false;
    }

    return true;
  }

  private getEmptyForm(): DoctorLeaveCreateDto {
    return {
      startDate: '',
      endDate: '',
      reason: ''
    };
  }

  private getTodayDate(): string {
    const today = new Date();
    const year = today.getFullYear();
    const month = String(today.getMonth() + 1).padStart(2, '0');
    const day = String(today.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
