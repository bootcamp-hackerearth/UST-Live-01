import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup,ReactiveFormsModule,Validators} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-appointment.html'
})
export class BookAppointmentComponent implements OnInit,OnDestroy {

  private readonly destroy$ = new Subject<void>();
  ngOnInit() {
    this.form.get('doctorId')?.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(value => {

    if (value) {
      this.loadSlots();
    }
    });

  }

  @Output() appointmentClosed = new EventEmitter<void>();
  form: FormGroup;
  doctors: any[] = [];
  slots: string[] = [];
  showSuccessPopup = false;
  isLoading = false;

  specializations = [
    'Cardiology',
    'Surgery',
    'Dentist',
    'Dermatology',
    'Neurologist',
    'Orthopedic',
    'Dermatologist',
    'General Medicine'
  ];

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private doctorService: DoctorService,
    private appointmentService: AppointmentService
  ) {
    this.form = this.fb.group({
      scheduledDate: ['', Validators.required],
      specialization: ['', Validators.required],
      doctorId: ['', Validators.required],
      timeSlot: ['', Validators.required]
    });

    
  this.form.get('doctorId')?.disable();
  this.form.get('timeSlot')?.disable();

  }
  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

 loadDoctors() {

  const spec = this.form.value.specialization;
  const date = this.form.value.scheduledDate;

  if (!spec || !date) return;

  const formatted = this.formatDateForApi(date);

  this.doctorService
    .getAvailableDoctors(spec, formatted)
    .subscribe(res => {

      this.doctors = res;

      //  enable doctor select
      this.form.get('doctorId')?.enable();

     
      this.slots = [];
      this.form.get('timeSlot')?.disable();
    });
}

loadSlots() {

  const doctorId = Number(this.form.get('doctorId')?.value);
  const date = this.form.get('scheduledDate')?.value;

  if (!doctorId || !date) return;

  const formatted = this.formatDateForApi(date);
  const today = this.formatDateForApi(new Date());
  const currentMinutes = this.getCurrentTimeInMinutes();

  this.appointmentService
    .getAvailableSlots(doctorId, formatted)
    .subscribe(res => {

      // Filter only for today so past slots are hidden
      if (formatted === today) {
        this.slots = res.filter(slot => this.parseSlotToMinutes(slot) >= currentMinutes);
      } else {
        this.slots = res;
      }

      this.form.get('timeSlot')?.enable();

    });
}

 bookAppointment() {

  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }

  this.isLoading = true;
  const data = this.form.getRawValue();

  this.appointmentService.bookAppointment(data)
    .subscribe({
      next: () => {
    this.isLoading = false;
    this.toastr.success('Appointment booked successfully','Success');
     setTimeout(() => {
     this.appointmentClosed.emit(); 
      }, 2000);
    },

      error: () => {
        this.isLoading = false;
        this.toastr.error('Unable to book appointment','Error');
      }
    });
}

private formatDateForApi(value: string | Date): string {
  const date = value instanceof Date ? value : new Date(value);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');

  return `${year}-${month}-${day}`;
}

private parseSlotToMinutes(slot: string): number {
  const normalized = slot.trim().toUpperCase();

  const regex = /^(\d{1,2})(?::(\d{2}))?\s*(AM|PM)?$/;
  const match = regex.exec(normalized);

  if (!match) {
    return Number.MAX_SAFE_INTEGER;
  }

  let hours = Number(match[1]);
  const minutes = Number(match[2] ?? '0');
  const suffix = match[3];

  if (suffix === 'PM' && hours < 12) {
    hours += 12;
  } else if (suffix === 'AM' && hours === 12) {
    hours = 0;
  }

  return hours * 60 + minutes;
}

private getCurrentTimeInMinutes(): number {
  const now = new Date();
  return now.getHours() * 60 + now.getMinutes();
}
}