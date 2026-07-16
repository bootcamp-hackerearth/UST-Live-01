import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService} from '../service/auth.service';
import { RegisterForm } from '../models/RegisterForm/RegisterForm';
import { AppPopupComponent } from '../shared/app-popup/app-popup';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterModule, FormsModule, AppPopupComponent],
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

popupVisible = false;
popupTitle = '';
popupMessage = '';
popupType: 'success' | 'error' | 'warning' = 'success';

  constructor(private authService: AuthService) {}

  submit() {

    if (this.form.password !== this.form.confirmPassword) {
      this.showPopup('Password Mismatch', 'Passwords do not match.', 'warning');
      return;
    }

    this.authService.register(this.form).subscribe({
      next: (res) => {
        this.showPopup('Registration Successful', res.message || 'Account created successfully.', 'success');
        window.location.href = '/login';
      },
      error: (err) => {
        this.showPopup('Registration Failed', err.error || 'Unable to create your account.', 'error');
      }
    });
  }

  private showPopup(title: string, message: string, type: 'success' | 'error' | 'warning' = 'success') {
    this.popupTitle = title;
    this.popupMessage = message;
    this.popupType = type;
    this.popupVisible = true;
  }

  closePopup() {
    this.popupVisible = false;
    this.popupTitle = '';
    this.popupMessage = '';
  }

}
