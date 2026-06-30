import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { DoctorInfo } from '../../../core/models/doctor-info.model';
import { DoctorService } from '../../../core/services/doctor.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-doctor-my-information',
  imports: [],
  templateUrl: './doctor-my-information.html',
  styleUrls: ['./doctor-my-information.css']
})
export class DoctorMyInformation implements OnInit {
  isLoading = false;
  errorMessage = '';

  doctor: DoctorInfo | null = null;

  constructor(
    private doctorService: DoctorService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    const referenceId = this.tokenService.getReferenceId();

    if (!referenceId) {
      this.errorMessage = 'Doctor information was not found. Please login again.';
      return;
    }

    this.loadDoctor(Number(referenceId));
  }

  loadDoctor(doctorId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.doctorService.getMyInformation(doctorId).subscribe({
      next: (doctor: DoctorInfo) => {
        this.doctor = doctor;
      },

      error: (error: HttpErrorResponse) => {
        console.log('Doctor information error:', error);

        this.errorMessage =
          error.error?.message ??
          `Unable to load doctor information. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  getSpecialisationName(value: number | string): string {
    if (typeof value === 'string' && isNaN(Number(value))) {
      return value;
    }

    const specialisationMap: Record<number, string> = {
      0: 'Cardiology',
      1: 'General Medicine',
      2: 'Dermatology',
      3: 'Pediatrics',
      4: 'Orthopedics',
      5: 'Neurology'
    };

    return specialisationMap[Number(value)] ??
      `Specialisation ${value}`;
  }

  formatFee(value: number): string {
    return new Intl.NumberFormat('en-IN', {
      style: 'currency',
      currency: 'INR'
    }).format(value);
  }
}