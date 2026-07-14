import { computed, Injectable, signal } from '@angular/core';
import { forkJoin, Observable, tap } from 'rxjs';

import {
  Appointment,
  UpdateAppointmentStatusRequest
} from '../models/appointment.model';
import { DoctorLeave } from '../models/doctor-leave.model';
import {
  AddHealthRecordRequest,
  HealthRecord
} from '../models/health-record.model';
import { Patient } from '../models/patient.model';
import { Doctor } from '../models/doctor.model';

import { AppointmentApiService } from './appointment-api.service';
import { AuthService } from './auth.service';
import { DoctorApiService } from './doctor-api.service';
import { DoctorLeaveApiService } from './doctor-leave-api.service';
import { HealthRecordApiService } from './health-record-api.service';
import { PatientApiService } from './patient-api.service';

@Injectable({
  providedIn: 'root'
})
export class DoctorPortalStateService {
  readonly doctorProfile = signal<Doctor | null>(null);
  readonly appointments = signal<Appointment[]>([]);
  readonly doctorLeaves = signal<DoctorLeave[]>([]);

  readonly selectedPatient = signal<Patient | null>(null);
  readonly selectedPatientHistory = signal<HealthRecord[]>([]);
  readonly selectedRecord = signal<HealthRecord | null>(null);

  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  readonly todayAppointments = computed(() => {
    const today = this.toDateOnly(new Date());

    return this.appointments()
      .filter(
        appointment =>
          this.toDateOnly(appointment.scheduledDate) === today
      )
      .sort((a, b) => a.timeSlot.localeCompare(b.timeSlot));
  });

  readonly todayCount = computed(
    () => this.todayAppointments().length
  );

  readonly pendingCount = computed(() =>
    this.appointments().filter(
      appointment => appointment.status === 'Pending'
    ).length
  );

  readonly confirmedCount = computed(() =>
    this.appointments().filter(
      appointment => appointment.status === 'Confirmed'
    ).length
  );

  readonly completedCount = computed(() =>
    this.appointments().filter(
      appointment => appointment.status === 'Completed'
    ).length
  );

  readonly upcomingLeave = computed<DoctorLeave | null>(() => {
    const today = this.toDateOnly(new Date());

    return [...this.doctorLeaves()]
      .filter(leave => this.toDateOnly(leave.endDate) >= today)
      .sort((a, b) =>
        this.toDateOnly(a.startDate).localeCompare(
          this.toDateOnly(b.startDate)
        )
      )[0] ?? null;
  });

  constructor(
    private readonly authService: AuthService,
    private readonly doctorApi: DoctorApiService,
    private readonly doctorLeaveApi: DoctorLeaveApiService,
    private readonly appointmentApi: AppointmentApiService,
    private readonly patientApi: PatientApiService,
    private readonly healthRecordApi: HealthRecordApiService
  ) {}

  loadDoctorPortal(): void {
    this.loadDoctorProfile();
    this.loadAppointments();
    this.loadDoctorLeaves();
  }

  loadDoctorProfile(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.doctorApi.getLoggedInDoctorProfile().subscribe({
      next: doctor => {
        this.doctorProfile.set(doctor);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
        this.isLoading.set(false);
      }
    });
  }

  loadAppointments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.appointmentApi.getAppointments().subscribe({
      next: appointments => {
        this.appointments.set(appointments);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
        this.isLoading.set(false);
      }
    });
  }

  loadDoctorLeaves(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.doctorLeaveApi.getMyLeaveHistory().subscribe({
      next: leaves => {
        this.doctorLeaves.set(leaves);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
        this.isLoading.set(false);
      }
    });
  }

  refreshAfterLeaveCreation(): void {
    this.loadDoctorLeaves();
    this.loadAppointments();
  }

  confirmAppointment(
    appointmentId: number
  ): Observable<Appointment> {
    return this.updateAppointmentStatus(appointmentId, {
      status: 'Confirmed'
    });
  }

  cancelAppointment(
    appointmentId: number,
    cancellationReason: string
  ): Observable<Appointment> {
    return this.updateAppointmentStatus(appointmentId, {
      status: 'Cancelled',
      cancellationReason
    });
  }

  addHealthRecord(
    request: AddHealthRecordRequest
  ): Observable<HealthRecord> {
    return this.healthRecordApi.addHealthRecord(request).pipe(
      tap(() => {
        this.loadAppointments();
      })
    );
  }

  loadPatientContext(patientId: number): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    forkJoin({
      patient: this.patientApi.getPatientById(patientId),
      history: this.healthRecordApi.getHealthRecordsByPatient(
        patientId
      )
    }).subscribe({
      next: result => {
        this.selectedPatient.set(result.patient);
        this.selectedPatientHistory.set(result.history);
        this.isLoading.set(false);
      },
      error: error => {
        this.errorMessage.set(
          this.authService.getErrorMessage(error)
        );
        this.isLoading.set(false);
      }
    });
  }

  closePatientContext(): void {
    this.selectedPatient.set(null);
    this.selectedPatientHistory.set([]);
  }

  loadRecordByAppointment(appointment: Appointment): void {
    this.healthRecordApi
      .getHealthRecordsByPatient(appointment.patientId)
      .subscribe({
        next: records => {
          const record =
            records.find(
              item =>
                item.appointmentId === appointment.appointmentId
            ) ?? null;

          this.selectedRecord.set(record);
        },
        error: error => {
          this.errorMessage.set(
            this.authService.getErrorMessage(error)
          );
        }
      });
  }

  closeRecord(): void {
    this.selectedRecord.set(null);
  }

  private updateAppointmentStatus(
    appointmentId: number,
    request: UpdateAppointmentStatusRequest
  ): Observable<Appointment> {
    return this.appointmentApi
      .updateAppointmentStatus(appointmentId, request)
      .pipe(
        tap(updatedAppointment => {
          this.appointments.update(appointments =>
            appointments.map(appointment =>
              appointment.appointmentId ===
              updatedAppointment.appointmentId
                ? updatedAppointment
                : appointment
            )
          );
        })
      );
  }

  private toDateOnly(value: string | Date): string {
    const date = new Date(value);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
