import { Component, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { authState } from '../../../core/auth-state';

@Component({
  selector: 'app-book-appointment',
  templateUrl: './book-appointment.html',
  standalone: false,
  styleUrl: './book-appointment.css'
})
export class BookAppointment {

  doctor = signal<any>(null);
  selectedSlot = signal<string | null>(null);
  selectedDate = signal<string | null>(null);
  message = signal('');

  constructor(private router: Router, private http: HttpClient) {

    const nav = this.router.getCurrentNavigation();
    const state = nav?.extras.state as any;

    if (state?.doctor) {
      this.doctor.set(state.doctor);
    }
  }

  selectSlot(slot: string) {
    this.selectedSlot.set(slot);
  }

  onDateChange(value: string) {
    this.selectedDate.set(value);
  }


  confirmBooking() {

    if (!this.selectedDate()) {
      this.message.set("Please select a date ❌");
      return;
    }

    if (!this.selectedSlot()) {
      this.message.set("Please select a time slot ❌");
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${authState().token}`
    });

    const payload = {
      doctorId: this.doctor().doctorId,
      scheduledDate: new Date(this.selectedDate()!).toISOString(), 
      timeSlot: this.selectedSlot()
    };

    this.http.post<any>(
      'https://localhost:7038/api/appointments',
      payload,
      { headers }
    ).subscribe({
      next: () => {
        this.message.set("Appointment booked successfully ✅");

        setTimeout(() => {
          this.router.navigateByUrl('/patient/dashboard');
        }, 1500);
      },

      error: (err) => {
        console.error(err);

        // ✅ SHOW ACTUAL ERROR MESSAGE
        const errorMessage =
          err?.error?.message || "Booking failed ❌";

        this.message.set(errorMessage);
      }
    });
  }

}
