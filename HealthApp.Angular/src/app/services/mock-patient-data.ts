import { Injectable } from '@angular/core';

export type GenderType = 'Male' | 'Female' | 'Transgender' | 'Other';

export type AppointmentStatus =
  | 'Pending'
  | 'Confirmed'
  | 'Cancelled'
  | 'Completed';

export type SpecialisationType =
  | 'Endocrinologist'
  | 'Oncologist'
  | 'Gynecologist'
  | 'OrthopedicSurgeon'
  | 'Psychiatrist'
  | 'Pediatrician'
  | 'Neurologist'
  | 'Dermatologist'
  | 'Cardiologist'
  | 'GeneralPractitioner';

export interface PatientDto {
  patientId: number;
  fullName: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  email?: string;
  insuranceId?: string;
  createdDate: string;
}

export interface DoctorDto {
  doctorId: number;
  fullName: string;
  specialisation: SpecialisationType;
  yearsOfExperience: number;
  consultationFee: number;
  isActive: boolean;
}

export interface AppointmentDto {
  appointmentId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  scheduledDate: string;
  timeSlot: string;
  status: AppointmentStatus;
  cancellationReason?: string;
}

export interface HealthRecordDto {
  healthRecordId: number;
  patientId: number;
  patientName?: string;
  doctorId: number;
  doctorName?: string;
  appointmentId: number;
  visitDate: string;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

export interface BookAppointmentRequest {
  patientId: number;
  doctorId: number;
  scheduledDate: string;
  timeSlot: string;
}

export interface CancelAppointmentRequest {
  appointmentId: number;
  reason: string;
}

export interface RegisterPatientRequest {
  fullName: string;
  email: string;
  password: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  insuranceId?: string;
}

export interface UpdatePatientProfileRequest {
  fullName: string;
  email: string;
  dateOfBirth: string;
  gender: GenderType;
  phoneNumber: string;
  insuranceId?: string;
}

export interface OperationResult<T = unknown> {
  success: boolean;
  message: string;
  data?: T;
}

export interface AddHealthRecordRequest {
  appointmentId: number;
  doctorId: number;
  diagnosis: string;
  prescription: string;
  notes?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MockPatientData {
  readonly specialisations: SpecialisationType[] = [
    'Endocrinologist',
    'Oncologist',
    'Gynecologist',
    'OrthopedicSurgeon',
    'Psychiatrist',
    'Pediatrician',
    'Neurologist',
    'Dermatologist',
    'Cardiologist',
    'GeneralPractitioner'
  ];

  readonly genders: GenderType[] = [
    'Male',
    'Female',
    'Transgender',
    'Other'
  ];

  readonly timeSlots: string[] = [
    '09:00 AM - 09:30 AM',
    '09:30 AM - 10:00 AM',
    '10:00 AM - 10:30 AM',
    '10:30 AM - 11:00 AM',
    '11:00 AM - 11:30 AM',
    '11:30 AM - 12:00 PM',
    '12:00 PM - 12:30 PM',
    '02:00 PM - 02:30 PM',
    '02:30 PM - 03:00 PM',
    '03:00 PM - 03:30 PM',
    '03:30 PM - 04:00 PM',
    '04:00 PM - 04:30 PM',
    '04:30 PM - 05:00 PM',
    '05:00 PM - 05:30 PM',
    '05:30 PM - 06:00 PM'
  ];

  patients: PatientDto[] = [
    {
      patientId: 1,
      fullName: 'Kevin',
      dateOfBirth: '2001-05-12',
      gender: 'Male',
      phoneNumber: '9876543210',
      email: 'kevin@gmail.com',
      insuranceId: 'INS1001',
      createdDate: this.toDateOnly(new Date())
    },
    {
      patientId: 2,
      fullName: 'Anu Joseph',
      dateOfBirth: '1999-09-18',
      gender: 'Female',
      phoneNumber: '9876501234',
      email: 'anu@gmail.com',
      insuranceId: 'INS1002',
      createdDate: this.toDateOnly(new Date())
    },
    {
      patientId: 3,
      fullName: 'Rahul Nair',
      dateOfBirth: '1997-02-23',
      gender: 'Male',
      phoneNumber: '9123456780',
      email: 'rahul@gmail.com',
      insuranceId: '',
      createdDate: this.toDateOnly(new Date())
    }
  ];

  doctors: DoctorDto[] = [
    {
      doctorId: 1,
      fullName: 'Dr Vignesh Kumar',
      specialisation: 'Cardiologist',
      yearsOfExperience: 9,
      consultationFee: 900,
      isActive: true
    },
    {
      doctorId: 2,
      fullName: 'Dr Anjali Menon',
      specialisation: 'Dermatologist',
      yearsOfExperience: 6,
      consultationFee: 700,
      isActive: true
    },
    {
      doctorId: 3,
      fullName: 'Dr Arjun Nair',
      specialisation: 'Neurologist',
      yearsOfExperience: 11,
      consultationFee: 1200,
      isActive: true
    }
  ];

  appointments: AppointmentDto[] = [
    {
      appointmentId: 1,
      patientId: 1,
      patientName: 'Kevin',
      doctorId: 1,
      doctorName: 'Dr Vignesh Kumar',
      scheduledDate: this.toDateOnly(this.addDays(-10)),
      timeSlot: '09:00 AM - 09:30 AM',
      status: 'Completed'
    },
    {
      appointmentId: 2,
      patientId: 1,
      patientName: 'Kevin',
      doctorId: 2,
      doctorName: 'Dr Anjali Menon',
      scheduledDate: this.toDateOnly(this.addDays(5)),
      timeSlot: '10:30 AM - 11:00 AM',
      status: 'Confirmed'
    },
    {
      appointmentId: 3,
      patientId: 1,
      patientName: 'Kevin',
      doctorId: 3,
      doctorName: 'Dr Arjun Nair',
      scheduledDate: this.toDateOnly(this.addDays(12)),
      timeSlot: '02:00 PM - 02:30 PM',
      status: 'Pending'
    },
    {
      appointmentId: 4,
      patientId: 2,
      patientName: 'Anu Joseph',
      doctorId: 2,
      doctorName: 'Dr Anjali Menon',
      scheduledDate: this.toDateOnly(this.addDays(-8)),
      timeSlot: '11:00 AM - 11:30 AM',
      status: 'Completed'
    },
    {
      appointmentId: 5,
      patientId: 2,
      patientName: 'Anu Joseph',
      doctorId: 1,
      doctorName: 'Dr Vignesh Kumar',
      scheduledDate: this.toDateOnly(this.addDays(3)),
      timeSlot: '09:30 AM - 10:00 AM',
      status: 'Cancelled',
      cancellationReason: 'Patient unavailable'
    },
    {
      appointmentId: 6,
      patientId: 2,
      patientName: 'Anu Joseph',
      doctorId: 3,
      doctorName: 'Dr Arjun Nair',
      scheduledDate: this.toDateOnly(this.addDays(16)),
      timeSlot: '03:00 PM - 03:30 PM',
      status: 'Confirmed'
    },
    {
      appointmentId: 7,
      patientId: 3,
      patientName: 'Rahul Nair',
      doctorId: 3,
      doctorName: 'Dr Arjun Nair',
      scheduledDate: this.toDateOnly(this.addDays(-15)),
      timeSlot: '04:00 PM - 04:30 PM',
      status: 'Completed'
    },
    {
      appointmentId: 8,
      patientId: 3,
      patientName: 'Rahul Nair',
      doctorId: 1,
      doctorName: 'Dr Vignesh Kumar',
      scheduledDate: this.toDateOnly(this.addDays(7)),
      timeSlot: '12:00 PM - 12:30 PM',
      status: 'Pending'
    },
    {
      appointmentId: 9,
      patientId: 3,
      patientName: 'Rahul Nair',
      doctorId: 2,
      doctorName: 'Dr Anjali Menon',
      scheduledDate: this.toDateOnly(this.addDays(18)),
      timeSlot: '05:00 PM - 05:30 PM',
      status: 'Confirmed'
    }
  ];

  healthRecords: HealthRecordDto[] = [
    {
      healthRecordId: 1,
      patientId: 1,
      patientName: 'Kevin',
      doctorId: 1,
      doctorName: 'Dr Vignesh Kumar',
      appointmentId: 1,
      visitDate: this.toDateOnly(this.addDays(-10)),
      diagnosis: 'Mild hypertension',
      prescription: 'Amlodipine 5mg once daily',
      notes: 'Follow-up advised after one month.'
    },
    {
      healthRecordId: 2,
      patientId: 2,
      patientName: 'Anu Joseph',
      doctorId: 2,
      doctorName: 'Dr Anjali Menon',
      appointmentId: 4,
      visitDate: this.toDateOnly(this.addDays(-8)),
      diagnosis: 'Skin allergy',
      prescription: 'Antihistamine tablet and topical cream',
      notes: 'Avoid known allergens and review if symptoms continue.'
    },
    {
      healthRecordId: 3,
      patientId: 3,
      patientName: 'Rahul Nair',
      doctorId: 3,
      doctorName: 'Dr Arjun Nair',
      appointmentId: 7,
      visitDate: this.toDateOnly(this.addDays(-15)),
      diagnosis: 'Migraine episode',
      prescription: 'Pain relief medication as needed',
      notes: 'Maintain sleep schedule and avoid trigger foods.'
    }
  ];

  getPatients(): PatientDto[] {
    return [...this.patients];
  }

  getPatientById(patientId: number): PatientDto | undefined {
    return this.patients.find(patient => patient.patientId === patientId);
  }

  getPatientByEmail(email: string): PatientDto | undefined {
    return this.patients.find(
      patient => patient.email?.toLowerCase() === email.trim().toLowerCase()
    );
  }

  addPatient(request: RegisterPatientRequest): OperationResult<PatientDto> {
    const validation = this.validateRegisterRequest(request);

    if (!validation.success) {
      return validation;
    }

    const nextPatientId = this.getNextPatientId();

    const patient: PatientDto = {
      patientId: nextPatientId,
      fullName: request.fullName.trim(),
      dateOfBirth: request.dateOfBirth,
      gender: request.gender,
      phoneNumber: request.phoneNumber.trim(),
      email: request.email.trim(),
      insuranceId: request.insuranceId?.trim() ?? '',
      createdDate: this.toDateOnly(new Date())
    };

    this.patients.push(patient);

    return {
      success: true,
      message: 'Patient registered successfully.',
      data: patient
    };
  }

  getDoctors(): DoctorDto[] {
    return this.doctors.filter(doctor => doctor.isActive);
  }

  getDoctorsBySpecialisation(specialisation: SpecialisationType): DoctorDto[] {
    return this.doctors.filter(
      doctor => doctor.isActive && doctor.specialisation === specialisation
    );
  }

  getDoctorById(doctorId: number): DoctorDto | undefined {
    return this.doctors.find(doctor => doctor.doctorId === doctorId);
  }

  getAppointmentsByPatient(patientId: number): AppointmentDto[] {
    return this.appointments
      .filter(appointment => appointment.patientId === patientId)
      .sort((a, b) =>
        new Date(b.scheduledDate).getTime() - new Date(a.scheduledDate).getTime()
      );
  }

  getStatusCount(patientId: number, status: AppointmentStatus): number {
    return this.appointments.filter(
      appointment =>
        appointment.patientId === patientId &&
        appointment.status === status
    ).length;
  }

  getUpcomingAppointmentsWithinDays(
    patientId: number,
    days: number
  ): AppointmentDto[] {
    const today = this.startOfDay(new Date());
    const limitDate = this.startOfDay(this.addDays(days));

    return this.appointments
      .filter(appointment => {
        const appointmentDate = this.startOfDay(new Date(appointment.scheduledDate));

        return (
          appointment.patientId === patientId &&
          appointmentDate >= today &&
          appointmentDate <= limitDate &&
          appointment.status !== 'Cancelled' &&
          appointment.status !== 'Completed'
        );
      })
      .sort((a, b) =>
        new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime()
      );
  }

  getAvailableSlots(doctorId: number, scheduledDate: string): string[] {
    if (!doctorId || !scheduledDate) {
      return [];
    }

    const bookedSlots = this.appointments
      .filter(appointment =>
        appointment.doctorId === doctorId &&
        appointment.scheduledDate === scheduledDate &&
        appointment.status !== 'Cancelled'
      )
      .map(appointment => appointment.timeSlot);

    return this.timeSlots.filter(slot => !bookedSlots.includes(slot));
  }

  bookAppointment(request: BookAppointmentRequest): OperationResult<AppointmentDto> {
    const validation = this.validateBookingRequest(request);

    if (!validation.success) {
      return validation;
    }

    const patient = this.getPatientById(request.patientId);
    const doctor = this.getDoctorById(request.doctorId);

    if (!patient || !doctor) {
      return {
        success: false,
        message: 'Patient or doctor details were not found.'
      };
    }

    const appointment: AppointmentDto = {
      appointmentId: this.getNextAppointmentId(),
      patientId: patient.patientId,
      patientName: patient.fullName,
      doctorId: doctor.doctorId,
      doctorName: doctor.fullName,
      scheduledDate: request.scheduledDate,
      timeSlot: request.timeSlot,
      status: 'Pending'
    };

    this.appointments.push(appointment);

    return {
      success: true,
      message: 'Appointment booked successfully. Status is pending confirmation.',
      data: appointment
    };
  }

  cancelAppointment(request: CancelAppointmentRequest): OperationResult<AppointmentDto> {
    const reason = request.reason.trim();

    if (!reason) {
      return {
        success: false,
        message: 'Cancellation reason is required.'
      };
    }

    if (reason.length > 200) {
      return {
        success: false,
        message: 'Cancellation reason must not exceed 200 characters.'
      };
    }

    const appointment = this.appointments.find(
      item => item.appointmentId === request.appointmentId
    );

    if (!appointment) {
      return {
        success: false,
        message: 'Appointment not found.'
      };
    }

    if (appointment.status !== 'Pending' && appointment.status !== 'Confirmed') {
      return {
        success: false,
        message: 'Only pending or confirmed appointments can be cancelled.'
      };
    }

    appointment.status = 'Cancelled';
    appointment.cancellationReason = reason;

    return {
      success: true,
      message: 'Appointment cancelled successfully.',
      data: appointment
    };
  }

  getHealthRecordsByPatient(patientId: number): HealthRecordDto[] {
    return this.healthRecords
      .filter(record => record.patientId === patientId)
      .sort((a, b) =>
        new Date(b.visitDate).getTime() - new Date(a.visitDate).getTime()
      );
  }

  getDoctorSpecialisation(doctorId: number): SpecialisationType | undefined {
    return this.getDoctorById(doctorId)?.specialisation;
  }

  getAppointmentsByDoctor(doctorId: number): AppointmentDto[] {
  return this.appointments
    .filter(appointment => appointment.doctorId === doctorId)
    .sort((a, b) =>
      new Date(a.scheduledDate).getTime() - new Date(b.scheduledDate).getTime()
    );
}

  getTodayAppointmentsByDoctor(doctorId: number): AppointmentDto[] {
    const today = this.toDateOnly(new Date());

    return this.appointments
      .filter(appointment =>
        appointment.doctorId === doctorId &&
        appointment.scheduledDate === today
      )
      .sort((a, b) => this.compareTimeSlots(a.timeSlot, b.timeSlot));
  }

  getDoctorStatusCount(
    doctorId: number,
    status: AppointmentStatus
  ): number {
    return this.appointments.filter(appointment =>
      appointment.doctorId === doctorId &&
      appointment.status === status
    ).length;
  }

  getDoctorTodayCount(doctorId: number): number {
    const today = this.toDateOnly(new Date());

    return this.appointments.filter(appointment =>
      appointment.doctorId === doctorId &&
      appointment.scheduledDate === today
    ).length;
  }

  private compareTimeSlots(firstSlot: string, secondSlot: string): number {
    return this.timeSlots.indexOf(firstSlot) - this.timeSlots.indexOf(secondSlot);
  }

  private validateRegisterRequest(
    request: RegisterPatientRequest
  ): OperationResult<PatientDto> {
    const fullNamePattern = /^[A-Z][a-zA-Z\s]{2,}$/;
    const phonePattern = /^[6-9]\d{9}$/;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!request.fullName.trim()) {
      return {
        success: false,
        message: 'Full name is required.'
      };
    }

    if (!fullNamePattern.test(request.fullName.trim())) {
      return {
        success: false,
        message: 'Full name must start with an uppercase letter and be at least 3 characters long.'
      };
    }

    if (!request.email.trim() || !emailPattern.test(request.email.trim())) {
      return {
        success: false,
        message: 'Valid email address is required.'
      };
    }

    if (this.getPatientByEmail(request.email)) {
      return {
        success: false,
        message: 'A patient with this email already exists.'
      };
    }

    if (!request.password || request.password.length < 6) {
      return {
        success: false,
        message: 'Password must be at least 6 characters long.'
      };
    }

    if (!request.dateOfBirth) {
      return {
        success: false,
        message: 'Date of birth is required.'
      };
    }

    if (new Date(request.dateOfBirth) > new Date()) {
      return {
        success: false,
        message: 'Date of birth cannot be in the future.'
      };
    }

    if (!request.gender) {
      return {
        success: false,
        message: 'Gender is required.'
      };
    }

    if (!request.phoneNumber.trim() || !phonePattern.test(request.phoneNumber.trim())) {
      return {
        success: false,
        message: 'Invalid phone number.'
      };
    }

    if (this.patients.some(patient => patient.phoneNumber === request.phoneNumber.trim())) {
      return {
        success: false,
        message: 'A patient with this phone number already exists.'
      };
    }

    return {
      success: true,
      message: 'Valid request.'
    };
  }

  private validateBookingRequest(
    request: BookAppointmentRequest
  ): OperationResult<AppointmentDto> {
    if (!request.patientId) {
      return {
        success: false,
        message: 'Patient details are required.'
      };
    }

    if (!request.doctorId) {
      return {
        success: false,
        message: 'Please select a doctor.'
      };
    }

    const doctor = this.getDoctorById(request.doctorId);

    if (!doctor || !doctor.isActive) {
      return {
        success: false,
        message: 'Selected doctor is not available.'
      };
    }

    if (!request.scheduledDate) {
      return {
        success: false,
        message: 'Please select an appointment date.'
      };
    }

    const selectedDate = this.startOfDay(new Date(request.scheduledDate));
    const today = this.startOfDay(new Date());

    if (selectedDate < today) {
      return {
        success: false,
        message: 'Appointment date cannot be in the past.'
      };
    }

    if (!request.timeSlot) {
      return {
        success: false,
        message: 'Please select a time slot.'
      };
    }

    if (!this.timeSlots.includes(request.timeSlot)) {
      return {
        success: false,
        message: 'Please select a valid time slot.'
      };
    }

    const doctorSlotAlreadyBooked = this.appointments.some(appointment =>
      appointment.doctorId === request.doctorId &&
      appointment.scheduledDate === request.scheduledDate &&
      appointment.timeSlot === request.timeSlot &&
      appointment.status !== 'Cancelled'
    );

    if (doctorSlotAlreadyBooked) {
      return {
        success: false,
        message: 'Selected slot is already booked for this doctor.'
      };
    }

    const patientSameSlot = this.appointments.some(appointment =>
      appointment.patientId === request.patientId &&
      appointment.scheduledDate === request.scheduledDate &&
      appointment.timeSlot === request.timeSlot &&
      appointment.status !== 'Cancelled'
    );

    if (patientSameSlot) {
      return {
        success: false,
        message: 'You already have an appointment at this date and time.'
      };
    }

    const patientSameDoctorSameDate = this.appointments.some(appointment =>
      appointment.patientId === request.patientId &&
      appointment.doctorId === request.doctorId &&
      appointment.scheduledDate === request.scheduledDate &&
      appointment.status !== 'Cancelled'
    );

    if (patientSameDoctorSameDate) {
      return {
        success: false,
        message: 'You already have an active appointment with this doctor on the selected date.'
      };
    }

    return {
      success: true,
      message: 'Valid booking.'
    };
  }

  private getNextPatientId(): number {
    return Math.max(...this.patients.map(patient => patient.patientId), 0) + 1;
  }

  private getNextAppointmentId(): number {
    return Math.max(...this.appointments.map(appointment => appointment.appointmentId), 100) + 1;
  }

  private addDays(days: number): Date {
    const date = new Date();
    date.setDate(date.getDate() + days);
    return date;
  }

  private startOfDay(date: Date): Date {
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }

  private toDateOnly(date: Date): string {
    return date.toISOString().split('T')[0];
  }

  updatePatientProfile(
  patientId: number,
  request: UpdatePatientProfileRequest): OperationResult<PatientDto> {
  const patient = this.patients.find(item => item.patientId === patientId);

  if (!patient) {
    return {
      success: false,
      message: 'Patient details were not found.'
    };
  }

  const fullNamePattern = /^[A-Z][a-zA-Z\s]{2,}$/;
  const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  const phonePattern = /^[6-9]\d{9}$/;

  if (!request.fullName.trim()) {
    return {
      success: false,
      message: 'Full name is required.'
    };
  }

  if (!fullNamePattern.test(request.fullName.trim())) {
    return {
      success: false,
      message: 'Full name must start with an uppercase letter and be at least 3 characters long.'
    };
  }

  if (!request.email.trim() || !emailPattern.test(request.email.trim())) {
    return {
      success: false,
      message: 'Valid email address is required.'
    };
  }

  const duplicateEmail = this.patients.some(item =>
    item.patientId !== patientId &&
    item.email?.toLowerCase() === request.email.trim().toLowerCase()
  );

  if (duplicateEmail) {
    return {
      success: false,
      message: 'A patient with this email already exists.'
    };
  }

  if (!request.phoneNumber.trim() || !phonePattern.test(request.phoneNumber.trim())) {
    return {
      success: false,
      message: 'Invalid phone number. Phone number must start with 6-9 and contain 10 digits.'
    };
  }

  const duplicatePhone = this.patients.some(item =>
    item.patientId !== patientId &&
    item.phoneNumber === request.phoneNumber.trim()
  );

  if (duplicatePhone) {
    return {
      success: false,
      message: 'A patient with this phone number already exists.'
    };
  }

  if (!request.dateOfBirth) {
    return {
      success: false,
      message: 'Date of birth is required.'
    };
  }

  if (new Date(request.dateOfBirth) > new Date()) {
    return {
      success: false,
      message: 'Date of birth cannot be in the future.'
    };
  }

  if (!request.gender) {
    return {
      success: false,
      message: 'Gender is required.'
    };
  }

  patient.fullName = request.fullName.trim();
  patient.email = request.email.trim();
  patient.phoneNumber = request.phoneNumber.trim();
  patient.gender = request.gender;
  patient.dateOfBirth = request.dateOfBirth;
  patient.insuranceId = request.insuranceId?.trim() ?? '';

  return {
    success: true,
    message: 'Profile updated successfully.',
    data: patient
  };
}
confirmAppointment(appointmentId: number, doctorId: number): OperationResult<AppointmentDto> {
  const appointment = this.appointments.find(item => item.appointmentId === appointmentId);

  if (!appointment) {
    return {
      success: false,
      message: 'Appointment not found.'
    };
  }

  if (appointment.doctorId !== doctorId) {
    return {
      success: false,
      message: 'You are not allowed to update another doctor appointment.'
    };
  }

  if (appointment.status !== 'Pending') {
    return {
      success: false,
      message: 'Only pending appointments can be confirmed.'
    };
  }

  appointment.status = 'Confirmed';

  return {
    success: true,
    message: 'Appointment confirmed successfully.',
    data: appointment
  };
}

getPatientHealthHistory(patientId: number): HealthRecordDto[] {
  return this.getHealthRecordsByPatient(patientId);
}

getHealthRecordByAppointmentId(appointmentId: number): HealthRecordDto | undefined {
  return this.healthRecords.find(record => record.appointmentId === appointmentId);
}

addHealthRecordForAppointment(
  request: AddHealthRecordRequest
): OperationResult<HealthRecordDto> {
  const appointment = this.appointments.find(
    item => item.appointmentId === request.appointmentId
  );

  if (!appointment) {
    return {
      success: false,
      message: 'Appointment not found.'
    };
  }

  if (appointment.doctorId !== request.doctorId) {
    return {
      success: false,
      message: 'You are not allowed to create a record for another doctor appointment.'
    };
  }

  if (appointment.status !== 'Confirmed') {
    return {
      success: false,
      message: 'Health record can be added only for confirmed appointments.'
    };
  }

  if (!request.diagnosis.trim()) {
    return {
      success: false,
      message: 'Diagnosis is required.'
    };
  }

  if (!request.prescription.trim()) {
    return {
      success: false,
      message: 'Prescription is required.'
    };
  }

  const existingRecord = this.getHealthRecordByAppointmentId(request.appointmentId);

  if (existingRecord) {
    return {
      success: false,
      message: 'Health record already exists for this appointment.'
    };
  }

  const patient = this.getPatientById(appointment.patientId);
  const doctor = this.getDoctorById(appointment.doctorId);

  const healthRecord: HealthRecordDto = {
    healthRecordId: this.getNextHealthRecordId(),
    patientId: appointment.patientId,
    patientName: patient?.fullName,
    doctorId: appointment.doctorId,
    doctorName: doctor?.fullName,
    appointmentId: appointment.appointmentId,
    visitDate: appointment.scheduledDate,
    diagnosis: request.diagnosis.trim(),
    prescription: request.prescription.trim(),
    notes: request.notes?.trim()
  };

  this.healthRecords.push(healthRecord);

  appointment.status = 'Completed';

  return {
    success: true,
    message: 'Health record added successfully. Appointment marked as completed.',
    data: healthRecord
  };
}

private getNextHealthRecordId(): number {
  return Math.max(...this.healthRecords.map(record => record.healthRecordId), 0) + 1;
}
}