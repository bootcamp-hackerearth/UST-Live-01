import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, signal } from '@angular/core';
import { PatientSidebarComponent } from '../../shared/patient-sidebar/patient-sidebar.component';


@Component({
  selector: 'app-book-appointment',
  standalone: true,
  imports: [CommonModule,PatientSidebarComponent],
  templateUrl: './book-appointment.component.html',
  styleUrls: ['./book-appointment.component.css']
})
export class BookAppointmentComponent implements OnInit {

  minDate: string = '';

  // Signals for form inputs
  selectedDate = signal<string>('');
  selectedSpecialization = signal<string>('');
  selectedDoctor = signal<any>(null);
  selectedSlot = signal<string>('');

  // Signals for UI state
  hasSearched = signal<boolean>(false);
  showSuccessModal = signal<boolean>(false);
  showErrorModal = signal<boolean>(false);
  errorMessage = signal<string>('');
  noDoctorsMessage = signal<string>('');
  isBooking = signal<boolean>(false);

  // Signals for data
  doctors = signal<any[]>([]);
  timeSlots = signal<string[]>([]);

  specializations = [
    "GeneralMedicine", "Cardiology", "Dermatology", "Neurology",
    "Orthopedics", "Pediatrics", "Gynecology", "Psychiatry",
    "Ophthalmology", "ENT", "Urology", "Oncology",
    "Endocrinology", "Gastroenterology", "Pulmonology", "Nephrology"
  ];

  constructor(private readonly http: HttpClient) { }

  ngOnInit() {
    this.minDate = new Date().toISOString().split('T')[0];
  }

  getDoctors() {

    if (!this.selectedDate() || !this.selectedSpecialization()) {
      this.hasSearched.set(false);
      this.doctors.set([]);
      this.timeSlots.set([]);
      this.selectedDoctor.set(null);
      this.selectedSlot.set('');
      this.noDoctorsMessage.set('');
      return;
    }

    // Reset immediately
    this.doctors.set([]);
    this.timeSlots.set([]);
    this.selectedDoctor.set(null);
    this.selectedSlot.set('');
    this.noDoctorsMessage.set('');

    this.hasSearched.set(true);

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<any>(
      '/api/doctors/available',
      {
        headers,
        params: {
          specialisation: this.selectedSpecialization(),
          date: this.selectedDate()
        }
      }
    )
      .subscribe({
        next: (res) => {

          console.log('Doctor API response', res);

          this.doctors.set(res.doctors || []);

          if (this.doctors().length === 0) {
            this.noDoctorsMessage.set(
              res.message || 'Doctor is not available'
            );
          }
        },
        error: (err) => {
          console.error(err);
        }
      });
  }



  onDoctorChange() {

    this.selectedSlot.set('');
    this.timeSlots.set([]);

    if (!this.selectedDoctor() || !this.selectedDate()) return;

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    this.http.get<string[]>(
      '/api/appointments/slots',
      {
        headers,
        params: {
          date: this.selectedDate(),
          doctorId: this.selectedDoctor().doctorId
        }
      }
    ).subscribe(res => {
      console.log("Slots:", res);
      this.timeSlots.set([...res]);
    });
  }

  closeSuccessModal() {
    this.showSuccessModal.set(false);
    this.showErrorModal.set(false);

    // Reset form for new booking
    this.selectedDate.set('');
    this.selectedSpecialization.set('');
    this.selectedDoctor.set(null);
    this.selectedSlot.set('');
    this.doctors.set([]);
    this.timeSlots.set([]);
    this.hasSearched.set(false);
    this.errorMessage.set('');
    this.noDoctorsMessage.set('');
  }

  closeErrorModal() {
    this.showErrorModal.set(false);
    this.errorMessage.set('');
  }

  onDateChange(event: Event) {
    const value = (event.target as HTMLInputElement).value;

    this.selectedDate.set(value);

    this.getDoctors();
  }

  onSpecializationChange(event: Event) {
    const value = (event.target as HTMLSelectElement).value;

    this.selectedSpecialization.set(value);

    this.getDoctors();
  }

  onDoctorSelectChange(event: Event) {
    const doctorId = Number(
      (event.target as HTMLSelectElement).value
    );

    const doctor = this.doctors()
      .find(d => d.doctorId === doctorId);

    this.selectedDoctor.set(doctor ?? null);

    this.onDoctorChange();
  }

  onSlotChange(event: Event) {
    const value =
      (event.target as HTMLSelectElement).value;

    this.selectedSlot.set(value);
  }

  bookAppointment() {

    if (this.isBooking()) return;

    if (!this.selectedDate() ||
      !this.selectedSpecialization() ||
      !this.selectedDoctor() ||
      !this.selectedSlot()) {

      alert("Please select all fields before booking");
      return;
    }

    this.isBooking.set(true);

    const token = localStorage.getItem('token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`
    });

    const body = {
      doctorId: this.selectedDoctor().doctorId,
      scheduledDate: this.selectedDate(),
      timeSlot: this.selectedSlot()
    };

    this.http.post(
      '/api/appointments/book',
      body,
      { headers }
    ).subscribe({
      next: () => {
        this.showSuccessModal.set(true);
        this.showErrorModal.set(false);
        this.isBooking.set(false);
      },
      error: (err) => {
        this.errorMessage.set(
          err.error?.message || "Unable to book appointment"
        );

        this.showErrorModal.set(true);
        this.showSuccessModal.set(false);
        this.isBooking.set(false);
      }
    });
  }
} 
