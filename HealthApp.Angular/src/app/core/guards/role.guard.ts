import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';

import { AuthService } from '../services/auth.service';

export const patientGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const role = authService.getRole();

  if (role === 'Patient') {
    return true;
  }

  if (role === 'Doctor') {
    return router.createUrlTree(['/doctor/dashboard']);
  }

  return router.createUrlTree(['/login']);
};

export const doctorGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const role = authService.getRole();

  if (role === 'Doctor') {
    return true;
  }

  if (role === 'Patient') {
    return router.createUrlTree(['/patient/dashboard']);
  }

  return router.createUrlTree(['/login']);
};