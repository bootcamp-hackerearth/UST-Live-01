import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup,ReactiveFormsModule,Validators} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-appointment.html'
})
export class BookAppointmentComponent implements OnInit {

 ngOnInit(): void {

  this.minDate = this.formatDateForApi(new Date());

  this.form.get('specialization')?.valueChanges.subscribe(() => {
    this.loadDoctors();
  });

  this.form.get('doctorId')?.valueChanges.subscribe(value => {

    if (value) {
      this.loadSlots();
    }
  });
  }

  @Output() appointmentClosed = new EventEmitter<void>();

  form: FormGroup;

  doctors: any[] = [];
  slots: string[] = [];
  minDate: string = '';
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

  doctorId: [
    { value: '', disabled: true },
    Validators.required
  ],

  timeSlot: [
    { value: '', disabled: true },
    Validators.required
  ]
});

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

  this.form.patchValue({
    doctorId: '',
    timeSlot: ''
  });

  this.slots = [];

  if (res.length > 0) {
    this.form.get('doctorId')?.enable();
  }
  else {
    this.form.get('doctorId')?.disable();
  }

  this.form.get('timeSlot')?.disable();

});
}

loadSlots(): void {

  const doctorId = Number(this.form.get('doctorId')?.value);
  const date = this.form.get('scheduledDate')?.value;

  if (!doctorId || !date) {
    this.slots = [];
    this.form.get('timeSlot')?.disable();
    return;
  }

  const formatted = this.formatDateForApi(date);
  const today = this.formatDateForApi(new Date());
  const currentMinutes = this.getCurrentTimeInMinutes();

  // Clear previous selection
  this.form.patchValue({
    timeSlot: ''
  });

  this.form.get('timeSlot')?.disable();

  this.appointmentService
    .getAvailableSlots(doctorId, formatted)
    .subscribe({
      next: (res) => {

        if (formatted === today) {
          // Show only future time slots for today
          this.slots = res.filter(slot =>
            this.parseSlotToMinutes(slot) > currentMinutes
          );
        } else {
          // Show all slots for future dates
          this.slots = res;
        }

        if (this.slots.length > 0) {
          this.form.get('timeSlot')?.enable();
        } else {
          this.form.get('timeSlot')?.disable();
        }
      },

      error: () => {
        this.slots = [];
        this.form.get('timeSlot')?.disable();
      }
    });

}

 bookAppointment() {

  if (this.form.invalid || this.isLoading) {
    return;
  }

  this.isLoading = true;

  const data = this.form.getRawValue();

  this.appointmentService.bookAppointment(data).subscribe({
    next: () => {

      this.toastr.success('Appointment booked successfully', 'Success');

      this.showSuccessPopup = true;

      setTimeout(() => {
        this.showSuccessPopup = false;
        this.appointmentClosed.emit();
      }, 2000);
    },
    error: () => {
      this.toastr.error('Unable to book appointment', 'Error');
      this.isLoading = false;
    },
    complete: () => {
      this.isLoading = false;
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