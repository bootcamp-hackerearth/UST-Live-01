import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth, MockUser } from '../../../services/mock-auth';
import {
  AppointmentDto,
  HealthRecordDto,
  MockPatientData,
  PatientDto
} from '../../../services/mock-patient-data';

@Component({
  selector: 'app-doctor-appointments',
  imports: [FormsModule],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css',
})
export class DoctorAppointments implements OnInit {
  private auth = inject(MockAuth);
  private patientData = inject(MockPatientData);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  currentUser?: MockUser;
  appointments: AppointmentDto[] = [];

  selectedPatient?: PatientDto;
  selectedPatientHistory: HealthRecordDto[] = [];

  selectedCancelAppointment?: AppointmentDto;
  cancellationReason = '';
  selectedCancelledAppointment?: AppointmentDto;

  selectedRecordAppointment?: AppointmentDto;
  diagnosis = '';
  prescription = '';
  notes = '';

  selectedViewRecord?: HealthRecordDto;
  selectedViewRecordPatient?: PatientDto;
  

  message = '';
  isError = false;
  searchText = '';
  statusFilter = 'All';
  dateFilter = 'All';

  ngOnInit(): void {
    const user = this.auth.getCurrentUser();

    if (!user || user.role !== 'Doctor') {
      this.router.navigate(['/login']);
      return;
    }

    this.currentUser = user;
    this.loadAppointments();

    const status = this.route.snapshot.queryParamMap.get('status');

    if (
      status === 'Pending' ||
      status === 'Confirmed' ||
      status === 'Completed' ||
      status === 'Cancelled'
    ) {
      this.statusFilter = status;
    }
  }

  loadAppointments(): void {
    if (!this.currentUser?.doctorId) {
      this.appointments = [];
      return;
    }

    this.appointments = this.patientData.getAppointmentsByDoctor(this.currentUser.doctorId);
  }

  openPatientModal(appointment: AppointmentDto): void {
    const patient = this.patientData.getPatientById(appointment.patientId);

    if (!patient) {
      this.showError('Patient details were not found.');
      return;
    }

    this.selectedPatient = patient;
    this.selectedPatientHistory = this.patientData.getPatientHealthHistory(patient.patientId);
  }

  closePatientModal(): void {
    this.selectedPatient = undefined;
    this.selectedPatientHistory = [];
  }

  confirmAppointment(appointment: AppointmentDto): void {
    if (!this.currentUser?.doctorId) {
      return;
    }

    const result = this.patientData.confirmAppointment(
      appointment.appointmentId,
      this.currentUser.doctorId
    );

    if (!result.success) {
      this.showError(result.message);
      return;
    }

    this.showSuccess(result.message);
    this.loadAppointments();
  }

  openCancelModal(appointment: AppointmentDto): void {
    this.selectedCancelAppointment = appointment;
    this.cancellationReason = '';
    this.clearMessage();
  }

  closeCancelModal(): void {
    this.selectedCancelAppointment = undefined;
    this.cancellationReason = '';
  }

  cancelAppointment(): void {
    if (!this.selectedCancelAppointment) {
      return;
    }

    const result = this.patientData.cancelAppointment({
      appointmentId: this.selectedCancelAppointment.appointmentId,
      reason: this.cancellationReason
    });

    if (!result.success) {
      this.showError(result.message);
      return;
    }

    this.showSuccess(result.message);
    this.closeCancelModal();
    this.loadAppointments();
  }
  get filteredAppointments(): AppointmentDto[] {
    let result = [...this.appointments];

    const search = this.searchText.trim().toLowerCase();

    if (search) {
      result = result.filter(appointment =>
        appointment.appointmentId.toString().includes(search) ||
        appointment.patientId.toString().includes(search) ||
        appointment.patientName?.toLowerCase().includes(search) ||
        appointment.timeSlot.toLowerCase().includes(search)
      );
    }

    if (this.statusFilter !== 'All') {
      result = result.filter(appointment => appointment.status === this.statusFilter);
    }

    const today = this.toDateOnly(new Date());

    if (this.dateFilter === 'Today') {
      result = result.filter(appointment => appointment.scheduledDate === today);
    }

    if (this.dateFilter === 'Upcoming') {
      result = result.filter(appointment => appointment.scheduledDate > today);
    }

    if (this.dateFilter === 'Past') {
      result = result.filter(appointment => appointment.scheduledDate < today);
    }

    return result;
  }
  
  openAddRecordPanel(appointment: AppointmentDto): void {
    this.selectedRecordAppointment = appointment;
    this.diagnosis = '';
    this.prescription = '';
    this.notes = '';
    this.clearMessage();
  }

  closeAddRecordPanel(): void {
    this.selectedRecordAppointment = undefined;
    this.diagnosis = '';
    this.prescription = '';
    this.notes = '';
  }

  addHealthRecord(): void {
    if (!this.selectedRecordAppointment || !this.currentUser?.doctorId) {
      return;
    }

    const result = this.patientData.addHealthRecordForAppointment({
      appointmentId: this.selectedRecordAppointment.appointmentId,
      doctorId: this.currentUser.doctorId,
      diagnosis: this.diagnosis,
      prescription: this.prescription,
      notes: this.notes
    });

    if (!result.success) {
      this.showError(result.message);
      return;
    }

    this.showSuccess(result.message);
    this.closeAddRecordPanel();
    this.loadAppointments();
  }

  openRecordModal(appointment: AppointmentDto): void {
    const record = this.patientData.getHealthRecordByAppointmentId(appointment.appointmentId);

    if (!record) {
      this.showError('No health record found for this completed appointment.');
      return;
    }

    this.selectedViewRecord = record;
    this.selectedViewRecordPatient = this.patientData.getPatientById(record.patientId);
  }

  closeRecordModal(): void {
    this.selectedViewRecord = undefined;
    this.selectedViewRecordPatient = undefined;
  }

  openCancelledDetailsModal(appointment: AppointmentDto): void {
    this.selectedCancelledAppointment = appointment;
  }

  closeCancelledDetailsModal(): void {
    this.selectedCancelledAppointment = undefined;
  }

  formatDate(dateText: string): string {
    return new Date(dateText).toLocaleDateString('en-IN', {
      day: '2-digit',
      month: 'short',
      year: 'numeric'
    });
  }

  private toDateOnly(date: Date): string {
    return date.toISOString().split('T')[0];
  }
  
  private showSuccess(message: string): void {
    this.message = message;
    this.isError = false;
  }

  private showError(message: string): void {
    this.message = message;
    this.isError = true;
  }

  private clearMessage(): void {
    this.message = '';
    this.isError = false;
  }
  
}