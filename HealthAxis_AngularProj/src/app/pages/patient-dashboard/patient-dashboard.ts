import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule, DatePipe } from '@angular/common';

import { AuthService } from '../../services/auth.service';
import { PatientService } from '../../services/patient.service';

import { ToastService } from '../../shared/toast/toast.service';
import { ConfirmService } from '../../shared/confirm/confirm.service';

@Component({
  standalone: true,
  imports: [FormsModule, CommonModule, DatePipe],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboardComponent {

  activeTab = 'overview';
  profileMode = 'view';

  sidebarCollapsed = false;

  patient: any;

  appointments: any[] = [];
  records: any[] = [];

  doctors: any[] = [];
  bookingDoctors: any[] = [];

  bookingError = '';
  bookingSuccess = '';
  bookingSubmitted = false;

  profileError = '';
  profileSuccess = '';

  passwordError = '';
  passwordSuccess = '';
  passwordSubmitted = false;

  todayDate: string = new Date().toISOString().split('T')[0];

  maxBookingDate: string = this.getDateAfterDays(30);

  timeSlots: string[] = [
    '09:00',
    '10:00',
    '11:00',
    '12:00',
    '14:00',
    '15:00',
    '16:00',
    '17:00'
  ];

  // ================= PAGINATION =================

  appointmentPageNumber = 1;
  appointmentPageSize = 6;
  appointmentTotalPages = 1;
  appointmentTotalCount = 0;

  recordPageNumber = 1;
  recordPageSize = 6;
  recordTotalPages = 1;
  recordTotalCount = 0;

  doctorPageNumber = 1;
  doctorPageSize = 6;
  doctorTotalPages = 1;
  doctorTotalCount = 0;

  // ================= FIND DOCTOR FILTERS =================

  doctorSearch = '';
  findDoctorSpecialisation = '';
  doctorStatus = '';

  // ================= BOOKING FILTER =================

  bookingSpecialisation = '';

  bookingData = {
    doctorId: '',
    scheduledDate: '',
    timeSlot: ''
  };

  editProfileData = {
    patientName: '',
    dateOfBirth: '',
    gender: 0,
    phoneNumber: '',
    email: ''
  };

  passwordData = {
    oldPassword: '',
    newPassword: '',
    confirmNewPassword: ''
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

  specializationOptions = [
    { value: 'Endocrinologist', label: 'Endocrinologist' },
    { value: 'Oncologist', label: 'Oncologist' },
    { value: 'Gynecologist', label: 'Gynecologist' },
    { value: 'Orthopedic Surgeon', label: 'Orthopedic Surgeon' },
    { value: 'Psychiatrist', label: 'Psychiatrist' },
    { value: 'Pediatrician', label: 'Pediatrician' },
    { value: 'Neurologist', label: 'Neurologist' },
    { value: 'Dermatologist', label: 'Dermatologist' },
    { value: 'Cardiologist', label: 'Cardiologist' },
    { value: 'General Practitioner', label: 'General Practitioner' }
  ];

  genderMap: Record<number, string> = {
    0: 'Male',
    1: 'Female',
    2: 'Transgender',
    3: 'Other'
  };

  constructor(
    private router: Router,
    private auth: AuthService,
    private patientService: PatientService,
    private toast: ToastService,
    private confirm: ConfirmService
  ) {}

  ngOnInit() {
    const patientId = this.auth.getReferenceId();

    if (!patientId) {
      this.router.navigate(['/login']);
      return;
    }

    this.loadPatient(patientId);
    this.loadAppointments(patientId);
    this.loadHealthRecords(patientId);
    this.loadDoctors();
    this.loadBookingDoctors();
  }

  toggleSidebar() {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  switchTab(tab: string) {
    this.activeTab = tab;
    this.profileMode = 'view';

    this.bookingError = '';
    this.bookingSuccess = '';
    this.profileError = '';
    this.profileSuccess = '';
    this.passwordError = '';
    this.passwordSuccess = '';

    if (tab === 'find-doctor') {
      this.doctorPageNumber = 1;
      this.refreshDoctorPagination();
    }
  }

  // ================= DATE HELPERS =================

  private getDateAfterDays(days: number): string {
    const date = new Date();
    date.setDate(date.getDate() + days);
    return date.toISOString().split('T')[0];
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

  // ================= DOCTOR HELPERS =================

  getDoctorSpecialisationValue(doctor: any): number | string {
    return doctor?.specialisation ?? doctor?.specialization ?? '';
  }

  getDoctorSpecialisationName(doctor: any): string {
    const value = this.getDoctorSpecialisationValue(doctor);

    if (typeof value === 'string' && isNaN(Number(value))) {
      return value || 'Not specified';
    }

    return this.specializationMap[Number(value)] || 'Not specified';
  }

  getDoctorIsActive(doctor: any): boolean {
    if (typeof doctor?.isActive === 'boolean') {
      return doctor.isActive;
    }

    const status = String(doctor?.status || '').toLowerCase();

    if (status === 'active') {
      return true;
    }

    if (status === 'inactive') {
      return false;
    }

    /**
     * If backend /Doctor/available returns only available doctors
     * and does not include isActive, treat returned doctors as active.
     */
    return true;
  }

  get filteredDoctorsAll(): any[] {
    let filtered = [...this.doctors];

    const search = this.doctorSearch.trim().toLowerCase();
    const selectedSpecialisation = this.findDoctorSpecialisation.trim().toLowerCase();
    const selectedStatus = this.doctorStatus.trim().toLowerCase();

    if (search) {
      filtered = filtered.filter((doctor: any) => {
        const name = String(doctor.doctorName || '').toLowerCase();
        const email = String(doctor.email || '').toLowerCase();
        const specialisation = this.getDoctorSpecialisationName(doctor).toLowerCase();

        return (
          name.includes(search) ||
          email.includes(search) ||
          specialisation.includes(search)
        );
      });
    }

    if (selectedSpecialisation) {
      filtered = filtered.filter((doctor: any) => {
        const doctorSpecialisation = this.getDoctorSpecialisationName(doctor)
          .trim()
          .toLowerCase();

        return doctorSpecialisation === selectedSpecialisation;
      });
    }

    if (selectedStatus) {
      filtered = filtered.filter((doctor: any) => {
        const isActive = this.getDoctorIsActive(doctor);

        if (selectedStatus === 'active') {
          return isActive === true;
        }

        if (selectedStatus === 'inactive') {
          return isActive === false;
        }

        return true;
      });
    }

    return filtered;
  }

  get filteredDoctorsForFind(): any[] {
    const filtered = this.filteredDoctorsAll;

    this.doctorTotalCount = filtered.length;
    this.doctorTotalPages = Math.max(
      1,
      Math.ceil(this.doctorTotalCount / this.doctorPageSize)
    );

    if (this.doctorPageNumber > this.doctorTotalPages) {
      this.doctorPageNumber = 1;
    }

    const start = (this.doctorPageNumber - 1) * this.doctorPageSize;
    const end = start + this.doctorPageSize;

    return filtered.slice(start, end);
  }

  get filteredBookingDoctors(): any[] {
    if (!this.bookingSpecialisation) {
      return this.bookingDoctors;
    }

    return this.bookingDoctors.filter((doctor: any) => {
      const doctorSpecialisation = this.getDoctorSpecialisationName(doctor)
        .trim()
        .toLowerCase();

      return doctorSpecialisation === this.bookingSpecialisation.trim().toLowerCase();
    });
  }

  refreshDoctorPagination() {
    this.doctorTotalCount = this.filteredDoctorsAll.length;
    this.doctorTotalPages = Math.max(
      1,
      Math.ceil(this.doctorTotalCount / this.doctorPageSize)
    );

    if (this.doctorPageNumber > this.doctorTotalPages) {
      this.doctorPageNumber = 1;
    }
  }

  // ================= LOAD DATA =================

  loadPatient(patientId: string) {
    this.patientService.getPatient(patientId).subscribe({
      next: (res: any) => {
        this.patient = res;

        this.editProfileData = {
          patientName: res.patientName || '',
          dateOfBirth: res.dateOfBirth ? res.dateOfBirth.split('T')[0] : '',
          gender: res.gender ?? 0,
          phoneNumber: res.phoneNumber || '',
          email: res.email || ''
        };
      },
      error: (err) => {
        console.error('Patient load error:', err);
        this.toast.error('Unable to load profile', this.getErrorMessage(err));
      }
    });
  }

  loadAppointments(patientId: string) {
    this.patientService
      .getAppointments(patientId, this.appointmentPageNumber, this.appointmentPageSize)
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
        },
        error: (err) => {
          if (err.status === 404) {
            this.appointments = [];
            this.appointmentTotalCount = 0;
            this.appointmentTotalPages = 1;
            return;
          }

          console.error('Appointments load error:', err);
          this.toast.error('Unable to load appointments', this.getErrorMessage(err));
        }
      });
  }

  loadHealthRecords(patientId: string) {
    this.patientService
      .getHealthRecords(patientId, this.recordPageNumber, this.recordPageSize)
      .subscribe({
        next: (res: any) => {
          this.records = this.extractItems(res);

          this.recordPageNumber = this.extractPageNumber(res, this.recordPageNumber);
          this.recordPageSize = this.extractPageSize(res, this.recordPageSize);
          this.recordTotalCount = this.extractTotalCount(res, this.records.length);
          this.recordTotalPages = this.extractTotalPages(
            res,
            this.recordTotalCount,
            this.recordPageSize
          );
        },
        error: (err) => {
          if (err.status === 404) {
            this.records = [];
            this.recordTotalCount = 0;
            this.recordTotalPages = 1;
            return;
          }

          console.error('Health records load error:', err);
          this.toast.error('Unable to load health records', this.getErrorMessage(err));
        }
      });
  }

  loadDoctors() {
    this.patientService
      .getDoctors(1, 1000)
      .subscribe({
        next: (res: any) => {
          this.doctors = this.extractItems(res);
          this.refreshDoctorPagination();
        },
        error: (err) => {
          console.error('Doctors load error:', err);
          this.toast.error('Unable to load doctors', this.getErrorMessage(err));
        }
      });
  }

  loadBookingDoctors() {
    this.patientService
      .getDoctors(1, 1000)
      .subscribe({
        next: (res: any) => {
          this.bookingDoctors = this.extractItems(res);
        },
        error: (err) => {
          console.error('Booking doctors load error:', err);
          this.toast.error('Unable to load doctors for booking', this.getErrorMessage(err));
        }
      });
  }

  // ================= FILTER METHODS =================

  onFindSpecialisationChange() {
    this.doctorPageNumber = 1;
    this.refreshDoctorPagination();
  }

  onDoctorSearchChange() {
    this.doctorPageNumber = 1;
    this.refreshDoctorPagination();
  }

  onDoctorStatusChange() {
    this.doctorPageNumber = 1;
    this.refreshDoctorPagination();
  }

  clearDoctorFilters() {
    this.doctorSearch = '';
    this.findDoctorSpecialisation = '';
    this.doctorStatus = '';
    this.doctorPageNumber = 1;
    this.refreshDoctorPagination();
  }

  onBookingSpecialisationChange() {
    this.bookingData.doctorId = '';
    this.bookingError = '';
    this.bookingSuccess = '';
  }

  // ================= PAGINATION =================

  goToAppointmentPage(page: number) {
    if (page < 1 || page > this.appointmentTotalPages) {
      return;
    }

    this.appointmentPageNumber = page;
    this.loadAppointments(this.auth.getReferenceId());
  }

  goToRecordPage(page: number) {
    if (page < 1 || page > this.recordTotalPages) {
      return;
    }

    this.recordPageNumber = page;
    this.loadHealthRecords(this.auth.getReferenceId());
  }

  goToDoctorPage(page: number) {
    if (page < 1 || page > this.doctorTotalPages) {
      return;
    }

    this.doctorPageNumber = page;
  }

  getPaginationPages(totalPages: number): number[] {
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  // ================= BOOKING =================

  selectDoctorForBooking(doctor: any) {
    this.bookingSpecialisation = this.getDoctorSpecialisationName(doctor);
    this.bookingData.doctorId = String(doctor.doctorId);

    const exists = this.bookingDoctors.some(
      d => String(d.doctorId) === String(doctor.doctorId)
    );

    if (!exists) {
      this.bookingDoctors = [doctor, ...this.bookingDoctors];
    }

    this.bookingError = '';
    this.bookingSuccess = '';
    this.activeTab = 'book';

    this.toast.info(
      'Doctor selected',
      `${doctor.doctorName} has been selected for appointment booking.`
    );
  }

  validateBooking(): boolean {
    this.bookingError = '';
    this.bookingSuccess = '';

    if (!this.bookingSpecialisation) {
      this.bookingError = 'Please select a specialisation.';
      return false;
    }

    if (!this.bookingData.doctorId) {
      this.bookingError = 'Please select a doctor.';
      return false;
    }

    const selectedDoctor = this.bookingDoctors.find(
      d => String(d.doctorId) === String(this.bookingData.doctorId)
    );

    if (!selectedDoctor) {
      this.bookingError = 'Selected doctor is not available.';
      return false;
    }

    const selectedDoctorSpecialisation = this.getDoctorSpecialisationName(selectedDoctor)
      .trim()
      .toLowerCase();

    if (selectedDoctorSpecialisation !== this.bookingSpecialisation.trim().toLowerCase()) {
      this.bookingError = 'Selected doctor does not belong to the selected specialisation.';
      return false;
    }

    if (!this.bookingData.scheduledDate) {
      this.bookingError = 'Please select an appointment date.';
      return false;
    }

    if (this.bookingData.scheduledDate < this.todayDate) {
      this.bookingError = 'Appointment date cannot be in the past.';
      return false;
    }

    if (this.bookingData.scheduledDate > this.maxBookingDate) {
      this.bookingError = 'Appointments can only be booked up to 30 days in advance.';
      return false;
    }

    if (!this.bookingData.timeSlot) {
      this.bookingError = 'Please select a time slot.';
      return false;
    }

    const selectedDateTime = new Date(
      `${this.bookingData.scheduledDate}T${this.bookingData.timeSlot}`
    );

    if (selectedDateTime <= new Date()) {
      this.bookingError = 'Previous date or past time slot cannot be booked.';
      return false;
    }

    return true;
  }

  async bookAppointment() {
    this.bookingSubmitted = true;

    if (!this.validateBooking()) {
      this.toast.warning('Booking validation failed', this.bookingError);
      return;
    }

    const selectedDoctor = this.bookingDoctors.find(
      d => String(d.doctorId) === String(this.bookingData.doctorId)
    );

    const confirmed = await this.confirm.confirm({
      title: 'Book appointment?',
      message: selectedDoctor
        ? `Confirm appointment with ${selectedDoctor.doctorName}?`
        : 'Please confirm that you want to book this appointment.',
      confirmText: 'Book Appointment',
      cancelText: 'Review Details',
      type: 'success'
    });

    if (!confirmed) {
      return;
    }

    const patientId = this.auth.getReferenceId();

    const payload = {
      patientId: Number(patientId),
      doctorId: Number(this.bookingData.doctorId),
      scheduledDate: this.bookingData.scheduledDate,
      timeSlot: this.bookingData.timeSlot
    };

    this.patientService.bookAppointment(payload).subscribe({
      next: () => {
        this.bookingSuccess = 'Appointment booked successfully.';
        this.bookingError = '';

        this.toast.success(
          'Appointment booked',
          'Your appointment request has been created successfully.'
        );

        this.bookingData = {
          doctorId: '',
          scheduledDate: '',
          timeSlot: ''
        };

        this.bookingSpecialisation = '';
        this.bookingSubmitted = false;

        this.appointmentPageNumber = 1;
        this.loadAppointments(patientId);

        setTimeout(() => {
          this.bookingSuccess = '';
          this.activeTab = 'appointments';
        }, 900);
      },
      error: (err) => {
        console.error('Booking error:', err);

        this.bookingSuccess = '';
        this.bookingError = this.getErrorMessage(err);

        /**
         * Backend appointment validation messages will show here.
         * Example:
         * - Doctor already booked
         * - Same patient already has appointment on this date
         * - Booking only allowed 30 days in advance
         */
        this.toast.error('Booking failed', this.bookingError);
      }
    });
  }

  async cancelAppointment(id: number) {
    if (!id) {
      return;
    }

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

    this.patientService.cancelAppointment(id).subscribe({
      next: () => {
        this.toast.success(
          'Appointment cancelled',
          'Your appointment was cancelled successfully.'
        );

        const patientId = this.auth.getReferenceId();
        this.loadAppointments(patientId);
      },
      error: (err) => {
        console.error('Cancel appointment error:', err);

        this.toast.error(
          'Cancellation failed',
          this.getErrorMessage(err)
        );
      }
    });
  }

  // ================= PROFILE =================

  editProfile() {
    this.profileMode = 'edit';
    this.profileError = '';
    this.profileSuccess = '';
  }

  cancelEditProfile() {
    this.profileMode = 'view';
    this.profileError = '';
    this.profileSuccess = '';

    if (this.patient) {
      this.editProfileData = {
        patientName: this.patient.patientName || '',
        dateOfBirth: this.patient.dateOfBirth ? this.patient.dateOfBirth.split('T')[0] : '',
        gender: this.patient.gender ?? 0,
        phoneNumber: this.patient.phoneNumber || '',
        email: this.patient.email || ''
      };
    }
  }

  validateProfile(): boolean {
    this.profileError = '';
    this.profileSuccess = '';

    if (!this.editProfileData.patientName || this.editProfileData.patientName.trim().length < 2) {
      this.profileError = 'Patient name must be at least 2 characters.';
      return false;
    }

    if (!this.editProfileData.dateOfBirth) {
      this.profileError = 'Date of birth is required.';
      return false;
    }

    if (!this.editProfileData.phoneNumber || !/^[0-9]{10}$/.test(this.editProfileData.phoneNumber)) {
      this.profileError = 'Enter a valid 10 digit phone number.';
      return false;
    }

    return true;
  }

  async saveProfile() {
    if (!this.validateProfile()) {
      this.toast.warning('Profile validation failed', this.profileError);
      return;
    }

    const confirmed = await this.confirm.confirm({
      title: 'Update profile?',
      message: 'Are you sure you want to save these profile changes?',
      confirmText: 'Save Changes',
      cancelText: 'Cancel',
      type: 'default'
    });

    if (!confirmed) {
      return;
    }

    const patientId = this.auth.getReferenceId();

    const payload = {
      patientName: this.editProfileData.patientName.trim(),
      dateOfBirth: this.editProfileData.dateOfBirth,
      gender: Number(this.editProfileData.gender),
      phoneNumber: this.editProfileData.phoneNumber,
      email: this.editProfileData.email
    };

    this.patientService.updatePatient(patientId, payload).subscribe({
      next: () => {
        this.profileSuccess = 'Profile updated successfully.';
        this.profileError = '';
        this.profileMode = 'view';

        this.toast.success(
          'Profile updated',
          'Your profile information has been saved successfully.'
        );

        this.loadPatient(patientId);
      },
      error: (err) => {
        console.error('Profile update error:', err);

        this.profileSuccess = '';
        this.profileError = this.getErrorMessage(err);

        this.toast.error('Profile update failed', this.profileError);
      }
    });
  }

  // ================= PASSWORD =================

  validatePasswordChange(): boolean {
    this.passwordError = '';
    this.passwordSuccess = '';

    if (!this.passwordData.oldPassword) {
      this.passwordError = 'Old password is required.';
      return false;
    }

    if (!this.passwordData.newPassword) {
      this.passwordError = 'New password is required.';
      return false;
    }

    if (this.passwordData.newPassword.length < 6) {
      this.passwordError = 'New password must be at least 6 characters.';
      return false;
    }

    if (!this.passwordData.confirmNewPassword) {
      this.passwordError = 'Confirm password is required.';
      return false;
    }

    if (this.passwordData.newPassword !== this.passwordData.confirmNewPassword) {
      this.passwordError = 'New password and confirm password do not match.';
      return false;
    }

    if (this.passwordData.oldPassword === this.passwordData.newPassword) {
      this.passwordError = 'New password cannot be same as old password.';
      return false;
    }

    return true;
  }

  async changePassword() {
    this.passwordSubmitted = true;

    if (!this.validatePasswordChange()) {
      this.toast.warning('Password validation failed', this.passwordError);
      return;
    }

    const confirmed = await this.confirm.confirm({
      title: 'Change password?',
      message: 'You will need to use the new password for your next login.',
      confirmText: 'Change Password',
      cancelText: 'Cancel',
      type: 'default'
    });

    if (!confirmed) {
      return;
    }

    const payload = {
      email: this.auth.getEmail(),
      oldPassword: this.passwordData.oldPassword,
      newPassword: this.passwordData.newPassword
    };

    this.auth.changePassword(payload).subscribe({
      next: () => {
        this.passwordSuccess = 'Password changed successfully.';
        this.passwordError = '';

        this.toast.success(
          'Password changed',
          'Your password has been updated successfully.'
        );

        this.passwordData = {
          oldPassword: '',
          newPassword: '',
          confirmNewPassword: ''
        };

        this.passwordSubmitted = false;
      },
      error: (err) => {
        console.error('Change password error:', err);

        this.passwordSuccess = '';
        this.passwordError = this.getErrorMessage(err);

        this.toast.error('Password change failed', this.passwordError);
      }
    });
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