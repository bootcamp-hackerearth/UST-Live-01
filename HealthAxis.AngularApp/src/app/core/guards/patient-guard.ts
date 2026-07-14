import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { authState } from '../auth-state';

export const patientGuard: CanActivateFn = () => {

  const router = inject(Router);

  if (
    authState().isLoggedIn &&
    authState().role.toLowerCase() === 'patient'
  ) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
