import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

import { DoctorService }
from '../../../core/services/doctor.service';

import { Leave }
from '../../../core/models/leave.model';

@Component({
  selector: 'app-leave',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './leave.html',
  styleUrl: './leave.css'
})
export class LeaveComponent {

  selectedDate = '';
  reason = '';
  today = new Date().toISOString().split('T')[0];

  leaves = signal<Leave[]>([]);
  loading = signal(false);

  constructor(
    private doctorService: DoctorService,
    private toastr: ToastrService
  ) {}

  addDate() {

    if (!this.selectedDate) return;

    //  prevent past dates
    if (this.selectedDate < this.today) {
      this.toastr.warning('Past dates are not allowed');
      return;
    }

    const exists = this.leaves().some(
      x => x.leaveDate === this.selectedDate
    );

    if (exists) {
      this.toastr.info('Date already added');
      return;
    }

    this.leaves.update(list => [
      ...list,
      {
        leaveDate: this.selectedDate,
        reason: this.reason
      }
    ]);

    this.selectedDate = '';
    this.reason = '';
  }

  removeDate(index: number) {
    this.leaves.update(list =>
      list.filter((_, i) => i !== index)
    );
  }

  submitLeaves() {

    if (this.leaves().length === 0) {
      this.toastr.warning('Please add at least one leave date');
      return;
    }

    this.loading.set(true);

    this.doctorService
      .addLeaves(this.leaves())
      .subscribe({
        next: () => {

          this.toastr.success('Leaves added successfully');

          this.leaves.set([]); 

          this.loading.set(false);
        },

        error: () => {

          this.toastr.error('Unable to add leaves');

          this.loading.set(false);
        }
      });
  }
}