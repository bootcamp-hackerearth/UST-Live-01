import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MockAuth, MockUser } from '../../services/mock-auth';

@Component({
  selector: 'app-change-password',
  imports: [RouterLink, FormsModule],
  templateUrl: './change-password.html',
  styleUrl: './change-password.css',
})
export class ChangePassword implements OnInit {
  private auth = inject(MockAuth);
  private router = inject(Router);

  currentUser?: MockUser;

  currentPassword = '';
  newPassword = '';
  confirmPassword = '';

  isSubmitting = false;
  message = '';
  isError = false;

  ngOnInit(): void {
    const user = this.auth.getCurrentUser();

    if (!user) {
      this.router.navigate(['/login']);
      return;
    }

    if (!user.mustChangePassword) {
      this.router.navigate(['/login']);
      return;
    }

    this.currentUser = user;
  }

  updatePassword(): void {
    this.message = '';
    this.isError = false;

    if (!this.currentPassword || !this.newPassword || !this.confirmPassword) {
      this.showError('Please fill all password fields.');
      return;
    }

    this.isSubmitting = true;

    setTimeout(() => {
      const result = this.auth.changeTemporaryPassword(
        this.currentPassword,
        this.newPassword,
        this.confirmPassword
      );

      this.isSubmitting = false;

      if (!result.success) {
        this.showError(result.message);
        return;
      }

      this.message = result.message;
      this.isError = false;

      setTimeout(() => {
        this.auth.logout();
        this.router.navigate(['/login']);
      }, 1000);
    }, 600);
  }

  private showError(message: string): void {
    this.isSubmitting = false;
    this.message = message;
    this.isError = true;
  }
}