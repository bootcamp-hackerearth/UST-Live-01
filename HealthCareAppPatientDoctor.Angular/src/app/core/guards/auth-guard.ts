import { CanActivateFn } from '@angular/router';
import { inject } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../services/auth-service';

export const authGuard: CanActivateFn = (route, state) => {

  const authService = inject(AuthService);

  const router = inject(Router);

  if (!authService.isLoggedIn()) {

    return router.createUrlTree(['/login']);

  }

  const role = localStorage.getItem('role');

  const mustChangePassword =
    localStorage.getItem('mustChangePassword');

  if (
    role === 'Doctor' &&
    mustChangePassword === 'true' &&
    state.url !== '/doctor/change-password'
  ) {

    return router.createUrlTree(
      ['/doctor/change-password']
    );

  }

  return true;

};