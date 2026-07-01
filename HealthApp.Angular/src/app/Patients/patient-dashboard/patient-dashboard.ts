import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Sidebar } from '../shared/sidebar/sidebar';
import { AuthService } from '../../service/auth.service';



import { PatientService } from '../../Patient.service/patientservice';
import { AppointmentService } from '../../Patient.service/appointmentservice';
import { HealthRecordService } from '../../Patient.service/health-recordservice';

import { Patient } from '../../models/patient/patient.model';
import { Appointment } from '../../models/appointment/appointment.model';
import { HealthRecord } from '../../models/health-record/health-record.model';

@Component({
  selector: 'app-patient-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, Sidebar],
  templateUrl: './patient-dashboard.html',
  styleUrls: ['./patient-dashboard.css']
})
export class PatientDashboard implements OnInit {

  patient: Patient | null = null;

  appointments: Appointment[] = [];
  records: HealthRecord[] = [];

  upcomingCount = 0;
  pendingCount = 0;
  showProfile = false;

  editMode = false;
  filteredAppointments: Appointment[] = [];
  paginatedAppointments: Appointment[] = [];
  pageNumber = 1;
  pageSize = 5;
  totalPages = 0;

  constructor(
    private patientService: PatientService,
    private appointmentService: AppointmentService,
    private recordService: HealthRecordService,
      private authService: AuthService 
  ) {}

  ngOnInit(): void {
    this.loadProfile();
  }

openProfile() {
  this.showProfile = true;
}

closeProfile() {
  this.showProfile = false;
}

  // LOAD PROFILE + RELATED DATA
  loadProfile() {
    this.patientService.getMyProfile().subscribe({
      next: (res) => {
        this.patient = res;

        this.loadAppointments();
        this.loadRecords();
      },
      error: (err) => {
        console.error('Profile load failed', err);
      }
    });
  }

  // APPOINTMENTS
loadAppointments() {
  this.appointmentService.getMyAppointments().subscribe({
    next: (res) => {

      const today = new Date();
      today.setHours(0, 0, 0, 0);

      const data = (res || [])
        .map((a: any) => ({
          ...a,
          scheduledDate: a.scheduledDate ? new Date(a.scheduledDate) : null
        }))
        .filter((a: any) => {

          const isUpcoming = a.scheduledDate && a.scheduledDate >= today;

          const isValidStatus =
            a.status === 'Pending' || a.status === 'Confirmed';

          return isUpcoming && isValidStatus;
        })
        .sort((a, b) =>
          new Date(a.scheduledDate!).getTime() -
          new Date(b.scheduledDate!).getTime()
        );

      this.appointments = data;
      this.filteredAppointments = data; 

      this.upcomingCount = data.length;
      this.pendingCount = data.filter(a => a.status === 'Pending').length;

      this.pageNumber = 1;

      this.updatePagination();
    },

    error: (err) => {
      console.error('Appointments load failed', err);
    }
  });
}



  // RECORDS
  loadRecords() {
    this.recordService.getMyRecords().subscribe({
      next: (res) => {
        this.records = res || [];
      },
      error: (err) => {
        console.error('Records load failed', err);
      }
    });
  }

  // CANCEL APPOINTMENT
  cancel(id: number) {
    const reason = prompt('Enter cancel reason');
    if (!reason) return;

    this.appointmentService.cancelAppointment(id, reason).subscribe({
      next: () => {
        this.loadAppointments();
      },
      error: (err) => {
        console.error('Cancel failed', err);
      }
    });
  }

  // EDIT PROFILE
  toggleEdit() {
    this.editMode = !this.editMode;
  }

  update() {
    if (!this.patient) return;

    this.patientService.updateMyProfile(this.patient).subscribe({
      next: () => {
        this.editMode = false;
      },
      error: (err) => {
        console.error('Update failed', err);
      }
    });
  }

  updatePagination() {
  this.totalPages = Math.ceil(this.filteredAppointments.length / this.pageSize);
  this.paginate();
}

paginate() {
  const start = (this.pageNumber - 1) * this.pageSize;
  const end = start + this.pageSize;

  this.paginatedAppointments = this.filteredAppointments.slice(start, end);
}

nextPage() {
  if (this.pageNumber < this.totalPages) {
    this.pageNumber++;
    this.paginate();
  }
}

prevPage() {
  if (this.pageNumber > 1) {
    this.pageNumber--;
    this.paginate();
  }
}

passwordForm = {
  currentPassword: '',
  newPassword: '',
  confirmPassword: ''
};
changePassword() {
  this.authService.changePassword(this.passwordForm).subscribe({
    next: (res: any) => {
      alert(res.message);
      
      this.passwordForm = {
        currentPassword: '',
        newPassword: '',
        confirmPassword: ''
      };
    },
    error: (err) => {
      alert(err.error);
    }
  });
}


}
