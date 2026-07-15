import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-contact-us',
  imports: [RouterLink],
  templateUrl: './contact-us.html',
  styleUrl: './contact-us.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ContactUs {
  private readonly authService = inject(AuthService);

  readonly dashboardRoute = computed(() => {
    return this.authService.role() === 'Doctor'
      ? '/doctor/dashboard'
      : '/patient/dashboard';
  });

  readonly dashboardLabel = computed(() => {
    return this.authService.role() === 'Doctor'
      ? 'Back to Doctor Dashboard'
      : 'Back to Patient Dashboard';
  });
}