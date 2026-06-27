import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LoginModal } from '../login-modal/login-modal';
import { RegisterModal } from '../register-modal/register-modal';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [LoginModal,RegisterModal],
  templateUrl: './landing.html',
  styleUrl: './landing.css',
})
export class LandingComponent  {

  showLogin = false;
  showRegister = false;

  openLogin() {
    this.showLogin = true;
    this.showRegister = false;
  }

  openRegister() {
    this.showRegister = true;
    this.showLogin = false;
  }

  closeModal() {
    this.showLogin = false;
    this.showRegister = false;
  }
}
