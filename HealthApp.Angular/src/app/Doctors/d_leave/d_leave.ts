import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/d_sidebar/d_sidebar';

import { DoctorLeaveService } from '../../Doctor.service/doctor-leave.service';
import { DoctorLeave } from '../../models/doctor-leave/doctor-leave.model';

@Component({
  selector: 'app-d-leave',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './d_leave.html',
  styleUrls: ['./d_leave.css']
})
export class DLeave implements OnInit {

  leaveForm = {
    startDate: '',
    endDate: '',
    reason: ''
  };

  leaveErrors = signal<any>({});
  doctorLeaves = signal<DoctorLeave[]>([]);

  showLeaveConfirm = signal(false);
  successMessage = signal('');
  errorMessage = signal('');

  today = new Date();
  tomorrowString = '';
  endMinDate = '';

  constructor(private doctorLeaveService: DoctorLeaveService) {}

  ngOnInit(): void {
    this.setDateLimits();
    this.loadMyLeaves();
  }

  setDateLimits() {
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);

    this.tomorrowString = tomorrow.toISOString().split('T')[0];
    this.endMinDate = this.tomorrowString;
  }

  onStartDateChange() {
    this.endMinDate = this.leaveForm.startDate || this.tomorrowString;

    if (
      this.leaveForm.endDate &&
      this.leaveForm.startDate &&
      this.leaveForm.endDate < this.leaveForm.startDate
    ) {
      this.leaveForm.endDate = '';
    }
  }

  loadMyLeaves() {
    this.doctorLeaveService.getMyLeaves().subscribe({
      next: (res) => {
        this.doctorLeaves.set(res || []);
      },
      error: (err) => {
        console.error(err);
        this.errorMessage.set('Failed to load leave history');
      }
    });
  }

  validateLeaveForm(): boolean {
    this.leaveErrors.set({});
    this.successMessage.set('');
    this.errorMessage.set('');

    if (!this.leaveForm.startDate) {
      this.leaveErrors.update(errors => ({ ...errors, startDate: 'Start date is required' }));
    }

    if (!this.leaveForm.endDate) {
      this.leaveErrors.update(errors => ({ ...errors, endDate: 'End date is required' }));
    }

    if (this.leaveForm.startDate && this.leaveForm.startDate < this.tomorrowString) {
      this.leaveErrors.update(errors => ({ ...errors, startDate: 'Start date must be from tomorrow onwards' }));
    }

    if (
      this.leaveForm.startDate &&
      this.leaveForm.endDate &&
      this.leaveForm.endDate < this.leaveForm.startDate
    ) {
      this.leaveErrors.update(errors => ({ ...errors, endDate: 'End date must be equal to or greater than start date' }));
    }

    if (!this.leaveForm.reason || !this.leaveForm.reason.trim()) {
      this.leaveErrors.update(errors => ({ ...errors, reason: 'Reason is required' }));
    }

    return Object.keys(this.leaveErrors()).length === 0;
  }

  openLeaveConfirm() {
    if (!this.validateLeaveForm()) return;
    this.showLeaveConfirm.set(true);
  }

  closeLeaveConfirm() {
    this.showLeaveConfirm.set(false);
  }

  submitLeave() {
    this.doctorLeaveService.createMyLeave(this.leaveForm).subscribe({
      next: () => {
        this.successMessage.set('Leave created successfully');
        this.errorMessage.set('');

        this.leaveForm = {
          startDate: '',
          endDate: '',
          reason: ''
        };

        this.setDateLimits();
        this.showLeaveConfirm.set(false);
        this.loadMyLeaves();
      },
      error: (err) => {
        this.errorMessage.set(this.getErrorMessage(err));
        this.successMessage.set('');
        this.showLeaveConfirm.set(false);
      }
    });
  }

  resetForm() {
    this.leaveForm = {
      startDate: '',
      endDate: '',
      reason: ''
    };

    this.leaveErrors.set({});
    this.successMessage.set('');
    this.errorMessage.set('');
    this.setDateLimits();
  }

  private getErrorMessage(err: any): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error?.message) {
      return err.error.message;
    }

    if (err.error?.errors) {
      const errors = err.error.errors;
      const firstKey = Object.keys(errors)[0];

      if (firstKey && errors[firstKey]?.length) {
        return errors[firstKey][0];
      }
    }

    return 'Leave creation failed';
  }
}
