import { Injectable } from '@angular/core';

import { AppointmentDto } from '../../shared/models/appointment.models';
import { DoctorDto } from '../../shared/models/doctor.models';
import { HealthRecordDto } from '../../shared/models/health-record.models';

interface DoctorDashboardSummary {
  upcomingCount: number;
  pendingCount: number;
  confirmedCount: number;
  completedCount: number;
  healthRecordCount: number;
}

interface DoctorChangePasswordDto {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

interface AddDoctorHealthRecordDto {
  patientId: number;
  patientName: string;
  doctorId: number;
  doctorName: string;
  appointmentId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorFakeDataService {
  private doctorPassword = 'Doctor@123';
  private isTemporaryPasswordActive = true;

  private doctor: DoctorDto = {
    doctorId: 1,
    doctorName: 'Dr. Aarav Menon',
    specialisation: 'General Practitioner',
    yearsOfExperience: 8,
    consultationFee: 500,
    isActive: true
  };

  private appointments: AppointmentDto[] = [
    {
      appointmentId: 1,
      patientId: 1,
      patientName: 'Rishi Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      scheduledDate: this.getTodayDate(),
      timeSlot: '10:00 AM - 10:30 AM',
      status: 'Confirmed'
    },
    {
      appointmentId: 2,
      patientId: 2,
      patientName: 'Ananya Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      scheduledDate: this.getDateAfterDays(2),
      timeSlot: '11:30 AM - 12:00 PM',
      status: 'Pending'
    },
    {
      appointmentId: 3,
      patientId: 3,
      patientName: 'Karthik Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      scheduledDate: this.getDateBeforeDays(4),
      timeSlot: '02:00 PM - 02:30 PM',
      status: 'Completed'
    },
    {
      appointmentId: 4,
      patientId: 4,
      patientName: 'Meera Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      specialisation: 'General Practitioner',
      scheduledDate: this.getDateAfterDays(5),
      timeSlot: '04:00 PM - 04:30 PM',
      status: 'Pending'
    }
  ];

  private healthRecords: HealthRecordDto[] = [
    {
      healthRecordId: 1,
      patientId: 3,
      patientName: 'Karthik Patient',
      doctorId: 1,
      doctorName: 'Dr. Aarav Menon',
      appointmentId: 3,
      visitDate: this.getDateBeforeDays(4),
      diagnosis: 'Seasonal fever symptoms observed',
      prescription: 'Paracetamol 500mg twice daily for 3 days',
      notes: 'Drink enough fluids and take rest.'
    }
  ];

  // ===============================
  // DOCTOR PROFILE / PASSWORD
  // ===============================

  getDoctorProfile(): DoctorDto {
    return { ...this.doctor };
  }

  shouldChangeTemporaryPassword(): boolean {
    return this.isTemporaryPasswordActive;
  }

  changeTemporaryPassword(request: DoctorChangePasswordDto): void {
    this.validatePasswordRequest(request);

    if (request.currentPassword !== this.doctorPassword) {
      throw new Error('Current temporary password is incorrect.');
    }

    this.doctorPassword = request.newPassword;
    this.isTemporaryPasswordActive = false;
  }

  changeDoctorPassword(request: DoctorChangePasswordDto): void {
    this.validatePasswordRequest(request);

    if (request.currentPassword !== this.doctorPassword) {
      throw new Error('Current password is incorrect.');
    }

    this.doctorPassword = request.newPassword;
  }

  // ===============================
  // DASHBOARD
  // ===============================

  getDashboardSummary(): DoctorDashboardSummary {
    const upcomingAppointments = this.getUpcomingAppointments();

    return {
      upcomingCount: upcomingAppointments.length,
      pendingCount: this.appointments.filter(
        (appointment: AppointmentDto) => appointment.status === 'Pending'
      ).length,
      confirmedCount: this.appointments.filter(
        (appointment: AppointmentDto) => appointment.status === 'Confirmed'
      ).length,
      completedCount: this.appointments.filter(
        (appointment: AppointmentDto) => appointment.status === 'Completed'
      ).length,
      healthRecordCount: this.healthRecords.length
    };
  }

  // ===============================
  // APPOINTMENTS
  // ===============================

  getDoctorAppointments(): AppointmentDto[] {
    return this.appointments.map((appointment: AppointmentDto) => ({
      ...appointment
    }));
  }

  getUpcomingAppointments(): AppointmentDto[] {
    const today = this.getTodayDate();

    return this.appointments
      .filter(
        (appointment: AppointmentDto) =>
          appointment.scheduledDate >= today &&
          appointment.status !== 'Cancelled' &&
          appointment.status !== 'Completed'
      )
      .map((appointment: AppointmentDto) => ({ ...appointment }))
      .sort(
        (a: AppointmentDto, b: AppointmentDto) =>
          new Date(a.scheduledDate).getTime() -
          new Date(b.scheduledDate).getTime()
      );
  }

  confirmAppointment(appointmentId: number): AppointmentDto {
    const appointment = this.findAppointment(appointmentId);

    if (appointment.status !== 'Pending') {
      throw new Error('Only pending appointments can be confirmed.');
    }

    appointment.status = 'Confirmed';

    return { ...appointment };
  }

  completeAppointment(appointmentId: number): AppointmentDto {
    const appointment = this.findAppointment(appointmentId);

    if (appointment.status !== 'Confirmed') {
      throw new Error('Only confirmed appointments can be marked as completed.');
    }

    if (!this.isHealthRecordAlreadyAdded(appointmentId)) {
      throw new Error('Appointment can be completed only after health record is saved.');
    }

    appointment.status = 'Completed';

    return { ...appointment };
  }

  // ===============================
  // HEALTH RECORDS
  // ===============================

  getDoctorHealthRecords(): HealthRecordDto[] {
    return this.healthRecords
      .map((record: HealthRecordDto) => ({ ...record }))
      .sort(
        (a: HealthRecordDto, b: HealthRecordDto) =>
          new Date(b.visitDate).getTime() -
          new Date(a.visitDate).getTime()
      );
  }

  getConfirmedTodayAppointmentsWithoutHealthRecord(): AppointmentDto[] {
    const today = this.getTodayDate();

    return this.appointments
      .filter(
        (appointment: AppointmentDto) =>
          appointment.status === 'Confirmed' &&
          appointment.scheduledDate === today &&
          !this.isHealthRecordAlreadyAdded(appointment.appointmentId)
      )
      .map((appointment: AppointmentDto) => ({ ...appointment }));
  }

  addHealthRecord(request: AddDoctorHealthRecordDto): HealthRecordDto {
    if (this.isHealthRecordAlreadyAdded(request.appointmentId)) {
      throw new Error('Health record is already added for this appointment.');
    }

    const appointment = this.findAppointment(request.appointmentId);

    if (appointment.status !== 'Confirmed') {
      throw new Error('Health record can be added only for confirmed appointments.');
    }

    if (appointment.scheduledDate !== this.getTodayDate()) {
      throw new Error('Health record can be added only on the appointment date.');
    }

    const diagnosis = request.diagnosis.trim();
    const prescription = request.prescription.trim();

    if (diagnosis.length < 3) {
      throw new Error('Diagnosis must contain at least 3 characters.');
    }

    if (prescription.length < 3) {
      throw new Error('Prescription must contain at least 3 characters.');
    }

    const newRecord: HealthRecordDto = {
      healthRecordId: this.getNextHealthRecordId(),
      patientId: request.patientId,
      patientName: request.patientName,
      doctorId: request.doctorId,
      doctorName: request.doctorName,
      appointmentId: request.appointmentId,
      visitDate: appointment.scheduledDate,
      diagnosis,
      prescription,
      notes: request.notes?.trim() || undefined
    };

    this.healthRecords.unshift(newRecord);

    return { ...newRecord };
  }

  isHealthRecordAlreadyAdded(appointmentId: number): boolean {
    return this.healthRecords.some(
      (record: HealthRecordDto) => record.appointmentId === appointmentId
    );
  }

  // ===============================
  // PRIVATE HELPERS
  // ===============================

  private findAppointment(appointmentId: number): AppointmentDto {
    const appointment = this.appointments.find(
      (item: AppointmentDto) => item.appointmentId === appointmentId
    );

    if (!appointment) {
      throw new Error('Appointment not found.');
    }

    return appointment;
  }

  private validatePasswordRequest(request: DoctorChangePasswordDto): void {
    if (
      !request.currentPassword ||
      !request.newPassword ||
      !request.confirmPassword
    ) {
      throw new Error('Please fill all password fields.');
    }

    if (request.newPassword !== request.confirmPassword) {
      throw new Error('New password and confirm password do not match.');
    }

    if (request.currentPassword === request.newPassword) {
      throw new Error('New password must be different from current password.');
    }

    const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$/;

    if (!passwordPattern.test(request.newPassword)) {
      throw new Error(
        'New password must be at least 8 characters and include uppercase letter, number, and special character.'
      );
    }
  }

  private getNextHealthRecordId(): number {
    if (this.healthRecords.length === 0) {
      return 1;
    }

    return (
      Math.max(
        ...this.healthRecords.map(
          (record: HealthRecordDto) => record.healthRecordId
        )
      ) + 1
    );
  }

  private getTodayDate(): string {
    return new Date().toISOString().split('T')[0];
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