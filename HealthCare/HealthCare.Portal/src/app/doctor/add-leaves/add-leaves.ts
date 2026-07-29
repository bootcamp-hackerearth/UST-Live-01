import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DoctorService } from '../../core/services/doctor.service';
import { CreateLeaveRequest } from '../../core/models/portal.models';

@Component({
  selector: 'app-add-leaves',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-leaves.html',
  styleUrl: './add-leaves.css'
})
export class AddLeaves {
  minDate = this.getTomorrowDate();

  leaveDate = signal('');
  reason = signal('');

  selectedLeaves = signal<CreateLeaveRequest[]>([]);

  isSubmitting = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  hasSelectedLeaves = computed(
    () => this.selectedLeaves().length > 0
  );

  constructor(private readonly doctorService: DoctorService) { }

  getTomorrowDate(): string {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    const year = tomorrow.getFullYear();
    const month = String(
      tomorrow.getMonth() + 1
    ).padStart(2, '0');

    const day = String(
      tomorrow.getDate()
    ).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  addLeave(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    if (!this.leaveDate()) {
      this.errorMessage.set('Please select leave date');
      return;
    }

    if (this.leaveDate() < this.minDate) {
      this.errorMessage.set(
        'Leave can only be added from tomorrow onwards.'
      );
      return;
    }

    const alreadyAdded = this.selectedLeaves().some(
      leave => leave.leaveDate === this.leaveDate()
    );

    if (alreadyAdded) {
      this.errorMessage.set(
        'This leave date is already added'
      );
      return;
    }

    const newLeave: CreateLeaveRequest = {
      leaveDate: this.leaveDate(),
      reason: this.reason()
    };

    this.selectedLeaves.update(leaves => [
      ...leaves,
      newLeave
    ]);

    this.leaveDate.set('');
  }

  removeLeave(index: number): void {
    this.selectedLeaves.update(leaves =>
      leaves.filter((_, i) => i !== index)
    );

    this.successMessage.set('');
    this.errorMessage.set('');
  }

  submitLeaves(): void {
    this.successMessage.set('');
    this.errorMessage.set('');

    if (this.selectedLeaves().length === 0) {
      this.errorMessage.set(
        'Please add at least one leave date'
      );
      return;
    }

    const containsInvalidDate =
      this.selectedLeaves().some(
        leave => leave.leaveDate < this.minDate
      );

    if (containsInvalidDate) {
      this.errorMessage.set(
        'Leave can only be added from tomorrow onwards.'
      );
      return;
    }

    const leavesToSubmit: CreateLeaveRequest[] =
      this.selectedLeaves().map(leave => ({
        leaveDate: leave.leaveDate,
        reason: this.reason() || leave.reason
      }));

    this.isSubmitting.set(true);

    this.doctorService.addLeaves(leavesToSubmit).subscribe({
      next: () => {
        this.successMessage.set(
          'Leaves added successfully'
        );

        this.selectedLeaves.set([]);
        this.leaveDate.set('');
        this.reason.set('');
        this.isSubmitting.set(false);
      },

      error: (error) => {
        this.errorMessage.set(
          error?.error?.message ||
          'Failed to add leaves'
        );

        this.isSubmitting.set(false);
      }
    });
  }
}
