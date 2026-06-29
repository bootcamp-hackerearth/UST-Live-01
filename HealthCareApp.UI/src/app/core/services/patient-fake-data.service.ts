import { Injectable } from '@angular/core';

import {
  AppointmentDto,
  BookAppointmentDto,
  CancelAppointmentDto
} from '../../shared/models/appointment.models';

import { DoctorDto } from '../../shared/models/doctor.models';
import { HealthRecordDto } from '../../shared/models/health-record.models';

import {
  ChangePasswordDto,
  PatientDto,
  UpdatePatientDto
} from '../../shared/models/patient.models';

@Injectable({
  providedIn: 'root'
})
export class PatientFakeDataService {

  private patientPassword = 'Patient@123';

  private patient: PatientDto = {
    patientId: 1,
    patientName: 'Rishi Patient',
    dateOfBirth: '2000-01-01',
    gender: 'Male',
    email: 'rishi.patient@example.com',
    phoneNumber: '+91 9876543210',
    insuranceId: 'INS-HA-1001'
  };

  private doctors: DoctorDto[] = [
    {
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      yearsOfExperience: 8,
      consultationFee: 500,
      isActive: true
    },
    {
      doctorId: 2,
      doctorName: 'Dr. Meera Iyer',
      specialisation: 'Cardiologist',
      yearsOfExperience: 12,
      consultationFee: 900,
      isActive: true
    },
    {
      doctorId: 3,
      doctorName: 'Dr. Nikhil Rao',
      specialisation: 'Dermatologist',
      yearsOfExperience: 6,
      consultationFee: 650,
      isActive: true
    },
    {
      doctorId: 4,
      doctorName: 'Dr. Kavya Sharma',
      specialisation: 'Pediatrician',
      yearsOfExperience: 10,
      consultationFee: 700,
      isActive: false
    }
  ];

  private appointments: AppointmentDto[] = [
    {
      appointmentId: 1,
      patientId: 1,
      patientName: 'Rishi Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      scheduledDate: this.getDateAfterDays(1),
      timeSlot: '10:00 AM - 10:30 AM',
      status: 'Confirmed'
    },
    {
      appointmentId: 2,
      patientId: 1,
      patientName: 'Rishi Patient',
      doctorId: 2,
      doctorName: 'Dr. Meera Iyer',
      specialisation: 'Cardiologist',
      scheduledDate: this.getDateAfterDays(3),
      timeSlot: '11:30 AM - 12:00 PM',
      status: 'Pending'
    },
    {
      appointmentId: 3,
      patientId: 1,
      patientName: 'Rishi Patient',
      doctorId: 3,
      doctorName: 'Dr. Nikhil Rao',
      specialisation: 'Dermatologist',
      scheduledDate: this.getDateBeforeDays(8),
      timeSlot: '02:00 PM - 02:30 PM',
      status: 'Completed'
    }
  ];

  private healthRecords: HealthRecordDto[] = [
    {
      healthRecordId: 1,
      patientId: 1,
      patientName: 'Rishi Patient',
      doctorId: 3,
      doctorName: 'Dr. Nikhil Rao',
      appointmentId: 3,
      visitDate: this.getDateBeforeDays(8),
      diagnosis: 'Skin allergy symptoms observed',
      prescription: 'Antihistamine tablet once daily for 5 days',
      notes: 'Avoid direct exposure to dust.'
    }
  ];

  getPatientProfile(): PatientDto {
    return { ...this.patient };
  }

  updatePatientProfile(request: UpdatePatientDto): PatientDto {
    this.patient = {
      ...this.patient,
      patientName: request.patientName,
      dateOfBirth: request.dateOfBirth,
      gender: request.gender,
      email: request.email,
      phoneNumber: request.phoneNumber,
      insuranceId: request.insuranceId
    };

    return { ...this.patient };
  }

  changePatientPassword(request: ChangePasswordDto): void {
    if (
      !request.currentPassword ||
      !request.newPassword ||
      !request.confirmPassword
    ) {
      throw new Error('Please fill all password fields.');
    }

    if (request.currentPassword !== this.patientPassword) {
      throw new Error('Current password is incorrect.');
    }

    if (request.newPassword !== request.confirmPassword) {
      throw new Error('New password and confirm password do not match.');
    }

    if (request.currentPassword === request.newPassword) {
      throw new Error('New password must be different from current password.');
    }

    this.patientPassword = request.newPassword;
  }

  getActiveDoctors(): DoctorDto[] {
    return this.doctors
      .filter((doctor: DoctorDto) => doctor.isActive)
      .map((doctor: DoctorDto) => ({ ...doctor }));
  }

  getSpecialisations(): string[] {
    return Array.from(
      new Set(
        this.doctors
          .filter((doctor: DoctorDto) => doctor.isActive)
          .map((doctor: DoctorDto) => doctor.specialisation)
      )
    );
  }

  getDoctorsBySpecialisation(specialisation: string): DoctorDto[] {
    return this.doctors
      .filter(
        (doctor: DoctorDto) =>
          doctor.isActive &&
          doctor.specialisation === specialisation
      )
      .map((doctor: DoctorDto) => ({ ...doctor }));
  }

  getAppointments(): AppointmentDto[] {
    return this.appointments.map((appointment: AppointmentDto) => ({
      ...appointment
    }));
  }

  getUpcomingAppointments(): AppointmentDto[] {
    const today = new Date().toISOString().split('T')[0];

    return this.appointments
      .filter(
        (appointment: AppointmentDto) =>
          appointment.scheduledDate >= today &&
          appointment.status !== 'Cancelled' &&
          appointment.status !== 'Completed'
      )
      .map((appointment: AppointmentDto) => ({ ...appointment }));
  }

 bookAppointment(request: BookAppointmentDto): AppointmentDto {
  const doctor = this.doctors.find(
    (item: DoctorDto) => item.doctorId === request.doctorId
  );

  if (!doctor) {
    throw new Error('Doctor not found.');
  }

  if (!doctor.isActive) {
    throw new Error('Doctor is not available for booking.');
  }

  const today = new Date().toISOString().split('T')[0];
  const maxBookingDate = this.getDateAfterDays(30);

  if (request.scheduledDate < today) {
    throw new Error('Past dates are not allowed for appointment booking.');
  }

  if (request.scheduledDate > maxBookingDate) {
    throw new Error('Appointments can only be booked within the next 30 days.');
  }

  if (
    this.isSlotBooked(
      request.doctorId,
      request.scheduledDate,
      request.timeSlot
    )
  ) {
    throw new Error('This slot is already booked. Please choose another.');
  }

  const newAppointment: AppointmentDto = {
    appointmentId: this.getNextAppointmentId(),
    patientId: request.patientId,
    patientName: this.patient.patientName,
    doctorId: doctor.doctorId,
    doctorName: doctor.doctorName,
    specialisation: doctor.specialisation,
    scheduledDate: request.scheduledDate,
    timeSlot: request.timeSlot,
    status: 'Pending'
  };

  this.appointments.unshift(newAppointment);

  return { ...newAppointment };
}
  cancelAppointment(request: CancelAppointmentDto): AppointmentDto {
    const appointment = this.appointments.find(
      (item: AppointmentDto) => item.appointmentId === request.appointmentId
    );

    if (!appointment) {
      throw new Error('Appointment not found.');
    }

    if (appointment.status !== 'Pending') {
      throw new Error('Only pending appointments can be cancelled.');
    }

    const reason = request.reason.trim();

    if (reason.length < 5) {
      throw new Error('Cancellation reason must contain at least 5 characters.');
    }

    appointment.status = 'Cancelled';
    appointment.cancellationReason = reason;

    return { ...appointment };
  }

  isSlotBooked(doctorId: number, scheduledDate: string, timeSlot: string): boolean {
    return this.appointments.some(
      (appointment: AppointmentDto) =>
        appointment.doctorId === doctorId &&
        appointment.scheduledDate === scheduledDate &&
        appointment.timeSlot === timeSlot &&
        appointment.status !== 'Cancelled'
    );
  }

  getHealthRecords(): HealthRecordDto[] {
    return this.healthRecords.map((record: HealthRecordDto) => ({
      ...record
    }));
  }

  getDashboardSummary(): {
    upcomingCount: number;
    pendingCount: number;
    completedCount: number;
    healthRecordCount: number;
  } {
    return {
      upcomingCount: this.getUpcomingAppointments().length,
      pendingCount: this.appointments.filter(
        (appointment: AppointmentDto) => appointment.status === 'Pending'
      ).length,
      completedCount: this.appointments.filter(
        (appointment: AppointmentDto) => appointment.status === 'Completed'
      ).length,
      healthRecordCount: this.healthRecords.length
    };
  }

  getAvailableTimeSlots(): string[] {
    return [
      '09:00 AM - 09:30 AM',
      '10:00 AM - 10:30 AM',
      '11:30 AM - 12:00 PM',
      '02:00 PM - 02:30 PM',
      '04:00 PM - 04:30 PM'
    ];
  }

  private getNextAppointmentId(): number {
    if (this.appointments.length === 0) {
      return 1;
    }

    return (
      Math.max(
        ...this.appointments.map(
          (appointment: AppointmentDto) => appointment.appointmentId
        )
      ) + 1
    );
  }

  private getDateAfterDays(days: number): string {
    const date = new Date();

    date.setDate(date.getDate() + days);

    return date.toISOString().split('T')[0];
  }

  private getDateBeforeDays(days: number): string {
    const date = new Date();

    date.setDate(date.getDate() - days);

    return date.toISOString().split('T')[0];
  }
}