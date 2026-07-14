import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { authState } from '../auth-state';

export const doctorGuard: CanActivateFn = () => {

  const router = inject(Router);

  if (
    authState().isLoggedIn &&
    authState().role.toLowerCase() === 'doctor'
  ) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
