import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DoctorService } from '../../../core/services/doctor.service';

import { Doctor } from '../../../core/models/doctor.model';
import { Leave } from '../../../core/models/leave.model';

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class DoctorProfileComponent implements OnInit {

  doctor = signal<Doctor>({} as Doctor);

  leaves = signal<Leave[]>([]);

  loading = signal(true);

  constructor(
    private readonly doctorService: DoctorService
  ) {}

  ngOnInit(): void {

    this.loadProfile();
    this.loadLeaves();

  }

  loadProfile() {

    this.doctorService.getProfile()
      .subscribe({

        next: res => {

          this.doctor.set(res);

          this.loading.set(false);

        },

        error: () => this.loading.set(false)

      });

  }

  loadLeaves() {

    this.doctorService.getMyLeaves()
      .subscribe({

        next: res => this.leaves.set(res),

        error: () => {}

      });

  }

  getDay(dateString: string): string {

    const date = new Date(dateString);

    return date.toLocaleDateString('en-US', { weekday: 'long' });

  }

  getInitials(): string {

    const fullName = this.doctor().fullName || '';

    const names = fullName.split(' ');

    if (names.length >= 2) {

      return names[0].charAt(0) + names[1].charAt(0);

    }

    return fullName.charAt(0);

  }

}