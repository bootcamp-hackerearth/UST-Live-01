import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-appointment.html'
})
export class BookAppointmentComponent {

  
  ngOnInit() {
    this.form.get('doctorId')?.valueChanges.subscribe(value => {

    if (value) {
      this.loadSlots();
    }
    });

  }

  @Output() close = new EventEmitter<void>();

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

 loadDoctors() {

  const spec = this.form.value.specialization;
  const date = this.form.value.scheduledDate;

  if (!spec || !date) return;

  const formatted = new Date(date).toISOString().split('T')[0];

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

  const formatted = new Date(date).toISOString().split('T')[0];

  console.log('Calling API with:', doctorId, formatted); 

  this.appointmentService
    .getAvailableSlots(doctorId, formatted)
    .subscribe(res => {

      console.log('Slots response:', res); 

      this.slots = res;

      // 
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

      this.showSuccessPopup = true;

      setTimeout(() => {
      this.close.emit(); 
      }, 2000);
    },

      error: () => {
        this.isLoading = false;
        alert('Booking failed');
      }
    });
}
}