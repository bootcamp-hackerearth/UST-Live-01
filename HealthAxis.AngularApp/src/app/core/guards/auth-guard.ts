import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { authState } from '../auth-state';

export const authGuard: CanActivateFn = () => {

  const router = inject(Router);

  if (authState().isLoggedIn) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};
