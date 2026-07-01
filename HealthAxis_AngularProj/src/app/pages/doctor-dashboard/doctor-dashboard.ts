import { Component } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor.service';

import { ToastService } from '../../shared/toast/toast.service';
import { ConfirmService } from '../../shared/confirm/confirm.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './doctor-dashboard.html',
  styleUrls: ['./doctor-dashboard.css']
})
export class DoctorDashboardComponent {

  activeTab = 'overview';

  sidebarCollapsed = false;

  doctor: any;
  appointments: any[] = [];

  selectedAppointment: any = null;
  selectedPatient: any = null;
  patientRecords: any[] = [];

  actionError = '';
  actionSuccess = '';

  recordError = '';
  recordSuccess = '';

  // ================= PAGINATION =================

  appointmentPageNumber = 1;
  appointmentPageSize = 6;
  appointmentTotalPages = 1;
  appointmentTotalCount = 0;

  recordPageNumber = 1;
  recordPageSize = 5;
  recordTotalPages = 1;
  recordTotalCount = 0;

  // Optional filters if backend supports them
  appointmentSearch = '';
  appointmentStatus = '';

  healthRecordForm = {
    patientId: 0,
    doctorId: 0,
    appointmentId: 0,
    visitDate: '',
    diagnosis: '',
    prescription: '',
    notes: ''
  };

  statusMap: Record<number, string> = {
    0: 'Pending',
    1: 'Confirmed',
    2: 'Cancelled',
    3: 'Completed'
  };

  specializationMap: Record<number, string> = {
    0: 'Endocrinologist',
    1: 'Oncologist',
    2: 'Gynecologist',
    3: 'Orthopedic Surgeon',
    4: 'Psychiatrist',
    5: 'Pediatrician',
    6: 'Neurologist',
    7: 'Dermatologist',
    8: 'Cardiologist',
    9: 'General Practitioner'
  };

  constructor(
    private auth: AuthService,
    private doctorService: DoctorService,
    private router: Router,
    private toast: ToastService,
    private confirm: ConfirmService
  ) {}

  ngOnInit() {
    const doctorId = this.auth.getReferenceId();

    if (!doctorId) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadDoctor(doctorId);
    this.loadAppointments(doctorId);
  }

  toggleSidebar() {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  switchTab(tab: string) {
    this.activeTab = tab;
    this.clearMessages();
  }

  // ================= RESPONSE HELPERS =================

  private extractItems(res: any): any[] {
    if (Array.isArray(res)) {
      return res;
    }

    return res?.items
      || res?.data
      || res?.records
      || res?.result
      || res?.results
      || [];
  }

  private extractPageNumber(res: any, fallback: number): number {
    return res?.pageNumber
      || res?.currentPage
      || res?.page
      || fallback;
  }

  private extractPageSize(res: any, fallback: number): number {
    return res?.pageSize || fallback;
  }

  private extractTotalCount(res: any, fallbackItemsLength: number): number {
    return res?.totalCount
      || res?.totalRecords
      || res?.count
      || fallbackItemsLength;
  }

  private extractTotalPages(res: any, totalCount: number, pageSize: number): number {
    return res?.totalPages
      || Math.ceil(totalCount / pageSize)
      || 1;
  }

  // ================= STATUS HELPERS =================

  getAppointmentStatus(appointment: any): string {
    return this.statusMap[appointment?.status] || 'Unknown';
  }

  canConfirmAppointment(appointment: any): boolean {
    return this.getAppointmentStatus(appointment) === 'Pending';
  }

  canCompleteAppointment(appointment: any): boolean {
    return this.getAppointmentStatus(appointment) === 'Confirmed';
  }

  canCancelAppointment(appointment: any): boolean {
    const status = this.getAppointmentStatus(appointment);

    return status !== 'Cancelled' && status !== 'Completed';
  }

  canCreateHealthRecord(): boolean {
    return !!this.selectedAppointment &&
      this.getAppointmentStatus(this.selectedAppointment) === 'Completed';
  }

  // ================= COUNTS =================

  get pendingCount(): number {
    return this.appointments.filter(
      a => this.statusMap[a.status] === 'Pending'
    ).length;
  }

  get confirmedCount(): number {
    return this.appointments.filter(
      a => this.statusMap[a.status] === 'Confirmed'
    ).length;
  }

  get completedCount(): number {
    return this.appointments.filter(
      a => this.statusMap[a.status] === 'Completed'
    ).length;
  }

  get cancelledCount(): number {
    return this.appointments.filter(
      a => this.statusMap[a.status] === 'Cancelled'
    ).length;
  }

  // ================= LOAD DATA =================

  loadDoctor(doctorId: string) {
    this.doctorService.getDoctor(doctorId).subscribe({
      next: (res) => {
        this.doctor = res;
      },
      error: (err) => {
        console.error('Doctor profile load error:', err);

        this.toast.error(
          'Unable to load profile',
          this.getErrorMessage(err)
        );
      }
    });
  }

  loadAppointments(doctorId: string) {
    this.doctorService
      .getDoctorAppointments(
        doctorId,
        this.appointmentPageNumber,
        this.appointmentPageSize,
        this.appointmentSearch,
        this.appointmentStatus
      )
      .subscribe({
        next: (res: any) => {
          this.appointments = this.extractItems(res);

          this.appointmentPageNumber = this.extractPageNumber(res, this.appointmentPageNumber);
          this.appointmentPageSize = this.extractPageSize(res, this.appointmentPageSize);
          this.appointmentTotalCount = this.extractTotalCount(res, this.appointments.length);
          this.appointmentTotalPages = this.extractTotalPages(
            res,
            this.appointmentTotalCount,
            this.appointmentPageSize
          );

          if (this.selectedAppointment?.appointmentId) {
            const updatedSelectedAppointment = this.appointments.find(
              a => Number(a.appointmentId) === Number(this.selectedAppointment.appointmentId)
            );

            if (updatedSelectedAppointment) {
              this.selectedAppointment = updatedSelectedAppointment;
            }
          }
        },
        error: (err) => {
          if (err.status === 404) {
            this.appointments = [];
            this.appointmentTotalCount = 0;
            this.appointmentTotalPages = 1;
            return;
          }

          console.error('Doctor appointments load error:', err);

          this.toast.error(
            'Unable to load appointments',
            this.getErrorMessage(err)
          );
        }
      });
  }

  loadPatientRecords(patientId: number) {
    this.doctorService
      .getPatientHealthRecords(patientId, this.recordPageNumber, this.recordPageSize)
      .subscribe({
        next: (res: any) => {
          this.patientRecords = this.extractItems(res);

          this.recordPageNumber = this.extractPageNumber(res, this.recordPageNumber);
          this.recordPageSize = this.extractPageSize(res, this.recordPageSize);
          this.recordTotalCount = this.extractTotalCount(res, this.patientRecords.length);
          this.recordTotalPages = this.extractTotalPages(
            res,
            this.recordTotalCount,
            this.recordPageSize
          );
        },
        error: (err) => {
          if (err.status === 404) {
            this.patientRecords = [];
            this.recordTotalCount = 0;
            this.recordTotalPages = 1;
            return;
          }

          console.error('Patient records load error:', err);

          this.toast.error(
            'Unable to load records',
            this.getErrorMessage(err)
          );
        }
      });
  }

  // ================= PAGINATION METHODS =================

  goToAppointmentPage(page: number) {
    if (page < 1 || page > this.appointmentTotalPages) {
      return;
    }

    this.appointmentPageNumber = page;
    this.loadAppointments(this.auth.getReferenceId());
  }

  goToRecordPage(page: number) {
    if (!this.selectedAppointment?.patientId) {
      return;
    }

    if (page < 1 || page > this.recordTotalPages) {
      return;
    }

    this.recordPageNumber = page;
    this.loadPatientRecords(Number(this.selectedAppointment.patientId));
  }

  getPaginationPages(totalPages: number): number[] {
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  // ================= APPOINTMENT ACTIONS =================

  async confirmAppointment(appointmentId: number) {
    const confirmed = await this.confirm.confirm({
      title: 'Confirm appointment?',
      message: 'Are you sure you want to confirm this patient appointment?',
      confirmText: 'Confirm Appointment',
      cancelText: 'Cancel',
      type: 'success'
    });

    if (!confirmed) {
      return;
    }

    this.clearMessages();

    this.doctorService.confirmAppointment(appointmentId).subscribe({
      next: () => {
        this.actionSuccess = 'Appointment confirmed successfully.';

        this.toast.success(
          'Appointment confirmed',
          'The appointment has been confirmed successfully.'
        );

        this.loadAppointments(this.auth.getReferenceId());
      },
      error: (err) => {
        console.error('Confirm appointment error:', err);

        this.actionError = this.getErrorMessage(err);

        this.toast.error(
          'Confirmation failed',
          this.actionError
        );
      }
    });
  }

  async completeAppointment(appointmentId: number) {
    const confirmed = await this.confirm.confirm({
      title: 'Complete appointment?',
      message: 'Mark this appointment as completed? Health record can be created after completion.',
      confirmText: 'Complete Appointment',
      cancelText: 'Cancel',
      type: 'success'
    });

    if (!confirmed) {
      return;
    }

    this.clearMessages();

    this.doctorService.completeAppointment(appointmentId).subscribe({
      next: () => {
        this.actionSuccess = 'Appointment completed successfully.';

        this.toast.success(
          'Appointment completed',
          'You can now add a health record for this appointment.'
        );

        if (this.selectedAppointment?.appointmentId === appointmentId) {
          this.selectedAppointment = {
            ...this.selectedAppointment,
            status: 3
          };
        }

        this.loadAppointments(this.auth.getReferenceId());
      },
      error: (err) => {
        console.error('Complete appointment error:', err);

        this.actionError = this.getErrorMessage(err);

        this.toast.error(
          'Completion failed',
          this.actionError
        );
      }
    });
  }

  async cancelAppointment(appointmentId: number) {
    const confirmed = await this.confirm.confirm({
      title: 'Cancel appointment?',
      message: 'Are you sure you want to cancel this appointment?',
      confirmText: 'Cancel Appointment',
      cancelText: 'Keep Appointment',
      type: 'danger'
    });

    if (!confirmed) {
      return;
    }

    this.clearMessages();

    const reason = 'Cancelled by doctor';

    this.doctorService.cancelAppointment(appointmentId, reason).subscribe({
      next: () => {
        this.actionSuccess = 'Appointment cancelled successfully.';

        this.toast.success(
          'Appointment cancelled',
          'The appointment has been cancelled successfully.'
        );

        if (this.selectedAppointment?.appointmentId === appointmentId) {
          this.selectedAppointment = {
            ...this.selectedAppointment,
            status: 2
          };
        }

        this.loadAppointments(this.auth.getReferenceId());
      },
      error: (err) => {
        console.error('Cancel appointment error:', err);

        this.actionError = this.getErrorMessage(err);

        this.toast.error(
          'Cancellation failed',
          this.actionError
        );
      }
    });
  }

  // ================= PATIENT DETAILS =================

  viewPatientDetails(appointment: any) {
    this.clearMessages();

    this.selectedAppointment = appointment;
    this.activeTab = 'patient';

    const patientId = appointment.patientId;

    if (!patientId) {
      this.recordError = 'Patient ID not found for this appointment.';

      this.toast.error(
        'Patient not found',
        this.recordError
      );

      return;
    }

    this.doctorService.getPatient(patientId).subscribe({
      next: (res) => {
        this.selectedPatient = res;
      },
      error: (err) => {
        console.error('Patient load error:', err);

        this.recordError = this.getErrorMessage(err);

        this.toast.error(
          'Unable to load patient',
          this.recordError
        );
      }
    });

    this.recordPageNumber = 1;
    this.loadPatientRecords(patientId);

    this.healthRecordForm = {
      patientId: Number(patientId),
      doctorId: Number(this.auth.getReferenceId()),
      appointmentId: Number(appointment.appointmentId),
      visitDate: new Date().toISOString().slice(0, 16),
      diagnosis: '',
      prescription: '',
      notes: ''
    };
  }

  // ================= HEALTH RECORD =================

  validateHealthRecord(): boolean {
    this.recordError = '';
    this.recordSuccess = '';

    if (!this.canCreateHealthRecord()) {
      this.recordError = 'Health record can be added only after the appointment is completed.';
      return false;
    }

    if (!this.healthRecordForm.patientId) {
      this.recordError = 'Patient ID is required.';
      return false;
    }

    if (!this.healthRecordForm.appointmentId) {
      this.recordError = 'Appointment ID is required.';
      return false;
    }

    if (!this.healthRecordForm.visitDate) {
      this.recordError = 'Visit date is required.';
      return false;
    }

    if (!this.healthRecordForm.diagnosis || this.healthRecordForm.diagnosis.trim().length < 3) {
      this.recordError = 'Diagnosis must be at least 3 characters.';
      return false;
    }

    if (!this.healthRecordForm.prescription || this.healthRecordForm.prescription.trim().length < 3) {
      this.recordError = 'Prescription must be at least 3 characters.';
      return false;
    }

    return true;
  }

  async createHealthRecord() {
    if (!this.canCreateHealthRecord()) {
      this.recordError = 'Health record can be added only after the appointment is completed.';

      this.toast.warning(
        'Appointment not completed',
        this.recordError
      );

      return;
    }

    if (!this.validateHealthRecord()) {
      this.toast.warning(
        'Health record validation failed',
        this.recordError
      );

      return;
    }

    const confirmed = await this.confirm.confirm({
      title: 'Save health record?',
      message: 'Are you sure you want to save this health record?',
      confirmText: 'Save Record',
      cancelText: 'Review',
      type: 'success'
    });

    if (!confirmed) {
      return;
    }

    const payload = {
      patientId: Number(this.healthRecordForm.patientId),
      doctorId: Number(this.healthRecordForm.doctorId),
      appointmentId: Number(this.healthRecordForm.appointmentId),
      visitDate: this.healthRecordForm.visitDate,
      diagnosis: this.healthRecordForm.diagnosis.trim(),
      prescription: this.healthRecordForm.prescription.trim(),
      notes: this.healthRecordForm.notes?.trim() || null
    };

    this.doctorService.createHealthRecord(payload).subscribe({
      next: () => {
        this.recordSuccess = 'Health record created successfully.';
        this.recordError = '';

        this.toast.success(
          'Health record saved',
          'The patient health record was created successfully.'
        );

        this.healthRecordForm.diagnosis = '';
        this.healthRecordForm.prescription = '';
        this.healthRecordForm.notes = '';

        this.recordPageNumber = 1;
        this.loadPatientRecords(payload.patientId);
      },
      error: (err) => {
        console.error('Create health record error:', err);

        this.recordError = this.getErrorMessage(err);

        this.toast.error(
          'Record creation failed',
          this.recordError
        );
      }
    });
  }

  // ================= SHARED =================

  clearMessages() {
    this.actionError = '';
    this.actionSuccess = '';
    this.recordError = '';
    this.recordSuccess = '';
  }

  logout() {
    this.auth.logout();

    this.toast.info(
      'Logged out',
      'You have been signed out successfully.'
    );

    this.router.navigate(['/login']);
  }

  private getErrorMessage(err: any): string {
    if (typeof err.error === 'string') {
      return err.error;
    }

    if (err.error?.message) {
      return err.error.message;
    }

    if (err.error?.errors) {
      return JSON.stringify(err.error.errors);
    }

    return 'Something went wrong. Please try again.';
  }
}