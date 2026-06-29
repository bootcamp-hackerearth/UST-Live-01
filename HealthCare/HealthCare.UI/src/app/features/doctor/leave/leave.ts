import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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

  leaves: Leave[] = [];

  loading = false;

  constructor(
    private doctorService: DoctorService
  ) {}

  addDate() {

    if (!this.selectedDate)
      return;

    const exists = this.leaves.some(
      x => x.leaveDate === this.selectedDate
    );

    if (exists) {

      alert('Date already added');
      return;
    }

    this.leaves.push({
      leaveDate: this.selectedDate
    });

    this.selectedDate = '';
  }

  removeDate(index: number) {

    this.leaves.splice(index, 1);
  }

  submitLeaves() {

    if (this.leaves.length === 0) {

      alert('Please add at least one leave date');
      return;
    }

    this.loading = true;

    this.doctorService
      .addLeaves(this.leaves)
      .subscribe({

        next: () => {

          alert('Leaves added successfully');

          this.leaves = [];

          this.loading = false;
        },

        error: () => {

          alert('Unable to add leaves');

          this.loading = false;
        }

      });
  }

}