import { CommonModule } from '@angular/common';
import { Component, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppointmentService } from '../../core/services/appointment.service';
import { HealthRecordService } from '../../core/services/health-record.service';
import {
  CreateHealthRecordRequest,
  HealthRecordListDto
} from '../../core/models/portal.models';

@Component({
  selector: 'app-doctor-appointments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-appointments.html',
  styleUrl: './doctor-appointments.css'
})
export class DoctorAppointments implements OnInit {

  showAddRecordDialog = signal(false);
  showViewRecordDialog = signal(false);

  selectedRecords = signal<HealthRecordListDto[]>([]);

  addRecordForm = signal<CreateHealthRecordRequest>({
    appointmentId: 0,
    diagnosis: '',
    prescription: '',
    notes: ''
  });
  constructor(
    public appointmentService: AppointmentService,
    private healthRecordService: HealthRecordService
  ) { }

  ngOnInit(): void {
    this.appointmentService.loadDoctorAppointments();
  }

 

  updateStatus(appointmentId: number, status: string): void {
    this.appointmentService.updateAppointmentStatus(appointmentId, status)
      .subscribe({
        next: () => {
          alert('Appointment status updated');
          this.appointmentService.loadDoctorAppointments();
        },
        error: () => {
          alert('Failed to update appointment status');
        }
      });
  }

  openAddRecordDialog(appointmentId: number): void {
    this.addRecordForm.set({
      appointmentId,
      diagnosis: '',
      prescription: '',
      notes: ''
    });

    this.showAddRecordDialog.set(true);
  }
  closeAddRecordDialog(): void {
    this.showAddRecordDialog.set(false);

    this.addRecordForm.set({
      appointmentId: 0,
      diagnosis: '',
      prescription: '',
      notes: ''
    });
  }

  updateAddRecordForm<K extends keyof CreateHealthRecordRequest>(
    field: K,
    value: CreateHealthRecordRequest[K]
  ): void {
    this.addRecordForm.update(form => ({
      ...form,
      [field]: value
    }));
  }

  saveHealthRecord(): void {
    const form = this.addRecordForm();

    if (
      !form.diagnosis.trim() ||
      !form.prescription.trim()
    ) {
      alert('Please enter diagnosis and prescription');
      return;
    }

    this.healthRecordService.createRecord(form)
      .subscribe({
        next: () => {
          alert('Health record added successfully');

          this.closeAddRecordDialog();
          this.appointmentService.loadDoctorAppointments();
        },
        error: (error) => {
          alert(error?.error?.message || 'Failed to add health record');
        }
      });
  }

  viewRecord(patientId: number): void {
    if (!patientId) {
      alert('Invalid patient ID');
      return;
    }

    this.selectedRecords.set([]);

    this.healthRecordService.getRecordsByPatient(patientId)
      .subscribe({
        next: (res) => {
          this.selectedRecords.set(res || []);
          this.showViewRecordDialog.set(true);
        },
        error: () => {
          this.selectedRecords.set([]);
          this.showViewRecordDialog.set(true);
        }
      });
  }

  closeViewRecordDialog(): void {
    this.showViewRecordDialog.set(false);
    this.selectedRecords.set([]);
  }
}
