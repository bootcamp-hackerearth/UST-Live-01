import { Component, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  email = signal('');
  password = signal('');
  confirmPassword = signal('');

  name = signal('');
  dateOfBirth = signal('');
  gender = signal('');
  phoneNo = signal('');
  insuranceID = signal('');

  error = signal('');
  success = signal('');

  constructor(private http: HttpClient, private router: Router) { }

  register() {
    const payload = {
      email: this.email(),
      password: this.password(),
      confirmPassword: this.confirmPassword(),
      role: "Patient",

      name: this.name(),
      dateOfBirth: this.dateOfBirth(),
      gender: this.gender(),
      phoneNo: this.phoneNo(),
      insuranceID: this.insuranceID()
    };

    this.http.post<any>('https://localhost:7038/api/auth/register', payload)
      .subscribe({
        next: (res) => {
          if (res.success) {
            this.success.set("Registration successful ✅");

            // ✅ redirect after short delay
            setTimeout(() => {
              this.router.navigate(['/login']);
            }, 1000);
          } else {
            this.error.set(res.message);
          }
        },
        error: (err) => {
          this.error.set("Registration failed");
        }
      });
  }
}
