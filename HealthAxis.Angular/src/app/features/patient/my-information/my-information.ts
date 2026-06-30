import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Patient } from '../../../core/models/patient.model';
import { PatientUpdateRequest } from '../../../core/models/patient-update-request';
import { PatientService } from '../../../core/services/patient.service';
import { TokenService } from '../../../core/models/token.service';

@Component({
  selector: 'app-my-information',
  imports: [FormsModule],
  templateUrl: './my-information.html',
  styleUrls: ['./my-information.css']
})
export class MyInformation implements OnInit {
  isLoading = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  patientId = 0;

  patient: Patient = {
    patientId: 0,
    fullName: '',
    dateOfBirth: '',
    gender: '',
    phoneNumber: '',
    email: ''
  };

  constructor(
    private patientService: PatientService,
    private tokenService: TokenService
  ) {}

  ngOnInit(): void {
    const referenceId = this.tokenService.getReferenceId();

    if (!referenceId) {
      this.errorMessage =
        'Patient information was not found. Please login again.';
      return;
    }

    this.patientId = Number(referenceId);
    this.loadPatient();
  }

  loadPatient(): void {
    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.patientService.getMyInformation(this.patientId).subscribe({
      next: (patient: Patient) => {
        this.patient = {
          ...patient,
          dateOfBirth: patient.dateOfBirth
            ? patient.dateOfBirth.split('T')[0]
            : ''
        };
      },

      error: (error: HttpErrorResponse) => {
        console.log('Patient information error:', error);

        this.errorMessage =
          error.error?.message ??
          this.getValidationErrors(error) ??
          `Unable to load information. Status: ${error.status}`;
      },

      complete: () => {
        this.isLoading = false;
      }
    });
  }

  saveChanges(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (!this.patient.fullName.trim()) {
      this.errorMessage = 'Full name is required.';
      return;
    }

    if (!this.patient.dateOfBirth) {
      this.errorMessage = 'Date of birth is required.';
      return;
    }

    if (this.patient.gender === '' || this.patient.gender === null) {
      this.errorMessage = 'Gender is required.';
      return;
    }

    if (!/^[0-9]{10}$/.test(this.patient.phoneNumber)) {
      this.errorMessage = 'Phone number must contain exactly 10 digits.';
      return;
    }

    if (
      !/^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/.test(
        this.patient.email
      )
    ) {
      this.errorMessage = 'Enter a valid email address.';
      return;
    }

    const request: PatientUpdateRequest = {
      fullName: this.patient.fullName.trim(),
      dateOfBirth: this.patient.dateOfBirth,
      gender: Number(this.patient.gender),
      phoneNumber: this.patient.phoneNumber.trim(),
      email: this.patient.email.trim()
    };

    this.isSaving = true;

    this.patientService
      .updateMyInformation(this.patientId, request)
      .subscribe({
        next: (updatedPatient: Patient) => {
          this.patient = {
            ...updatedPatient,
            dateOfBirth: updatedPatient.dateOfBirth
              ? updatedPatient.dateOfBirth.split('T')[0]
              : ''
          };

          this.successMessage = 'Information updated successfully.';
        },

        error: (error: HttpErrorResponse) => {
          console.log('Update patient information error:', error);
          console.log('Validation response:', error.error);

          this.errorMessage =
            error.error?.message ??
            this.getValidationErrors(error) ??
            `Unable to update information. Status: ${error.status}`;
        },

        complete: () => {
          this.isSaving = false;
        }
      });
  }

  private getValidationErrors(error: HttpErrorResponse): string | null {
    const validationErrors = error.error?.errors;

    if (!validationErrors) {
      return null;
    }

    const messages: string[] = [];

    Object.keys(validationErrors).forEach((key) => {
      const fieldErrors = validationErrors[key];

      if (Array.isArray(fieldErrors)) {
        messages.push(...fieldErrors);
      }
    });

    return messages.length > 0
      ? messages.join(' ')
      : null;
  }
}