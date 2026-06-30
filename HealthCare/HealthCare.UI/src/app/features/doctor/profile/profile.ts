import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DoctorService } from '../../../core/services/doctor.service';
import { Doctor } from '../../../core/models/doctor.model';

@Component({
  selector: 'app-doctor-profile',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './profile.html'
})
export class DoctorProfileComponent implements OnInit {

  doctor = signal<Doctor>({} as Doctor);
  loading = signal(true);

  constructor(private doctorService: DoctorService) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {

    this.loading.set(true);

    this.doctorService.getProfile()
      .subscribe({
        next: (res) => {
          console.log('Doctor profile:', res);

          this.doctor.set(res); 

          this.loading.set(false);
        },
        error: (err) => {
          console.error(err);
          this.loading.set(false);
        }
      });
  }
}