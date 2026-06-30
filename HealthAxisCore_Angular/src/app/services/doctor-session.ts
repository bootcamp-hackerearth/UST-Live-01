import { Injectable } from '@angular/core';

export interface DoctorProfile {
  doctorId: number;
  doctorName: string;
  email: string;
  phoneNumber: string;
  specialisation: string;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorSession {
  private doctorProfile: DoctorProfile = {
    doctorId: 1,
    doctorName: 'Dr. Isha Nair',
    email: 'isha.doctor@healthaxis.com',
    phoneNumber: '9876543210',
    specialisation: 'Dermatologist',
    yearsOfExperience: 10,
    consultationFee: 1000,
    isActive: true
  };

  getCurrentDoctor(): DoctorProfile {
    return this.doctorProfile;
  }

  updateCurrentDoctor(updatedDoctor: DoctorProfile): void {
    this.doctorProfile = {
      ...updatedDoctor
    };

    console.log('Updated doctor profile:', this.doctorProfile);
  }
}
