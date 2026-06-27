import { Injectable } from '@angular/core';

export interface PatientProfile {
  patientId: number;
  fullName: string;
  email: string;
  phoneNumber: string;
  dateOfBirth: string;
  gender: string;
  insuranceID: string;
}

@Injectable({
  providedIn: 'root'
})
export class PatientSession {
  private patientProfile: PatientProfile = {
    patientId: 1,
    fullName: 'Arun Menon',
    email: 'arun.patient@healthaxis.com',
    phoneNumber: '9876543210',
    dateOfBirth: '1998-04-15',
    gender: 'Male',
    insuranceID: 'INS-PAT-001'
  };

  getCurrentPatient(): PatientProfile {
    return this.patientProfile;
  }

  updateCurrentPatient(updatedProfile: PatientProfile): void {
    this.patientProfile = {
      ...updatedProfile
    };

    console.log('Updated patient profile:', this.patientProfile);
  }
}
