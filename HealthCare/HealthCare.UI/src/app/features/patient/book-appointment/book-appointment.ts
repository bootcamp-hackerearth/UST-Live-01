import {Component,EventEmitter,Output,OnInit,signal,inject,DestroyRef} from '@angular/core';
import { CommonModule } from '@angular/common';

import {FormBuilder,FormGroup,ReactiveFormsModule,Validators} from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { DoctorService } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';

import {combineLatest,of,startWith,switchMap} from 'rxjs';

import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-appointment.html'
})
export class BookAppointmentComponent implements OnInit {

  private readonly destroyRef = inject(DestroyRef);

  @Output() appointmentClosed = new EventEmitter<void>();

  form: FormGroup;

  doctors = signal<any[]>([]);
  slots = signal<string[]>([]);

  minDate = '';
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

  ngOnInit(): void {

    this.minDate = this.formatDateForApi(new Date());

    const specialization$ =
      this.form.get('specialization')!
        .valueChanges
        .pipe(
          startWith(
            this.form.get('specialization')!.value
          )
        );

    const date$ =
      this.form.get('scheduledDate')!
        .valueChanges
        .pipe(
          startWith(
            this.form.get('scheduledDate')!.value
          )
        );

    combineLatest([
      specialization$,
      date$
    ])
      .pipe(
        switchMap(([spec, date]) => {

          this.form.patchValue({
            doctorId: '',
            timeSlot: ''
          });

          this.form.get('doctorId')?.disable();
          this.form.get('timeSlot')?.disable();

          this.slots.set([]);
          this.doctors.set([]);

          if (!spec || !date) {
            return of([]);
          }

          return this.doctorService.getAvailableDoctors(
            spec,
            this.formatDateForApi(date)
          );
        }),
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(res => {

        this.doctors.set(res);

        if (res.length > 0) {
          this.form.get('doctorId')?.enable();
        }
      });

    this.form.get('doctorId')!
      .valueChanges
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe(id => {

        if (id) {
          this.loadSlots();
        }
      });
  }

  loadSlots(): void {

    const doctorId =
      Number(this.form.get('doctorId')?.value);

    const date =
      this.form.get('scheduledDate')?.value;

    if (!doctorId || !date) {

      this.slots.set([]);

      this.form.get('timeSlot')?.disable();

      return;
    }

    const formatted =
      this.formatDateForApi(date);

    const today =
      this.formatDateForApi(new Date());

    const currentMinutes =
      this.getCurrentTimeInMinutes();

    this.form.patchValue({
      timeSlot: ''
    });

    this.form.get('timeSlot')?.disable();

    this.appointmentService
      .getAvailableSlots(
        doctorId,
        formatted
      )
      .pipe(
        takeUntilDestroyed(this.destroyRef)
      )
      .subscribe({

        next: (res) => {

          const filteredSlots =
            formatted === today
              ? res.filter(slot =>
                  this.parseSlotToMinutes(slot) >
                  currentMinutes
                )
              : res;

          this.slots.set(filteredSlots);

          if (filteredSlots.length > 0) {
            this.form.get('timeSlot')?.enable();
          }
        },

        error: () => {

          this.slots.set([]);

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

    this.appointmentService
      .bookAppointment(data)
      .subscribe({

        next: () => {

          this.toastr.success(
            'Appointment booked successfully',
            'Success'
          );

          this.showSuccessPopup = true;

          setTimeout(() => {

            this.showSuccessPopup = false;

            this.appointmentClosed.emit();

          }, 2000);
        },

        error: () => {

          this.toastr.error(
            'Unable to book appointment',
            'Error'
          );

          this.isLoading = false;
        },

        complete: () => {

          this.isLoading = false;
        }
      });
  }

  private formatDateForApi(
    value: string | Date
  ): string {

    const date =
      value instanceof Date
        ? value
        : new Date(value);

    return `${date.getFullYear()}-${
      String(date.getMonth() + 1)
        .padStart(2, '0')
    }-${
      String(date.getDate())
        .padStart(2, '0')
    }`;
  }

  private parseSlotToMinutes(
    slot: string
  ): number {

    const normalized =
      slot.trim().toUpperCase();

    const regex =
      /^(\d{1,2})(?::(\d{2}))?\s*(AM|PM)?$/;

    const match =
      regex.exec(normalized);

    if (!match) {
      return Number.MAX_SAFE_INTEGER;
    }

    let hours =
      Number(match[1]);

    const minutes =
      Number(match[2] ?? '0');

    const suffix =
      match[3];

    if (
      suffix === 'PM' &&
      hours < 12
    ) {
      hours += 12;
    }
    else if (
      suffix === 'AM' &&
      hours === 12
    ) {
      hours = 0;
    }

    return hours * 60 + minutes;
  }

  private getCurrentTimeInMinutes(): number {

    const now = new Date();

    return (
      now.getHours() * 60 +
      now.getMinutes()
    );
  }
}