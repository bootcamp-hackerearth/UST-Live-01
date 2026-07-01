import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
  UrlTree
} from '@angular/router';

import { AuthService } from '../services/auth.service';

export const firstLoginGuard: CanActivateFn = (): boolean | UrlTree => {
  const authService = inject(AuthService);

  const router = inject(Router);

  const currentUser = authService.getCurrentUserSnapshot();

  if (
    currentUser?.role === 'Doctor' &&
    currentUser.firstLogin
  ) {
    return router.createUrlTree(['/doctor/change-password']);
  }

  return true;
};
