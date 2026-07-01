import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService} from '../service/auth.service';
import {RegisterForm} from '../models/RegisterForm/RegisterForm';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, FormsModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class Register {


form: RegisterForm = {
  email: '',
  password: '',
  confirmPassword: '',
  fullName: '',
  dateOfBirth: '',
  gender: '',
  phoneNumber: '',
  insuranceId: ''
};


  constructor(private authService: AuthService) {}

  submit() {

    if (this.form.password !== this.form.confirmPassword) {
      alert("Passwords do not match ");
      return;
    }

    this.authService.register(this.form).subscribe({
      next: (res) => {
        alert(res.message);
      },
      error: (err) => {
        alert(err.error);  
      }
    });
  }

}
